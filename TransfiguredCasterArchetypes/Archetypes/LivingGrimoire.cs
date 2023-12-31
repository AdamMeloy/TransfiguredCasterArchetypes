using System;
using BlueprintCore.Utils;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.Configurators.Classes.Spells;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Enums;
using Kingmaker.EntitySystem.Stats;
using TransfiguredCasterArchetypes.Util;
using CharacterOptionsPlus.Util;
using BlueprintCore.Utils.Types;
using TabletopTweaks.Core.NewComponents;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Settings;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Utility;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.Blueprints.JsonSystem.Converters;
using UnityEngine;
using BlueprintCore.Blueprints.CustomConfigurators;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using BuffConfigurator = BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs.BuffConfigurator;
using Kingmaker.Designers.Mechanics.Facts;
using static Kingmaker.UnitLogic.ActivatableAbilities.ActivatableAbilityResourceLogic;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.FactLogic;
using System.ComponentModel;

namespace TransfiguredCasterArchetypes.Archetypes {
	static class LivingGrimoire {
        internal const string ArchetypeName = "LivingGrimoire";
        internal const string ArchetypeDisplayName = "LivingGrimoire.Name";
        internal const string ArchetypeDescription = "LivingGrimoire.Description";
        internal const string HolyBook = "LivingGrimoire.HolyBook";
        internal const string HolyBookName = "LivingGrimoire.HolyBook.Name";
        internal const string HolyBookDescription = "LivingGrimoire.HolyBook.Description";

        internal const string SacredWord = "LivingGrimoire.SacredWord";
        internal const string SacredWordName = "LivingGrimoire.SacredWord.Name";
        internal const string SacredWordDescription = "LivingGrimoire.SacredWord.Description";
        internal const string SacredWordBuffBase = "LivingGrimoire.SacredWord.Buff";
        internal const string SacredWordBuff1d6 = "LivingGrimoire.SacredWord.Buff.1d6";
        internal const string SacredWordBuff1d8 = "LivingGrimoire.SacredWord.Buff.1d8";
        internal const string SacredWordBuff1d10 = "LivingGrimoire.SacredWord.Buff.1d10";
        internal const string SacredWordBuff2d6 = "LivingGrimoire.SacredWord.Buff.2d6";
        internal const string SacredWordBuff2d8 = "LivingGrimoire.SacredWord.Buff.2d8";
        internal const string SacredWordSwitch = "LivingGrimoire.SacredWordWeaponSwitch";
        internal const string SacredWordBaseDamage = "LivingGrimoire.SacredWordBaseDamage";
        internal const string SacredWordOnBuff = "LivingGrimoire.SacredWordOnBuff";
        internal const string SacredWordOnAbility = "LivingGrimoire.SacredWordOnAbility";

        internal const string SacredWordEnchantSwitch = "LivingGrimoire.SacredWordEnchantSwitch";
        internal const string SacredWordEnchantResource = "LivingGrimoire.SacredWordEnchantResource";
        internal const string SacredWordEnchant = "LivingGrimoire.SacredWordEnchant";
        internal const string SacredWordEnchantDescription = "LivingGrimoire.SacredWordEnchant.Description";
        internal const string SacredWordEnchantPlus2 = "LivingGrimoire.SacredWordEnchantPlus2";
        internal const string SacredWordEnchantPlus2Name = "LivingGrimoire.SacredWordEnchantPlus2.Name";
        internal const string SacredWordEnchantPlus3 = "LivingGrimoire.SacredWordEnchantPlus3";
        internal const string SacredWordEnchantPlus3Name = "LivingGrimoire.SacredWordEnchantPlus3.Name";
        internal const string SacredWordEnchantPlus4 = "LivingGrimoire.SacredWordEnchantPlus4";
        internal const string SacredWordEnchantPlus4Name = "LivingGrimoire.SacredWordEnchantPlus4.Name";
        internal const string SacredWordEnchantPlus5 = "LivingGrimoire.SacredWordEnchantPlus5";
        internal const string SacredWordEnchantPlus5Name = "LivingGrimoire.SacredWordEnchantPlus5.Name";

        internal const string BlessedScriptName = "LivingGrimoire.BlessedScript.Name";
        internal const string BlessedScriptDescription = "LivingGrimoire.BlessedScript.Description";
        internal const string BlessedScript5 = "LivingGrimoire.BlessedScript5";
        internal const string BlessedScript5Table = "LivingGrimoire.BlessedScript5.Table";
        internal const string BlessedScript8 = "LivingGrimoire.BlessedScript8";
        internal const string BlessedScript8Table = "LivingGrimoire.BlessedScript8.Table";
        internal const string BlessedScript12 = "LivingGrimoire.BlessedScript12";
        internal const string BlessedScript12Table = "LivingGrimoire.BlessedScript12.Table";
        internal const string BlessedScript16 = "LivingGrimoire.BlessedScript16";
        internal const string BlessedScript16Table = "LivingGrimoire.BlessedScript16.Table";

        internal const string WordOfGod = "LivingGrimoire.WordOfGod";
        internal const string WordOfGodName = "LivingGrimoire.WordOfGod.Name";
        internal const string WordOfGodDescription = "LivingGrimoire.WordOfGod.Description";
        internal const string WordOfGodAbility = "LivingGrimoire.WordOfGod.Ability";
        internal const string WordOfGodResource = "LivingGrimoire.WordOfGod.Resource";
        internal const string WordOfGodBuff = "LivingGrimoire.WordOfGod.Buff";

        internal const string LivingGrimoireProficiencies = "LivingGrimoire.Proficiencies";
        internal const string LivingGrimoireSpellbook = "LivingGrimoire.Spellbook";
        internal const string LivingGrimoireCantrips = "LivingGrimoire.Orisons";
        internal const string LivingGrimoireCantripsName = "LivingGrimoire.Orisons.Name";
        internal const string LivingGrimoireCantripsDescription = "LivingGrimoire.Orisons.Description";

        internal const string MysticTheurgeLivingGrimoireLevelUp = "LivingGrimoire.MysticTheurge.LevelUp";
        internal const string MysticTheurgeLivingGrimoire = "LivingGrimoire.MysticTheurge";
        internal const string MysticTheurgeLivingGrimoireDescription = "LivingGrimoire.MysticTheurge.Description";
        internal const string HellknightSigniferLivingGrimoire = "LivingGrimoire.HellknightSignifer";
        internal const string HellknightSigniferLivingGrimoireDescription = "LivingGrimoire.HellknightSignifer.Description";
        internal const string LoremasterLivingGrimoire = "LivingGrimoire.Loremaster";
        internal const string LoremasterLivingGrimoireDescription = "LivingGrimoire.Loremaster.Description";

        internal const string WeaponFocusHolyBook = "LivingGrimoire.HolyBook.WeaponFocus";
        internal const string WeaponFocusHolyBookName = "LivingGrimoire.HolyBook.WeaponFocus.Name";
        internal const string WeaponFocusHolyBookDescription = "LivingGrimoire.HolyBook.WeaponFocus.Description";

        internal static readonly Logging.Logger Logger = Logging.GetLogger(ArchetypeName);

        internal const string IconPrefix = "assets/icons/";
        internal const string LivingGrimoireIcon = IconPrefix + "livinggrimoire.png";

        internal static void Configure()
        {
            try
            {
                if (Settings.IsEnabled(Guids.LivingGrimoireArchetype))
                    ConfigureEnabled();
                else
                    ConfigureDisabled();
            }
            catch (Exception e)
            {
                Logger.LogException("LivingGrimoire.Configure", e);
            }
        }

        internal static void ConfigureDelayed()
        {
            try
            {
                if (Settings.IsEnabled(Guids.LivingGrimoireArchetype))
                    ConfigureEnabledDelayed();
            }
            catch (Exception e)
            {
                Logger.LogException("LivingGrimoire.ConfigureDelayed", e);
            }
        }

