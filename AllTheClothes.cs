using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;
using System.Reflection;
using SPTarkov.Server.Core.Models.Logging;
using SPTarkov.Server.Core.Services;
using System.Reflection.Metadata;

namespace AllTheClothes
{
    public record ModMetadata : AbstractModMetadata
    {
        public override string ModGuid { get; init; } = "com.rairaitheraichu.alltheclothes";
        public override string Name { get; init; } = "AllTheClothes";
        public override string Author { get; init; } = "RaiRaiTheRaichu";
        public override List<string>? Contributors { get; init; }
        public override SemanticVersioning.Version Version { get; init; } = new("3.1.0");
        public override SemanticVersioning.Range SptVersion { get; init; } = new("~4.0.0");
        public override List<string>? Incompatibilities { get; init; }
        public override Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; }
        public override string? Url { get; init; } = "https://github.com/RaiRaiTheRaichu/SPT_All-The-Clothes-Mod";
        public override bool? IsBundleMod { get; init; } = true;
        public override string? License { get; init; } = "NCSA";
    }

    [Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)]
    public class DatabaseEdit(
        DatabaseServer databaseServer,
        ConfigServer configServer,
        DatabaseService databaseService,
        ISptLogger<DatabaseEdit> logger,
        ModHelper modHelper) : IOnLoad
    {
        private readonly DatabaseServer _databaseServer = databaseServer;
        private readonly ConfigServer _configServer = configServer;
        private readonly ISptLogger<DatabaseEdit> _logger = logger;
        private readonly ModHelper _modHelper = modHelper;

        private readonly char OS_SEPARATOR = System.IO.Path.DirectorySeparatorChar;

        private ConfigType ModConfig;

        public Task OnLoad()
        {
            // Load config
            var modPath = _modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
            ModConfig = _modHelper.GetJsonDataFromFile<ConfigType>(modPath + OS_SEPARATOR + "config", "config.jsonc");

            // Handling for SPT version < 4.0.2, where Fence can not use the Services menu
            var SPTVersion = SPTarkov.Server.Core.Utils.ProgramStatics.SPT_VERSION();
            if (SPTVersion.Patch < 2)
            {
                _logger.LogWithColor("[AllTheClothes] NOTICE: On SPT versions prior to 4.0.2, clothing can only be sold on Ragman regardless of config settings." +
                             "\nServices tab on Fence is unavailable on this version, please update your SPT server to the latest version to enable Fence's services!" +
                             "\nDo not report this as a bug.", LogTextColor.Yellow);
                ModConfig.UseRagmanInsteadOfFence = true;
            }

            // Add trades for the prestige clothes, allow prestige voices to be selected by default
            if (ModConfig.UnlockPrestigeOptions)
            {
                UnlockPrestigeClothing();
            }

            // Unlock clothing, heads, and voices for Scavs and Bear/Usecs
            UnlockFactionalCustomization();

            // Add new hand customization database entries for new first-person models
            if (ModConfig.handEntries != null)
            {
                GenerateHands();
            }

            // Add new head customization database entries for some bosses to be chosen by the player
            if (ModConfig.headEntries.Count > 0
                && ModConfig.UnlockScavHeads)
            {
                GenerateHeads();
            }
            
            // Create customization entries for the new clothes, such as Shturman's parka and Tagilla's chest
            if (ModConfig.customEntries.Count > 0)
            {
                GenerateClothing();
            }
            
            // Create kit entries
            if (ModConfig.needKit?.tops.Count > 0 || ModConfig.needKit?.lowers.Count > 0)
            {
                GenerateKits();
            }

            // Create trader offers
            if (ModConfig.needTrade.Count > 0)
            {
                GenerateTraderAssorts();
            }

            if (ModConfig.replaceTrade.Count > 0)
            {
                ReplaceTraderSuitOffers();
            }

            // Clear all requirements from vanilla suit offers
            if (ModConfig.AllPMCClothesFree)
            {
                UnlockVanillaClothing();
            }

            if (ModConfig.RemoveRefRequirement)
            {
                ChangeLoyaltyRequirements();
            }

            // Add localization entries
            var localeDbPath = _modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly()) + $"{OS_SEPARATOR}db{OS_SEPARATOR}locale";
            string[] files = Directory.GetFiles(localeDbPath);

            if (files.Length > 0) GenerateLocalization(files);

            //GenerateDebugLog();

            return Task.CompletedTask;
        }

        private Task UnlockVanillaClothing()
        {
            var ragmanSuits = _databaseServer.GetTables().Traders["5ac3b934156ae10c4430e83c"].Suits;
            if (ragmanSuits == null) return Task.CompletedTask;

            foreach (var suitOffer in ragmanSuits)
            {
                suitOffer.Requirements = new SuitRequirements
                {
                    AchievementRequirements = [],
                    LoyaltyLevel = 0,
                    ProfileLevel = 0,
                    Standing = 0,
                    PrestigeLevel = 0,
                    SkillRequirements = [],
                    QuestRequirements = [],
                    ItemRequirements = [],
                    RequiredTid = "5ac3b934156ae10c4430e83c"
                };
            }

            return Task.CompletedTask;
        }

        private Task UnlockPrestigeClothing()
        {
            var customizationDatabase = _databaseServer.GetTables().Templates.Customization;
            var customizationStorage = _databaseServer.GetTables().Templates.CustomisationStorage;
            var ragmanSuits = _databaseServer.GetTables().Traders["5ac3b934156ae10c4430e83c"].Suits;

            // Add trades for the Hawaiian shirts
            foreach (var prestigeSuit in ModConfig.prestigeClothes)
            {
                ragmanSuits.Add(prestigeSuit.Value);
            }

            // Unlock Voices
            customizationDatabase["684696e8199c6a77dc0f9bc8"].Properties.AvailableAsDefault = true; // "Rob"
            customizationDatabase["6846990ed5d969efe3078408"].Properties.AvailableAsDefault = true; // "Vitaly"

            customizationStorage.Add(new CustomisationStorage()
            {
                Id = "684696e8199c6a77dc0f9bc8",
                Source = "default",
                Type = "voice"
            });

            customizationStorage.Add(new CustomisationStorage()
            {
                Id = "6846990ed5d969efe3078408",
                Source = "default",
                Type = "voice"
            });

            return Task.CompletedTask;
        }

        private Task ChangeLoyaltyRequirements()
        {
            var ragmanSuits = _databaseServer.GetTables().Traders["5ac3b934156ae10c4430e83c"].Suits;

            foreach (Suit suit in ragmanSuits)
            {
                if (suit.Requirements?.RequiredTid == "6617beeaa9cfa777ca915b7c")
                {
                    suit.Requirements.RequiredTid = "5ac3b934156ae10c4430e83c";
                }
            }

            return Task.CompletedTask;
        }

        private Task UnlockFactionalCustomization()
        {
            var customizationDatabase = _databaseServer.GetTables().Templates.Customization;
            var storageDatabase = _databaseServer.GetTables().Templates.CustomisationStorage;
            var characterDatabase = _databaseServer.GetTables().Templates.Character;

            foreach (var entry in customizationDatabase)
            {
                // Skipping some unnecessary entries
                if (entry.Value.Id == "5d5f8ba486f77431254e7fd2"        // The "Empty" head, skipping
                    || entry.Value.Name.ToLower().Contains("zombie")    // The zombie head models
                    || (entry.Value.Id == "6644d2da35d958070c02642c" && !ModConfig.EnableMannequinHead))    // The Mannequin head model
                {
                    continue;
                }

                var parent = entry.Value.Parent;

                // Clothing (Tops and Lowers)
                if (parent == "5cd944ca1388ce03a44dc2a4"        // Upper Kits
                    || parent == "5cd944d01388ce000a659df9")    // Lower Kits
                {
                    // Unlocking Scav clothing
                    if (entry.Value.Properties.Side.Contains("Savage")
                        && ModConfig.UnlockScavClothing)
                    {
                        entry.Value.Properties.Side = ["Usec", "Bear", "Savage"];  // Set "Savage" clothes to Usec and BEAR

                        if (!characterDatabase.Contains(entry.Value.Id))
                        {
                            characterDatabase.Add(entry.Value.Id);                 // Add the ID to the Character Database
                        }
                        continue;
                    }

                    // Unlocking PMC factional clothing
                    if ((entry.Value.Properties.Side.Contains("Bear")
                        || entry.Value.Properties.Side.Contains("Usec"))
                        && ModConfig.UnlockFactionalClothing)
                    {
                        entry.Value.Properties.Side = ["Usec", "Bear"];
                    }
                }

                // Heads
                if (parent == "5cc085e214c02e000c6bea67")
                {
                    // Scav heads playable by PMCs
                    if (entry.Value.Properties.Side.Contains("Savage")
                        && ModConfig.UnlockScavHeads)
                    {
                        // Make the Scav heads playable by PMC factions
                        entry.Value.Properties.Side = ["Savage", "Bear", "Usec"];
                        entry.Value.Properties.AvailableAsDefault = true;

                        // Add the proper CustomisationStorage entry, required to make the head appear to select
                        CustomisationStorage newHead = new()
                        {
                            Id = entry.Value.Id,
                            Source = "default",
                            Type = "head"
                        };
                        storageDatabase.Add(newHead);

                        // Add the Id to the Character Database if necessary
                        if (!characterDatabase.Contains(entry.Value.Id))
                        {
                            characterDatabase.Add(entry.Value.Id);
                        }
                        continue;
                    }

                    // PMC heads playable regardless of faction
                    if ((entry.Value.Properties.Side.Contains("Bear")
                        || entry.Value.Properties.Side.Contains("Usec"))
                        && ModConfig.UnlockFactionalHeads)
                    {
                        entry.Value.Properties.Side = ["Usec", "Bear"];
                    }
                    continue;
                }

                // Voices
                if (parent == "5fc100cf95572123ae738483")
                {
                    // Skipping the voice node
                    if (entry.Value.Id == "5fc100cf95572123ae738483")
                    {
                        continue;
                    }

                    if (entry.Value.Properties.Side.Contains("Savage")
                        && ModConfig.UnlockScavVoices)
                    {
                        // Make the Scav voices playable by PMC factions
                        entry.Value.Properties.Side = ["Savage", "Usec", "Bear"];
                        entry.Value.Properties.AvailableAsDefault = true;

                        // Add the proper CustomisationStorage entry, required to make the head appear to select
                        CustomisationStorage newVoice = new()
                        {
                            Id = entry.Value.Id,
                            Source = "default",
                            Type = "voice"
                        };
                        storageDatabase.Add(newVoice);

                        // Add the Id to the Character Database if necessary
                        if (!characterDatabase.Contains(entry.Value.Id))
                        {
                            characterDatabase.Add(entry.Value.Id);
                        }
                        continue;
                    }

                    // PMC voices playable regardless of faction
                    if ((entry.Value.Properties.Side.Contains("Usec")
                        || entry.Value.Properties.Side.Contains("Bear"))
                        && ModConfig.UnlockFactionalVoices)
                    {
                        entry.Value.Properties.Side = ["Usec", "Bear"];
                    }
                }
            }

            return Task.CompletedTask;
        }

        private Task GenerateHands()
        {
            var customizationDatabase = _databaseServer.GetTables().Templates.Customization;

            foreach (var handEntry in ModConfig.handEntries)
            {
                CustomizationItem newHand = new()
                {
                    Id = handEntry.Value.id,
                    Name = handEntry.Key,
                    Parent = (MongoId)"5cc086a314c02e000c6bea69",
                    Type = "Item",
                    Properties = new CustomizationProperties()
                    {
                        Name = "DefaultBearHands",
                        ShortName = "DefaultBearHands",
                        Description = "DefaultBearHands",
                        Side = new List<string>() { "Bear", "Usec", "Savage" },
                        BodyPart = "Hands",
                        Prefab = new Prefab()
                        {
                            Path = handEntry.Value.bundle,
                            Rcid = ""
                        },
                        WatchPrefab = new Prefab()
                        {
                            Path = "",
                            Rcid = ""
                        },
                        IntegratedArmorVest = false,
                        WatchPosition = new XYZ()
                        {
                            X = 0,
                            Y = 0,
                            Z = 0
                        },
                        WatchRotation = new XYZ()
                        {
                            X = 0,
                            Y = 0,
                            Z = 0
                        }
                    },
                    Prototype = (MongoId)"5cc0876314c02e000c6bea6b"
                };
                customizationDatabase.Add(handEntry.Value.id, newHand);
            }

            return Task.CompletedTask;
        }

        private Task GenerateHeads()
        {
            var customizationDatabase = _databaseServer.GetTables().Templates.Customization;
            var storageDatabase = _databaseServer.GetTables().Templates.CustomisationStorage;
            var characterDatabase = _databaseServer.GetTables().Templates.Character;

            foreach (var headEntry in ModConfig.headEntries)
            {
                customizationDatabase[headEntry.Value.Id] = headEntry.Value;
                characterDatabase.Add(headEntry.Value.Id);
                CustomisationStorage storageEntry = new()
                {
                    Id = headEntry.Value.Id,
                    Source = "default",
                    Type = "head"
                };

                storageDatabase.Add(storageEntry);
            }

            // We will never forget the civ scav. <3
            customizationDatabase["5d28afe786f774292668618d"].Properties.Prefab = new Prefab()
            {
                Path = "assets/content/characters/character/prefabs/wild_head_3.bundle",
                Rcid = ""
            };

            return Task.CompletedTask;
        }

        private Task GenerateClothing()
        {
            var customizationDatabase = _databaseServer.GetTables().Templates.Customization;
            var characterDatabase = _databaseServer.GetTables().Templates.Character;

            foreach (var customizationEntry in ModConfig.customEntries)
            {
                customizationDatabase.Add(customizationEntry.Value.Id, customizationEntry.Value);
                characterDatabase.Add(customizationEntry.Value.Id);
            };
            

            return Task.CompletedTask;
        }

        private Task GenerateKits()
        {
            var customizationDatabase = _databaseServer.GetTables().Templates.Customization;
            var characterDatabase = _databaseServer.GetTables().Templates.Character;

            if (ModConfig.needKit.tops != null)
            {
                foreach (var configTopData in ModConfig.needKit.tops)
                {
                    CustomizationItem customizationItem = new CustomizationItem()
                    {
                        Id = configTopData.Value.id,
                        Name = configTopData.Key + "_kit",
                        Parent = (MongoId)"5cd944ca1388ce03a44dc2a4",
                        Type = "Item",
                        Properties = new CustomizationProperties()
                        {
                            Name = configTopData.Key,
                            ShortName = configTopData.Key,
                            Description = configTopData.Key,
                            Side = new List<string>() { "Usec", "Bear", "Savage" },
                            ProfileVersions = [],
                            AvailableAsDefault = false,
                            DisableForMannequin = false,
                            Body = configTopData.Value.body,
                            Hands = configTopData.Value.hands
                        },
                        Prototype = (MongoId)"5cde9ec17d6c8b04723cf479"
                    };

                    customizationDatabase.Add(configTopData.Value.id, customizationItem);
                    characterDatabase.Add(configTopData.Value.id);
                };
            };
            
            if (ModConfig.needKit.lowers != null)
            {
                foreach (var configLowerData in ModConfig.needKit.lowers)
                {
                    CustomizationItem customizationItem = new CustomizationItem()
                    {
                        Id = configLowerData.Value.id,
                        Name = configLowerData.Key + "_kit",
                        Parent = (MongoId)"5cd944d01388ce000a659df9",
                        Type = "Item",
                        Properties = new CustomizationProperties()
                        {
                            Name = configLowerData.Key,
                            ShortName = configLowerData.Key,
                            Description = configLowerData.Key,
                            Side = new List<string>() { "Usec", "Bear", "Savage" },
                            ProfileVersions = [],
                            AvailableAsDefault = false,
                            DisableForMannequin = false,
                            Feet = configLowerData.Value.feet
                        },
                        Prototype = (MongoId)"5cde9ec17d6c8b04723cf479"
                    };

                    customizationDatabase.Add(configLowerData.Value.id, customizationItem);
                    characterDatabase.Add(configLowerData.Value.id);
                }; 
            };

            return Task.CompletedTask;
        }

        private Task GenerateTraderAssorts()
        {
            var traderDatabase = _databaseServer.GetTables().Traders;

            foreach (var tradeEntry in ModConfig.needTrade)
            {
                if (!tradeEntry.Value.availability) continue;

                MongoId traderId = "";

                if (ModConfig.UseRagmanInsteadOfFence || tradeEntry.Value.ragman)
                {
                    traderId = "5ac3b934156ae10c4430e83c";
                }
                else
                {
                    traderId = "579dc571d53a0658a154fbec";
                }

                Suit newSuitOffer = new()
                {
                    Id = tradeEntry.Value.suiteTradeID,
                    Tid = traderId,
                    SuiteId = tradeEntry.Value.kitToSell,
                    IsActive = true,
                    ExternalObtain = false,
                    InternalObtain = true,
                    IsHiddenInPVE = false,
                    Requirements = new SuitRequirements()
                    {
                        LoyaltyLevel = ModConfig.AllScavClothesFree ? 0 : tradeEntry.Value.tradeRequirements.loyaltyLevel,
                        ProfileLevel = ModConfig.AllScavClothesFree ? 0 : tradeEntry.Value.tradeRequirements.profileLevel,
                        Standing = ModConfig.AllScavClothesFree ? 0 : tradeEntry.Value.tradeRequirements.standing,
                        PrestigeLevel = ModConfig.AllScavClothesFree ? 0 : tradeEntry.Value.tradeRequirements.prestigeLevel,
                        SkillRequirements = ModConfig.AllScavClothesFree ? [] : tradeEntry.Value.tradeRequirements.skillRequirements,
                        QuestRequirements = ModConfig.AllScavClothesFree ? [] : tradeEntry.Value.tradeRequirements.questRequirements,
                        ItemRequirements = ModConfig.AllScavClothesFree ? [] : tradeEntry.Value.tradeRequirements.itemRequirements,
                        AchievementRequirements = ModConfig.AllScavClothesFree ? [] : tradeEntry.Value.tradeRequirements.achievementRequirements,
                        RequiredTid = traderId
                    }
                };

                traderDatabase[traderId].Suits ??= new List<Suit>();

                traderDatabase[traderId].Suits.Add(newSuitOffer);
            }

            if (traderDatabase["579dc571d53a0658a154fbec"].Suits != null)
            {
                traderDatabase["579dc571d53a0658a154fbec"].Base.CustomizationSeller = true;
            }

            return Task.CompletedTask;
        }

        private Task ReplaceTraderSuitOffers()
        {
            var traderDatabase = _databaseServer.GetTables().Traders;
            Dictionary<MongoId, List<MongoId>> tradesToRemove = new Dictionary<MongoId, List<MongoId>>();
            foreach (var tradeEntry in ModConfig.replaceTrade)
            {
                if (!tradesToRemove.ContainsKey(tradeEntry.Value.Tid))
                {
                    tradesToRemove.Add(tradeEntry.Value.Tid, new List<MongoId>());
                }
                tradesToRemove[tradeEntry.Value.Tid].Add(tradeEntry.Value.SuiteId);
            }

            foreach (var trader in tradesToRemove.Keys)
            {
                List<Suit> newSuitList = traderDatabase[trader].Suits.FindAll(offer => !tradesToRemove[trader].Contains(offer.SuiteId));
                
                foreach (var replacementOffer in ModConfig.replaceTrade)
                {
                    if (replacementOffer.Value.Tid != trader) continue;

                    newSuitList.Add(replacementOffer.Value);
                }

                traderDatabase[trader].Suits = newSuitList;
            }

            return Task.CompletedTask;
        }

        private Task GenerateLocalization(string[] files)
        {

            foreach (string file in files)
            {
                string localeJson = System.IO.Path.GetFileName(file);
                string localePath = System.IO.Path.GetDirectoryName(file);
                string localeKey = System.IO.Path.GetFileNameWithoutExtension(file).ToLower();

                var localeFile = _modHelper.GetJsonDataFromFile<Dictionary<String, ModLocaleType>>(localePath, localeJson);

                if (databaseService.GetLocales().Global.TryGetValue(localeKey, out var lazyloadedValue))
                {
                    lazyloadedValue.AddTransformer(lazyloadedLocaleData =>
                    {
                        foreach (var localeEntry in localeFile)
                        {
                            lazyloadedLocaleData[$"{localeEntry.Key} Name"] = localeEntry.Value.Name;
                            lazyloadedLocaleData[$"{localeEntry.Key} ShortName"] = localeEntry.Value.ShortName;
                            lazyloadedLocaleData[$"{localeEntry.Key} Description"] = localeEntry.Value.Description;
                        }
                        return lazyloadedLocaleData;
                    });
                }
            }

            return Task.CompletedTask;
        }

        
        private void GenerateDebugLog()
        {
            var localeDatabase = _databaseServer.GetTables().Locales.Global;
            var customizationDatabase = _databaseServer.GetTables().Templates.Customization;
            var characterDatabase = _databaseServer.GetTables().Templates.Character;
            var traderDatabase = _databaseServer.GetTables().Traders;

            var localeJson = Newtonsoft.Json.JsonConvert.SerializeObject(localeDatabase["en"]);
            var customizationJson = Newtonsoft.Json.JsonConvert.SerializeObject(customizationDatabase);
            var characterJson = Newtonsoft.Json.JsonConvert.SerializeObject(characterDatabase);
            var ragmanJson = Newtonsoft.Json.JsonConvert.SerializeObject(traderDatabase["5ac3b934156ae10c4430e83c"]);
            var fenceJson = Newtonsoft.Json.JsonConvert.SerializeObject(traderDatabase["579dc571d53a0658a154fbec"]);

            Directory.CreateDirectory("ATC_Dumps");

            File.WriteAllText($"ATC_Dumps{OS_SEPARATOR}localeJson.json", localeJson);
            File.WriteAllText($"ATC_Dumps{OS_SEPARATOR}customizationJson.json", customizationJson);
            File.WriteAllText($"ATC_Dumps{OS_SEPARATOR}characterJson.json", characterJson);
            File.WriteAllText($"ATC_Dumps{OS_SEPARATOR}suitsRagmanJson.json", ragmanJson);
            File.WriteAllText($"ATC_Dumps{OS_SEPARATOR}suitsFenceJson.json", fenceJson);
        }
        
    }
}
