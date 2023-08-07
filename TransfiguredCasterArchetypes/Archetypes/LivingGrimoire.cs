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
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
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
using Kingmaker.Blueprints.Classes.Selection;
using BlueprintCore.Blueprints.Configurators.Classes.Selection;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.RuleSystem;
using ValueType = Kingmaker.UnitLogic.Mechanics.ValueType;
using Kingmaker.UnitLogic.Abilities.Components;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using TransfiguredCasterArchetypes.Blueprints;
using BuffConfigurator = BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs.BuffConfigurator;
using Kingmaker.Designers.Mechanics.Facts;

namespace TransfiguredCasterArchetypes.Archetypes {
	static class LivingGrimoire {

        internal const string ArchetypeName = "LivingGrimoire";
        internal const string ArchetypeDisplayName = "LivingGrimoire.Name";
        internal const string ArchetypeDescription = "LivingGrimoire.Description";

        internal const string BlessedScriptSelection = "LivingGrimoire.BlessedScriptSelection";

        internal const string HolyBook = "LivingGrimoire.HolyBook";
        internal const string HolyBookName = "LivingGrimoire.HolyBook.Name";
        internal const string HolyBookDescription = "LivingGrimoire.HolyBook.Description";
        internal const string WeaponFocusHolyBook = "LivingGrimoire.HolyBook.WeaponFocus";
        internal const string WeaponFocusHolyBookName = "LivingGrimoire.HolyBook.WeaponFocus.Name";
        internal const string WeaponFocusHolyBookDescription = "LivingGrimoire.HolyBook.WeaponFocus.Description";

        internal const string SacredWord = "LivingGrimoire.SacredWord";
        internal const string SacredWordName = "LivingGrimoire.SacredWord.Name";
        internal const string SacredWordDescription = "LivingGrimoire.SacredWord.Description";

        internal const string SacredWordBuffBase = "LivingGrimoire.SacredWord.Buff";
        internal const string SacredWordBuff1d6 = "LivingGrimoire.SacredWord.Buff.1d6";
        internal const string SacredWordBuff1d8 = "LivingGrimoire.SacredWord.Buff.1d8";
        internal const string SacredWordBuff1d10 = "LivingGrimoire.SacredWord.Buff.1d10";
        internal const string SacredWordBuff2d6 = "LivingGrimoire.SacredWord.Buff.2d6";
        internal const string SacredWordBuff2d8 = "LivingGrimoire.SacredWord.Buff.2d8";

        internal const string SacredWordWeaponSwitch = "LivingGrimoire.SacredWordWeaponSwitch";
        internal const string SacredWordBaseDamageFeature = "LivingGrimoire.SacredWordBaseDamageFeature";
        internal const string SacredWordEnchantFeature = "LivingGrimoire.SacredWordEnchantFeature";
        internal const string SacredWordEnchantPlus2 = "LivingGrimoire.SacredWordEnchantPlus2";
        internal const string SacredWordEnchantPlus3 = "LivingGrimoire.SacredWordEnchantPlus3";
        internal const string SacredWordEnchantPlus4 = "LivingGrimoire.SacredWordEnchantPlus4";
        internal const string SacredWordEnchantPlus5 = "LivingGrimoire.SacredWordEnchantPlus5";

        internal const string BlessedScript = "LivingGrimoire.BlessedScript";
        internal const string BlessedScript5 = "LivingGrimoire.BlessedScript5";
        internal const string BlessedScript8 = "LivingGrimoire.BlessedScript8";
        internal const string BlessedScript12 = "LivingGrimoire.BlessedScript12";
        internal const string BlessedScript16 = "LivingGrimoire.BlessedScript16";

        internal const string WordOfGod = "LivingGrimoire.WordOfGod";
        internal const string WordOfGodName = "LivingGrimoire.WordOfGod.Name";
        internal const string WordOfGodDescription = "LivingGrimoire.WordOfGod.Description";
        internal const string WordOfGodAbility = "LivingGrimoire.WordOfGod.Ability";
        internal const string WordOfGodResource = "LivingGrimoire.WordOfGod.Resource";
        internal const string WordOfGodBuff = "LivingGrimoire.WordOfGod.Buff";

        internal const string SacredWordEnchantBaneChoice = "";
        internal const string SacredWordEnchantBaneBuff = "";