        private static void ConfigureDisabled()
        {
            Logger.Log($"Configuring {ArchetypeName} (disabled)");

            ArchetypeConfigurator.New(ArchetypeName, Guids.LivingGrimoireArchetype)
                .SetLocalizedName(ArchetypeDisplayName)
                .SetLocalizedDescription(ArchetypeDescription)
                .Configure();

            SpellbookConfigurator.New(LivingGrimoireSpellbook, Guids.LivingGrimoireSpellbook)
                .SetName(ArchetypeDisplayName)
                .Configure();

            FeatureConfigurator.New(LivingGrimoireCantrips, Guids.LivingGrimoireCantrips).Configure();
            FeatureConfigurator.New(HolyBook, Guids.LivingGrimoireHolyBook).Configure();
            FeatureConfigurator.New(SacredWord, Guids.SacredWord).Configure();
            ActivatableAbilityConfigurator.New(SacredWordSwitch, Guids.SacredWordSwitch).Configure();
            FeatureConfigurator.New(SacredWordBaseDamage, Guids.SacredWordBaseDamage).Configure();
            FeatureConfigurator.New(SacredWordEnchant, Guids.SacredWordEnchant).Configure();
            FeatureConfigurator.New(SacredWordEnchantPlus2, Guids.SacredWordEnchantPlus2).Configure();
            FeatureConfigurator.New(SacredWordEnchantPlus3, Guids.SacredWordEnchantPlus3).Configure();
            FeatureConfigurator.New(SacredWordEnchantPlus4, Guids.SacredWordEnchantPlus4).Configure();
            FeatureConfigurator.New(SacredWordEnchantPlus5, Guids.SacredWordEnchantPlus5).Configure();
            FeatureConfigurator.New(BlessedScript5, Guids.BlessedScript5).Configure();
            FeatureConfigurator.New(BlessedScript8, Guids.BlessedScript8).Configure();
            FeatureConfigurator.New(BlessedScript12, Guids.BlessedScript12).Configure();
            FeatureConfigurator.New(BlessedScript16, Guids.BlessedScript16).Configure();
            FeatureConfigurator.New(WordOfGod, Guids.WordOfGod).Configure();
        }

        private static void ConfigureEnabled()
        {
            Logger.Log($"Configuring {ArchetypeName}");

            var archetype = ArchetypeConfigurator.New(ArchetypeName, Guids.LivingGrimoireArchetype, CharacterClassRefs.InquisitorClass)
                .SetLocalizedName(ArchetypeDisplayName)
                .SetLocalizedDescription(ArchetypeDescription)
                .SetReplaceSpellbook(CreateSpellbook())
                .SetRecommendedAttributes(StatType.Intelligence)
                .SetNotRecommendedAttributes(StatType.Wisdom, StatType.Charisma)
                .SetOverrideAttributeRecommendations(true)

                .SetStartingGold(411)
                .SetStartingItems(
                    BlueprintTool.GetRef<BlueprintItemReference>(ItemWeaponRefs.StandardLightCrossbow.ToString()),
                    BlueprintTool.GetRef<BlueprintItemReference>(ItemEquipmentUsableRefs.ColdIronArrowsQuiverItem.ToString()),
                    BlueprintTool.GetRef<BlueprintItemReference>(ItemArmorRefs.ScalemailStandard.ToString()),
                    BlueprintTool.GetRef<BlueprintItemReference>(ItemShieldRefs.LightShield.ToString()),
                    BlueprintTool.GetRef<BlueprintItemReference>(ItemEquipmentUsableRefs.ScrollOfShieldOfFaith.ToString())
                )
                .SetReplaceStartingEquipment(true);

            archetype
                .AddToRemoveFeatures(level: 1, FeatureRefs.InquisitorOrisonsFeature.ToString(), FeatureRefs.JudgmentFeature.ToString())
                .AddToRemoveFeatures(level: 2, FeatureRefs.CunningInitiative.ToString())
                .AddToRemoveFeatures(level: 4, FeatureRefs.JudgmentAdditionalUse.ToString())
                .AddToRemoveFeatures(level: 5, FeatureRefs.InquisitorBaneNormalFeatureAdd.ToString())
                .AddToRemoveFeatures(level: 7, FeatureRefs.JudgmentAdditionalUse.ToString())
                .AddToRemoveFeatures(level: 8, FeatureRefs.SecondJudgment.ToString())
                .AddToRemoveFeatures(level: 10, FeatureRefs.JudgmentAdditionalUse.ToString())
                .AddToRemoveFeatures(level: 12, FeatureRefs.InquisitorBaneGreaterFeature.ToString())
                .AddToRemoveFeatures(level: 13, FeatureRefs.JudgmentAdditionalUse.ToString())
                .AddToRemoveFeatures(level: 16, FeatureRefs.JudgmentAdditionalUse.ToString(), FeatureRefs.ThirdJudgment.ToString())
                .AddToRemoveFeatures(level: 19, FeatureRefs.JudgmentAdditionalUse.ToString())
                .AddToRemoveFeatures(level: 20, FeatureRefs.TrueJudgmentFeature.ToString());

            archetype
                .AddToAddFeatures(level: 1, CreateCantrips(), CreateHolyBook())
                .AddToAddFeatures(level: 2, CreateSacredWord())
                .AddToAddFeatures(level: 4, CreateSacredWordEnchant())
                .AddToAddFeatures(level: 5, CreateBlessedScript5())
                .AddToAddFeatures(level: 8, CreateSacredWordPlus2(), CreateBlessedScript8())
                .AddToAddFeatures(level: 12, CreateSacredWordPlus3(), CreateBlessedScript12())
                .AddToAddFeatures(level: 16, CreateSacredWordPlus4(), CreateBlessedScript16())
                .AddToAddFeatures(level: 20, CreateWordOfGod(), CreateSacredWordPlus5());

            archetype.Configure();
        }

        private static void ConfigureEnabledDelayed()
        {
            if (!Settings.IsTTTBaseEnabled())
                return;

            Logger.Log("Patching TTT Loremaster compatbility for Living Grimoire");
            FeatureSelectionConfigurator.For(Guids.TTTLoremasterSpellbookSelection)
                .AddToAllFeatures(Guids.LivingGrimoireLoremaster)
                .SkipAddToSelections()
                .Configure();
        }

