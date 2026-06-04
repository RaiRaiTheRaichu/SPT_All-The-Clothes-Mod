# All The Clothes v3.0.1
Author: RaiRaiTheRaichu

### ---BUILT FOR SPT VERSION 4.0.13---

Written from the ground up for SPT 4.0+, All The Clothes is back with more options than ever.
Included out of the box are all Arena and Battlepass-only clothing, integrated into the trader services menu.
All scav and boss clothing by default can be found on Fence's new Services tab*, all PMC clothing can be found on Ragman as usual.
	- On SPT version 4.0.0 and 4.0.1, scav and boss clothing will always be found on Ragman. Please upgrade your version of SPT in order to use Service menus from other traders, including Fence.
Clothing, voices, and heads are all able to be unlocked and equipped regardless of faction, with their own unique unlock requirements.
Check out the file config.json for all options, including making all the clothing, modded and vanilla, free to be worn without any requirements at all.

You can:
- Choose scav and boss voices for your PMC!
- Choose scav and boss faces for your PMC! (Only available at profile creation - see "Swapping Heads" to change head models on existing profiles.)
- Choose any faction voice for your PMC! Play as a USEC with a Bear voice or vice versa!
- Choose any faction clothing for your PMC! Dress in your favorite Bear clothing as a USEC, or dress up as a USEC as a Bear!
- Disable the unlock requirements for all these new Scav/Boss clothes! Wear whatever you want right at the start for maximum roleplay!
- Come up with your own unlocks and rebalance them however you'd like!

If you'd like to rename some of the new clothing/head/voices, the localization can be found within the "db/locale/" folder.
Remember, you can change your voice at any time in the game's Settings, in the Audio tab.

Feel free to leave a comment or contact me using Discord if you have any suggestions or have any issues!

## ---BUILD INFO---

Requirements:
- .NET 9.0

The following packages installed:
- Newtonsoft.Json (`13.0.4`)
- SPTarkov.Common (`4.0.0`)
- SPTarkov.DI (`4.0.0`)
- SPTarkov.Reflection (`4.0.0`)
- SPTarkov.Server.Core (`4.0.0`)
(You can select `Manage NuGet Packages...` within the Dependencies tab of Visual Studio to install them. Pay special attention to the version being installed, mods compiled with packages marked `4.0.13`, for example, will not run on any version below that.)

Open AllTheClothes.sln with Visual Studio and build the mod in Release mode.
Alternatively, run `dotnet build -c Release` from the project directory root.

The output will be stored in the project's `bin\Release\` folder.

BUNDLES ARE NOT INCLUDED ON THE REPO DUE TO FILE SIZE CONCERNS, please download them and add them to the AllTheClothes mod folder manually.
https://drive.google.com/file/d/1sFOXYyheN4YQg3zUSfrl6kjkKSUrtgDU/view

I will try to keep this link accurate and up-to-date with the required bundles to run the mod.

## ---INSTALL INFO---

How to install:
Drag and drop the included `SPT` folder from this zip into your game folder, allow it to merge with your existing folder.

If you're updating from an older version of the mod, please be sure to delete the old mod folder from your `SPT/user/mods` folder.

## ---CHANGELOGS---

#### v1.1.0 Changelog: 
- Added first person arms/hands to every clothing! Now you can see the proper hands for all clothing.
- Added Killa's pants to Ragman.
- Added config option to make all default clothing (clothes already on Ragman) free as well.
- Added config option to use Ragman for all new clothing instead of Fence.
- Added custom language file support.
- Added Russian language support (thanks Sk1p36!)
- Tagilla's/Shturman's head is no longer packaged with his top.
- Tagilla's head can now be selected at character creation on its own.
- Minor change to the version in the package.json, compatible with version 3.2.3 of SPT.

#### v1.1.1 Changelog:
- Fixed issue with Big Pipe and Sanitar tops causing loading issues.
- Fixed issue with Overview tab breaking if you updated from v1.0.0.
- Tying in to the previous change, clothing you previously unlocked in v1.0.0 are now carried over, assuming you haven't already repurchased them.
- Minor change to the version in the package.json, compatible with version 3.2.4 of SPT.