        internal const string LivingGrimoireProficiencies = "LivingGrimoire.Proficiencies";

        internal const string LivingGrimoireSpellbook = "LivingGrimoire.Spellbook";

        internal const string LivingGrimoireCantrips = "LivingGrimoire.Orisons";
        internal const string LivingGrimoireCantripsName = "LivingGrimoire.Orisons.Name";
        internal const string LivingGrimoireCantripsDescription = "LivingGrimoire.Orisons.Description";

        internal const string HellknightSigniferLivingGrimoire = "LivingGrimoire.HellknightSignifer";
        internal const string HellknightSigniferLivingGrimoireDescription = "LivingGrimoire.HellknightSignifer.Description";

        internal const string LoremasterLivingGrimoire = "LivingGrimoire.Loremaster";
        internal const string LoremasterLivingGrimoireDescription = "LivingGrimoire.Loremaster.Description";

        internal const string MysticTheurgeLivingGrimoireLevelUp = "LivingGrimoire.MysticTheurge.LevelUp";
        internal const string MysticTheurgeLivingGrimoire = "LivingGrimoire.MysticTheurge";
        internal const string MysticTheurgeLivingGrimoireDescription = "LivingGrimoire.MysticTheurge.Description";

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
            FeatureConfigurator.New(SacredWord, Guids.LivingGrimoireSacredWord).Configure();
            ActivatableAbilityConfigurator.New(SacredWordWeaponSwitch, Guids.SacredWordWeaponSwitch).Configure();
            FeatureConfigurator.New(SacredWordBaseDamageFeature, Guids.SacredWordBaseDamageFeature).Configure();
/*            FeatureConfigurator.New(SacredWordEnchantFeature, Guids.SacredWordEnchantFeature).Configure();
            FeatureConfigurator.New(SacredWordEnchantPlus2, Guids.SacredWordEnchantPlus2).Configure();
            FeatureConfigurator.New(SacredWordEnchantPlus3, Guids.SacredWordEnchantPlus3).Configure();
            FeatureConfigurator.New(SacredWordEnchantPlus4, Guids.SacredWordEnchantPlus4).Configure();
            FeatureConfigurator.New(SacredWordEnchantPlus5, Guids.SacredWordEnchantPlus5).Configure();
            FeatureConfigurator.New(BlessedScript, Guids.LivingGrimoireBlessedScript).Configure();
            FeatureConfigurator.New(BlessedScript5, Guids.BlessedScript5).Configure();
            FeatureConfigurator.New(BlessedScript8, Guids.BlessedScript8).Configure();
            FeatureConfigurator.New(BlessedScript12, Guids.BlessedScript12).Configure();
            FeatureConfigurator.New(BlessedScript16, Guids.BlessedScript16).Configure();
*/            FeatureConfigurator.New(WordOfGod, Guids.LivingGrimoireWordOfGod).Configure();
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
                .AddToAddFeatures(level: 2, CreateSacredWordAbility(), CreateSacredWord())/*
                .AddToAddFeatures(level: 4, CreateSacredWordEnchant())
                .AddToAddFeatures(level: 5, CreateBlessedScript5())
                .AddToAddFeatures(level: 8, CreateSacredWordPlus2(), CreateBlessedScript8())
                .AddToAddFeatures(level: 12, CreateSacredWordPlus3(), CreateBlessedScript12())
                .AddToAddFeatures(level: 16, CreateSacredWordPlus4(), CreateBlessedScript16())*/
                .AddToAddFeatures(level: 20, CreateWordOfGod()/*, CreateSacredWordPlus5()*/);

            archetype.Configure();
        }

        private static void ConfigureEnabledDelayed()
        {
            if (!Settings.IsTTTBaseEnabled())
                return;

            Logger.Log("Patching TTT Loremaster compatbility for Living Grimoire");
            FeatureSelectionConfigurator.For(Guids.TTTLoremasterSpellbookSelection)
                .AddToAllFeatures(Guids.LoremasterLivingGrimoire)
                .SkipAddToSelections()
                .Configure();
        }