        #region Spells
        private static BlueprintSpellbook CreateSpellbook()
        {
            Logger.Log("Creating Living Grimoire Spellbook");
            var spellbook = SpellbookConfigurator.New(LivingGrimoireSpellbook, Guids.LivingGrimoireSpellbook)
                .CopyFrom(SpellbookRefs.InquisitorSpellbook, typeof(NameModifier))
                .SetIsMythic(false)
                .SetSpellsKnown(null)
                .SetSpellList(SpellListRefs.InquisitorSpellList.ToString())
                .SetCharacterClass(CharacterClassRefs.InquisitorClass.ToString())
                .SetCastingAttribute(StatType.Intelligence)
                .SetSpontaneous(false)
                .SetSpellsPerLevel(2)
                .SetAllSpellsKnown(false)
                .SetCantripsType(CantripsType.Orisions)
                .SetCasterLevelModifier(0)
                .SetCanCopyScrolls(true)
                .SetIsArcane(false)
                .SetIsArcanist(false)
                .SetHasSpecialSpellList(false)
                .Configure();

            var inquisitor = CharacterClassRefs.InquisitorClass.ToString();

            Logger.Log("Living Grimoire Mystic Theurge Support");
            Logger.Log("Restricting Base Class and other Archetypes from using LG's Progression");
            ProgressionConfigurator.For(ProgressionRefs.MysticTheurgeInquisitorProgression)
                .AddPrerequisiteNoArchetype
                (
                    characterClass: CharacterClassRefs.InquisitorClass.ToString(),
                    archetype: Guids.LivingGrimoireArchetype
                )
                .Configure();

            Logger.Log("Creating LG's Mystic Theurge LevelUp Feature");
            var mtLGL = FeatureConfigurator.New(MysticTheurgeLivingGrimoireLevelUp, Guids.LivingGrimoireMysticTheurgeLevelUp)
                .AddSpellbookLevel(spellbook)
                .SetDisplayName(ArchetypeDisplayName)
                .SetDescription(MysticTheurgeLivingGrimoireDescription)
                .SetHideInUI(true)
                .SetHideInCharacterSheetAndLevelUp(false)
                .SetHideNotAvailibleInUI(false)
                .SetRanks(10)
                .SetReapplyOnLevelUp(false)
                .SetIsClassFeature(true)
                .AddComponent(new AdditionalSpellSelection())
                .Configure();
            var mtLGLS = mtLGL.ToString();

            Logger.Log("Creating LG's Mystic Theurge Spellbook Progression");
            var mtLG = ProgressionConfigurator.New(MysticTheurgeLivingGrimoire, Guids.LivingGrimoireMysticTheurge)
                .AddPrerequisiteClassSpellLevel
                (
                    characterClass: CharacterClassRefs.InquisitorClass.ToString(),
                    requiredSpellLevel: 2
                )
                .AddMysticTheurgeSpellbook
                (
                    characterClass: CharacterClassRefs.InquisitorClass.ToString(),
                    mysticTheurge: CharacterClassRefs.MysticTheurgeClass.ToString()
                )
                .SetDisplayName(ArchetypeDisplayName)
                .SetDescription(MysticTheurgeLivingGrimoireDescription)
                .SetHideInUI(true)
                .SetHideInCharacterSheetAndLevelUp(false)
                .SetHideNotAvailibleInUI(true)
                .SetGroups(FeatureGroup.MysticTheurgeDivineSpellbook)
                .SetRanks(1)
                .SetReapplyOnLevelUp(false)
                .SetIsClassFeature(true)
                .SetClasses
                (
                    (
                        CharacterClassRefs.MysticTheurgeClass.ToString(),
                        0
                    ),
                    (
                        CharacterClassRefs.InquisitorClass.ToString(),
                        0
                    )
                )
                .SetArchetypes(Guids.LivingGrimoireArchetype.ToString())
                .SetForAllOtherClasses(false)
                .SetLevelEntries(
                    LevelEntryBuilder.New()
                        .AddEntry(0, mtLGLS)
                        .AddEntry(1, mtLGLS)
                        .AddEntry(2, mtLGLS)
                        .AddEntry(3, mtLGLS)
                        .AddEntry(4, mtLGLS)
                        .AddEntry(5, mtLGLS)
                        .AddEntry(6, mtLGLS)
                        .AddEntry(7, mtLGLS)
                        .AddEntry(8, mtLGLS)
                        .AddEntry(9, mtLGLS)
                        .AddEntry(10, mtLGLS)
                        .AddEntry(11, mtLGLS)
                        .AddEntry(12, mtLGLS)
                        .AddEntry(13, mtLGLS)
                        .AddEntry(14, mtLGLS)
                        .AddEntry(15, mtLGLS)
                )
                .SetExclusiveProgression(CharacterClassRefs.MysticTheurgeClass.ToString())
                .SetGiveFeaturesForPreviousLevels(false)
                .Configure();

            Logger.Log("Adding LG Spellbook to Mystic Theurge Spellbook Selection");
            FeatureSelectionConfigurator.For(FeatureSelectionRefs.MysticTheurgeDivineSpellbookSelection.ToString())
                .AddToAllFeatures(mtLG)
                .SkipAddToSelections()
                .Configure();

            Logger.Log("Living Grimoire Hellknight Signifer Support");
            Logger.Log("Restricting Base Class and other Archetypes from using LG's Progression");
            ProgressionConfigurator.For(ProgressionRefs.HellknightSigniferInquisitorProgression)
                .AddPrerequisiteNoArchetype
                (
                    characterClass: inquisitor,
                    archetype: Guids.LivingGrimoireArchetype
                )
                .Configure();

            Logger.Log("Creating LG's Hellknight Signifer Spellbook Progression");
            var replacementHS = FeatureReplaceSpellbookConfigurator.New(HellknightSigniferLivingGrimoire, Guids.LivingGrimoireHellknightSignifer)
                .AddPrerequisiteClassSpellLevel
                (
                    characterClass: inquisitor,
                    requiredSpellLevel: 3
                )
                .AddPrerequisiteArchetypeLevel
                (
                    characterClass: inquisitor,
                    archetype: Guids.LivingGrimoireArchetype
                )
                .SetDisplayName(ArchetypeDisplayName)
                .SetDescription(HellknightSigniferLivingGrimoireDescription)
                .SetHideInUI(true)
                .SetHideInCharacterSheetAndLevelUp(false)
                .SetHideNotAvailibleInUI(true)
                .SetGroups(FeatureGroup.HellknightSigniferSpellbook)
                .SetRanks(1)
                .SetReapplyOnLevelUp(false)
                .SetIsClassFeature(true)
                .SetSpellbook(spellbook)
                .Configure();

            Logger.Log("Adding LG Spellbook to Hellknight Signifer Spellbook");
            FeatureSelectionConfigurator.For(FeatureSelectionRefs.HellknightSigniferSpellbook.ToString())
                .AddToAllFeatures(replacementHS)
                .SkipAddToSelections()
                .Configure();

            Logger.Log("Living Grimoire Loremaster Support");
            Logger.Log("Restricting Base Class and other Archetypes from using LG's Progression");
            ProgressionConfigurator.For(ProgressionRefs.LoremasterInquisitorProgression)
                .AddPrerequisiteNoArchetype
                (
                    characterClass: inquisitor,
                    archetype: Guids.LivingGrimoireArchetype
                )
                .Configure();

            Logger.Log("Creating LG's Loremaster Spellbook Progression");
            var replacementLM = FeatureReplaceSpellbookConfigurator.New(LoremasterLivingGrimoire, Guids.LivingGrimoireLoremaster)
                .AddPrerequisiteClassSpellLevel
                (
                    characterClass: inquisitor,
                    requiredSpellLevel: 3
                )
                .AddPrerequisiteArchetypeLevel
                (
                    characterClass: inquisitor,
                    archetype: Guids.LivingGrimoireArchetype
                )
                .SetDisplayName(ArchetypeDisplayName)
                .SetDescription(LoremasterLivingGrimoireDescription)
                .SetHideInUI(true)
                .SetHideInCharacterSheetAndLevelUp(false)
                .SetHideNotAvailibleInUI(true)
                .SetGroups(FeatureGroup.MythicAdditionalProgressions)
                .SetRanks(1)
                .SetReapplyOnLevelUp(false)
                .SetIsClassFeature(true)
                .SetSpellbook(spellbook)
                .Configure();

            Logger.Log("Adding LG Spellbook to Loremaster Spellbook Selection");
            FeatureSelectionConfigurator.For(FeatureSelectionRefs.LoremasterSpellbookSelection.ToString())
                .AddToAllFeatures(replacementLM)
                .SkipAddToSelections()
                .Configure();

            Common.AddToLoremasterSecrets
            (
                Guids.LivingGrimoireLoremaster,
                ParametrizedFeatureRefs.LoremasterClericSecretInquisitor.ToString(),
                ParametrizedFeatureRefs.LoremasterDruidSecretInquisitor.ToString(),
                ParametrizedFeatureRefs.LoremasterWizardSecretInquisitor.ToString()
            );

            return spellbook;
        }

        private static BlueprintFeature CreateCantrips()
        {
            Logger.Log("Creating Living Grimoire Cantrips Feature");
            var cantrips = FeatureConfigurator.New(LivingGrimoireCantrips, Guids.LivingGrimoireCantrips)
                .CopyFrom(FeatureRefs.InquisitorOrisonsFeature, typeof(LearnSpells), typeof(BindAbilitiesToClass))
                .SetDisplayName(LivingGrimoireCantripsName)
                .AddBindAbilitiesToClass(stat: StatType.Intelligence)
                .AddFacts
                (
                    new()
                    {
                        AbilityRefs.AcidSplash.ToString(),
                        AbilityRefs.Daze.ToString(),
                        AbilityRefs.DismissAreaEffect.ToString(),
                        AbilityRefs.DisruptUndead.ToString(),
                        AbilityRefs.DivineZap.ToString(),
                        AbilityRefs.Guidance.ToString(),
                        AbilityRefs.MageLight.ToString(),
                        AbilityRefs.Resistance.ToString(),
                        AbilityRefs.Stabilize.ToString(),
                        AbilityRefs.Virtue.ToString()
                    }
                )
                .SetIsClassFeature(true)
                .SetHideInUI(false)
                .SetHideInCharacterSheetAndLevelUp(false)
                .SetHideNotAvailibleInUI(false)
                .SetRanks(1)
                .SetReapplyOnLevelUp(false)
                .SetIsClassFeature(true)
                .Configure();

            return cantrips;
        }
        #endregion
        
