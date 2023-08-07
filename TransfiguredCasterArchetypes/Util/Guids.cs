using System;
using System.Linq;
using System.Collections.Generic;
using Kingmaker.Utility;
using TransfiguredCasterArchetypes.Archetypes;
using TransfiguredCasterArchetypes.ClassFeatures;
using TransfiguredCasterArchetypes.Classes;
using TransfiguredCasterArchetypes.Feats;
using TransfiguredCasterArchetypes.Homebrew;
using TransfiguredCasterArchetypes.Weapons;
using System.Data;

namespace TransfiguredCasterArchetypes.Util
{
    /// <summary>List of new Guids.</summary>
    internal static class Guids
    {
        private static readonly Logging.Logger Logger = Logging.GetLogger(nameof(Guids));
        #region Archetypes
        internal const string LivingGrimoireArchetype = "40edee0c-5258-4955-90c1-6dad9ed31ceb";
        internal const string LivingGrimoireCantrips = "b00f7255-3e04-4236-8082-f70763312832";
        internal const string LivingGrimoireSpellbook = "03f7cfc2-2601-4644-80e9-c5d134e116f6";

        internal const string LivingGrimoireHolyBook = "5c904fe3-a669-4469-b337-21918a08fdcb";
        internal const string WeaponFocusHolyBook = "c6a718bf-d499-4fd2-8223-662e9177fea1";

        internal const string LivingGrimoireSacredWord = "6aab1970-2b2f-45b1-be6e-bbdd7395baa3";
        internal const string SacredWordBaseDamageFeature = "35d822cd-a58c-4474-9a1d-d862bcf94e6e";
        internal const string SacredWordWeaponSwitch = "7500aa92-a520-49f2-b674-1fb87e3d2504";

        internal const string SacredWordBuffBase = "79dfc2a9-6832-4c0d-be61-2dd6e575c800";
        internal const string SacredWordBuff1d6 = "71c0586a-8bde-4eb3-87d8-505244c042f4";
        internal const string SacredWordBuff1d8 = "0220b0c9-5c1c-4b44-bbb9-bb2aa40eb6a5";
        internal const string SacredWordBuff1d10 = "b3ce498d-2128-4262-9635-8958b47d5156";
        internal const string SacredWordBuff2d6 = "15bfce87-d2ea-46bd-b89e-a97096e858d8";
        internal const string SacredWordBuff2d8 = "7d5ed577-1d7c-48a3-bd58-206a02b9203e";

        internal const string SacredWordOnBuff = "8d02bdaa-24fe-4bf5-a91c-57080f765b85";
        internal const string SacredWordOnAbility = "ac00a980-6d1f-4f9c-80f4-da99148fba5c";
        internal const string SacredWordEnchantSwitchAbility = "aa8a52c2-bd32-4860-b7f0-20e30dab105b";
        internal const string SacredWordEnchantFeature = "de049b6b-0cf7-463e-ab14-7e49d7e58799";
        internal const string SacredWordEnchantResource = "7daa5558-26b7-4f56-b8ce-c666dbb07a7f";
        internal const string SacredWordEnchantPlus2 = "6fbf5c44-4072-4879-afb0-c159fb9c7b3a";
        internal const string SacredWordEnchantPlus3 = "c74e71fc-1f9a-432e-9fc9-29d129045cde";
        internal const string SacredWordEnchantPlus4 = "a7876ca7-09b6-4789-9b0a-bf1841226e9e";
        internal const string SacredWordEnchantPlus5 = "55a81ea1-177d-4331-8a0d-aa8d9b870d93";

        internal const string LivingGrimoireBlessedScript5 = "b1b045b8-829d-4850-8ecc-e025458074d4";
        internal const string LivingGrimoireBlessedScript8 = "1ddfc107-50be-435d-a833-0b5ed4b76164";
        internal const string LivingGrimoireBlessedScript12 = "9c7c9470-4ec0-47e2-b441-30de3f78d3a8";
        internal const string LivingGrimoireBlessedScript16 = "80e813f9-ddf6-4426-aae4-1d53d6314354";
        internal const string LivingGrimoireWordOfGod = "f2e47e4f-259d-48a6-a743-fb404a4e0fee";
        internal const string LivingGrimoireWordOfGodAbility = "70668bfc-b86b-420e-a9ba-78a8ee61ac15";
        internal const string LivingGrimoireWordOfGodResource = "f68cccbe-a8e0-4453-9aca-05ce932d6c0e";
        internal const string LivingGrimoireWordOfGodBuff = "90257617-02b7-4d6e-98ac-d31cc891d7e1";

        internal const string MysticTheurgeLivingGrimoireLevelUp = "fda9291b-5d43-4863-985c-1e02e8531077";
        internal const string MysticTheurgeLivingGrimoire = "205a451e-bb85-491d-b4d2-7c8eaaa5d42d";
        internal const string HellknightSigniferLivingGrimoire = "7714ce86-1c56-4cbb-a18b-3a31962bde3c";
        internal const string LoremasterLivingGrimoire = "f48c483e-71ea-4e9b-9ddb-629fd63f21d1";