        #region Spells
        private static BlueprintSpellbook CreateSpellbook()
        {
            var spellbook = SpellbookConfigurator.New(LivingGrimoireSpellbook, Guids.LivingGrimoireSpellbook)
                .CopyFrom(SpellbookRefs.InquisitorSpellbook)
                .SetSpellsKnown(null)
                .SetCastingAttribute(StatType.Intelligence)
                .SetSpontaneous(false)
                .SetSpellsPerLevel(2)
                .SetCanCopyScrolls(true)
                .Configure();

            var inquisitor = CharacterClassRefs.InquisitorClass.ToString();

            //old code moved to txt file
            // Mystic Theurge Support
            Logger.Log($"Restricting {Guids.LivingGrimoireArchetype} from using {inquisitor} Progression");
            ProgressionConfigurator.For(ProgressionRefs.MysticTheurgeInquisitorProgression)
                .AddPrerequisiteNoArchetype(characterClass: inquisitor, archetype: Guids.LivingGrimoireArchetype)
                .Configure();

            Logger.Log($"Creating {Guids.MysticTheurgeLivingGrimoire} LevelUp Feature");
            var mtLGL = FeatureConfigurator.New(MysticTheurgeLivingGrimoireLevelUp, Guids.MysticTheurgeLivingGrimoireLevelUp)
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

            Logger.Log($"Creating {Guids.MysticTheurgeLivingGrimoire} Progression");

            var mtLG = ProgressionConfigurator.New(MysticTheurgeLivingGrimoire, Guids.MysticTheurgeLivingGrimoire)
                .AddPrerequisiteClassSpellLevel(characterClass: CharacterClassRefs.InquisitorClass.ToString(), requiredSpellLevel: 2)
                .AddMysticTheurgeSpellbook(characterClass: CharacterClassRefs.InquisitorClass.ToString(), mysticTheurge: CharacterClassRefs.MysticTheurgeClass.ToString())
                .SetDisplayName(ArchetypeDisplayName)
                .SetDescription(MysticTheurgeLivingGrimoireDescription)
                .SetHideInUI(true)
                .SetHideInCharacterSheetAndLevelUp(false)
                .SetHideNotAvailibleInUI(true)
                .SetGroups(FeatureGroup.MysticTheurgeDivineSpellbook)
                .SetRanks(1)
                .SetReapplyOnLevelUp(false)
                .SetIsClassFeature(true)
                .SetClasses((CharacterClassRefs.MysticTheurgeClass.ToString(), 0), (CharacterClassRefs.InquisitorClass.ToString(), 0))
                .SetArchetypes(Guids.LivingGrimoireArchetype.ToString())
                .SetForAllOtherClasses(false)
                //.SetAlternateProgressionClasses() no
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

            Logger.Log($"Adding {Guids.MysticTheurgeLivingGrimoire} Spellbook to {FeatureSelectionRefs.MysticTheurgeDivineSpellbookSelection.ToString()}");
            FeatureSelectionConfigurator.For(FeatureSelectionRefs.MysticTheurgeDivineSpellbookSelection.ToString())
                .AddToAllFeatures(mtLG)
                .SkipAddToSelections()
                .Configure();


            // Hellknight Signifer Support
            Logger.Log("Adding Archetype Spellbook for Hellknight Signifer");
            Logger.Log($"Restricting {Guids.LivingGrimoireArchetype} from using {inquisitor} Progression");
            ProgressionConfigurator.For(ProgressionRefs.HellknightSigniferInquisitorProgression)
                .AddPrerequisiteNoArchetype(characterClass: inquisitor, archetype: Guids.LivingGrimoireArchetype)
                .Configure();

            Logger.Log($"Creating {Guids.HellknightSigniferLivingGrimoire} Spellbook from Class Progression");
            var replacementHS = FeatureReplaceSpellbookConfigurator.New(HellknightSigniferLivingGrimoire, Guids.HellknightSigniferLivingGrimoire)
                .AddPrerequisiteClassSpellLevel(characterClass: inquisitor, requiredSpellLevel: 3)
                .AddPrerequisiteArchetypeLevel(characterClass: inquisitor, archetype: Guids.LivingGrimoireArchetype)
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

            Logger.Log($"Adding {Guids.HellknightSigniferLivingGrimoire} Spellbook to {FeatureSelectionRefs.HellknightSigniferSpellbook.ToString()}");
            FeatureSelectionConfigurator.For(FeatureSelectionRefs.HellknightSigniferSpellbook.ToString())
                .AddToAllFeatures(replacementHS)
                .SkipAddToSelections()
                .Configure();

            // Loremaster Support
            Logger.Log("Adding Archetype Spellbook for Loremaster");
            Logger.Log($"Restricting {Guids.LivingGrimoireArchetype} from using {inquisitor} Progression");
            ProgressionConfigurator.For(ProgressionRefs.LoremasterInquisitorProgression)
                .AddPrerequisiteNoArchetype(characterClass: inquisitor, archetype: Guids.LivingGrimoireArchetype)
                .Configure();

            Logger.Log($"Creating {Guids.LoremasterLivingGrimoire} Spellbook from Class Progression");
            var replacementLM = FeatureReplaceSpellbookConfigurator.New(LoremasterLivingGrimoire, Guids.LoremasterLivingGrimoire)
                .AddPrerequisiteClassSpellLevel(characterClass: inquisitor, requiredSpellLevel: 3)
                .AddPrerequisiteArchetypeLevel(characterClass: inquisitor, archetype: Guids.LivingGrimoireArchetype)
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

            Logger.Log($"Adding {Guids.LoremasterLivingGrimoire} Spellbook to {FeatureSelectionRefs.LoremasterSpellbookSelection.ToString()}");
            FeatureSelectionConfigurator.For(FeatureSelectionRefs.LoremasterSpellbookSelection.ToString())
                .AddToAllFeatures(replacementLM)
                .SkipAddToSelections()
                .Configure();

            Common.AddToLoremasterSecrets
            (
                Guids.LoremasterLivingGrimoire,
                ParametrizedFeatureRefs.LoremasterClericSecretInquisitor.ToString(),
                ParametrizedFeatureRefs.LoremasterDruidSecretInquisitor.ToString(),
                ParametrizedFeatureRefs.LoremasterWizardSecretInquisitor.ToString()
            );

            return spellbook;
        }