        private static BlueprintFeature CreateHolyBook()
        {
            Logger.Log("Creating Weapon Focus (Holy Book)");
            FeatureConfigurator.New(WeaponFocusHolyBook, Guids.WeaponFocusHolyBook)
                .AddWeaponFocus
                    (
                        weaponType: Guids.HolyBookWeaponType,
                        attackBonus: 1,
                        descriptor: ModifierDescriptor.UntypedStackable
                    )
                .SetAllowNonContextActions(false)
                .SetDisplayName(WeaponFocusHolyBookName)
                .SetDescription(WeaponFocusHolyBookDescription)
                .SetIcon((Sprite)UnityObjectConverter.AssetList.Get("51f2bd71d233b374a9c63717b08581a6", 21300000))  //use weapon focus icon
                .SetHideInUI(false)
                .SetHideInCharacterSheetAndLevelUp(false)
                .SetHideNotAvailibleInUI(false)
                .SetRanks(1)
                .SetReapplyOnLevelUp(false)
                .SetIsClassFeature(true)
                .SetGroups(FeatureGroup.Feat, FeatureGroup.CombatFeat)
                .SkipAddToSelections()
                .Configure();

            string WeaponGuid = Guids.HolyBookWeapon;
            if (Settings.IsEnabled(Guids.MindfulEnchantmentHomebrew))
                if (Settings.IsEnabled(Guids.MindfulHolyBookWeapon))
                {
                    WeaponGuid = Guids.MindfulHolyBookWeapon;
                    Logger.Log("Using Holy Book (Mindful)");
                }
                else
                    Logger.Log("Using Holy Book (Base)");
            else
                Logger.Log("Using Holy Book (Base)");

            Logger.Log("Creating Holy Book Feature");
            return FeatureConfigurator.New(HolyBook, Guids.LivingGrimoireHolyBook)
                .SetDisplayName(HolyBookName)
                .SetDescription(HolyBookDescription)
                .AddStartingEquipment(new() { BlueprintTool.GetRef<BlueprintItemReference>(WeaponGuid) })
                .SetIsClassFeature(true)
                .AddFeatureOnApply(Guids.WeaponFocusHolyBook)
                .Configure();
        }

        #region Sacred Word
        private static BlueprintFeature CreateSacredWord()
        {
            Logger.Log("Creating Sacred Word Damage Buffs");
            Blueprints.BuffConfigurator.New(SacredWordBuff1d6, Guids.SacredWordBuff1d6)
                .AddSacredWordDamageOverride
                (
                    feature: Guids.WeaponFocusHolyBook,
                    formula: new DiceFormula(1, DiceType.D6)
                )
                .SetAllowNonContextActions(false)
                .SetIsClassFeature(true)
                .SetFlags(BlueprintBuff.Flags.HiddenInUi, BlueprintBuff.Flags.StayOnDeath)
                .SetStacking(StackingType.Replace)
                .SetRanks(0)
                .SetTickEachSecond(false)
                .SetFrequency(DurationRate.Rounds)
                .SetDisplayName(SacredWordName)
                .Configure();

            Blueprints.BuffConfigurator.New(SacredWordBuff1d8, Guids.SacredWordBuff1d8)
                .AddSacredWordDamageOverride
                (
                    feature: Guids.WeaponFocusHolyBook,
                    formula: new DiceFormula(1, DiceType.D8)
                )
                .SetAllowNonContextActions(false)
                .SetIsClassFeature(true)
                .SetFlags(BlueprintBuff.Flags.HiddenInUi, BlueprintBuff.Flags.StayOnDeath)
                .SetStacking(StackingType.Replace)
                .SetRanks(0)
                .SetTickEachSecond(false)
                .SetFrequency(DurationRate.Rounds)
                .SetDisplayName(SacredWordName)
                .Configure();

            Blueprints.BuffConfigurator.New(SacredWordBuff1d10, Guids.SacredWordBuff1d10)
                .AddSacredWordDamageOverride
                (
                    feature: Guids.WeaponFocusHolyBook,
                    formula: new DiceFormula(1, DiceType.D10)
                )
                .SetAllowNonContextActions(false)
                .SetIsClassFeature(true)
                .SetFlags(BlueprintBuff.Flags.HiddenInUi, BlueprintBuff.Flags.StayOnDeath)
                .SetStacking(StackingType.Replace)
                .SetRanks(0)
                .SetTickEachSecond(false)
                .SetFrequency(DurationRate.Rounds)
                .SetDisplayName(SacredWordName)
                .Configure();

            Blueprints.BuffConfigurator.New(SacredWordBuff2d6, Guids.SacredWordBuff2d6)
                .AddSacredWordDamageOverride
                (
                    feature: Guids.WeaponFocusHolyBook,
                    formula: new DiceFormula(2, DiceType.D6)
                )
                .SetAllowNonContextActions(false)
                .SetIsClassFeature(true)
                .SetFlags(BlueprintBuff.Flags.HiddenInUi, BlueprintBuff.Flags.StayOnDeath)
                .SetStacking(StackingType.Replace)
                .SetRanks(0)
                .SetTickEachSecond(false)
                .SetFrequency(DurationRate.Rounds)
                .SetDisplayName(SacredWordName)
                .Configure();

            Blueprints.BuffConfigurator.New(SacredWordBuff2d8, Guids.SacredWordBuff2d8)
                .AddSacredWordDamageOverride
                (
                    feature: Guids.WeaponFocusHolyBook,
                    formula: new DiceFormula(2, DiceType.D8)
                )
                .SetAllowNonContextActions(false)
                .SetIsClassFeature(true)
                .SetFlags(BlueprintBuff.Flags.HiddenInUi, BlueprintBuff.Flags.StayOnDeath)
                .SetStacking(StackingType.Replace)
                .SetRanks(0)
                .SetTickEachSecond(false)
                .SetFrequency(DurationRate.Rounds)
                .SetDisplayName(SacredWordName)
                .Configure();

            Logger.Log("Creating Sacred Word Buff Base");
            BuffConfigurator.New(SacredWordBuffBase, Guids.SacredWordBuffBase)
                .CopyFrom(BuffRefs.WarpriestSacredWeaponBuffBase, typeof(ContextCalculateSharedValue))
                .AddContextRankConfig
                (
                    ContextRankConfigs.ClassLevel
                    (
                        classes: new[] { CharacterClassRefs.InquisitorClass.ToString() },
                        excludeClasses: false,
                        type: default,
                        max: 20,
                        min: 0
                    )
                )
                .AddFactContextActions
                (
                    activated: ActionsBuilder.New()
                    .Conditional
                    (
                        conditions: ConditionsBuilder.New()
                        .AddOrAndLogic
                        (
                            ConditionsBuilder.New()
                            .SharedValueHigher
                            (
                                negate: true,
                                sharedValue: AbilitySharedValue.Damage,
                                higherOrEqual: 5,
                                inverted: false
                            )
                        ),
                        ifTrue: ActionsBuilder.New()
                        .ApplyBuffPermanent
                        (
                            buff: Guids.SacredWordBuff1d6,
                            isFromSpell: false,
                            isNotDispelable: false,
                            toCaster: false,
                            asChild: true,
                            sameDuration: false,
                            ignoreParentContext: default,
                            notLinkToAreaEffect: default
                        ),
                        ifFalse: ActionsBuilder.New()
                        .Conditional
                        (
                            conditions: ConditionsBuilder.New()
                            .AddOrAndLogic
                            (
                                ConditionsBuilder.New()
                                .SharedValueHigher
                                (
                                    negate: true,
                                    sharedValue: AbilitySharedValue.Damage,
                                    higherOrEqual: 10,
                                    inverted: false
                                )
                            ),
                            ifTrue: ActionsBuilder.New()
                            .ApplyBuffPermanent
                            (
                                buff: Guids.SacredWordBuff1d8,
                                isFromSpell: false,
                                isNotDispelable: false,
                                toCaster: false,
                                asChild: true,
                                sameDuration: false,
                                ignoreParentContext: default,
                                notLinkToAreaEffect: default
                            ),
                            ifFalse: ActionsBuilder.New()
                            .Conditional
                            (
                                conditions: ConditionsBuilder.New()
                                .AddOrAndLogic
                                (
                                    ConditionsBuilder.New()
                                    .SharedValueHigher
                                    (
                                        negate: true,
                                        sharedValue: AbilitySharedValue.Damage,
                                        higherOrEqual: 15,
                                        inverted: false
                                    )
                                ),
                                ifTrue: ActionsBuilder.New()
                                .ApplyBuffPermanent
                                (
                                    buff: Guids.SacredWordBuff1d10,
                                    isFromSpell: false,
                                    isNotDispelable: false,
                                    toCaster: false,
                                    asChild: true,
                                    sameDuration: false,
                                    ignoreParentContext: default,
                                    notLinkToAreaEffect: default
                                ),
                                ifFalse: ActionsBuilder.New()
                                .Conditional
                                (
                                    conditions: ConditionsBuilder.New()
                                    .AddOrAndLogic
                                    (
                                        ConditionsBuilder.New()
                                        .SharedValueHigher
                                        (
                                            negate: true,
                                            sharedValue: AbilitySharedValue.Damage,
                                            higherOrEqual: 20,
                                            inverted: false
                                        )
                                    ),
                                    ifTrue: ActionsBuilder.New()
                                    .ApplyBuffPermanent
                                    (
                                        buff: Guids.SacredWordBuff2d6,
                                        isFromSpell: false,
                                        isNotDispelable: false,
                                        toCaster: false,
                                        asChild: true,
                                        sameDuration: false,
                                        ignoreParentContext: default,
                                        notLinkToAreaEffect: default
                                    ),
                                    ifFalse: ActionsBuilder.New()
                                    .ApplyBuffPermanent
                                    (
                                        buff: Guids.SacredWordBuff2d8,
                                        isFromSpell: false,
                                        isNotDispelable: false,
                                        toCaster: false,
                                        asChild: true,
                                        sameDuration: false,
                                        ignoreParentContext: default,
                                        notLinkToAreaEffect: default
                                    )
                                )
                            )
                        )
                    )
                )
                .SetDisplayName(SacredWordName)
                .AddSacredWeaponFavoriteDamageOverride()
                .Configure();

            Logger.Log("Creating Sacred Word Ability");
            ActivatableAbilityConfigurator.New(SacredWordSwitch, Guids.SacredWordSwitch)
                .SetDisplayName(SacredWordName)
                .SetDescription(SacredWordDescription)
                .SetBuff(Guids.SacredWordBuffBase)
                .SetGroup(ActivatableAbilityGroup.None)
                .SetWeightInGroup(1)
                .SetIsOnByDefault(false)
                .SetDeactivateIfCombatEnded(false)
                .SetDeactivateAfterFirstRound(false)
                .SetDeactivateImmediately(true)
                .SetIsTargeted(false)
                .SetDeactivateIfOwnerDisabled(false)
                .SetDeactivateIfOwnerUnconscious(false)
                .SetOnlyInCombat(false)
                .SetDoNotTurnOffOnRest(false)
                .SetActivationType(AbilityActivationType.Immediately)
                .SetActivateWithUnitCommand(UnitCommand.CommandType.Free)
                .SetActivateOnUnitAction(AbilityActivateOnUnitActionType.Attack)
                .SetIcon((Sprite)UnityObjectConverter.AssetList.Get("0556cc4f679127b4fb4115a1e52cf3ea", 21300000))  //use sacred weapon icon
                .Configure();

            Logger.Log("Creating Sacred Word Base Damage Feature");
            return FeatureConfigurator.New(SacredWordBaseDamage, Guids.SacredWordBaseDamage)
                .AddFacts
                (
                    facts: new()
                    {
                        Guids.SacredWordSwitch.ToString()
                    }
                )
                .SetDisplayName(SacredWordName)
                .SetDescription(SacredWordDescription)
                .SetHideInUI(false)
                .SetHideInCharacterSheetAndLevelUp(true)
                .SetHideNotAvailibleInUI(false)
                .SetRanks(1)
                .SetReapplyOnLevelUp(false)
                .SetIsClassFeature(true)
                .Configure();
        }