#### v1.1.2 Changelog:
- Fixed issue where the save sanitizing feature would crash the server upon loading a newly created profile.
- Fixed issue where some SCAV clothing had the incorrect first person hand meshes.
- Minor refactors to the save sanitizing feature for compatibility with version 3.3.0 of SPT.
- Minor change to the version in the package.json, compatible with version 3.3.0 of SPT.

#### v1.2.0 Changelog: 
- Major refactor to localization to comply with new standards as of client version 20765.
- Minor change to the version in the package.json, compatible with version 3.4.0 of SPT.

#### v1.3.0 Changelog: 
- Added new clothing with the EFT 0.12.13 patch - "Victory" clothing set and "Zryachiy" clothing set.
- Added hand mesh for Zryachiy's ghillie suit.
- Minor refactors to handle clothing/heads/voices not being added to the player customization list.
- Minor change to the version in the package.json, compatible with version 3.5.0 of SPT.

#### v1.4.0 Changelog
- Added new clothing with the EFT 0.13.5 Patch - Kaban's clothing* and new Scav Feldparka
- Added hand mesh for Kaban's clothing
- Added playable head model for Kaban*
- Added two new voices for the Bloodhound characters
- Slight localization changes
- Russian translation now up to date (thanks JustNU!)
- Refactored the config to jsonc - comments are now being used for additional clarity
- Minor change to the version in the package.json, compatible with version 3.7.0 of SPT and above.

* Clipping issues currently exist for Kaban due to his larger model. These issues will be rectified in a later patch, optional variants that better fit the standard playermodel will be included.

#### v1.5.0 Changelog
- Minor code refactor
- Added all new clothing for the EFT 0.14.0 Patch - Kollontay's clothing* and new Scav leather jackets
- Added hand meshes for the new clothing
- Added playable head model for Kollontay*
- Added new voices for Kollontay, Kaban, and Shturman
- New custom-made "Hood Down" variant of Shturman's Parka by me, no more clipping with headgear or certain heads
- Removed "Empty" head for player, this hasn't been needed for awhile after Shturman and Tagilla had custom top meshes
- Russian translation up to date once more (thanks JustNU!)
- Minor change to the version in the package.json, compatible with version 3.8.0 of SPT and above.

* Clipping issues currently exist for Kollontay due to his larger model. These issues will be rectified in a later patch, optional variants that better fit the standard playermodel will be included.

#### v2.0.0 Changelog
- Complete mod rewrite! Everything is refactored, so check out the new config.jsonc file for options
- Added new cultist jacket (replacing the old one, you can re-enable it in the config)
- Added six new tops/bottoms, three for each PMC faction, to Ragman by default
- Minor material improvements to custom-built bundles
- First pass of bundle compression, file size has been reduced substantially
- Minor change to the version in the package.json, compatible with version 3.9.0 of SPT and above

#### v2.0.1 Changelog
- Hotfix for missing custom head entries + Cig Scav head mesh

#### v2.1.0 Changelog
- Refactor to allow clothing to be sold on Fence without any external dependencies
- Added Partisan's head, voice, and clothing
- Added Mannequin Head option, off by default (see config.json)
- Reverted shader for Shturman's Hood Down variant
- Added "Required Achievements" as a possible unlock requirement for clothing
- Minor change to the version in the package.json, compatible with version 3.10.0 of SPT and above

#### v2.1.1 Changelog
- Workaround to mods that add customization entries lacking a '_name' field

#### v2.1.2 Changelog
- Properly fixed the Shturman (Hood Down) variant (for real this time!)

#### v3.0.0 Changelog
- Complete rewrite for SPT 4.0 in C#, fixing several issues and expanding the configuration
	- Please note that clothing can not be found on Fence on SPT version 4.0.0 and 4.0.1, the clothes will instead be found on Ragman on those versions regardless of config settings.