        //private static BlueprintParametrizedFeature CreateBlessedScriptSelection()
        //{
        //    return null;
        //}LivingGrimoireProficiencies

        private static BlueprintFeature CreateCantrips()
        {
            var cantrips = FeatureConfigurator.New(LivingGrimoireCantrips, Guids.LivingGrimoireCantrips)
                .CopyFrom(FeatureRefs.InquisitorOrisonsFeature)
                .SetDisplayName(LivingGrimoireCantripsName)
                .AddBindAbilitiesToClass(stat: StatType.Intelligence)
                .AddFacts
                (
                    new()
                    {
                        AbilityRefs.Daze.ToString(), AbilityRefs.Resistance.ToString(), AbilityRefs.MageLight.ToString(),
                        AbilityRefs.AcidSplash.ToString(), AbilityRefs.DisruptUndead.ToString(), AbilityRefs.Virtue.ToString(),
                        AbilityRefs.Guidance.ToString(), AbilityRefs.DivineZap.ToString(), AbilityRefs.DismissAreaEffect.ToString()
                    }
                )
                .SetIsClassFeature(true)
                .Configure();

            return cantrips;
        }
        #endregion
        
        private static BlueprintFeature CreateHolyBook()
        {
            Logger.Log($"Configuring {Guids.HolyBookWeaponType} Weapon Focus ({Guids.WeaponFocusHolyBook})");
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
                .SetIcon((Sprite)UnityObjectConverter.AssetList.Get("51f2bd71d233b374a9c63717b08581a6", 21300000))
                .SetHideInUI(false)
                .SetHideInCharacterSheetAndLevelUp(false)
                .SetHideNotAvailibleInUI(false)
                .SetRanks(1)
                .SetReapplyOnLevelUp(false)
                .SetIsClassFeature(true)
                .SetGroups(FeatureGroup.Feat, FeatureGroup.CombatFeat)
                .Configure();

            Logger.Log($"Configuring {HolyBook}");
            string WeaponGuid = Guids.HolyBookWeapon;
            if (Settings.IsEnabled(Guids.MindfulEnchantmentHomebrew))
                if (Settings.IsEnabled(Guids.MindfulHolyBookWeapon))
                    WeaponGuid = Guids.MindfulHolyBookWeapon;

            Logger.Log($"Blueprint {WeaponGuid} will be used for HolyBook");
            return FeatureConfigurator.New(HolyBook, Guids.LivingGrimoireHolyBook)
                .SetDisplayName(HolyBookName)
                .SetDescription(HolyBookDescription)
                .AddStartingEquipment(new() { BlueprintTool.GetRef<BlueprintItemReference>(WeaponGuid) })
                .SetIsClassFeature(true)
                .AddFeatureOnApply(Guids.WeaponFocusHolyBook)
                .Configure();
        }