        private static BlueprintFeature CreateSacredWordEnchant()
        {
            Logger.Log("Creating Sacred Word Enchant Resource");
            AbilityResourceConfigurator.New(SacredWordEnchantResource, Guids.SacredWordEnchantResource)
                .SetMaxAmount
                (
                    ResourceAmountBuilder.New(0)
                    .IncreaseByLevel
                    (
                        classes: new[] { CharacterClassRefs.InquisitorClass.ToString() },
                        bonusPerLevel: 1
                    )
                )
                .SetUseMax(false)
                .SetMax(20)
                .Configure();

            Logger.Log("Creating Sacred Word Enchant Buff");
            BuffConfigurator.New(SacredWordOnBuff, Guids.SacredWordOnBuff)
                .AddFactContextActions
                (
                    activated: ActionsBuilder.New().CastSpell(Guids.SacredWordEnchantSwitch),
                    newRound: ActionsBuilder.New().CastSpell(Guids.SacredWordEnchantSwitch)
                )
                .SetIsClassFeature(false)
                .SetFlags
                (
                    flags: new[]
                    {
                        BlueprintBuff.Flags.HiddenInUi,
                        BlueprintBuff.Flags.StayOnDeath
                    }
                )
                .SetStacking(StackingType.Replace)
                .SetRanks(0)
                .SetTickEachSecond(false)
                .SetFrequency(DurationRate.Rounds)
                .Configure();

            Logger.Log("Creating Sacred Word Enchant Activatable Ability");
            ActivatableAbilityConfigurator.New(SacredWordOnAbility, Guids.SacredWordOnAbility)
                .SetDisplayName(SacredWordName)
                .SetDescription(SacredWordEnchantDescription)
                .AddActivatableAbilityResourceLogic
                (
                    spendType: ResourceSpendType.NewRound,
                    requiredResource: Guids.SacredWordEnchantResource
                )
                .SetBuff(Guids.SacredWordOnBuff)
                .SetGroup(ActivatableAbilityGroup.None)
                .SetWeightInGroup(1)
                .SetIsOnByDefault(false)
                .SetDeactivateIfCombatEnded(true)
                .SetDeactivateAfterFirstRound(false)
                .SetDeactivateImmediately(true)
                .SetIsTargeted(false)
                .SetDeactivateIfOwnerDisabled(false)
                .SetDeactivateIfOwnerUnconscious(true)
                .SetOnlyInCombat(false)
                .SetDoNotTurnOffOnRest(false)
                .SetActivationType(AbilityActivationType.WithUnitCommand)
                .SetActivateWithUnitCommand(UnitCommand.CommandType.Swift)
                .SetActivateOnUnitAction(AbilityActivateOnUnitActionType.Attack)
                .SetIcon((Sprite)UnityObjectConverter.AssetList.Get("ce0962e7437e5c448b4b4ea2dd40e4a0", 21300000))  //use sacred weapon icon
                .Configure();

            Logger.Log("Creating Sacred Word Enchant Ability");
            AbilityConfigurator.New(SacredWordEnchantSwitch, Guids.SacredWordEnchantSwitch)
                .CopyFrom(AbilityRefs.SacredWeaponEnchantSwitchAbility, typeof(AbilityEffectRunAction), typeof(DisplayNameAttribute), typeof(DescriptionAttribute))
                .SetAllowNonContextActions(false)
                .SetType(AbilityType.Supernatural)
                .SetRange(AbilityRange.Personal)
                .SetShowNameForVariant(false)
                .SetOnlyForAllyCaster(false)
                .SetCanTargetPoint(false)
                .SetCanTargetEnemies(false)
                .SetCanTargetFriends(true)
                .SetCanTargetSelf(true)
                .SetSpellResistance(false)
                .SetActionBarAutoFillIgnored(false)
                .SetHidden(true)
                .SetNeedEquipWeapons(false)
                .SetNotOffensive(false)
                .SetEffectOnAlly(AbilityEffectOnUnit.Helpful)
                .SetEffectOnEnemy(AbilityEffectOnUnit.None)
                .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
                .SetHasFastAnimation(false)
                .SetTargetMapObjects(false)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetAvailableMetamagic
                (
                    availableMetamagic: new[]
                    {
                        Metamagic.Extend,
                        Metamagic.Heighten
                    }
                )
                .SetIsFullRoundAction(false)
                .SetDisableLog(true)
                .SetIcon((Sprite)UnityObjectConverter.AssetList.Get("7fa6bad9b8b287e40bb6b813ebc5799b", 21300000))  //use sacred weapon icon
                .Configure();

            Logger.Log("Creating Sacred Word Enchant Feature");
            return FeatureConfigurator.New(SacredWordEnchant, Guids.SacredWordEnchant)
                .SetDisplayName(SacredWordName)
                .SetDescription(SacredWordEnchantDescription)
                .AddFacts
                (
                    facts: new()
                    {
                        Guids.SacredWordEnchantSwitch,
                        Guids.SacredWordOnAbility,
                        ActivatableAbilityRefs.SacredWeaponEnchantFlamingChoice.ToString(),
                        ActivatableAbilityRefs.SacredWeaponEnchantFrostChoice.ToString(),
                        ActivatableAbilityRefs.SacredWeaponEnchantShockChoice.ToString(),
                        ActivatableAbilityRefs.SacredWeaponEnchantKeenChoice.ToString()
                    },
                    casterLevel: 0,
                    doNotRestoreMissingFacts: false,
                    hasDifficultyRequirements: false,
                    invertDifficultyRequirements: false,
                    minDifficulty: GameDifficultyOption.Story
                )
                .AddAbilityResources
                (
                    useThisAsResource: false,
                    resource: Guids.SacredWordEnchantResource,
                    amount: 0,
                    restoreAmount: true,
                    restoreOnLevelUp:false
                )
                .SetHideInUI(false)
                .SetHideInCharacterSheetAndLevelUp(false)
                .SetHideNotAvailibleInUI(false)
                .SetRanks(1)
                .SetReapplyOnLevelUp(false)
                .SetIsClassFeature(true)
                .Configure();
        }
        
