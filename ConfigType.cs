using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;

namespace AllTheClothes
{
    public class ConfigType
    {
        public bool UnlockScavClothing { get; set; }
        public bool UnlockScavVoices { get; set; }
        public bool UnlockScavHeads { get; set; }
        public bool UnlockPrestigeOptions { get; set; }
        public bool UnlockFactionalClothing { get; set; }
        public bool UnlockFactionalHeads { get; set; }
        public bool UnlockFactionalVoices { get; set; }
        public bool EnableMannequinHead { get; set; }
        public bool RemoveRefRequirement { get; set; }
        public bool AllScavClothesFree { get; set; }
        public bool AllPMCClothesFree { get; set; }
        public bool UseRagmanInsteadOfFence { get; set; }
        public Dictionary<string, ConfigTradeType>? needTrade { get; set; }
        public Dictionary<MongoId, Suit>? replaceTrade { get; set; }
        public Dictionary<string, Suit>? prestigeClothes { get; set; }
        public ConfigKitType? needKit { get; set; }
        public Dictionary<string, CustomizationItem>? customEntries { get; set; }
        public Dictionary<string, CustomizationItem>? headEntries { get; set; }
        public Dictionary<string, ConfigHandEntryType>? handEntries { get; set; }
    }

    public class ConfigTradeType
    {
        public bool availability { get; set; }
        public MongoId suiteTradeID { get; set; }
        public MongoId kitToSell { get; set; }
        public bool ragman { get; set; }
        public required ConfigTradeRequirementsType tradeRequirements { get; set; }
    }
    public class ConfigTradeRequirementsType
    {
        public int loyaltyLevel { get; set; }
        public int profileLevel { get; set; }
        public int standing { get; set; }
        public int prestigeLevel { get; set; }
        public List<string>? skillRequirements { get; set; }
        public List<string>? questRequirements { get; set; }
        public List<ItemRequirement>? itemRequirements { get; set; }
        public List<string>? achievementRequirements { get; set; }
    }

    public class ConfigKitType
    {
        public Dictionary<string, ConfigKitTopType>? tops { get; set; }
        public Dictionary<string, ConfigKitLowerType>? lowers { get; set; }
    }
    public class ConfigKitTopType
    {
        public MongoId hands { get; set; }
        public MongoId body { get; set; }
        public MongoId id { get; set; }
    }
    public class ConfigKitLowerType
    {
        public MongoId feet { get; set; }
        public MongoId id { get; set; }
    }
    public class ConfigHandEntryType
    {
        public required string bundle {  get; set; }
        public MongoId id { get; set; }
    }

    public class ModLocaleType
    {
        public required string Description { get; set; }
        public required string Name { get; set; }
        public required string ShortName { get; set; }
    }
}