        #region Sacred Word

        private static BlueprintFeature CreateSacredWordAbility()
        {
            Logger.Log($"Creating Weapon Buffs For {Guids.LivingGrimoireSacredWord}");
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
                .Configure();

            Logger.Log($"Creating Buff Base Feature For {Guids.LivingGrimoireSacredWord}");
            BuffConfigurator.New(SacredWordBuffBase, Guids.SacredWordBuffBase)
                .CopyFrom(BuffRefs.WarpriestSacredWeaponBuffBase)
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
                        )
                    )
                )
                .AddContextRankConfig
                (
                    ContextRankConfigs.MaxClassLevelWithArchetype
                    (
                        classes: new[] { CharacterClassRefs.InquisitorClass.ToString() },
                        archetypes: new[] { Guids.LivingGrimoireArchetype },
                        excludeClasses: false,
                        type: default,
                        max: 20,
                        min: 0
                    )
                )
                .AddContextCalculateSharedValue
                (
                    valueType: AbilitySharedValue.Damage,
                    value: default,
                    modifier: 1.0
                )
                .AddSacredWeaponFavoriteDamageOverride()
                .Configure();

            Logger.Log($"Creating Weapon Switch Feature For {Guids.LivingGrimoireSacredWord}");
            ActivatableAbilityConfigurator.New(SacredWordWeaponSwitch, Guids.SacredWordWeaponSwitch)
                .CopyFrom(ActivatableAbilityRefs.WarpriestSacredWeaponSwitch)
                .SetDisplayName(SacredWordName)
                .SetDescription(SacredWordDescription)
                .SetBuff(Guids.SacredWordBuffBase)
                .Configure();

            Logger.Log($"Creating Base Damage Feature For Sacred Word {Guids.SacredWordBaseDamageFeature}");
            return FeatureConfigurator.New(SacredWordBaseDamageFeature, Guids.SacredWordBaseDamageFeature)
                .CopyFrom(FeatureRefs.WarpriestSacredWeaponBaseDamageFeature)
                .AddFacts
                (
                    facts: new()
                    {
                        BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.SacredWordWeaponSwitch).ToString()
                    }
                )
                .SetDisplayName(SacredWordName)
                .SetDescription(SacredWordDescription)
                .Configure();
        }

        private static BlueprintFeature CreateSacredWord()
        {
            return FeatureConfigurator.New(SacredWord, Guids.LivingGrimoireSacredWord)
                .SetDisplayName(SacredWordName)
                .SetDescription(SacredWordDescription)
                .SetIcon((Sprite)UnityObjectConverter.AssetList.Get("ea962cdf84daeb8469e0464390ee165c", 21300000)) //use sacred weapon icon
                .SetHideInUI(false)
                .SetHideInCharacterSheetAndLevelUp(false)
                .SetHideNotAvailibleInUI(false)
                .SetRanks(1)
                .SetReapplyOnLevelUp(false)
                .SetIsClassFeature(true)
                .Configure();
        }

        /*
        private static BlueprintFeature CreateSacredWordEnchant()
        {
            return FeatureConfigurator.New(SacredWordEnchantFeature, Guids.SacredWordEnchantFeature)
                .Configure();
        }

        private static BlueprintFeature CreateSacredWordPlus2()
        {
            return FeatureConfigurator.New(SacredWordEnchantPlus2, Guids.SacredWordEnchantPlus2)
                .Configure();
        }

        private static BlueprintFeature CreateSacredWordPlus3()
        {
            return FeatureConfigurator.New(SacredWordEnchantPlus3, Guids.SacredWordEnchantPlus3)
                .Configure();
        }

        private static BlueprintFeature CreateSacredWordPlus4()
        {
            return FeatureConfigurator.New(SacredWordEnchantPlus4, Guids.SacredWordEnchantPlus4)
                .Configure();
        }

        private static BlueprintFeature CreateSacredWordPlus5()
        {
            return FeatureConfigurator.New(SacredWordEnchantPlus5, Guids.SacredWordEnchantPlus5)
                .Configure();
        }*/
        #endregion

        #region Blessed Script
        
        private static BlueprintFeature CreateBlessedScript5()
        {
            var s = SpellbookConfigurator.For(Guids.LivingGrimoireSpellbook)
                .SetHasSpecialSpellList(true)
                .SetSpecialSpellListName("Blessed Script")
                .Configure();

            return FeatureConfigurator.New(BlessedScript5, Guids.BlessedScript5)

                .Configure();
        }
        /*
        private static BlueprintFeature CreateBlessedScript8()
        {
            return FeatureConfigurator.New(BlessedScript8, Guids.BlessedScript8)
                .Configure();
        }

        private static BlueprintFeature CreateBlessedScript12()
        {
            return FeatureConfigurator.New(BlessedScript12, Guids.BlessedScript12)
                .Configure();
        }

        private static BlueprintFeature CreateBlessedScript16()
        {
            return FeatureConfigurator.New(BlessedScript16, Guids.BlessedScript16)
                .Configure();
        }*/
        #endregion

        #region Word Of God
        private static BlueprintFeature CreateWordOfGod()
        {
            Logger.Log("Creating Ability Resource for Word of God");
            AbilityResourceConfigurator.New(WordOfGodResource, Guids.LivingGrimoireWordOfGodResource)
                .SetIcon((Sprite)UnityObjectConverter.AssetList.Get("32647606f1accbb4d9e64f65ae2b771c", 21300000))
                .SetMaxAmount
                (
                    ResourceAmountBuilder.New(7).Build()
                )
                .SetUseMax(false)
                .SetMax(7)
                .SetMin(0)
                .Configure();

            Logger.Log("Creating Caster Buff for Word of God");
            BuffConfigurator.New(WordOfGodBuff, Guids.LivingGrimoireWordOfGodBuff)
                //.CopyFrom(BuffRefs.TrueJudgmentCasterBuff)
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

            Logger.Log("Creating Ability for Word of God");
            AbilityConfigurator.New(WordOfGodAbility, Guids.LivingGrimoireWordOfGodAbility)
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
                    requiredResource: Guids.LivingGrimoireWordOfGodResource,
                    isSpendResource: true,
                    costIsCustom: false,
                    amount: 1
                )
                .SetAllowNonContextActions(false)
                .SetDisplayName(WordOfGodName)
                .SetDescription(WordOfGodDescription)
                .SetIcon((Sprite)UnityObjectConverter.AssetList.Get("32647606f1accbb4d9e64f65ae2b771c",21300000)) //use true judgement icon
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
            return FeatureConfigurator.New(WordOfGod, Guids.LivingGrimoireWordOfGod)
                .AddFacts
                (
                    facts: new()
                    {
                        BlueprintTool.GetRef<BlueprintUnitFactReference>
                        (
                            BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.LivingGrimoireWordOfGodAbility).ToString()
                            //BlueprintTool.GetRef<BlueprintAbilityReference>(AbilityRefs.TrueJudgmentAbility.ToString()).ToString()
                        )
                    },
                    casterLevel: 0,
                    doNotRestoreMissingFacts: false,
                    hasDifficultyRequirements: false,
                    invertDifficultyRequirements: false,
                    minDifficulty: GameDifficultyOption.Story
                )
                .AddReplaceAbilitiesStat
                (
                    ability: new() { BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.LivingGrimoireWordOfGodAbility) },
                    //ability: new() { BlueprintTool.GetRef<BlueprintAbilityReference>(AbilityRefs.TrueJudgmentAbility.ToString()) },
                    stat: StatType.Intelligence
                )
                .AddReplaceCasterLevelOfAbility
                (
                    spell: BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.LivingGrimoireWordOfGodAbility),
                    clazz: CharacterClassRefs.InquisitorClass.ToString(),
                    archetypes: new() { BlueprintTool.GetRef<BlueprintArchetypeReference>(Guids.LivingGrimoireArchetype) }
                )
                .AddBindAbilitiesToClass
                (
                    abilites: new() { BlueprintTool.GetRef<BlueprintAbilityReference>(Guids.LivingGrimoireWordOfGodAbility) },
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
                    resource: Guids.LivingGrimoireWordOfGodResource,
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
            {
                evt.WeaponDamageDice.Modify(Formula, base.Fact);
            }
        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt) {}
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          