        private static BlueprintFeature CreateSacredWordPlus2()
        {
            Logger.Log("Creating Sacred Word Enchant +2");
            return FeatureConfigurator.New(SacredWordEnchantPlus2, Guids.SacredWordEnchantPlus2)
                .SetDisplayName(SacredWordEnchantPlus2Name)
                .SetDescription(SacredWordEnchantDescription)
                .AddIncreaseActivatableAbilityGroupSize(ActivatableAbilityGroup.SacredWeaponProperty)
                .AddFacts
                (
                    facts: new()
                    {
                        ActivatableAbilityRefs.SacredWeaponEnchantAnarchicChoice.ToString(),
                        ActivatableAbilityRefs.SacredWeaponEnchantAxiomaticChoice.ToString(),
                        ActivatableAbilityRefs.SacredWeaponEnchantHolyChoice.ToString(),
                        ActivatableAbilityRefs.SacredWeaponEnchantUnholyChoice.ToString()
                    },
                    casterLevel: 0,
                    doNotRestoreMissingFacts: false,
                    hasDifficultyRequirements: false,
                    invertDifficultyRequirements: false,
                    minDifficulty: GameDifficultyOption.Story
                )
                .SetHideInCharacterSheetAndLevelUp(false)
                .SetHideNotAvailibleInUI(false)
                .SetRanks(1)
                .SetReapplyOnLevelUp(false)
                .SetIsClassFeature(true)
                .Configure();
        }

        private static BlueprintFeature CreateSacredWordPlus3()
        {
            Logger.Log("Creating Sacred Word Enchant +3");
            return FeatureConfigurator.New(SacredWordEnchantPlus3, Guids.SacredWordEnchantPlus3)
                .SetDisplayName(SacredWordEnchantPlus3Name)
                .SetDescription(SacredWordEnchantDescription)
                .SetHideInCharacterSheetAndLevelUp(false)
                .SetHideNotAvailibleInUI(false)
                .SetRanks(1)
                .SetReapplyOnLevelUp(false)
                .SetIsClassFeature(true)
                .Configure();
        }

        private static BlueprintFeature CreateSacredWordPlus4()
        {
            Logger.Log("Creating Sacred Word Enchant +4");
            return FeatureConfigurator.New(SacredWordEnchantPlus4, Guids.SacredWordEnchantPlus4)
                .SetDisplayName(SacredWordEnchantPlus4Name)
                .SetDescription(SacredWordEnchantDescription)
                .SetHideInCharacterSheetAndLevelUp(false)
                .SetHideNotAvailibleInUI(false)
                .SetRanks(1)
                .SetReapplyOnLevelUp(false)
                .SetIsClassFeature(true)
                .Configure();
        }

        private static BlueprintFeature CreateSacredWordPlus5()
        {
            Logger.Log("Creating Sacred Word Enchant +5");
            return FeatureConfigurator.New(SacredWordEnchantPlus5, Guids.SacredWordEnchantPlus5)
                .SetDisplayName(SacredWordEnchantPlus5Name)
                .SetDescription(SacredWordEnchantDescription)
                .AddFacts
                (
                    facts: new()
                    {
                        ActivatableAbilityRefs.SacredWeaponEnchantBrilliantEnergyChoice.ToString()
                    },
                    casterLevel: 0,
                    doNotRestoreMissingFacts: false,
                    hasDifficultyRequirements: false,
                    invertDifficultyRequirements: false,
                    minDifficulty: GameDifficultyOption.Story
                )
                .SetHideInCharacterSheetAndLevelUp(false)
                .SetHideNotAvailibleInUI(false)
                .SetRanks(1)
                .SetReapplyOnLevelUp(false)
                .SetIsClassFeature(true)
                .Configure();
        }
        #endregion

        #region Blessed Script
        private static BlueprintFeature CreateBlessedScript5()
        {
            SpellsTableConfigurator.New(BlessedScript5Table, Guids.BlessedScript5Table)
                .SetLevels
                (
                    new SpellsLevelEntry[]
                    {
                        new SpellsLevelEntry(),                                         //0
                        new SpellsLevelEntry{Count = new int[]{0, 1}},                  //1
                        new SpellsLevelEntry{Count = new int[]{0, 2}},                  //2
                        new SpellsLevelEntry{Count = new int[]{0, 3}},                  //3
                        new SpellsLevelEntry{Count = new int[]{0, 3, 1}},               //4
                        new SpellsLevelEntry{Count = new int[]{0, 5, 2}},               //5
                        new SpellsLevelEntry{Count = new int[]{0, 5, 3}},               //6
                        new SpellsLevelEntry{Count = new int[]{0, 5, 3, 1}},            //7
                        new SpellsLevelEntry{Count = new int[]{0, 5, 4, 2}},            //8
                        new SpellsLevelEntry{Count = new int[]{0, 5, 4, 3}},            //9
                        new SpellsLevelEntry{Count = new int[]{0, 6, 4, 3, 1}},         //10
                        new SpellsLevelEntry{Count = new int[]{0, 6, 4, 4, 2}},         //11
                        new SpellsLevelEntry{Count = new int[]{0, 6, 5, 4, 3}},         //12
                        new SpellsLevelEntry{Count = new int[]{0, 6, 5, 4, 3, 1}},      //13
                        new SpellsLevelEntry{Count = new int[]{0, 6, 5, 4, 4, 2}},      //14
                        new SpellsLevelEntry{Count = new int[]{0, 6, 5, 5, 4, 3}},      //15
                        new SpellsLevelEntry{Count = new int[]{0, 6, 5, 5, 4, 3, 1}},   //16
                        new SpellsLevelEntry{Count = new int[]{0, 6, 5, 5, 4, 4, 2}},   //17
                        new SpellsLevelEntry{Count = new int[]{0, 6, 5, 5, 5, 4, 3}},   //18
                        new SpellsLevelEntry{Count = new int[]{0, 6, 5, 5, 5, 5, 4}},   //19
                        new SpellsLevelEntry{Count = new int[]{0, 6, 5, 5, 5, 5, 5}}    //20
                    }
                )
                .Configure();

            SpellbookConfigurator.For(Guids.LivingGrimoireSpellbook)
                .SetSpellSlots(Guids.BlessedScript5Table)
                .SetSpellsPerDay(Guids.BlessedScript5Table)
                .Configure();

            return FeatureConfigurator.New(BlessedScript5, Guids.BlessedScript5)
                .SetDisplayName(BlessedScriptName)
                .SetDescription(BlessedScriptDescription)
                .Configure();
        }
        
