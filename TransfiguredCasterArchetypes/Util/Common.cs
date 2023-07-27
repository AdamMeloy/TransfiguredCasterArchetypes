using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.Configurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Spells;
using Microsoft.Build.Utilities;
using System;
using TransfiguredCasterArchetypes.Archetypes;
using TransfiguredCasterArchetypes.Util;

namespace CharacterOptionsPlus.Util
{
    /// <summary>
    /// Container for common utils or constants.
    /// </summary>
    internal static class Common
    {
        internal static readonly string DurationVaries = "Duration.Varies";
        internal static readonly string DurationRoundPerTwoLevels = "Duration.RoundPerTwoLevels";

        internal static readonly string SavingThrowVaries = "SavingThrow.Varies";
        internal static readonly string SavingThrowReflexHalfWillNegates = "SavingThrow.ReflexHalf.WillNegates";
        internal static readonly string SavingThrowFortPartialOrWillNegates = "SavingThrow.FortPartial.Or.WillNegates";

        internal static readonly Logging.Logger Logger = Logging.GetLogger(LivingGrimoire.ArchetypeName);

        /// <summary>
        /// Adds a new BlueprintFeatureReplaceSpellbook for an archetype which alters a spellbook.
        /// </summary>
        /// <param name="characterClass">Guid / name of class for archetype</param>
        /// <param name="archetype">Guid / name of archetype</param>
        /// <param name="baseReplacement">Base class spellbook replacement</param>
        /// <param name="sourceReplacement">Spellbook replacement to copy from</param>
        /// <param name="replacementName">New replacement name</param>
        /// <param name="replacementGuid">New replacement guid</param>
        /// <param name="spellbook">Spellbook for new replacement</param>
        /// <param name="replacementSelection">Guid / name of replacement selection bp</param>
        internal static void ConfigureArchetypeSpellbookReplacement(
            string characterClass,
            string archetype,
            string baseReplacement,
            string sourceReplacement,
            string replacementName,
            string replacementGuid,
            BlueprintSpellbook spellbook,
            string replacementSelection,
            params Type[] typesToCopy)
        {
            Logger.Log($"Setting Prereqisites for {baseReplacement} Spellbook");
            FeatureReplaceSpellbookConfigurator.For(baseReplacement)
                .AddPrerequisiteNoArchetype(characterClass: characterClass, archetype: archetype)
                .Configure();
            var types = CommonTool.Append(typesToCopy, typeof(PrerequisiteClassSpellLevel));
            Logger.Log($"Creating {baseReplacement} Spellbook from Base Class");
            var replacement = FeatureReplaceSpellbookConfigurator.New(replacementName, replacementGuid)
                .CopyFrom(sourceReplacement, types)
                .AddPrerequisiteArchetypeLevel(characterClass: characterClass, archetype: archetype)
                .SetSpellbook(spellbook)
                .Configure();

            Logger.Log($"Adding {baseReplacement} Spellbook to {replacementSelection}");
            FeatureSelectionConfigurator.For(replacementSelection)
                .AddToAllFeatures(replacement)
                .SkipAddToSelections()
                .Configure();
        }

        /// <summary>
        /// Adds a new BlueprintProgression for an archetype which alters progression.
        /// </summary>
        /// <param name="characterClass">Guid / name of class for archetype</param>
        /// <param name="archetype">Guid / name of archetype</param>
        /// <param name="baseReplacement">Base class progression replacement</param>
        /// <param name="sourceReplacement">Progression replacement to copy from</param>
        /// <param name="replacementName">New replacement name</param>
        /// <param name="replacementGuid">New replacement guid</param>
        /// <param name="replacementSelection">Guid / name of replacement selection bp</param>
        internal static void ConfigureArchetypeProgressionReplacement(
            string characterClass,
            string archetype,
            string baseReplacement,
            string sourceReplacement,
            string replacementName,
            string replacementGuid,
            string replacementSelection,
            params Type[] typesToCopy)
        {
            Logger.Log($"Setting Prereqisites for {baseReplacement} Progression");
            ProgressionConfigurator.For(baseReplacement)
                .AddPrerequisiteNoArchetype(characterClass: characterClass, archetype: archetype)
                .Configure();
            var types = CommonTool.Append(typesToCopy, typeof(PrerequisiteClassSpellLevel));
            Logger.Log($"Creating {baseReplacement} Progression from Base Class");
            var replacement = ProgressionConfigurator.New(replacementName, replacementGuid)
                .CopyFrom(sourceReplacement, types)
                .AddPrerequisiteArchetypeLevel(characterClass: characterClass, archetype: archetype)
                .Configure();

            Logger.Log($"Adding {baseReplacement} Progression to {replacementSelection}");
            FeatureSelectionConfigurator.For(replacementSelection)
                .AddToAllFeatures(replacement)
                .SkipAddToSelections()
                .Configure();
        }