        internal static readonly (string guid, string displayName)[] Archetypes =
            new (string, string)[]
            {
                (LivingGrimoireArchetype, LivingGrimoire.ArchetypeDisplayName),
            };
        #endregion

        #region Classes
        internal const string InvestigatorClass = "6acb0e12-7f1b-4edd-a619-d8438c7f23cf";

        internal static readonly (string guid, string displayName)[] Classes =
            new (string, string)[]
            {

            };
        #endregion

        #region Class Features


        internal static readonly (string guid, string displayName)[] ClassFeatures =
            new (string, string)[]
            {

            };
        #endregion

        #region Feats

        internal static readonly (string guid, string displayName)[] Feats =
            new (string, string)[]
            {

            };
        #endregion

        #region Weapons
        internal const string HolyBookWeapon = "e16ebc96-f796-43ed-91fa-c36fc4391953";
        internal const string MindfulHolyBookWeapon = "dccc986b-4ef8-4932-95f5-8a1ffa160874";

        internal static readonly (string guid, string displayName)[] Weapons =
            new (string, string)[]
            {
                (MindfulHolyBookWeapon, MindfulHolyBook.WeaponName)
            };
        #endregion

        #region WeaponTypes
        internal const string HolyBookWeaponType = "52e35127-abe2-463a-b561-49e03ac5a1e8";

        internal static readonly (string guid, string displayName)[] WeaponTypes =
            new (string, string)[]
            {

            };
        #endregion

        #region Homebrew
        internal const string MindfulEnchantmentHomebrew = "d34a6a50-bf7f-480b-9954-2cc21b8cade8";

        internal static readonly (string guid, string displayName)[] Homebrew =
            new (string, string)[]
            {
                (MindfulEnchantmentHomebrew, MindfulEnchantment.EnchantmentName)
            };
        #endregion

        #region TTT
        internal const string TTTLoremasterSpellbookSelection = "af469dfb-c8c3-424a-b2eb-a33b754c3077";
        #endregion

        #region Dynamic GUIDs
        private const string GUID_0 = "4d802e91-ea0e-41bd-9ebe-1e92263605e8";
        private const string GUID_1 = "6ba9a23f-30af-4e26-9c4a-836cc7431c4f";
        private const string GUID_2 = "3da99391-21de-4c43-b3fa-34fe8dc822bb";
        private const string GUID_3 = "1e4b3c37-9746-48a0-8bdd-4abaf1dd8414";
        private const string GUID_4 = "69ece93c-ac3f-472e-8d50-1b994a1b294f";
        private const string GUID_5 = "00d4365c-dc04-4ca9-a465-5c6e778a8d18";
        private const string GUID_6 = "23534d14-8d85-45f3-bcee-c2c618582983";
        private const string GUID_7 = "187ad65f-5e2e-4c81-aa29-e42f70a4ffc5";
        private const string GUID_8 = "fd85b244-42fc-4d2c-90c9-d438a3797971";
        private const string GUID_9 = "fa0dc9ed-bd43-4f6f-85c1-02f1aaf2ce31";
        private const string GUID_10 = "f08e2782-af70-42ae-a3a8-8d3dc0167687";
        private const string GUID_11 = "50f92e9a-7cef-46b5-88c5-5525bb41df12";
        private const string GUID_12 = "a81e728e-31f5-4e80-9db0-689142d6b310";
        private const string GUID_13 = "89ededba-5096-4f40-bbe3-c9bd1695e5f8";
        private const string GUID_14 = "3553884f-d2f3-4caf-9ed4-68fc147e6315";
        private const string GUID_15 = "1305e710-3836-4fba-a18d-c1c71eb8ff8d";
        private const string GUID_16 = "2e3968fb-9292-4b53-a3b3-1e183699f64d";
        private const string GUID_17 = "76096c62-3f48-4f67-8e0a-51ff57f9f79b";
        private const string GUID_18 = "8de1e388-8a79-409e-922f-822d13b7fff6";
        private const string GUID_19 = "aa816f85-1832-4ba2-82b8-88aa147c941a";

        private static readonly List<string> GUIDS =
            new()
            {
                GUID_0, GUID_1, GUID_2, GUID_3, GUID_4, GUID_5, GUID_6, GUID_7, GUID_8, GUID_9,
                GUID_10, GUID_11, GUID_12, GUID_13, GUID_14, GUID_15, GUID_16, GUID_17, GUID_18, GUID_19
            };

        /// <summary>
        /// Reserves and returns one of the cached GUIDs used for dynamic blueprint generation.
        /// </summary>
        /// 
        /// <remarks>Will generat new GUIDs if all cached GUIDs are reserved.</remarks>
        internal static string ReserveDynamic()
        {
            string guid;
            if (GUIDS.Any())
            {
                guid = GUIDS[0];
                GUIDS.RemoveAt(0);
                Logger.Verbose(() => $"Reserving dynamic guid {guid}");
                return guid;
            }
            guid = Guid.NewGuid().ToString();
            Logger.Verbose(() => $"Generating dynamic guid {guid}");
            return guid;
        }
        #endregion
    }
}