        private static BlueprintFeature CreateBlessedScript8()
        {
            SpellsTableConfigurator.New(BlessedScript8Table, Guids.BlessedScript8Table)
                .SetLevels
                (
                    new SpellsLevelEntry[]
                    {
                        new SpellsLevelEntry(),                                         //0
                        new SpellsLevelEntry{Count = new int[]{0, 1}},                  //1
                        new SpellsLevelEntry{Count = new int[]{0, 2}},                  //2
                        new SpellsLevelEntry{Count = new int[]{0, 3}},                  //3
                        new SpellsLevelEntry{Count = new int[]{0, 3, 1}},               //4
                        new SpellsLevelEntry{Count = new int[]{0, 5, 2}},               //5
                        new SpellsLevelEntry{Count = new int[]{0, 5, 3}},               //6
                        new SpellsLevelEntry{Count = new int[]{0, 5, 3, 1}},            //7
                        new SpellsLevelEntry{Count = new int[]{0, 5, 5, 2}},            //8
                        new SpellsLevelEntry{Count = new int[]{0, 5, 5, 3}},            //9
                        new SpellsLevelEntry{Count = new int[]{0, 6, 5, 3, 1}},         //10
                        new SpellsLevelEntry{Count = new int[]{0, 6, 5, 4, 2}},         //11
                        new SpellsLevelEntry{Count = new int[]{0, 6, 5, 4, 3}},         //12
                        new SpellsLevelEntry{Count = new int[]{0, 6, 6, 4, 3, 1}},      //13
                        new SpellsLevelEntry{Count = new int[]{0, 6, 6, 4, 4, 2}},      //14
                        new SpellsLevelEntry{Count = new int[]{0, 6, 6, 5, 4, 3}},      //15
                        new SpellsLevelEntry{Count = new int[]{0, 6, 6, 5, 4, 3, 1}},   //16
                        new SpellsLevelEntry{Count = new int[]{0, 6, 6, 5, 4, 4, 2}},   //17
                        new SpellsLevelEntry{Count = new int[]{0, 6, 6, 5, 5, 4, 3}},   //18
                        new SpellsLevelEntry{Count = new int[]{0, 6, 6, 5, 5, 5, 4}},   //19
                        new SpellsLevelEntry{Count = new int[]{0, 6, 6, 5, 5, 5, 5}}    //20
                    }
                )
                .Configure();

            SpellbookConfigurator.For(Guids.LivingGrimoireSpellbook)
                .SetSpellSlots(Guids.BlessedScript8Table)
                .SetSpellsPerDay(Guids.BlessedScript8Table)
                .Configure();

            return FeatureConfigurator.New(BlessedScript8, Guids.BlessedScript8)
                .SetDisplayName(BlessedScriptName)
                .SetDescription(BlessedScriptDescription)
                .Configure();
        }

        private static BlueprintFeature CreateBlessedScript12()
        {
            SpellsTableConfigurator.New(BlessedScript12Table, Guids.BlessedScript12Table)
                .SetLevels
                (
                    new SpellsLevelEntry[]
                    {
                        new SpellsLevelEntry(),                                         //0
                        new SpellsLevelEntry{Count = new int[]{0, 1}},                  //1
                        new SpellsLevelEntry{Count = new int[]{0, 2}},                  //2
                        new SpellsLevelEntry{Count = new int[]{0, 3}},                  //3
                        new SpellsLevelEntry{Count = new int[]{0, 3, 1}},               //4
                        new SpellsLevelEntry{Count = new int[]{0, 5, 2}},               //5
                        new SpellsLevelEntry{Count = new int[]{0, 5, 3}},               //6
                        new SpellsLevelEntry{Count = new int[]{0, 5, 3, 1}},            //7
                        new SpellsLevelEntry{Count = new int[]{0, 5, 5, 2}},            //8
                        new SpellsLevelEntry{Count = new int[]{0, 5, 5, 3}},            //9
                        new SpellsLevelEntry{Count = new int[]{0, 6, 5, 3, 1}},         //10
                        new SpellsLevelEntry{Count = new int[]{0, 6, 5, 4, 2}},         //11
                        new SpellsLevelEntry{Count = new int[]{0, 6, 5, 5, 3}},         //12
                        new SpellsLevelEntry{Count = new int[]{0, 6, 6, 5, 3, 1}},      //13
                        new SpellsLevelEntry{Count = new int[]{0, 6, 6, 5, 4, 2}},      //14
                        new SpellsLevelEntry{Count = new int[]{0, 6, 6, 5, 4, 3}},      //15
                        new SpellsLevelEntry{Count = new int[]{0, 6, 6, 6, 4, 3, 1}},   //16
                        new SpellsLevelEntry{Count = new int[]{0, 6, 6, 6, 4, 4, 2}},   //17
                        new SpellsLevelEntry{Count = new int[]{0, 6, 6, 6, 5, 4, 3}},   //18
                        new SpellsLevelEntry{Count = new int[]{0, 6, 6, 6, 5, 5, 4}},   //19
                        new SpellsLevelEntry{Count = new int[]{0, 6, 6, 6, 5, 5, 5}}    //20
                    }
                )
                .Configure();

            SpellbookConfigurator.For(Guids.LivingGrimoireSpellbook)
                .SetSpellSlots(Guids.BlessedScript12Table)
                .SetSpellsPerDay(Guids.BlessedScript12Table)
                .Configure();

            return FeatureConfigurator.New(BlessedScript12, Guids.BlessedScript12)
                .SetDisplayName(BlessedScriptName)
                .SetDescription(BlessedScriptDescription)
                .Configure();
        }

        private static BlueprintFeature CreateBlessedScript16()
        {
            SpellsTableConfigurator.New(BlessedScript16Table, Guids.BlessedScript16Table)
                .SetLevels
                (
                    new SpellsLevelEntry[]
                    {
                        new SpellsLevelEntry(),                                         //0
                        new SpellsLevelEntry{Count = new int[]{0, 1}},                  //1
                        new SpellsLevelEntry{Count = new int[]{0, 2}},                  //2
                        new SpellsLevelEntry{Count = new int[]{0, 3}},                  //3
                        new SpellsLevelEntry{Count = new int[]{0, 3, 1}},               //4
                        new SpellsLevelEntry{Count = new int[]{0, 5, 2}},               //5
                        new SpellsLevelEntry{Count = new int[]{0, 5, 3}},               //6
                        new SpellsLevelEntry{Count = new int[]{0, 5, 3, 1}},            //7
                        new SpellsLevelEntry{Count = new int[]{0, 5, 5, 2}},            //8
                        new SpellsLevelEntry{Count = new int[]{0, 5, 5, 3}},            //9
                        new SpellsLevelEntry{Count = new int[]{0, 6, 5, 3, 1}},         //10
                        new SpellsLevelEntry{Count = new int[]{0, 6, 5, 4, 2}},         //11
                        new SpellsLevelEntry{Count = new int[]{0, 6, 5, 5, 3}},         //12
                        new SpellsLevelEntry{Count = new int[]{0, 6, 6, 5, 3, 1}},      //13
                        new SpellsLevelEntry{Count = new int[]{0, 6, 6, 5, 4, 2}},      //14
                        new SpellsLevelEntry{Count = new int[]{0, 6, 6, 5, 4, 3}},      //15
                        new SpellsLevelEntry{Count = new int[]{0, 6, 6, 6, 5, 3, 1}},   //16
                        new SpellsLevelEntry{Count = new int[]{0, 6, 6, 6, 5, 4, 2}},   //17
                        new SpellsLevelEntry{Count = new int[]{0, 6, 6, 6, 5, 4, 3}},   //18
                        new SpellsLevelEntry{Count = new int[]{0, 6, 6, 6, 6, 5, 4}},   //19
                        new SpellsLevelEntry{Count = new int[]{0, 6, 6, 6, 6, 5, 5}}    //20
                    }
                )
                .Configure();

            SpellbookConfigurator.For(Guids.LivingGrimoireSpellbook)
                .SetSpellSlots(Guids.BlessedScript16Table)
                .SetSpellsPerDay(Guids.BlessedScript16Table)
                .Configure();

            return FeatureConfigurator.New(BlessedScript16, Guids.BlessedScript16)
                .SetDisplayName(BlessedScriptName)
                .SetDescription(BlessedScriptDescription)
                .Configure();
        }
        #endregion