        /// <summary>
        /// Adds a new BlueprintFeatureReplaceSpellbook for an archetype which alters a spellbook.
        /// </summary>
        /// <param name="characterClass">Guid / name of class for archetype</param>
        /// <param name="archetype">Guid / name of archetype</param>
        /// <param name="baseReplacement">Base class progression replacement</param>
        /// <param name="classSpellLevel">Spell level to copy from Base Class</param>
        /// <param name="replacementName">New replacement name</param>
        /// <param name="replacementGuid">New replacement guid</param>
        /// <param name="spellbook">Spellbook for new replacement</param>
        /// <param name="replacementSelection">Guid / name of replacement selection bp</param>
        internal static void ConfigureArchetypeSpellbookReplacementFromProgression(
            string characterClass,
            string archetype,
            string baseReplacement,
            int classSpellLevel,
            string replacementName,
            string replacementGuid,
            BlueprintSpellbook spellbook,
            string replacementSelection)
        {
            Logger.Log($"Restricting {archetype} from using {baseReplacement} Progression");
            ProgressionConfigurator.For(baseReplacement)
                .AddPrerequisiteNoArchetype(characterClass: characterClass, archetype: archetype)
                .Configure(); 

            Logger.Log($"Creating {replacementGuid} Spellbook from Class Progression");
            var replacement = FeatureReplaceSpellbookConfigurator.New(replacementName, replacementGuid)
                .AddPrerequisiteClassSpellLevel(characterClass: characterClass, requiredSpellLevel: classSpellLevel)
                .AddPrerequisiteArchetypeLevel(characterClass: characterClass, archetype: archetype)
                .SetSpellbook(spellbook)
                .SetGroups()
                .Configure();

            Logger.Log($"Adding {replacementGuid} Spellbook to {replacementSelection}");
            FeatureSelectionConfigurator.For(replacementSelection)
                .AddToAllFeatures(replacement)
                .SkipAddToSelections()
                .Configure();
        }

        /// <summary>
        /// Adds the replace spellbook feature to the prerequisites for the specified secrets for loremaster.
        /// </summary>
        internal static void AddToLoremasterSecrets(string replaceSpellbook, params string[] secrets)
        {
            foreach (var secret in secrets)
            {
                ParametrizedFeatureConfigurator.For(secret)
                    .EditComponent<PrerequisiteFeaturesFromList>(
                        c =>
                            c.m_Features =
                                CommonTool.Append(c.m_Features, BlueprintTool.GetRef<BlueprintFeatureReference>(replaceSpellbook)))
                    .Configure();
            }
        }

        /// <summary>
        /// Updates the prereq to have the provided features in its IsPrerequisiteFor
        /// </summary>
        /// <param name="prereq">The feature which is a prereq for the features</param>
        /// <param name="features">Features the prereq leads to</param>
        internal static void AddIsPrequisiteFor(Blueprint<BlueprintReference<BlueprintFeature>> prereq, params Blueprint<BlueprintFeatureReference>[] features)
        {
            FeatureConfigurator.For(prereq).SkipAddToSelections().AddToIsPrerequisiteFor(features).Configure();
        }
    }
}
