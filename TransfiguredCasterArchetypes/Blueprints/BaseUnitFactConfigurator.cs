using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.Components.Replacements;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Assets;
using BlueprintCore.Utils.Types;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.RuleSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransfiguredCasterArchetypes.Archetypes;
using BlueprintCore.Blueprints.Configurators.Facts;
using BlueprintCore.Blueprints.Configurators.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;

namespace TransfiguredCasterArchetypes.Blueprints
{
    //
    // Summary:
    //     Configurator for Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.
    public class BuffConfigurator : BaseBuffConfigurator<BlueprintBuff, BuffConfigurator>
    {
        private BuffConfigurator(Blueprint<BlueprintReference<BlueprintBuff>> blueprint)
            : base(blueprint)
        {
        }

        //
        // Summary:
        //     Returns a configurator to modify the specified blueprint.
        //
        // Remarks:
        //     Use this to modify existing blueprints, such as blueprints from the base game.
        //     If you're using WrathModificationTemplate blueprints defined in JSON already
        //     exist.
        public static BuffConfigurator For(Blueprint<BlueprintReference<BlueprintBuff>> blueprint)
        {
            return new BuffConfigurator(blueprint);
        }

        //
        // Summary:
        //     Creates a new blueprint and returns a new configurator to modify it.
        //
        // Remarks:
        //     After creating a blueprint with this method you can use either name or GUID to
        //     reference the blueprint in BlueprintCore API calls.
        //     An implicit cast converts the string to BlueprintCore.Utils.Blueprint`1, exposing
        //     the blueprint instance and its reference.
        public static BuffConfigurator New(string name, string guid)
        {
            BlueprintTool.Create<BlueprintBuff>(name, guid);
            return For((Blueprint<BlueprintReference<BlueprintBuff>>)name);
        }

        public new BuffConfigurator SetRanks(int ranks)
        {
            base.SetRanks(ranks);
            return OnConfigureInternal(delegate (BlueprintBuff bp)
            {
                bp.Stacking = StackingType.Rank;
            });
        }
    }

    //
    // Summary:
    //     Implements common fields and components for blueprints inheriting from Kingmaker.Blueprints.Facts.BlueprintUnitFact.
    public abstract class BaseUnitFactConfigurator<T, TBuilder> : BaseFactConfigurator<T, TBuilder> where T : BlueprintUnitFact where TBuilder : BaseUnitFactConfigurator<T, TBuilder>
    {
        protected BaseUnitFactConfigurator(Blueprint<BlueprintReference<T>> blueprint) : base(blueprint)
        {
        }

        //
        // Summary:
        //     Sets the value of Kingmaker.Blueprints.Facts.BlueprintUnitFact.m_AllowNonContextActions
        public TBuilder SetAllowNonContextActions(bool allowNonContextActions = true)
        {
            return OnConfigureInternal(delegate (T bp)
            {
                bp.m_AllowNonContextActions = allowNonContextActions;
            });
        }

        //
        // Summary:
        //     Adds Kingmaker.UnitLogic.Mechanics.Components.SacredWeaponDamageOverride
        //
        // Parameters:
        //   feature:
        //     Blueprint of type BlueprintFeature. You can pass in the blueprint
        //     using:
        //     • A blueprint instance –
        //     • A blueprint reference –
        //     • A blueprint id as a string, Guid, or BlueprintGuid –
        //     • A blueprint name registered with BlueprintTool –
        //     See Blueprint for more details.
        //
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        public TBuilder AddSacredWordDamageOverride(Blueprint<BlueprintFeatureReference>? feature = null, DiceFormula? formula = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail)
        {
            var component = new SacredWordDamageOverride();
            component.m_Feature = feature?.Reference ?? component.m_Feature;
            if (component.m_Feature is null)
            {
                component.m_Feature = BlueprintTool.GetRef<BlueprintFeatureReference>(null);
            }
            component.Formula = formula ?? component.Formula;
            return AddUniqueComponent(component, mergeBehavior, merge);
        }

    }
}