        #region Word Of God
        private static BlueprintFeature CreateWordOfGod()
        {
            Logger.Log("Creating Word of God Resource");
            AbilityResourceConfigurator.New(WordOfGodResource, Guids.WordOfGodResource)
                .SetIcon((Sprite)UnityObjectConverter.AssetList.Get("32647606f1accbb4d9e64f65ae2b771c", 21300000))
                .SetMaxAmount
                (
                    ResourceAmountBuilder.New(7).Build()
                )
                .SetUseMax(false)
                .SetMax(7)
                .SetMin(0)
                .Configure();

            Logger.Log("Creating Word of God Caster Buff");
            BuffConfigurator.New(WordOfGodBuff, Guids.WordOfGodBuff)
                .AddInitiatorAttackWithWeaponTrigger
                (
                    onlyHit: true,
                    onMiss: false,
                    onlyOnFullAttack: false,
                    onlyOnFirstAttack: false,
                    onlyOnFirstHit: false,
                    criticalHit: false,
                    onAttackOfOpportunity: false,
                    notCriticalHit: false,
                    onlySneakAttack: false,
                    notSneakAttack: false,
                    weaponType: Guids.HolyBookWeaponType,
                    checkWeaponCategory: false,
                    category: WeaponCategory.UnarmedStrike,
                    checkWeaponGroup: false,
                    group: WeaponFighterGroup.None,
                    checkWeaponRangeType: false,
                    rangeType: WeaponRangeType.Melee,
                    actionsOnInitiator: false,
                    reduceHPToZero: false,
                    checkDistance: false,
                    distanceLessEqual: new Feet(0),
                    allNaturalAndUnarmed: false,
                    duelistWeapon: false,
                    notExtraAttack: false,
                    onCharge: false,
                    action: ActionsBuilder.New()
                    .Conditional
                    (
                        conditions: ConditionsBuilder.New()
                        .AddOrAndLogic
                        (
                            ConditionsBuilder.New()
                            .HasBuffFromCaster
                            (
                                negate: false,
                                buff: BuffRefs.TrueJudgmentCooldownBuff.ToString()
                            )
                        ),
                        ifFalse: ActionsBuilder.New()
                        .SavingThrow
                        (
                            type: SavingThrowType.Fortitude,
                            fromBuff: false,
                            conditionalDCModifiers: default,
                            customDC: default,
                            onResult: ActionsBuilder.New()
                            .ConditionalSaved
                            (
                                failed: ActionsBuilder.New().ApplyBuff
                                (
                                    buff: BuffRefs.TrueJudgmentDamageBuff.ToString(),
                                    durationValue: new ContextDurationValue(),
                                    isFromSpell: false,
                                    isNotDispelable: true,
                                    toCaster: false,
                                    asChild: false,
                                    sameDuration: false,
                                    ignoreParentContext: default,
                                    notLinkToAreaEffect: default
                                )
                            ),
                            useDCFromContextSavingThrow: false
                        )
                        .ApplyBuff
                        (
                            buff: BuffRefs.TrueJudgmentCooldownBuff.ToString(),
                            durationValue: new ContextDurationValue(),
                            isFromSpell: false,
                            isNotDispelable: true,
                            toCaster: false,
                            asChild: false,
                            sameDuration: false,
                            ignoreParentContext: default,
                            notLinkToAreaEffect: default
                        )
                        .RemoveSelf()
                    )
                    .Build()
                )
                .SetAllowNonContextActions(false)
                .SetDisplayName(WordOfGodName)
                .SetDescription(WordOfGodDescription)
                .SetIcon((Sprite)UnityObjectConverter.AssetList.Get("32647606f1accbb4d9e64f65ae2b771c", 21300000)) //use true judgement icon
                .SetIsClassFeature(true)
                .SetFlags(0)
                .SetStacking(StackingType.Replace)
                .SetRanks(0)
                .SetTickEachSecond(false)
                .SetFrequency(DurationRate.Rounds)
                .Configure();

            Logger.Log("Creating Word of God Ability");
            AbilityConfigurator.New(WordOfGodAbility, Guids.WordOfGodAbility)
                .AddAbilityEffectRunAction
                (
                    actions: ActionsBuilder.New()
                    .ApplyBuffPermanent
                    (
                        buff: BuffRefs.TrueJudgmentCasterBuff.ToString(),
                        isFromSpell: false,
                        isNotDispelable: true,
                        toCaster: false,
                        asChild: false,
                        sameDuration: false,
                        ignoreParentContext: default,
                        notLinkToAreaEffect: default
                    ).Build()
                )
                .AddAbilityResourceLogic
                (
                    requiredResource: Guids.WordOfGodResource,
                    isSpendResource: true,
                    costIsCustom: false,
                    amount: 1
                )
                .SetAllowNonContextActions(false)
                .SetDisplayName(WordOfGodName)
                .SetDescription(WordOfGodDescription)
                .SetIcon((Sprite)UnityObjectConverter.AssetList.Get("32647606f1accbb4d9e64f65ae2b771c", 21300000)) //use true judgement icon
                .SetType(AbilityType.Supernatural)
                .SetRange(AbilityRange.Personal)
                .SetShowNameForVariant(false)
                .SetOnlyForAllyCaster(false)
                .SetCanTargetPoint(false)
                .SetCanTargetEnemies(false)
                .SetCanTargetFriends(false)
                .SetCanTargetSelf(true)
                .SetSpellResistance(false)
                .SetActionBarAutoFillIgnored(false)
                .SetHidden(false)
                .SetNeedEquipWeapons(false)
                .SetNotOffensive(false)
                .SetEffectOnAlly(AbilityEffectOnUnit.None)
                .SetEffectOnEnemy(AbilityEffectOnUnit.None)
                .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
                .SetHasFastAnimation(false)
                .SetTargetMapObjects(false)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .SetDisableLog(false)
                .Configure();

            Logger.Log("Creating Word of God Feature");
            return FeatureConfigurator.New(WordOfGod, Guids.WordOfGod)
                .AddFacts
                (
                    facts: new()
                    {
                        Guids.WordOfGodAbility.ToString()
                        //AbilityRefs.TrueJudgmentAbility.ToString()
                    },
                    casterLevel: 0,
                    doNotRestoreMissingFacts: false,
                    hasDifficultyRequirements: false,
                    invertDifficultyRequirements: false,
                    minDifficulty: GameDifficultyOption.Story
                )
                .AddReplaceAbilitiesStat
                (
                    ability: new() { BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.WordOfGodAbility) },
                    stat: StatType.Intelligence
                )
                .AddReplaceCasterLevelOfAbility
                (
                    spell: BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.WordOfGodAbility),
                    clazz: CharacterClassRefs.InquisitorClass.ToString(),
                    archetypes: new() { BlueprintTool.GetRef<BlueprintArchetypeReference>(Guids.LivingGrimoireArchetype) }
                )
                .AddBindAbilitiesToClass
                (
                    abilites: new() { BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.WordOfGodAbility) },
                    cantrip: false,
                    characterClass: CharacterClassRefs.InquisitorClass.ToString(),
                    archetypes: new() { BlueprintTool.GetRef<BlueprintArchetypeReference>(Guids.LivingGrimoireArchetype) },
                    stat: StatType.Intelligence,
                    levelStep: 2,
                    odd: true,
                    fullCasterChecks: true
                )
                .AddAbilityResources
                (
                    useThisAsResource: false,
                    resource: Guids.WordOfGodResource,
                    amount: 0,
                    restoreAmount: true,
                    restoreOnLevelUp: false
                )
                .AddPrerequisiteFeature(BlueprintTool.GetRef<BlueprintFeatureReference>(Guids.LivingGrimoireHolyBook))
                .SetAllowNonContextActions(false)
                .SetDisplayName(WordOfGodName)
                .SetDescription(WordOfGodDescription)
                .SetIcon((Sprite)UnityObjectConverter.AssetList.Get("32647606f1accbb4d9e64f65ae2b771c", 21300000)) //use true judgement icon
                .SetHideInUI(false)
                .SetHideInCharacterSheetAndLevelUp(false)
                .SetHideNotAvailibleInUI(false)
                .SetRanks(1)
                .SetReapplyOnLevelUp(false)
                .SetIsClassFeature(true)
                .Configure();
        }
        #endregion
    }
    [TypeId("7ddc53e032064d17b25740d1d94a7c22")]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    public class SacredWordDamageOverride : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateWeaponStats>, IRulebookHandler<RuleCalculateWeaponStats>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public DiceFormula Formula;

        [SerializeField]
        public BlueprintFeatureReference m_Feature;
        public BlueprintFeature Feature => m_Feature;

        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
        {
            if (evt.Weapon.Blueprint.Name.Equals("Holy Book"))
                evt.WeaponDamageDice.Modify(this.Formula, base.Fact);
        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt) {}
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          