- Updated typing for new EFT database requirements
- Recreated all arm mesh bundles from scratch, with correct shader parameters
- Recompressed bundles to maximize storage space (Filesize shrunk by roughly 60-80% per file)
- Added new custom Zryichiy's Ghillie Suit (Hood Down) variant
- Added all new clothing from EFT Arena Season 0 battlepass
- Removed trades for the initial Arena gear 
	- Added config to replace the vanilla requirement of Ref loyalty levels with Ragman (on by default)
- Added config option to add trades for the prestige-only clothing and allow selecting the prestige voices
- Locales updated to have a basic category for each clothing item

#### v3.0.1 Changelog
- Fixed an OS-dependent issue with loading database and config files

## ---SWAPPING HEADS---

Note: An easier option than editing your profile by hand below is using the following mod: https://forge.sp-tarkov.com/mod/1277/wtt-head-and-voice-selector

If you still want to manually edit your profile.json file, continue reading below.

Make sure your server is shut down!

Open your profile's .json located within `SPT/user/profiles` with any text editor (preferably Notepad ++).

Look for a line starting with `"Head": `, which should be near the top, right below `"Customization": {`.

Swap the ID for the head for any of the following, and save your profile:

`"5cc084dd14c02e000b0550a3"` for: Bear - Vasilyev
`"5fdb7571e4ed5b5ea251e529"` for: Bear - Volkov
`"60a6aaad42fd2735e4589978"` for: Bear - Gavrilov
`"619f94f5b90286142b59d45f"` for: Bear - Kolesnikov
`"62a9e7d15ea3b87d6f642a28"` for: Bear - Danilov

`"5cde96047d6c8b20b577f016"` for: USEC - Taylor
`"5fdb5950f5264a66150d1c6e"` for: USEC - Foreman
`"5fdb4139e4ed5b5ea251e4ed"` for: USEC - Hudson
`"60a6aa8fd559ae040d0d951f"` for: USEC - Mullen
`"62a9e7d15ea3b87d6f642a28"` for: USEC - Grant

`"5fc614290b735e7b024c76e5"` for: SCAV - Drozd
`"5f68c4a7c174a17c0f4c8945"` for: SCAV - Kot
`"5fc614390b735e7b024c76e6"` for: SCAV - Misha
`"5f68c4c217d579077152a252"` for: SCAV - Mozg
`"5fc614130b735e7b024c76e4"` for: SCAV - Sanya
`"5cc2e4d014c02e000d0115f8"` for: SCAV - Samogon
`"5fc613e10b735e7b024c76e3"` for: SCAV - Seva
`"5fc6144b0b735e7b024c76e7"` for: SCAV - Stasik
`"5cde9ff17d6c8b0474535daa"` for: SCAV - Tankist
`"5fc613c80b735e7b024c76e2"` for: SCAV - Yarik
`"5d28afe786f774292668618d"` for: Cig Scav
`"5d28b01486f77429242fc898"` for: Reshala
`"5e99767c86f7741ac7399393"` for: Sanitar
`"5d28b03e86f7747f7e69ab8a"` for: Killa
`"5d5e805d86f77439eb4c2d0e"` for: Glukhar
`"6287b0d239d8207cb27d66c7"` for: Big Pipe
`"628b57d800f171376e7b2634"` for: Birdeye
`"62875ad50828252c7a28b95c"` for: Knight
`"5fb5297a0359a84b77066e56"` for: Cultist Warrior 1
`"5fb52a537b5d1342ee24bd57"` for: Cultist Warrior 2
`"5fb52a3a1c69e5198e234118"` for: Cultist Priest
`"66a25a47f878d663dbe9c302"` for: Tagilla
`"636129784aa74b8fe30ab418"` for: Zryachiy
`"64809e3077c11aeac5078e3c"` for: Kaban
`"654b5ed58558fd71f97254f4"` for: Kollontay
`"66aa5ca23d22c9416c010be0"` for: Partisan


## ---CONTACT---

@RaiRaiTheRaichu - Discord
https://forge.sp-tarkov.com/user/3576/rairaitheraichu

## ---LICENSE---

   Copyright 2026 RaiRaiTheRaichu

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.