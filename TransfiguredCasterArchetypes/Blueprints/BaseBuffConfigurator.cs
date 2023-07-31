using System;
using System.Collections.Generic;
using System.Linq;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.Components.Replacements;
using BlueprintCore.Blueprints.Configurators.Facts;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Assets;
using BlueprintCore.Utils.Types;
using Kingmaker.Armies.TacticalCombat.Brain;
using Kingmaker.Armies.TacticalCombat.Components;
using Kingmaker.Armies.TacticalCombat.LeaderSkills;
using Kingmaker.Armies.TacticalCombat.LeaderSkills.UnitBuffComponents;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Corruption;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Buffs.HalfOfPair;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem;
using Kingmaker.UI.GenericSlot;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Class.Kineticist;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using Kingmaker.Visual;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Kingmaker.Visual.HitSystem;
using UnityEngine;

namespace TransfiguredCasterArchetypes.Blueprints
{
    //
    // Summary:
    //     Implements common fields and components for blueprints inheriting from Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.
    public abstract class BaseBuffConfigurator<T, TBuilder> : BaseUnitFactConfigurator<T, TBuilder> where T : BlueprintBuff where TBuilder : BaseBuffConfigurator<T, TBuilder>
    {
        protected BaseBuffConfigurator(Blueprint<BlueprintReference<T>> blueprint)
            : base(blueprint)
        {
        }

        //
        // Summary:
        //     Sets the value of Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Stacking
        //
        // Parameters:
        //   stacking:
        //     InfoBox: Replace - New buff removes existing buff and takes its place Prolong
        //     - Existing buff duration get prolonged, new buff is otherwise ignored Ignore
        //     - New buff is ignored Stack - Both buffs are added and function independently
        //     Poison - Special stacking type for poison Summ - Duration is added to current
        //     duration Rank - For buffs with limited stack
        //
        // Remarks:
        //     Use BlueprintCore.Blueprints.Configurators.UnitLogic.Buffs.BaseBuffConfigurator`2.SetRanks(System.Int32)
        //     for StackingType.Rank.
        public TBuilder SetStacking(StackingType stacking)
        {
            return OnConfigureInternal(delegate (T bp)
            {
                bp.Stacking = stacking;
            });
        }

        //
        // Summary:
        //     Sets the value of Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.IsClassFeature
        public TBuilder SetIsClassFeature(bool isClassFeature = true)
        {
            return OnConfigureInternal(delegate (T bp)
            {
                bp.IsClassFeature = isClassFeature;
            });
        }

        //
        // Summary:
        //     Sets the value of Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.m_Flags
        public TBuilder SetFlags(params BlueprintBuff.Flags[] flags)
        {
            BlueprintBuff.Flags[] flags2 = flags;
            return OnConfigureInternal(delegate (T bp)
            {
                bp.m_Flags = flags2.Aggregate((BlueprintBuff.Flags)0, (BlueprintBuff.Flags f1, BlueprintBuff.Flags f2) => f1 | f2);
            });
        }

        //
        // Summary:
        //     Adds to the contents of Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.m_Flags
        public TBuilder AddToFlags(params BlueprintBuff.Flags[] flags)
        {
            BlueprintBuff.Flags[] flags2 = flags;
            return OnConfigureInternal(delegate (T bp)
            {
                flags2.ForEach(delegate (BlueprintBuff.Flags f)
                {
                    bp.m_Flags |= f;
                });
            });
        }

        //
        // Summary:
        //     Removes elements from Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.m_Flags
        public TBuilder RemoveFromFlags(params BlueprintBuff.Flags[] flags)
        {
            BlueprintBuff.Flags[] flags2 = flags;
            return OnConfigureInternal(delegate (T bp)
            {
                flags2.ForEach(delegate (BlueprintBuff.Flags f)
                {
                    bp.m_Flags &= ~f;
                });
            });
        }

        //
        // Summary:
        //     Sets the value of Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Ranks
        public TBuilder SetRanks(int ranks)
        {
            return OnConfigureInternal(delegate (T bp)
            {
                bp.Ranks = ranks;
            });
        }

        //
        // Summary:
        //     Sets the value of Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.TickEachSecond
        public TBuilder SetTickEachSecond(bool tickEachSecond = true)
        {
            return OnConfigureInternal(delegate (T bp)
            {
                bp.TickEachSecond = tickEachSecond;
            });
        }

        //
        // Summary:
        //     Sets the value of Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Frequency
        public TBuilder SetFrequency(DurationRate frequency)
        {
            return OnConfigureInternal(delegate (T bp)
            {
                bp.Frequency = frequency;
            });
        }

        //
        // Summary:
        //     Sets the value of Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.FxOnStart
        //
        // Parameters:
        //   fxOnStart:
        //     You can pass in the animation using a PrefabLink or it's AssetId.
        public TBuilder SetFxOnStart(AssetLink<PrefabLink> fxOnStart)
        {
            AssetLink<PrefabLink> fxOnStart2 = fxOnStart;
            return OnConfigureInternal(delegate (T bp)
            {
                bp.FxOnStart = fxOnStart2?.Get();
            });
        }

        //
        // Summary:
        //     Modifies Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.FxOnStart by invoking
        //     the provided action.
        public TBuilder ModifyFxOnStart(Action<PrefabLink> action)
        {
            Action<PrefabLink> action2 = action;
            return OnConfigureInternal(delegate (T bp)
            {
                if ((object)bp.FxOnStart != null)
                {
                    action2(bp.FxOnStart);
                }
            });
        }

        //
        // Summary:
        //     Sets the value of Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.FxOnRemove
        //
        // Parameters:
        //   fxOnRemove:
        //     You can pass in the animation using a PrefabLink or it's AssetId.
        public TBuilder SetFxOnRemove(AssetLink<PrefabLink> fxOnRemove)
        {
            AssetLink<PrefabLink> fxOnRemove2 = fxOnRemove;
            return OnConfigureInternal(delegate (T bp)
            {
                bp.FxOnRemove = fxOnRemove2?.Get();
            });
        }

        //
        // Summary:
        //     Modifies Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.FxOnRemove by invoking
        //     the provided action.
        public TBuilder ModifyFxOnRemove(Action<PrefabLink> action)
        {
            Action<PrefabLink> action2 = action;
            return OnConfigureInternal(delegate (T bp)
            {
                if ((object)bp.FxOnRemove != null)
                {
                    action2(bp.FxOnRemove);
                }
            });
        }

        //
        // Summary:
        //     Sets the value of Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.ResourceAssetIds
        public TBuilder SetResourceAssetIds(params string[] resourceAssetIds)
        {
            string[] resourceAssetIds2 = resourceAssetIds;
            return OnConfigureInternal(delegate (T bp)
            {
                bp.ResourceAssetIds = resourceAssetIds2;
            });
        }

        //
        // Summary:
        //     Adds to the contents of Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.ResourceAssetIds
        public TBuilder AddToResourceAssetIds(params string[] resourceAssetIds)
        {
            string[] resourceAssetIds2 = resourceAssetIds;
            return OnConfigureInternal(delegate (T bp)
            {
                bp.ResourceAssetIds = bp.ResourceAssetIds ?? new string[0];
                bp.ResourceAssetIds = CommonTool.Append(bp.ResourceAssetIds, resourceAssetIds2);
            });
        }

        //
        // Summary:
        //     Removes elements from Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.ResourceAssetIds
        public TBuilder RemoveFromResourceAssetIds(params string[] resourceAssetIds)
        {
            string[] resourceAssetIds2 = resourceAssetIds;
            return OnConfigureInternal(delegate (T bp)
            {
                if (bp.ResourceAssetIds != null)
                {
                    bp.ResourceAssetIds = bp.ResourceAssetIds.Where((string val) => !resourceAssetIds2.Contains<string>(val)).ToArray();
                }
            });
        }

        //
        // Summary:
        //     Removes elements from Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.ResourceAssetIds
        //     that match the provided predicate.
        public TBuilder RemoveFromResourceAssetIds(Func<string, bool> predicate)
        {
            Func<string, bool> predicate2 = predicate;
            return OnConfigureInternal(delegate (T bp)
            {
                if (bp.ResourceAssetIds != null)
                {
                    bp.ResourceAssetIds = bp.ResourceAssetIds.Where((string e) => !predicate2(e)).ToArray();
                }
            });
        }

        //
        // Summary:
        //     Removes all elements from Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.ResourceAssetIds
        public TBuilder ClearResourceAssetIds()
        {
            return OnConfigureInternal(delegate (T bp)
            {
                bp.ResourceAssetIds = new string[0];
            });
        }

        //
        // Summary:
        //     Modifies Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.ResourceAssetIds
        //     by invoking the provided action on each element.
        public TBuilder ModifyResourceAssetIds(Action<string> action)
        {
            Action<string> action2 = action;
            return OnConfigureInternal(delegate (T bp)
            {
                if (bp.ResourceAssetIds != null)
                {
                    bp.ResourceAssetIds.ForEach<string>(action2);
                }
            });
        }

        //
        // Summary:
        //     Adds BlueprintCore.Blueprints.Configurators.UnitLogic.Buffs.BaseBuffConfigurator`2.AddContextStatBonus(Kingmaker.EntitySystem.Stats.StatType,Kingmaker.UnitLogic.Mechanics.ContextValue,System.Nullable{Kingmaker.Enums.ModifierDescriptor},System.Nullable{System.Int32},System.Nullable{System.Int32})
        //
        // Remarks:
        //     ComponentName: Add stat bonus
        //     • Used by
        //     • Abrogail_Feature_Prebuff –f0cad5e5b57b49f8b0983392a8c72eea
        //     • FiendflashShifterAspectGreaterDemonBuff –1165e9a30e4544d9956c9bc057d6783c
        //     • XantirOnlySwarm_MidnightFaneInThePastACFeature –5c0ef576cc68f374c96a0070fd3b047c
        public TBuilder AddContextStatBonus(StatType stat, ContextValue value, ModifierDescriptor? descriptor = null, int? minimal = null, int? multiplier = null)
        {
            AddContextStatBonus addContextStatBonus = new AddContextStatBonus();
            addContextStatBonus.Stat = stat;
            addContextStatBonus.Value = value;
            addContextStatBonus.Descriptor = descriptor ?? addContextStatBonus.Descriptor;
            addContextStatBonus.Minimal = minimal ?? addContextStatBonus.Minimal;
            addContextStatBonus.HasMinimal = !minimal.HasValue;
            addContextStatBonus.Multiplier = multiplier ?? addContextStatBonus.Multiplier;
            return AddComponent(addContextStatBonus);
        }

        //
        // Summary:
        //     Adds BlueprintCore.Blueprints.Configurators.UnitLogic.Buffs.BaseBuffConfigurator`2.AddEffectFastHealing(System.Int32,Kingmaker.UnitLogic.Mechanics.ContextValue,System.Action{Kingmaker.Blueprints.BlueprintComponent,Kingmaker.Blueprints.BlueprintComponent},BlueprintCore.Blueprints.CustomConfigurators.ComponentMerge)
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     ComponentName: Buffs/AddEffect/FastHealing
        //     • Used by
        //     • BlackLinnormStewBuff –4fbf48fff5bff9148accad55c116b8f2
        //     • FinneanArmourerArmorBuff –0d9604f466fee484ab05dcb6c435ab9f
        //     • VrolikaiAspectFeature –0ed608f1a0695cd4cb80bf6d415ab295
        public TBuilder AddEffectFastHealing(int heal, ContextValue? bonus = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail)
        {
            AddEffectFastHealing addEffectFastHealing = new AddEffectFastHealing();
            addEffectFastHealing.Heal = heal;
            addEffectFastHealing.Bonus = bonus ?? addEffectFastHealing.Bonus;
            if (addEffectFastHealing.Bonus == null)
            {
                addEffectFastHealing.Bonus = ContextValues.Constant(0);
            }

            return AddUniqueComponent(addEffectFastHealing, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.UnitLogic.Mechanics.Components.ContextRankConfig
        //
        // Remarks:
        //     Use BlueprintCore.Utils.Types.ContextRankConfigs to create the ContextRankConfig
        //     component.
        //     • Used by
        //     • 5_DeadStage_AcidBuff –96afbbab53c34c549a5313a1f7aed13b
        //     • HellsDecreeAbilityMagicEnchantmentBuff –e9e8867539c2b664d9e23de7c18dc912
        //     • ZoneOfPredeterminationArea –1ff4dfed4f7eb504fa0447e93d1bcf64
        public TBuilder AddContextRankConfig(ContextRankConfig component)
        {
            return AddComponent(component);
        }

        //
        // Summary:
        //     Adds Kingmaker.Blueprints.Classes.Spells.SpellDescriptorComponent
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • Abrikandilu_Frozen_Buff –b2df7031cdad480caddf962c894ca484
        //     • HideousLaughterTiefling –ae9e3a143e40f20419aa2b1ec92e2e06
        //     • ZachariusFearAuraBuff –4d9144b465bbefe4786cfe86c745ea4e
        public TBuilder AddSpellDescriptorComponent(SpellDescriptorWrapper descriptor, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail)
        {
            SpellDescriptorComponent spellDescriptorComponent = new SpellDescriptorComponent();
            spellDescriptorComponent.Descriptor = descriptor;
            return AddUniqueComponent(spellDescriptorComponent, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds BlueprintCore.Blueprints.Configurators.UnitLogic.Buffs.BaseBuffConfigurator`2.AddOutgoingDamageTriggerFixed(BlueprintCore.Blueprints.Components.Replacements.AddOutgoingDamageTriggerFixed)
        public TBuilder AddOutgoingDamageTriggerFixed(AddOutgoingDamageTriggerFixed component)
        {
            return AddComponent(component);
        }

        //
        // Summary:
        //     Adds BlueprintCore.Blueprints.Configurators.UnitLogic.Buffs.BaseBuffConfigurator`2.AddStatBonusIfHasFactFixed(BlueprintCore.Blueprints.Components.Replacements.AddStatBonusIfHasFactFixed)
        public TBuilder AddStatBonusIfHasFactFixed(AddStatBonusIfHasFactFixed component)
        {
            return AddComponent(component);
        }

        //
        // Summary:
        //     Adds Kingmaker.Corruption.GlobalMapSpeedModifier
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • DivineServiceSpeedModifier –23c769eaac4409742a786a157ea96273
        public TBuilder AddGlobalMapSpeedModifier(Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail, float? speedModifier = null)
        {
            GlobalMapSpeedModifier globalMapSpeedModifier = new GlobalMapSpeedModifier();
            globalMapSpeedModifier.SpeedModifier = speedModifier ?? globalMapSpeedModifier.SpeedModifier;
            return AddUniqueComponent(globalMapSpeedModifier, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.Blueprints.Classes.Spells.ArmorWeightCoef
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • PurifierCelestialArmorFeature –7dc8d7dede2704640956f7bc4102760a
        public TBuilder AddArmorWeightCoef(Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail, float? weightCoef = null)
        {
            ArmorWeightCoef armorWeightCoef = new ArmorWeightCoef();
            armorWeightCoef.WeightCoef = weightCoef ?? armorWeightCoef.WeightCoef;
            return AddUniqueComponent(armorWeightCoef, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.Blueprints.Classes.Spells.ChangeHitFxType
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • LannSparringBuff –0b87395f642f67048aafeaf65146edb0
        public TBuilder AddChangeHitFxType(Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail, BloodType? overrideBloodType = null)
        {
            ChangeHitFxType changeHitFxType = new ChangeHitFxType();
            changeHitFxType.OverrideBloodType = overrideBloodType ?? changeHitFxType.OverrideBloodType;
            return AddUniqueComponent(changeHitFxType, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds BlueprintCore.Blueprints.Configurators.UnitLogic.Buffs.BaseBuffConfigurator`2.AddForceMove(Kingmaker.UnitLogic.Mechanics.ContextValue,System.Action{Kingmaker.Blueprints.BlueprintComponent,Kingmaker.Blueprints.BlueprintComponent},BlueprintCore.Blueprints.CustomConfigurators.ComponentMerge)
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • TsunamiBuff –1694ee72db34ecf49a63005e845e175d
        public TBuilder AddForceMove(ContextValue? feetPerRound = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail)
        {
            AddForceMove addForceMove = new AddForceMove();
            addForceMove.FeetPerRound = feetPerRound ?? addForceMove.FeetPerRound;
            if (addForceMove.FeetPerRound == null)
            {
                addForceMove.FeetPerRound = ContextValues.Constant(0);
            }

            return AddUniqueComponent(addForceMove, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds BlueprintCore.Blueprints.Configurators.UnitLogic.Buffs.BaseBuffConfigurator`2.AddGoldenDragonSkillBonus(System.Nullable{Kingmaker.Enums.ModifierDescriptor},System.Nullable{Kingmaker.EntitySystem.Stats.StatType})
        //
        // Remarks:
        //     ComponentName: Add stat bonus
        //     • Used by
        //     • GoldenDragonSharedSkillUMD –4e0b919e4fbd85142bce959fae129d1a
        public TBuilder AddGoldenDragonSkillBonus(ModifierDescriptor? descriptor = null, StatType? stat = null)
        {
            AddGoldenDragonSkillBonus addGoldenDragonSkillBonus = new AddGoldenDragonSkillBonus();
            addGoldenDragonSkillBonus.Descriptor = descriptor ?? addGoldenDragonSkillBonus.Descriptor;
            addGoldenDragonSkillBonus.Stat = stat ?? addGoldenDragonSkillBonus.Stat;
            return AddComponent(addGoldenDragonSkillBonus);
        }

        //
        // Summary:
        //     Adds BlueprintCore.Blueprints.Configurators.UnitLogic.Buffs.BaseBuffConfigurator`2.AddRestTrigger(BlueprintCore.Actions.Builder.ActionsBuilder,System.Action{Kingmaker.Blueprints.BlueprintComponent,Kingmaker.Blueprints.BlueprintComponent},BlueprintCore.Blueprints.CustomConfigurators.ComponentMerge)
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • ArcanistArcaneReservoirFeature –55db1859bd72fd04f9bd3fe1f10e4cbb
        //     • Player_restTrigger –ac7f1eff7837432e8acdccd52308c09b
        //     • TricksterLoreNature3Feature –b88ca3a5476ebcc4ea63d5c1e92ce222
        public TBuilder AddRestTrigger(ActionsBuilder? action = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail)
        {
            AddRestTrigger addRestTrigger = new AddRestTrigger();
            addRestTrigger.Action = action?.Build() ?? addRestTrigger.Action;
            if (addRestTrigger.Action == null)
            {
                addRestTrigger.Action = Constants.Empty.Actions;
            }

            return AddUniqueComponent(addRestTrigger, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds BlueprintCore.Blueprints.Configurators.UnitLogic.Buffs.BaseBuffConfigurator`2.AddRunwayLogic(System.Action{Kingmaker.Blueprints.BlueprintComponent,Kingmaker.Blueprints.BlueprintComponent},BlueprintCore.Blueprints.CustomConfigurators.ComponentMerge)
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • SecretOfSubduingRunwayBuff –a4d60878d85d652489c5b0fd9b11e1ac
        public TBuilder AddRunwayLogic(Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail)
        {
            AddRunwayLogic component = new AddRunwayLogic();
            return AddUniqueComponent(component, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds BlueprintCore.Blueprints.Configurators.UnitLogic.Buffs.BaseBuffConfigurator`2.AddTemporaryFeat(BlueprintCore.Utils.Blueprint{Kingmaker.Blueprints.BlueprintFeatureReference})
        //
        // Parameters:
        //   feat:
        //     Blueprint of type BlueprintFeature. You can pass in the blueprint using:
        //     • A blueprint instance –
        //     • A blueprint reference –
        //     • A blueprint id as a string, Guid, or BlueprintGuid –
        //     • A blueprint name registered with BlueprintTool –
        //     See Blueprint for more details.
        //
        // Remarks:
        //     • Used by
        //     • DivineHuntersBlessingBuff –493b217b69ed4ad4ca3dc71983a356c6
        //     • PackRagerPreciseStrikeBuff –1eb018f9dcbbc2547babb490c6a362a6
        //     • TriceratopsStatuetteBuff –b0b20d062e5419a43b5c0c829cdfcd8d
        public TBuilder AddTemporaryFeat(Blueprint<BlueprintFeatureReference>? feat = null)
        {
            AddTemporaryFeat addTemporaryFeat = new AddTemporaryFeat();
            addTemporaryFeat.m_Feat = feat?.Reference ?? addTemporaryFeat.m_Feat;
            if (addTemporaryFeat.m_Feat == null)
            {
                addTemporaryFeat.m_Feat = BlueprintTool.GetRef<BlueprintFeatureReference>(null);
            }

            return AddComponent(addTemporaryFeat);
        }

        //
        // Summary:
        //     Adds BlueprintCore.Blueprints.Configurators.UnitLogic.Buffs.BaseBuffConfigurator`2.AddWeaponEnhancementBonusToStat(System.Nullable{Kingmaker.Enums.ModifierDescriptor},System.Nullable{System.Int32},System.Nullable{Kingmaker.EntitySystem.Stats.StatType})
        //
        // Remarks:
        //     • Used by
        //     • PerfectStormFeature –f93deb8fb11e06743b6941626cd6f2e0
        public TBuilder AddWeaponEnhancementBonusToStat(ModifierDescriptor? descriptor = null, int? multiplier = null, StatType? stat = null)
        {
            AddWeaponEnhancementBonusToStat addWeaponEnhancementBonusToStat = new AddWeaponEnhancementBonusToStat();
            addWeaponEnhancementBonusToStat.Descriptor = descriptor ?? addWeaponEnhancementBonusToStat.Descriptor;
            addWeaponEnhancementBonusToStat.Multiplier = multiplier ?? addWeaponEnhancementBonusToStat.Multiplier;
            addWeaponEnhancementBonusToStat.Stat = stat ?? addWeaponEnhancementBonusToStat.Stat;
            return AddComponent(addWeaponEnhancementBonusToStat);
        }

        //
        // Summary:
        //     Adds Kingmaker.UnitLogic.FactLogic.BuffEnchantArmor
        //
        // Parameters:
        //   enchantments:
        //     Blueprint of type BlueprintItemEnchantment. You can pass in the blueprint using:
        //     • A blueprint instance –
        //     • A blueprint reference –
        //     • A blueprint id as a string, Guid, or BlueprintGuid –
        //     • A blueprint name registered with BlueprintTool –
        //     See Blueprint for more details.
        //
        // Remarks:
        //     • Used by
        //     • AasimarRedMask_MagicalVestmentArmorBuff –2d6f3b09fddf442da939f66c751b1b14
        //     • Inquisitor_Liotr_MagicalVestmentShieldBuff –af3878638a198144faa4f8ff660c09a8
        //     • MagicalVestmentShieldBuff –2e8446f820936a44f951b50d70a82b16
        public TBuilder AddBuffEnchantArmor(List<Blueprint<BlueprintItemEnchantmentReference>>? enchantments = null, BuffEnchantArmor.ItemType? itemType = null, BuffScaling? scaling = null)
        {
            BuffEnchantArmor buffEnchantArmor = new BuffEnchantArmor();
            buffEnchantArmor.m_Enchantments = enchantments?.Select((Blueprint<BlueprintItemEnchantmentReference> bp) => bp.Reference)?.ToArray() ?? buffEnchantArmor.m_Enchantments;
            if (buffEnchantArmor.m_Enchantments == null)
            {
                buffEnchantArmor.m_Enchantments = new BlueprintItemEnchantmentReference[0];
            }

            buffEnchantArmor.m_ItemType = itemType ?? buffEnchantArmor.m_ItemType;
            Validate(scaling);
            buffEnchantArmor.m_Scaling = scaling ?? buffEnchantArmor.m_Scaling;
            return AddComponent(buffEnchantArmor);
        }

        //
        // Summary:
        //     Adds Kingmaker.UnitLogic.FactLogic.BuffEnchantSpecificWeaponWorn
        //
        // Parameters:
        //   enchantmentBlueprint:
        //     Blueprint of type BlueprintItemEnchantment. You can pass in the blueprint using:
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
        //
        //   weaponBlueprint:
        //     Blueprint of type BlueprintItemWeapon. You can pass in the blueprint using:
        //     • A blueprint instance –
        //     • A blueprint reference –
        //     • A blueprint id as a string, Guid, or BlueprintGuid –
        //     • A blueprint name registered with BlueprintTool –
        //     See Blueprint for more details.
        //
        // Remarks:
        //     • Used by
        //     • MightyBlowOfGoodBuff –864a5775ceaed974f9f8c3acc1d5dcd1
        //     • RadianceGoodBuff6 –7b860e2d2b8542e1adc0ba562bed3a6b
        //     • TheUndyingLoveOfTheHopebringer_LightNova_GiveBuff –8c9b07c246a3469e8c154d98ee594ee3
        public TBuilder AddBuffEnchantSpecificWeaponWorn(Blueprint<BlueprintItemEnchantmentReference>? enchantmentBlueprint = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail, Blueprint<BlueprintItemWeaponReference>? weaponBlueprint = null)
        {
            BuffEnchantSpecificWeaponWorn buffEnchantSpecificWeaponWorn = new BuffEnchantSpecificWeaponWorn();
            buffEnchantSpecificWeaponWorn.m_EnchantmentBlueprint = enchantmentBlueprint?.Reference ?? buffEnchantSpecificWeaponWorn.m_EnchantmentBlueprint;
            if (buffEnchantSpecificWeaponWorn.m_EnchantmentBlueprint == null)
            {
                buffEnchantSpecificWeaponWorn.m_EnchantmentBlueprint = BlueprintTool.GetRef<BlueprintItemEnchantmentReference>(null);
            }

            buffEnchantSpecificWeaponWorn.m_WeaponBlueprint = weaponBlueprint?.Reference ?? buffEnchantSpecificWeaponWorn.m_WeaponBlueprint;
            if (buffEnchantSpecificWeaponWorn.m_WeaponBlueprint == null)
            {
                buffEnchantSpecificWeaponWorn.m_WeaponBlueprint = BlueprintTool.GetRef<BlueprintItemWeaponReference>(null);
            }

            return AddUniqueComponent(buffEnchantSpecificWeaponWorn, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.UnitLogic.FactLogic.BuffEnchantWornItem
        //
        // Parameters:
        //   enchantmentBlueprint:
        //     Blueprint of type BlueprintItemEnchantment. You can pass in the blueprint using:
        //     • A blueprint instance –
        //     • A blueprint reference –
        //     • A blueprint id as a string, Guid, or BlueprintGuid –
        //     • A blueprint name registered with BlueprintTool –
        //     See Blueprint for more details.
        //
        // Remarks:
        //     • Used by
        //     • AlignWeaponChaosBuff –2f2eb113da65b6b4fbf69e35c90afe02
        //     • Inquisitor_Buff_Bane –4efb6790e1edf3c419bad6b52f36e32f
        //     • WeaponOfDeathBuff –a8c1e364f631f8a46b8ef23a5528c806
        public TBuilder AddBuffEnchantWornItem(bool? allWeapons = null, Blueprint<BlueprintItemEnchantmentReference>? enchantmentBlueprint = null, EquipSlotBase.SlotType? slot = null)
        {
            BuffEnchantWornItem buffEnchantWornItem = new BuffEnchantWornItem();
            buffEnchantWornItem.AllWeapons = allWeapons ?? buffEnchantWornItem.AllWeapons;
            buffEnchantWornItem.m_EnchantmentBlueprint = enchantmentBlueprint?.Reference ?? buffEnchantWornItem.m_EnchantmentBlueprint;
            if (buffEnchantWornItem.m_EnchantmentBlueprint == null)
            {
                buffEnchantWornItem.m_EnchantmentBlueprint = BlueprintTool.GetRef<BlueprintItemEnchantmentReference>(null);
            }

            buffEnchantWornItem.Slot = slot ?? buffEnchantWornItem.Slot;
            return AddComponent(buffEnchantWornItem);
        }

        //
        // Summary:
        //     Adds Kingmaker.UnitLogic.FactLogic.DropLootAndDestroyOnDeactivate
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • DismemberedUnitBuff –c98d765d063f57a49a03f13d4f697c33
        public TBuilder AddDropLootAndDestroyOnDeactivate(IDisposable? coroutine = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail)
        {
            DropLootAndDestroyOnDeactivate dropLootAndDestroyOnDeactivate = new DropLootAndDestroyOnDeactivate();
            Validate(coroutine);
            dropLootAndDestroyOnDeactivate.m_Coroutine = coroutine ?? dropLootAndDestroyOnDeactivate.m_Coroutine;
            return AddUniqueComponent(dropLootAndDestroyOnDeactivate, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.UnitLogic.FactLogic.RemoveBuffIfPartyNotInCombat
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • BootsOfMagicalWhirlCountBuff –815fa1c6d2dc4c56859bfa84eee96107
        //     • DisarmMainHandBuff –f7db19748af8b69469073485a65f37cf
        //     • ShiftersRushBuff –c3365d5a75294b9b879c587668620bd4
        public TBuilder AddRemoveBuffIfPartyNotInCombat(Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail)
        {
            RemoveBuffIfPartyNotInCombat component = new RemoveBuffIfPartyNotInCombat();
            return AddUniqueComponent(component, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.UnitLogic.FactLogic.SetMagusFeatureActive
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • SpellCombatBuff –91e4b45ab5f29574aa1fb41da4bbdcf2
        //     • SpellStrikeBuff –06e0c9887eb1724409977dac7168bfd7
        public TBuilder AddSetMagusFeatureActive(SetMagusFeatureActive.FeatureType? feature = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail)
        {
            SetMagusFeatureActive setMagusFeatureActive = new SetMagusFeatureActive();
            setMagusFeatureActive.m_Feature = feature ?? setMagusFeatureActive.m_Feature;
            return AddUniqueComponent(setMagusFeatureActive, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.UnitLogic.FactLogic.ShifterGrabInitiatorBuff
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • BearGrappledInitiatorBuff –0b2215c8c7d34554a53f970310ac3f35
        //     • LizardGrappledInitiatorBuff –0498769dcce4428bb0ae8b22693e47db
        //     • TigerGrappledInitiatorBuff –652a71aaa0c3492db4ee5b006dfc18fb
        public TBuilder AddShifterGrabInitiatorBuff(int? attackRollBonus = null, int? dexterityBonus = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail)
        {
            ShifterGrabInitiatorBuff shifterGrabInitiatorBuff = new ShifterGrabInitiatorBuff();
            shifterGrabInitiatorBuff.m_AttackRollBonus = attackRollBonus ?? shifterGrabInitiatorBuff.m_AttackRollBonus;
            shifterGrabInitiatorBuff.m_DexterityBonus = dexterityBonus ?? shifterGrabInitiatorBuff.m_DexterityBonus;
            return AddUniqueComponent(shifterGrabInitiatorBuff, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.UnitLogic.FactLogic.ShifterGrabTargetBuff
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • BearGrappledTargetBuff –88be6cbfaf534e009c501e1d2ef3c1f6
        //     • LizardGrappledTargetBuff –adc4348649814455a3d8a9f0c837c62e
        //     • TigerGrappledTargetBuff –a55e4d7febc3488cbb087c632f83fc52
        public TBuilder AddShifterGrabTargetBuff(int? attackRollBonus = null, int? dexterityBonus = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail, int? pinnedACBonus = null)
        {
            ShifterGrabTargetBuff shifterGrabTargetBuff = new ShifterGrabTargetBuff();
            shifterGrabTargetBuff.m_AttackRollBonus = attackRollBonus ?? shifterGrabTargetBuff.m_AttackRollBonus;
            shifterGrabTargetBuff.m_DexterityBonus = dexterityBonus ?? shifterGrabTargetBuff.m_DexterityBonus;
            shifterGrabTargetBuff.m_PinnedACBonus = pinnedACBonus ?? shifterGrabTargetBuff.m_PinnedACBonus;
            return AddUniqueComponent(shifterGrabTargetBuff, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.UnitLogic.FactLogic.SuppressBuffInSafeZone
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • ShifterWildShapeGriffonBuff –e76d475eb1f1470e9950a5fee99ddb40
        //     • ShifterWildShapeGriffonDemonBuff14 –431ca9188d6f401f9f8df8079c526e59
        //     • ShifterWildShapeGriffonGodBuff9 –d8b979bf19554b85bbed05e6369c0f63
        public TBuilder AddSuppressBuffInSafeZone(Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail)
        {
            SuppressBuffInSafeZone component = new SuppressBuffInSafeZone();
            return AddUniqueComponent(component, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.UnitLogic.FactLogic.UniqueBuff
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • AngelSwordEffectBuff –f5f500d6a2a39fc4181af32ad79af488
        //     • ProfaneAscensionConstitutionBuff1 –4bd5006416df4d13982bb957e82ae8ed
        //     • ZeorisDaggerRing_GoverningBuff –e248e5ef1ae04d559d5fe82ef719ee47
        public TBuilder AddUniqueBuff(Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail)
        {
            UniqueBuff component = new UniqueBuff();
            return AddUniqueComponent(component, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds BlueprintCore.Blueprints.Configurators.UnitLogic.Buffs.BaseBuffConfigurator`2.AddKineticistBlade(BlueprintCore.Utils.Blueprint{Kingmaker.Blueprints.BlueprintItemWeaponReference},System.Action{Kingmaker.Blueprints.BlueprintComponent,Kingmaker.Blueprints.BlueprintComponent},BlueprintCore.Blueprints.CustomConfigurators.ComponentMerge)
        //
        // Parameters:
        //   blade:
        //     Blueprint of type BlueprintItemWeapon. You can pass in the blueprint using:
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
        //
        // Remarks:
        //     • Used by
        //     • KineticBladeAirBlastBuff –f7a55ccd8553f974a89482dd98672bbc
        //     • KineticBladeIceBlastBuff –9e7a7d7e8334a5748a8834b0116cf6c4
        //     • KineticBladeWaterBlastBuff –abe55a6590a056f4e8161802ae2b43c5
        public TBuilder AddKineticistBlade(Blueprint<BlueprintItemWeaponReference>? blade = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail)
        {
            AddKineticistBlade addKineticistBlade = new AddKineticistBlade();
            addKineticistBlade.m_Blade = blade?.Reference ?? addKineticistBlade.m_Blade;
            if (addKineticistBlade.m_Blade == null)
            {
                addKineticistBlade.m_Blade = BlueprintTool.GetRef<BlueprintItemWeaponReference>(null);
            }

            return AddUniqueComponent(addKineticistBlade, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.UnitLogic.Buffs.Polymorph
        //
        // Parameters:
        //   additionalLimbs:
        //     Blueprint of type BlueprintItemWeapon. You can pass in the blueprint using:
        //     • A blueprint instance –
        //     • A blueprint reference –
        //     • A blueprint id as a string, Guid, or BlueprintGuid –
        //     • A blueprint name registered with BlueprintTool –
        //     See Blueprint for more details.
        //
        //   allowDamageTransfer:
        //     InfoBox: If checked some components can partially transfer damage characteristics
        //     from special weapons to the natural weapons of the polymorph. E.g. component
        //     &apos;PolymorphDamageTransfer&apos; used for ShifterClawBuffLevelNN will take
        //     some characteristics of the claws and apply them to attacks of the weapons in
        //     the SecondaryAdditionalLimbs slots.
        //
        //   facts:
        //     Blueprint of type BlueprintUnitFact. You can pass in the blueprint using:
        //     • A blueprint instance –
        //     • A blueprint reference –
        //     • A blueprint id as a string, Guid, or BlueprintGuid –
        //     • A blueprint name registered with BlueprintTool –
        //     See Blueprint for more details.
        //
        //   mainHand:
        //     Blueprint of type BlueprintItemWeapon. You can pass in the blueprint using:
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
        //
        //   offHand:
        //     Blueprint of type BlueprintItemWeapon. You can pass in the blueprint using:
        //     • A blueprint instance –
        //     • A blueprint reference –
        //     • A blueprint id as a string, Guid, or BlueprintGuid –
        //     • A blueprint name registered with BlueprintTool –
        //     See Blueprint for more details.
        //
        //   portrait:
        //     Blueprint of type BlueprintPortrait. You can pass in the blueprint using:
        //     • A blueprint instance –
        //     • A blueprint reference –
        //     • A blueprint id as a string, Guid, or BlueprintGuid –
        //     • A blueprint name registered with BlueprintTool –
        //     See Blueprint for more details.
        //
        //   prefab:
        //     You can pass in the animation using a UnitViewLink or it's AssetId.
        //
        //   prefabFemale:
        //     You can pass in the animation using a UnitViewLink or it's AssetId.
        //
        //   race:
        //     Blueprint of type BlueprintRace. You can pass in the blueprint using:
        //     • A blueprint instance –
        //     • A blueprint reference –
        //     • A blueprint id as a string, Guid, or BlueprintGuid –
        //     • A blueprint name registered with BlueprintTool –
        //     See Blueprint for more details.
        //
        //   replaceUnitForInspection:
        //     Blueprint of type BlueprintUnit. You can pass in the blueprint using:
        //     • A blueprint instance –
        //     • A blueprint reference –
        //     • A blueprint id as a string, Guid, or BlueprintGuid –
        //     • A blueprint name registered with BlueprintTool –
        //     See Blueprint for more details.
        //
        //   secondaryAdditionalLimbs:
        //     Blueprint of type BlueprintItemWeapon. You can pass in the blueprint using:
        //     • A blueprint instance –
        //     • A blueprint reference –
        //     • A blueprint id as a string, Guid, or BlueprintGuid –
        //     • A blueprint name registered with BlueprintTool –
        //     See Blueprint for more details.
        //
        //   useSizeAsBaseForDamage:
        //     InfoBox: If checked, the Size value will be used as base size instead of the
        //     &apos;Medium&apos; size.
        //
        // Remarks:
        //     • Used by
        //     • Anevia_DressPolymorph –6267b23ce31a4ad8b1b3557826671708
        //     • Nidalynn_PolymorphBuff –885ee6e3ecd2409bbda4f6ecfe914c6d
        //     • YozzPolymorfBuff –ed4e29772921bc84098f1a9a1dcc3ddb
        public TBuilder AddPolymorph(List<Blueprint<BlueprintItemWeaponReference>>? additionalLimbs = null, bool? allowDamageTransfer = null, int? constitutionBonus = null, int? dexterityBonus = null, Polymorph.VisualTransitionSettings? enterTransition = null, Polymorph.VisualTransitionSettings? exitTransition = null, List<Blueprint<BlueprintUnitFactReference>>? facts = null, bool? keepSlots = null, Blueprint<BlueprintItemWeaponReference>? mainHand = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail, int? naturalArmor = null, Blueprint<BlueprintItemWeaponReference>? offHand = null, Blueprint<BlueprintPortraitReference>? portrait = null, PortraitTypeEntry? portraitTypeEntry = null, AssetLink<UnitViewLink>? prefab = null, AssetLink<UnitViewLink>? prefabFemale = null, Blueprint<BlueprintRaceReference>? race = null, Blueprint<BlueprintUnitReference>? replaceUnitForInspection = null, List<Blueprint<BlueprintItemWeaponReference>>? secondaryAdditionalLimbs = null, bool? silentCaster = null, Size? size = null, SpecialDollType? specialDollType = null, int? strengthBonus = null, PolymorphTransitionSettings? transitionExternal = null, bool? useSizeAsBaseForDamage = null)
        {
            Polymorph polymorph = new Polymorph();
            polymorph.m_AdditionalLimbs = additionalLimbs?.Select((Blueprint<BlueprintItemWeaponReference> bp) => bp.Reference)?.ToArray() ?? polymorph.m_AdditionalLimbs;
            if (polymorph.m_AdditionalLimbs == null)
            {
                polymorph.m_AdditionalLimbs = new BlueprintItemWeaponReference[0];
            }

            polymorph.AllowDamageTransfer = allowDamageTransfer ?? polymorph.AllowDamageTransfer;
            polymorph.ConstitutionBonus = constitutionBonus ?? polymorph.ConstitutionBonus;
            polymorph.DexterityBonus = dexterityBonus ?? polymorph.DexterityBonus;
            Validate(enterTransition);
            polymorph.m_EnterTransition = enterTransition ?? polymorph.m_EnterTransition;
            Validate(exitTransition);
            polymorph.m_ExitTransition = exitTransition ?? polymorph.m_ExitTransition;
            polymorph.m_Facts = facts?.Select((Blueprint<BlueprintUnitFactReference> bp) => bp.Reference)?.ToArray() ?? polymorph.m_Facts;
            if (polymorph.m_Facts == null)
            {
                polymorph.m_Facts = new BlueprintUnitFactReference[0];
            }

            polymorph.m_KeepSlots = keepSlots ?? polymorph.m_KeepSlots;
            polymorph.m_MainHand = mainHand?.Reference ?? polymorph.m_MainHand;
            if (polymorph.m_MainHand == null)
            {
                polymorph.m_MainHand = BlueprintTool.GetRef<BlueprintItemWeaponReference>(null);
            }

            polymorph.NaturalArmor = naturalArmor ?? polymorph.NaturalArmor;
            polymorph.m_OffHand = offHand?.Reference ?? polymorph.m_OffHand;
            if (polymorph.m_OffHand == null)
            {
                polymorph.m_OffHand = BlueprintTool.GetRef<BlueprintItemWeaponReference>(null);
            }

            polymorph.m_Portrait = portrait?.Reference ?? polymorph.m_Portrait;
            if (polymorph.m_Portrait == null)
            {
                polymorph.m_Portrait = BlueprintTool.GetRef<BlueprintPortraitReference>(null);
            }

            polymorph.m_Prefab = prefab?.Get() ?? polymorph.m_Prefab;
            polymorph.m_PrefabFemale = prefabFemale?.Get() ?? polymorph.m_PrefabFemale;
            polymorph.m_Race = race?.Reference ?? polymorph.m_Race;
            if (polymorph.m_Race == null)
            {
                polymorph.m_Race = BlueprintTool.GetRef<BlueprintRaceReference>(null);
            }

            polymorph.m_ReplaceUnitForInspection = replaceUnitForInspection?.Reference ?? polymorph.m_ReplaceUnitForInspection;
            if (polymorph.m_ReplaceUnitForInspection == null)
            {
                polymorph.m_ReplaceUnitForInspection = BlueprintTool.GetRef<BlueprintUnitReference>(null);
            }

            polymorph.m_SecondaryAdditionalLimbs = secondaryAdditionalLimbs?.Select((Blueprint<BlueprintItemWeaponReference> bp) => bp.Reference)?.ToArray() ?? polymorph.m_SecondaryAdditionalLimbs;
            if (polymorph.m_SecondaryAdditionalLimbs == null)
            {
                polymorph.m_SecondaryAdditionalLimbs = new BlueprintItemWeaponReference[0];
            }

            polymorph.m_SilentCaster = silentCaster ?? polymorph.m_SilentCaster;
            polymorph.Size = size ?? polymorph.Size;
            polymorph.m_SpecialDollType = specialDollType ?? polymorph.m_SpecialDollType;
            polymorph.StrengthBonus = strengthBonus ?? polymorph.StrengthBonus;
            Validate(transitionExternal);
            polymorph.m_TransitionExternal = transitionExternal ?? polymorph.m_TransitionExternal;
            polymorph.UseSizeAsBaseForDamage = useSizeAsBaseForDamage ?? polymorph.UseSizeAsBaseForDamage;
            return AddUniqueComponent(polymorph, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.UnitLogic.Buffs.PolymorphBonuses
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • AnimalAspectGorillaBuff –2b0b1321fdc53df4dabae1cbf33d46f4
        //     • AspectOfTheStagBuff –624ed102958178c4781ce19531d51281
        //     • IronBodyBuff –2eabea6a1f9a58246a822f207e8ca79e
        public TBuilder AddPolymorphBonuses(int? masterShifterBonus = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail)
        {
            PolymorphBonuses polymorphBonuses = new PolymorphBonuses();
            polymorphBonuses.masterShifterBonus = masterShifterBonus ?? polymorphBonuses.masterShifterBonus;
            return AddUniqueComponent(polymorphBonuses, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.UnitLogic.Buffs.RemoveBuffOnLoad
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • BurnFireBuff –55ed3454d637b074e829ae664452a412
        //     • DLC3_BrokenTricksterFlowerBuff –bfe6e5f25a8d43aeae84c416db984ac1
        //     • SmiteEvilBuff –b6570b8cbb32eaf4ca8255d0ec3310b0
        public TBuilder AddRemoveBuffOnLoad(Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail, bool? onlyFromParty = null)
        {
            RemoveBuffOnLoad removeBuffOnLoad = new RemoveBuffOnLoad();
            removeBuffOnLoad.OnlyFromParty = onlyFromParty ?? removeBuffOnLoad.OnlyFromParty;
            return AddUniqueComponent(removeBuffOnLoad, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.UnitLogic.Buffs.RemoveBuffOnTurnOn
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • FadeInOut –9c9da5cde5e7e5b488c7d48e86b1d99f
        //     • Galfrey_AfterCouncilPolymorph –727d5a386464485ead4b2919b4003b1e
        //     • NPC_Switch2Neutral –a121ed5d673eaa94880d0b38a72787ef
        public TBuilder AddRemoveBuffOnTurnOn(Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail, bool? onlyFromParty = null)
        {
            RemoveBuffOnTurnOn removeBuffOnTurnOn = new RemoveBuffOnTurnOn();
            removeBuffOnTurnOn.OnlyFromParty = onlyFromParty ?? removeBuffOnTurnOn.OnlyFromParty;
            return AddUniqueComponent(removeBuffOnTurnOn, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds BlueprintCore.Blueprints.Configurators.UnitLogic.Buffs.BaseBuffConfigurator`2.AddAreaEffect(BlueprintCore.Utils.Blueprint{Kingmaker.Blueprints.BlueprintAbilityAreaEffectReference},System.Action{Kingmaker.Blueprints.BlueprintComponent,Kingmaker.Blueprints.BlueprintComponent},BlueprintCore.Blueprints.CustomConfigurators.ComponentMerge)
        //
        // Parameters:
        //   areaEffect:
        //     Blueprint of type BlueprintAbilityAreaEffect. You can pass in the blueprint using:
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
        //
        // Remarks:
        //     • Used by
        //     • AeonAoOGazeBaseBuff –da22fac7ee174766a1d924749245b8e9
        //     • GolemSummerEntangleAuraBuff –f8ff09d49cdc6a64dbfeca21031bca49
        //     • ZeorisDaggerRing_BetrayalAreaBuff –b2b739c6e18b457d9ba30ab389b0520f
        public TBuilder AddAreaEffect(Blueprint<BlueprintAbilityAreaEffectReference>? areaEffect = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail)
        {
            AddAreaEffect addAreaEffect = new AddAreaEffect();
            addAreaEffect.m_AreaEffect = areaEffect?.Reference ?? addAreaEffect.m_AreaEffect;
            if (addAreaEffect.m_AreaEffect == null)
            {
                addAreaEffect.m_AreaEffect = BlueprintTool.GetRef<BlueprintAbilityAreaEffectReference>(null);
            }

            return AddUniqueComponent(addAreaEffect, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds BlueprintCore.Blueprints.Configurators.UnitLogic.Buffs.BaseBuffConfigurator`2.AddAttackBonus(System.Nullable{System.Int32},System.Action{Kingmaker.Blueprints.BlueprintComponent,Kingmaker.Blueprints.BlueprintComponent},BlueprintCore.Blueprints.CustomConfigurators.ComponentMerge)
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     ComponentName: Buffs/Attack bonus
        //     • Used by
        //     • DLC3_BesmaraPirateBaseBuff –c2ea5f0cbdd640af9f24a1b63a2bfb6c
        //     • KnightOfTheWallDefensiveChallengeBuff –d064c2b40f9b78240a1527bead977df3
        public TBuilder AddAttackBonus(int? bonus = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail)
        {
            AddAttackBonus addAttackBonus = new AddAttackBonus();
            addAttackBonus.Bonus = bonus ?? addAttackBonus.Bonus;
            return AddUniqueComponent(addAttackBonus, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds BlueprintCore.Blueprints.Configurators.UnitLogic.Buffs.BaseBuffConfigurator`2.AddCheatDamageBonus(System.Nullable{System.Int32},System.Action{Kingmaker.Blueprints.BlueprintComponent,Kingmaker.Blueprints.BlueprintComponent},BlueprintCore.Blueprints.CustomConfigurators.ComponentMerge)
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     ComponentName: Buffs/Damage bonus
        //     • Used by
        //     • CheatEmpowerBuff –9da73e0f62054254d835013c46f3a27a
        public TBuilder AddCheatDamageBonus(int? bonus = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail)
        {
            AddCheatDamageBonus addCheatDamageBonus = new AddCheatDamageBonus();
            addCheatDamageBonus.Bonus = bonus ?? addCheatDamageBonus.Bonus;
            return AddUniqueComponent(addCheatDamageBonus, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds BlueprintCore.Blueprints.Configurators.UnitLogic.Buffs.BaseBuffConfigurator`2.AddDispelMagicFailedTrigger(BlueprintCore.Actions.Builder.ActionsBuilder,BlueprintCore.Actions.Builder.ActionsBuilder,System.Action{Kingmaker.Blueprints.BlueprintComponent,Kingmaker.Blueprints.BlueprintComponent},BlueprintCore.Blueprints.CustomConfigurators.ComponentMerge)
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • SoulsCloakCurseBuff –40f948d8e5ee2534eb3d701f256f96b5
        public TBuilder AddDispelMagicFailedTrigger(ActionsBuilder? actionOnCaster = null, ActionsBuilder? actionOnOwner = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail)
        {
            AddDispelMagicFailedTrigger addDispelMagicFailedTrigger = new AddDispelMagicFailedTrigger();
            addDispelMagicFailedTrigger.ActionOnCaster = actionOnCaster?.Build() ?? addDispelMagicFailedTrigger.ActionOnCaster;
            if (addDispelMagicFailedTrigger.ActionOnCaster == null)
            {
                addDispelMagicFailedTrigger.ActionOnCaster = Constants.Empty.Actions;
            }

            addDispelMagicFailedTrigger.ActionOnOwner = actionOnOwner?.Build() ?? addDispelMagicFailedTrigger.ActionOnOwner;
            if (addDispelMagicFailedTrigger.ActionOnOwner == null)
            {
                addDispelMagicFailedTrigger.ActionOnOwner = Constants.Empty.Actions;
            }

            return AddUniqueComponent(addDispelMagicFailedTrigger, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds BlueprintCore.Blueprints.Configurators.UnitLogic.Buffs.BaseBuffConfigurator`2.AddEffectContextFastHealing(Kingmaker.UnitLogic.Mechanics.ContextValue,System.Action{Kingmaker.Blueprints.BlueprintComponent,Kingmaker.Blueprints.BlueprintComponent},BlueprintCore.Blueprints.CustomConfigurators.ComponentMerge)
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     ComponentName: Buffs/AddEffect/FastHealing
        //     • Used by
        //     • FlameOfLifeEffectBuff –d79ca3a14a6d46e4987aa2127dafefe3
        //     • MonsterMythicFeature6GoodBuff –46ec10f8db674998a0dd7a9b96cdd2f3
        //     • SwordofValorCosmeticBuff –e68e0bedcfd3410798f7513a54e71b75
        public TBuilder AddEffectContextFastHealing(ContextValue? bonus = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail)
        {
            AddEffectContextFastHealing addEffectContextFastHealing = new AddEffectContextFastHealing();
            addEffectContextFastHealing.Bonus = bonus ?? addEffectContextFastHealing.Bonus;
            if (addEffectContextFastHealing.Bonus == null)
            {
                addEffectContextFastHealing.Bonus = ContextValues.Constant(0);
            }

            return AddUniqueComponent(addEffectContextFastHealing, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds BlueprintCore.Blueprints.Configurators.UnitLogic.Buffs.BaseBuffConfigurator`2.AddEffectRegeneration(System.Nullable{System.Boolean},Kingmaker.Enums.Damage.DamageAlignment[],Kingmaker.Enums.Damage.DamageEnergyType[],Kingmaker.Enums.Damage.PhysicalDamageMaterial[],System.Nullable{System.Int32},System.Action{Kingmaker.Blueprints.BlueprintComponent,Kingmaker.Blueprints.BlueprintComponent},BlueprintCore.Blueprints.CustomConfigurators.ComponentMerge,System.Nullable{System.Boolean})
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     ComponentName: Buffs/AddEffect/Regeneration
        //     • Used by
        //     • BrandedTrollRegeneration –da6269afb82c5a14f86281c4e7ff9a4d
        //     • RegenerationEvil10 –8035661fe9d80964cb96c29406a079d9
        //     • WildLink_MonarchEffectBuff –814b2b51959705945ad6c5ceb08ffbd4
        public TBuilder AddEffectRegeneration(bool? cancelByMagicWeapon = null, DamageAlignment[]? cancelDamageAlignmentTypes = null, DamageEnergyType[]? cancelDamageEnergyTypes = null, PhysicalDamageMaterial[]? cancelDamageMaterials = null, int? heal = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail, bool? unremovable = null)
        {
            AddEffectRegeneration addEffectRegeneration = new AddEffectRegeneration();
            addEffectRegeneration.CancelByMagicWeapon = cancelByMagicWeapon ?? addEffectRegeneration.CancelByMagicWeapon;
            addEffectRegeneration.CancelDamageAlignmentTypes = cancelDamageAlignmentTypes ?? addEffectRegeneration.CancelDamageAlignmentTypes;
            if (addEffectRegeneration.CancelDamageAlignmentTypes == null)
            {
                addEffectRegeneration.CancelDamageAlignmentTypes = new DamageAlignment[0];
            }

            addEffectRegeneration.CancelDamageEnergyTypes = cancelDamageEnergyTypes ?? addEffectRegeneration.CancelDamageEnergyTypes;
            if (addEffectRegeneration.CancelDamageEnergyTypes == null)
            {
                addEffectRegeneration.CancelDamageEnergyTypes = new DamageEnergyType[0];
            }

            addEffectRegeneration.CancelDamageMaterials = cancelDamageMaterials ?? addEffectRegeneration.CancelDamageMaterials;
            if (addEffectRegeneration.CancelDamageMaterials == null)
            {
                addEffectRegeneration.CancelDamageMaterials = new PhysicalDamageMaterial[0];
            }

            addEffectRegeneration.Heal = heal ?? addEffectRegeneration.Heal;
            addEffectRegeneration.Unremovable = unremovable ?? addEffectRegeneration.Unremovable;
            return AddUniqueComponent(addEffectRegeneration, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds BlueprintCore.Blueprints.Configurators.UnitLogic.Buffs.BaseBuffConfigurator`2.AddGenericStatBonus(System.Nullable{Kingmaker.Enums.ModifierDescriptor},System.Nullable{Kingmaker.EntitySystem.Stats.StatType},System.Nullable{System.Int32})
        //
        // Remarks:
        //     ComponentName: Buffs/Generic stat bonus
        //     • Used by
        //     • AxiomiteMeleeMiniboss_Buff_LegendaryProportions –acf2b59d0d374711b969c5ea864e9656
        //     • FormOfTheDragonIIIBlueBuff –a4993affb4c4ad6429eca6daeb7b86a8
        //     • TrueStrikeBuff –a3ce3b226c1817846b0419fa182e6ea0
        public TBuilder AddGenericStatBonus(ModifierDescriptor? descriptor = null, StatType? stat = null, int? value = null)
        {
            AddGenericStatBonus addGenericStatBonus = new AddGenericStatBonus();
            addGenericStatBonus.Descriptor = descriptor ?? addGenericStatBonus.Descriptor;
            addGenericStatBonus.Stat = stat ?? addGenericStatBonus.Stat;
            addGenericStatBonus.Value = value ?? addGenericStatBonus.Value;
            return AddComponent(addGenericStatBonus);
        }

        //
        // Summary:
        //     Adds BlueprintCore.Blueprints.Configurators.UnitLogic.Buffs.BaseBuffConfigurator`2.AddMirrorImage(Kingmaker.UnitLogic.Mechanics.ContextDiceValue,System.Nullable{System.Int32},System.Action{Kingmaker.Blueprints.BlueprintComponent,Kingmaker.Blueprints.BlueprintComponent},BlueprintCore.Blueprints.CustomConfigurators.ComponentMerge)
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • AnomalyTemplateDefensive_ImagesOfChaosEffectBuff –ae6e6c03a29d44df9bed230e940af75c
        //     • MirrorImageBuff –98dc7e7cc6ef59f4abe20c65708ac623
        //     • TricksterFirstAscensionBuff –b585708811497fc49836fc9112ff1732
        public TBuilder AddMirrorImage(ContextDiceValue? count = null, int? maxCount = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail)
        {
            AddMirrorImage addMirrorImage = new AddMirrorImage();
            addMirrorImage.Count = count ?? addMirrorImage.Count;
            if (addMirrorImage.Count == null)
            {
                addMirrorImage.Count = Constants.Empty.DiceValue;
            }

            addMirrorImage.MaxCount = maxCount ?? addMirrorImage.MaxCount;
            return AddUniqueComponent(addMirrorImage, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds BlueprintCore.Blueprints.Configurators.UnitLogic.Buffs.BaseBuffConfigurator`2.AddSpellSchool(System.Action{Kingmaker.Blueprints.BlueprintComponent,Kingmaker.Blueprints.BlueprintComponent},BlueprintCore.Blueprints.CustomConfigurators.ComponentMerge,System.Nullable{Kingmaker.Blueprints.Classes.Spells.SpellSchool})
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • DisplacementBuff –00402bae4442a854081264e498e7a833
        //     • MirrorImageBuff –98dc7e7cc6ef59f4abe20c65708ac623
        public TBuilder AddSpellSchool(Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail, SpellSchool? school = null)
        {
            AddSpellSchool addSpellSchool = new AddSpellSchool();
            addSpellSchool.School = school ?? addSpellSchool.School;
            return AddUniqueComponent(addSpellSchool, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.UnitLogic.Buffs.Components.FakeDeathAnimationState
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • FakeDeath –c970916dd6ed4796aa35fcdc12dacb0a
        public TBuilder AddFakeDeathAnimationState(Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail)
        {
            FakeDeathAnimationState component = new FakeDeathAnimationState();
            return AddUniqueComponent(component, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.UnitLogic.Buffs.Components.IsPositiveEffect
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • CarnivorousCrystal_Buff_SubsonicHum_Immunity –88e345f3233c8024e9d191a807c40223
        //     • RatSwarmDamageEffectImmunity –60549b98735cde44e87bf247042604c1
        //     • WildGazeImmunity –2e64086ebcd066c4b8d1e46c00c8636f
        public TBuilder AddIsPositiveEffect(Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail)
        {
            IsPositiveEffect component = new IsPositiveEffect();
            return AddUniqueComponent(component, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.UnitLogic.Buffs.Components.NegativeLevelComponent
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • Colyphyr_HepzamirahDebuff –28c27ef896fc5704cb232af04f86f694
        //     • MongrelsBlessingBuff –c5989f96b6184573988a305b4220b9b5
        //     • NegativeLevelsBuff –b02b6b9221241394db720ca004ea9194
        public TBuilder AddNegativeLevelComponent(Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail)
        {
            NegativeLevelComponent component = new NegativeLevelComponent();
            return AddUniqueComponent(component, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.UnitLogic.Buffs.Components.NotDispelable
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • ArueshalaeImmortalityBuff –7ad9d9982302e2244a7dd73fee6c381b
        public TBuilder AddNotDispelable(Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail)
        {
            NotDispelable component = new NotDispelable();
            return AddUniqueComponent(component, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.UnitLogic.Buffs.Components.RemoveBuffIfCasterIsMissing
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • AirElementalInWhirlwind –8b1b723a20f644c469b99bd541a13a3b
        //     • ImprovedFiendishQuarryBuffEnemy –c52dfead7d06dd14aa132f03be2bd508
        //     • WitchHexDeathCurseBuff2 –5e6aeb6852a77b3449b37a4bdb9f7bb4
        public TBuilder AddRemoveBuffIfCasterIsMissing(Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail, bool? removeOnCasterDeath = null)
        {
            RemoveBuffIfCasterIsMissing removeBuffIfCasterIsMissing = new RemoveBuffIfCasterIsMissing();
            removeBuffIfCasterIsMissing.RemoveOnCasterDeath = removeOnCasterDeath ?? removeBuffIfCasterIsMissing.RemoveOnCasterDeath;
            return AddUniqueComponent(removeBuffIfCasterIsMissing, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.UnitLogic.Buffs.Components.RemoveWhenCombatEnded
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • AngelRepelTheProfaneBuff –50a18ee630a4973479e9950011425748
        //     • HoldAnimalBuff –2090955a573cec3438db7f47707610f9
        //     • VinetrapEntangledBuff –231a622f767e8ed4e9b3e435265a3e99
        public TBuilder AddRemoveWhenCombatEnded(Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail)
        {
            RemoveWhenCombatEnded component = new RemoveWhenCombatEnded();
            return AddUniqueComponent(component, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.UnitLogic.Buffs.Components.ResurrectionLogic
        //
        // Parameters:
        //   firstFx:
        //     You can pass in the animation using a GameObject or it's AssetId.
        //
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        //   secondFx:
        //     You can pass in the animation using a GameObject or it's AssetId.
        //
        //   shouldRemoveBuff:
        //     InfoBox: If false buff will be remove according to common buff logic. If true
        //     will be removed after fx applied
        //
        //   spawnNearMainCharacter:
        //     InfoBox: Not change value as &apos;false&apos; when has targets allies!
        //
        // Remarks:
        //     • Used by
        //     • DLC4_ResurrectionBhogaSwarm –73e2514b871542ce8f08e40ddf479cd9
        //     • ResurrectionBuff –12f2f2cf326dfd743b2cce5b14e99b3c
        //     • SongOfTheFallenResurrectionBuff –e2cd971a6a004c53b55abd336ac8da03
        public TBuilder AddResurrectionLogic(Asset<GameObject>? firstFx = null, float? firstFxDelay = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail, bool? offsetPosition = null, Asset<GameObject>? secondFx = null, float? secondFxDelay = null, bool? shouldRemoveBuff = null, bool? spawnNearMainCharacter = null)
        {
            ResurrectionLogic resurrectionLogic = new ResurrectionLogic();
            resurrectionLogic.FirstFx = firstFx?.Get() ?? resurrectionLogic.FirstFx;
            resurrectionLogic.FirstFxDelay = firstFxDelay ?? resurrectionLogic.FirstFxDelay;
            resurrectionLogic.OffsetPosition = offsetPosition ?? resurrectionLogic.OffsetPosition;
            resurrectionLogic.SecondFx = secondFx?.Get() ?? resurrectionLogic.SecondFx;
            resurrectionLogic.SecondFxDelay = secondFxDelay ?? resurrectionLogic.SecondFxDelay;
            resurrectionLogic.ShouldRemoveBuff = shouldRemoveBuff ?? resurrectionLogic.ShouldRemoveBuff;
            resurrectionLogic.SpawnNearMainCharacter = spawnNearMainCharacter ?? resurrectionLogic.SpawnNearMainCharacter;
            return AddUniqueComponent(resurrectionLogic, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.UnitLogic.Buffs.Components.SetBuffOnsetDelay
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • Archpriest_PossessionBuff_ShadowBalorEncounter –4fc454d17bbc41e7aac430dd570e61c6
        //     • Archpriest_PossessionBuff_ShadowBalorEncounter_Target –2e73650948d3419b82c74da67030c67e
        public TBuilder AddSetBuffOnsetDelay(ContextDurationValue? delay = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail, ActionsBuilder? onStart = null)
        {
            SetBuffOnsetDelay setBuffOnsetDelay = new SetBuffOnsetDelay();
            Validate(delay);
            setBuffOnsetDelay.Delay = delay ?? setBuffOnsetDelay.Delay;
            setBuffOnsetDelay.OnStart = onStart?.Build() ?? setBuffOnsetDelay.OnStart;
            if (setBuffOnsetDelay.OnStart == null)
            {
                setBuffOnsetDelay.OnStart = Constants.Empty.Actions;
            }

            return AddUniqueComponent(setBuffOnsetDelay, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.UnitLogic.Buffs.Components.SpecialAnimationState
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     ComponentName: Buffs/Special/AnimationState
        //     • Used by
        //     • AirElementalInWhirlwind –8b1b723a20f644c469b99bd541a13a3b
        //     • NightcrawlerBurrowedBuff –c568b045991644c89c58667c6a17180d
        //     • WyvernInFlight –ad06fa795a9e7124a88878446c675aaa
        public TBuilder AddSpecialAnimationState(UnitAnimationActionBuffState? animation = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail)
        {
            SpecialAnimationState specialAnimationState = new SpecialAnimationState();
            Validate(animation);
            specialAnimationState.Animation = animation ?? specialAnimationState.Animation;
            return AddUniqueComponent(specialAnimationState, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.UnitLogic.Buffs.Components.SummonedUnitBuff
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     ComponentName: Buffs/Special/Summoned unit
        //     • Used by
        //     • CR12_NabasuAdvancedSummoned –a1351a09365f46c4afb71e710eec328b
        //     • SummonedUnitBuff –8728e884eeaa8b047be04197ecf1a0e4
        public TBuilder AddSummonedUnitBuff(Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail)
        {
            SummonedUnitBuff component = new SummonedUnitBuff();
            return AddUniqueComponent(component, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.UnitLogic.Buffs.Components.WeaponAttackTypeDamageBonus
        //
        // Parameters:
        //   attackBonus:
        //     InfoBox: It&apos;s actually damage bonus
        //
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     ComponentName: Buffs/Damage bonus
        //     • Used by
        //     • ArmyStandartRageBuff –77c8d5b837c04fa0a7b44bb7592aee56
        //     • EyeImplantFeature –4456e13ff90d9e6498462b92cb97ed21
        //     • WarDomainBaseBuff –aefec65136058694ab20cd71941eec81
        public TBuilder AddWeaponAttackTypeDamageBonus(int? attackBonus = null, ModifierDescriptor? descriptor = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail, WeaponRangeType? type = null, ContextValue? value = null)
        {
            WeaponAttackTypeDamageBonus weaponAttackTypeDamageBonus = new WeaponAttackTypeDamageBonus();
            weaponAttackTypeDamageBonus.AttackBonus = attackBonus ?? weaponAttackTypeDamageBonus.AttackBonus;
            weaponAttackTypeDamageBonus.Descriptor = descriptor ?? weaponAttackTypeDamageBonus.Descriptor;
            weaponAttackTypeDamageBonus.Type = type ?? weaponAttackTypeDamageBonus.Type;
            weaponAttackTypeDamageBonus.Value = value ?? weaponAttackTypeDamageBonus.Value;
            if (weaponAttackTypeDamageBonus.Value == null)
            {
                weaponAttackTypeDamageBonus.Value = ContextValues.Constant(0);
            }

            return AddUniqueComponent(weaponAttackTypeDamageBonus, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.UnitLogic.ActivatableAbilities.ActivatableAbilitySetItem
        //
        // Parameters:
        //   activatableAbility:
        //     Blueprint of type BlueprintActivatableAbility. You can pass in the blueprint
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
        //
        // Remarks:
        //     • Used by
        //     • ShifterAspectChimeraBearBuff –993d1b4af8be4facbc3b33b0edda6f15
        //     • ShifterAspectChimeraLizardBuff –dc3f3ec0246b432f8551cedec87bc00a
        //     • ShifterAspectChimeraWolverineBuff –b6cfc6cc443c40318062490ff20cefb7
        public TBuilder AddActivatableAbilitySetItem(Blueprint<BlueprintActivatableAbilityReference>? activatableAbility = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail, ActivatableAbilitySetId? setId = null)
        {
            ActivatableAbilitySetItem activatableAbilitySetItem = new ActivatableAbilitySetItem();
            activatableAbilitySetItem.m_ActivatableAbility = activatableAbility?.Reference ?? activatableAbilitySetItem.m_ActivatableAbility;
            if (activatableAbilitySetItem.m_ActivatableAbility == null)
            {
                activatableAbilitySetItem.m_ActivatableAbility = BlueprintTool.GetRef<BlueprintActivatableAbilityReference>(null);
            }

            activatableAbilitySetItem.m_SetId = setId ?? activatableAbilitySetItem.m_SetId;
            return AddUniqueComponent(activatableAbilitySetItem, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.UnitLogic.ActivatableAbilities.ActivatableAbilitySetSwitch
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • ShifterChimeraAspectBuff –bbf7614374ae4e3da9e284130473a89e
        //     • ShifterChimeraAspectBuffFinal –5f9990acdfbc4331a5d618e1962eb304
        //     • ShifterChimeraAspectBuffGreater –7eba1cd350f94617b949641de9766446
        public TBuilder AddActivatableAbilitySetSwitch(Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail, ActivatableAbilitySetId? setId = null)
        {
            ActivatableAbilitySetSwitch activatableAbilitySetSwitch = new ActivatableAbilitySetSwitch();
            activatableAbilitySetSwitch.m_SetId = setId ?? activatableAbilitySetSwitch.m_SetId;
            return AddUniqueComponent(activatableAbilitySetSwitch, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.UnitLogic.ActivatableAbilities.ShiftersFuryItemBuff
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • ShifterFuryBuff –621bcb0994d74dd09db6931e8f53cc91
        public TBuilder AddShiftersFuryItemBuff(Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail)
        {
            ShiftersFuryItemBuff component = new ShiftersFuryItemBuff();
            return AddUniqueComponent(component, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds BlueprintCore.Blueprints.Configurators.UnitLogic.Buffs.BaseBuffConfigurator`2.AdditionalDiceForWeapon(System.Nullable{Kingmaker.RuleSystem.DiceFormula},System.Nullable{System.Int32},System.Action{Kingmaker.Blueprints.BlueprintComponent,Kingmaker.Blueprints.BlueprintComponent},BlueprintCore.Blueprints.CustomConfigurators.ComponentMerge,System.Nullable{System.Boolean},System.Nullable{System.Boolean},System.Nullable{System.Boolean},System.Nullable{System.Boolean},System.Nullable{System.Boolean},System.Collections.Generic.List{Kingmaker.Enums.WeaponCategory},System.Collections.Generic.List{Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup},System.Collections.Generic.List{BlueprintCore.Utils.Blueprint{Kingmaker.Blueprints.BlueprintItemWeaponReference}},System.Collections.Generic.List{BlueprintCore.Utils.Blueprint{Kingmaker.Blueprints.BlueprintWeaponTypeReference}})
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        //   useWeaponBlueprintFilter:
        //     InfoBox: Filters: the component is applied if no filters enabled or the weapon
        //     fits to any of filter lists.
        //
        //   weapons:
        //     Blueprint of type BlueprintItemWeapon. You can pass in the blueprint using:
        //     • A blueprint instance –
        //     • A blueprint reference –
        //     • A blueprint id as a string, Guid, or BlueprintGuid –
        //     • A blueprint name registered with BlueprintTool –
        //     See Blueprint for more details.
        //
        //   weaponTypes:
        //     Blueprint of type BlueprintWeaponType. You can pass in the blueprint using:
        //     • A blueprint instance –
        //     • A blueprint reference –
        //     • A blueprint id as a string, Guid, or BlueprintGuid –
        //     • A blueprint name registered with BlueprintTool –
        //     See Blueprint for more details.
        //
        // Remarks:
        //     • Used by
        //     • BackgroundAlkenstarAlchemist –f51af2d4fa3358844879cbc5ee0f1073
        //     • ShifterAspectExtraFourSpikes –1a2dcae8717f44439497be99a49a16f1
        //     • ShifterAspectExtraTwoSpikes –13fc0a54e43c4757be4722665b5a2c3d
        public TBuilder AdditionalDiceForWeapon(DiceFormula? additionalDamageFormula = null, int? diceMultiplier = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail, bool? useCustomAdditionalDamageFormula = null, bool? useWeaponBlueprintFilter = null, bool? useWeaponCategoryFilter = null, bool? useWeaponGroupFilter = null, bool? useWeaponTypeFilter = null, List<WeaponCategory>? weaponCategories = null, List<WeaponFighterGroup>? weaponGroups = null, List<Blueprint<BlueprintItemWeaponReference>>? weapons = null, List<Blueprint<BlueprintWeaponTypeReference>>? weaponTypes = null)
        {
            AdditionalDiceForWeapon additionalDiceForWeapon = new AdditionalDiceForWeapon();
            additionalDiceForWeapon.m_AdditionalDamageFormula = additionalDamageFormula ?? additionalDiceForWeapon.m_AdditionalDamageFormula;
            additionalDiceForWeapon.m_DiceMultiplier = diceMultiplier ?? additionalDiceForWeapon.m_DiceMultiplier;
            additionalDiceForWeapon.m_UseCustomAdditionalDamageFormula = useCustomAdditionalDamageFormula ?? additionalDiceForWeapon.m_UseCustomAdditionalDamageFormula;
            additionalDiceForWeapon.m_UseWeaponBlueprintFilter = useWeaponBlueprintFilter ?? additionalDiceForWeapon.m_UseWeaponBlueprintFilter;
            additionalDiceForWeapon.m_UseWeaponCategoryFilter = useWeaponCategoryFilter ?? additionalDiceForWeapon.m_UseWeaponCategoryFilter;
            additionalDiceForWeapon.m_UseWeaponGroupFilter = useWeaponGroupFilter ?? additionalDiceForWeapon.m_UseWeaponGroupFilter;
            additionalDiceForWeapon.m_UseWeaponTypeFilter = useWeaponTypeFilter ?? additionalDiceForWeapon.m_UseWeaponTypeFilter;
            additionalDiceForWeapon.m_WeaponCategories = weaponCategories ?? additionalDiceForWeapon.m_WeaponCategories;
            if (additionalDiceForWeapon.m_WeaponCategories == null)
            {
                additionalDiceForWeapon.m_WeaponCategories = new List<WeaponCategory>();
            }

            additionalDiceForWeapon.m_WeaponGroups = weaponGroups ?? additionalDiceForWeapon.m_WeaponGroups;
            if (additionalDiceForWeapon.m_WeaponGroups == null)
            {
                additionalDiceForWeapon.m_WeaponGroups = new List<WeaponFighterGroup>();
            }

            additionalDiceForWeapon.m_Weapons = weapons?.Select((Blueprint<BlueprintItemWeaponReference> bp) => bp.Reference)?.ToList() ?? additionalDiceForWeapon.m_Weapons;
            if (additionalDiceForWeapon.m_Weapons == null)
            {
                additionalDiceForWeapon.m_Weapons = new List<BlueprintItemWeaponReference>();
            }

            additionalDiceForWeapon.m_WeaponTypes = weaponTypes?.Select((Blueprint<BlueprintWeaponTypeReference> bp) => bp.Reference)?.ToList() ?? additionalDiceForWeapon.m_WeaponTypes;
            if (additionalDiceForWeapon.m_WeaponTypes == null)
            {
                additionalDiceForWeapon.m_WeaponTypes = new List<BlueprintWeaponTypeReference>();
            }

            return AddUniqueComponent(additionalDiceForWeapon, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.UnitLogic.Mechanics.Components.ContextCalculateAbilityParams
        //
        // Parameters:
        //   customProperty:
        //     Blueprint of type BlueprintUnitProperty. You can pass in the blueprint using:
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
        //
        // Remarks:
        //     • Used by
        //     • 1_FirstStage_AcidBuff –6afe27c9a2d64eb890673ff3649dacb3
        //     • DeathThroesFeature –49a64c524e7f8e548b4d5ea41041a226
        //     • Yozz_Feature_AdditionalAttacks –bcf37abbb0b1485b83059600ed440881
        public TBuilder AddContextCalculateAbilityParams(ContextValue? casterLevel = null, Blueprint<BlueprintUnitPropertyReference>? customProperty = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail, bool? replaceCasterLevel = null, bool? replaceSpellLevel = null, ContextValue? spellLevel = null, StatType? statType = null, bool? statTypeFromCustomProperty = null, bool? useKineticistMainStat = null)
        {
            ContextCalculateAbilityParams contextCalculateAbilityParams = new ContextCalculateAbilityParams();
            contextCalculateAbilityParams.CasterLevel = casterLevel ?? contextCalculateAbilityParams.CasterLevel;
            if (contextCalculateAbilityParams.CasterLevel == null)
            {
                contextCalculateAbilityParams.CasterLevel = ContextValues.Constant(0);
            }

            contextCalculateAbilityParams.m_CustomProperty = customProperty?.Reference ?? contextCalculateAbilityParams.m_CustomProperty;
            if (contextCalculateAbilityParams.m_CustomProperty == null)
            {
                contextCalculateAbilityParams.m_CustomProperty = BlueprintTool.GetRef<BlueprintUnitPropertyReference>(null);
            }

            contextCalculateAbilityParams.ReplaceCasterLevel = replaceCasterLevel ?? contextCalculateAbilityParams.ReplaceCasterLevel;
            contextCalculateAbilityParams.ReplaceSpellLevel = replaceSpellLevel ?? contextCalculateAbilityParams.ReplaceSpellLevel;
            contextCalculateAbilityParams.SpellLevel = spellLevel ?? contextCalculateAbilityParams.SpellLevel;
            if (contextCalculateAbilityParams.SpellLevel == null)
            {
                contextCalculateAbilityParams.SpellLevel = ContextValues.Constant(0);
            }

            contextCalculateAbilityParams.StatType = statType ?? contextCalculateAbilityParams.StatType;
            contextCalculateAbilityParams.StatTypeFromCustomProperty = statTypeFromCustomProperty ?? contextCalculateAbilityParams.StatTypeFromCustomProperty;
            contextCalculateAbilityParams.UseKineticistMainStat = useKineticistMainStat ?? contextCalculateAbilityParams.UseKineticistMainStat;
            return AddUniqueComponent(contextCalculateAbilityParams, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.UnitLogic.Mechanics.Components.ContextCalculateAbilityParamsBasedOnClass
        //
        // Parameters:
        //   characterClass:
        //     Blueprint of type BlueprintCharacterClass. You can pass in the blueprint using:
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
        //
        // Remarks:
        //     • Used by
        //     • AirBlastAbility –31f668b12011e344aa542aa07ab6c8d9
        //     • PlasmaBlastBladeDamage –fc22c06d63a95154291272577daa0b4d
        //     • XantirOnlySwarm_MidnightFaneInThePastFeature –5131c4b93f314bd4589edf612b4eb600
        public TBuilder AddContextCalculateAbilityParamsBasedOnClass(Blueprint<BlueprintCharacterClassReference>? characterClass = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail, StatType? statType = null, bool? useKineticistMainStat = null)
        {
            ContextCalculateAbilityParamsBasedOnClass contextCalculateAbilityParamsBasedOnClass = new ContextCalculateAbilityParamsBasedOnClass();
            contextCalculateAbilityParamsBasedOnClass.m_CharacterClass = characterClass?.Reference ?? contextCalculateAbilityParamsBasedOnClass.m_CharacterClass;
            if (contextCalculateAbilityParamsBasedOnClass.m_CharacterClass == null)
            {
                contextCalculateAbilityParamsBasedOnClass.m_CharacterClass = BlueprintTool.GetRef<BlueprintCharacterClassReference>(null);
            }

            contextCalculateAbilityParamsBasedOnClass.StatType = statType ?? contextCalculateAbilityParamsBasedOnClass.StatType;
            contextCalculateAbilityParamsBasedOnClass.UseKineticistMainStat = useKineticistMainStat ?? contextCalculateAbilityParamsBasedOnClass.UseKineticistMainStat;
            return AddUniqueComponent(contextCalculateAbilityParamsBasedOnClass, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.UnitLogic.Mechanics.Components.ContextCalculateSharedValue
        //
        // Remarks:
        //     • Used by
        //     • AbyssalCreatureAcidTemplate –6e6fda1c8a35069468e7398082cd30f5
        //     • KnightsResolveDeterminedAbility –29a78cf77ed275f479c0349a95583b94
        //     • WrackBloodBlastAbility –0199d49f59833104198e2c0196235a45
        public TBuilder AddContextCalculateSharedValue(double? modifier = null, ContextDiceValue? value = null, AbilitySharedValue? valueType = null)
        {
            ContextCalculateSharedValue contextCalculateSharedValue = new ContextCalculateSharedValue();
            contextCalculateSharedValue.Modifier = modifier ?? contextCalculateSharedValue.Modifier;
            contextCalculateSharedValue.Value = value ?? contextCalculateSharedValue.Value;
            if (contextCalculateSharedValue.Value == null)
            {
                contextCalculateSharedValue.Value = Constants.Empty.DiceValue;
            }

            contextCalculateSharedValue.ValueType = valueType ?? contextCalculateSharedValue.ValueType;
            return AddComponent(contextCalculateSharedValue);
        }

        //
        // Summary:
        //     Adds Kingmaker.UnitLogic.Mechanics.Components.ContextSetAbilityParams
        //
        // Parameters:
        //   casterLevel:
        //     InfoBox: If set to negative value, will be calculated by default mechanic. Positive
        //     or zero value will be set as is (plus bonuses)
        //
        //   concentration:
        //     InfoBox: If set to negative value, will be calculated by default mechanic. Positive
        //     or zero value will be set as is (plus bonuses)
        //
        //   dC:
        //     InfoBox: If set to negative value, will be calculated by default mechanic. Positive
        //     or zero value will be set as is (plus bonuses)
        //
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        //   spellLevel:
        //     InfoBox: If set to negative value, will be calculated by default mechanic. Positive
        //     or zero value will be set as is
        //
        // Remarks:
        //     • Used by
        //     • AbruptForceEnchantment –c31b3edcf2088a64e80133ebbd6374cb
        //     • HeartOfIcebergAbility –38d7bac2134ff0a48968dc2aacfc5973
        //     • ZombieSlashingExplosion –f6b63adab8b645c8beb9cab170dac9d3
        public TBuilder AddContextSetAbilityParams(bool? add10ToDC = null, ContextValue? casterLevel = null, ContextValue? concentration = null, ContextValue? dC = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail, ContextValue? spellLevel = null)
        {
            ContextSetAbilityParams contextSetAbilityParams = new ContextSetAbilityParams();
            contextSetAbilityParams.Add10ToDC = add10ToDC ?? contextSetAbilityParams.Add10ToDC;
            contextSetAbilityParams.CasterLevel = casterLevel ?? contextSetAbilityParams.CasterLevel;
            if (contextSetAbilityParams.CasterLevel == null)
            {
                contextSetAbilityParams.CasterLevel = ContextValues.Constant(0);
            }

            contextSetAbilityParams.Concentration = concentration ?? contextSetAbilityParams.Concentration;
            if (contextSetAbilityParams.Concentration == null)
            {
                contextSetAbilityParams.Concentration = ContextValues.Constant(0);
            }

            contextSetAbilityParams.DC = dC ?? contextSetAbilityParams.DC;
            if (contextSetAbilityParams.DC == null)
            {
                contextSetAbilityParams.DC = ContextValues.Constant(0);
            }

            contextSetAbilityParams.SpellLevel = spellLevel ?? contextSetAbilityParams.SpellLevel;
            if (contextSetAbilityParams.SpellLevel == null)
            {
                contextSetAbilityParams.SpellLevel = ContextValues.Constant(0);
            }

            return AddUniqueComponent(contextSetAbilityParams, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.UnitLogic.Mechanics.Actions.DuplicateDamageToCaster
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • DLC4_UsedBodyCreature –7eb5f47230824e89ad17cef3cfae851c
        public TBuilder AddDuplicateDamageToCaster(Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail)
        {
            DuplicateDamageToCaster component = new DuplicateDamageToCaster();
            return AddUniqueComponent(component, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.UnitLogic.Abilities.Components.AbilityDifficultyLimitDC
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • StewardOfTheSkeinGazeBuff –4f18044ca197eb945b7d1b557dd9b447
        //     • Weird –870af83be6572594d84d276d7fc583e0
        //     • WildHunt_ScoutCrystalAbility –c470c62b38db74e4fb6b84b331beda30
        public TBuilder AddAbilityDifficultyLimitDC(Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail)
        {
            AbilityDifficultyLimitDC component = new AbilityDifficultyLimitDC();
            return AddUniqueComponent(component, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.Armies.TacticalCombat.LeaderSkills.UnitBuffComponents.DamageBonusAgainstTacticalTarget
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • ArmyBuildingCathedralBuff –e9df77ae53da4993a882bb1d1047a4e8
        //     • ArmyLeadership8DamageToInfantryBuff –406cc284f5714ff0ab3d6231e0aed94a
        //     • FavoredEnemyLargeBuff –45f4acbece1c4cf4889aaceb46318ae7
        public TBuilder AddDamageBonusAgainstTacticalTarget(Kingmaker.UnitLogic.Mechanics.ValueType? _valueType = null, int? bonusPercentValue = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail, TargetFilter? targetFilter = null, ContextValue? value = null)
        {
            DamageBonusAgainstTacticalTarget damageBonusAgainstTacticalTarget = new DamageBonusAgainstTacticalTarget();
            damageBonusAgainstTacticalTarget._valueType = _valueType ?? damageBonusAgainstTacticalTarget._valueType;
            damageBonusAgainstTacticalTarget.m_BonusPercentValue = bonusPercentValue ?? damageBonusAgainstTacticalTarget.m_BonusPercentValue;
            Validate(targetFilter);
            damageBonusAgainstTacticalTarget.m_TargetFilter = targetFilter ?? damageBonusAgainstTacticalTarget.m_TargetFilter;
            damageBonusAgainstTacticalTarget.m_Value = value ?? damageBonusAgainstTacticalTarget.m_Value;
            if (damageBonusAgainstTacticalTarget.m_Value == null)
            {
                damageBonusAgainstTacticalTarget.m_Value = ContextValues.Constant(0);
            }

            return AddUniqueComponent(damageBonusAgainstTacticalTarget, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.Armies.TacticalCombat.Components.ReplaceSquadAbilities
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        //   newAbilities:
        //     Blueprint of type BlueprintAbility. You can pass in the blueprint using:
        //     • A blueprint instance –
        //     • A blueprint reference –
        //     • A blueprint id as a string, Guid, or BlueprintGuid –
        //     • A blueprint name registered with BlueprintTool –
        //     See Blueprint for more details.
        //
        // Remarks:
        //     • Used by
        //     • ArmyBuildingBreweryBuff –7e2a4a3c182c4f2483a90e8c6d7e0bd8
        public TBuilder AddReplaceSquadAbilities(bool? forOneTurn = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail, List<Blueprint<BlueprintAbilityReference>>? newAbilities = null)
        {
            ReplaceSquadAbilities replaceSquadAbilities = new ReplaceSquadAbilities();
            replaceSquadAbilities.m_ForOneTurn = forOneTurn ?? replaceSquadAbilities.m_ForOneTurn;
            replaceSquadAbilities.m_NewAbilities = newAbilities?.Select((Blueprint<BlueprintAbilityReference> bp) => bp.Reference)?.ToList() ?? replaceSquadAbilities.m_NewAbilities;
            if (replaceSquadAbilities.m_NewAbilities == null)
            {
                replaceSquadAbilities.m_NewAbilities = new List<BlueprintAbilityReference>();
            }

            return AddUniqueComponent(replaceSquadAbilities, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.Armies.TacticalCombat.Components.TacticalCombatConfusion
        //
        // Parameters:
        //   aiAttackNearestAction:
        //     InfoBox: Set action with Can Attack allies flag
        //     Blueprint of type BlueprintTacticalCombatAiAction. You can pass in the blueprint
        //     using:
        //     • A blueprint instance –
        //     • A blueprint reference –
        //     • A blueprint id as a string, Guid, or BlueprintGuid –
        //     • A blueprint name registered with BlueprintTool –
        //     See Blueprint for more details.
        //
        //   harmSelfAction:
        //     InfoBox: Owner is target here
        //
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • ArmyConfusedBuff –1da4d884ccac4c1a89465ea8ad48c7e4
        public TBuilder AddTacticalCombatConfusion(Blueprint<BlueprintTacticalCombatAiActionReference>? aiAttackNearestAction = null, ActionsBuilder? harmSelfAction = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail)
        {
            TacticalCombatConfusion tacticalCombatConfusion = new TacticalCombatConfusion();
            tacticalCombatConfusion.m_AiAttackNearestAction = aiAttackNearestAction?.Reference ?? tacticalCombatConfusion.m_AiAttackNearestAction;
            if (tacticalCombatConfusion.m_AiAttackNearestAction == null)
            {
                tacticalCombatConfusion.m_AiAttackNearestAction = BlueprintTool.GetRef<BlueprintTacticalCombatAiActionReference>(null);
            }

            tacticalCombatConfusion.m_HarmSelfAction = harmSelfAction?.Build() ?? tacticalCombatConfusion.m_HarmSelfAction;
            if (tacticalCombatConfusion.m_HarmSelfAction == null)
            {
                tacticalCombatConfusion.m_HarmSelfAction = Constants.Empty.Actions;
            }

            return AddUniqueComponent(tacticalCombatConfusion, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.Armies.TacticalCombat.Components.TacticalMoraleChanceModifier
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        //   negativeMoraleChancePercentDelta:
        //     InfoBox: Negative Morale chance formula: Chance = -Morale / NegativeMoraleModifier
        //     + NegativeMoraleChance
        //
        //   positiveMoraleChancePercentDelta:
        //     InfoBox: Positive Morale chance formula: Chance = UnitMorale / PositiveMoraleModifier
        //     + PositiveMoraleChance
        //
        // Remarks:
        //     • Used by
        //     • ArmyBuildingBulletingBoard –d3fc356cf3ad44129a995b64fbb513ab
        //     • ArmyBuildingTavern –5b7dae6b75e7483ba1bc10d890a690c7
        //     • Ziforian_feature –59820030350e40fe86a83d3ca412b221
        public TBuilder AddTacticalMoraleChanceModifier(bool? changeNegativeMorale = null, bool? changePositiveMorale = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail, ContextValue? negativeMoraleChancePercentDelta = null, ContextValue? positiveMoraleChancePercentDelta = null)
        {
            TacticalMoraleChanceModifier tacticalMoraleChanceModifier = new TacticalMoraleChanceModifier();
            tacticalMoraleChanceModifier.m_ChangeNegativeMorale = changeNegativeMorale ?? tacticalMoraleChanceModifier.m_ChangeNegativeMorale;
            tacticalMoraleChanceModifier.m_ChangePositiveMorale = changePositiveMorale ?? tacticalMoraleChanceModifier.m_ChangePositiveMorale;
            tacticalMoraleChanceModifier.m_NegativeMoraleChancePercentDelta = negativeMoraleChancePercentDelta ?? tacticalMoraleChanceModifier.m_NegativeMoraleChancePercentDelta;
            if (tacticalMoraleChanceModifier.m_NegativeMoraleChancePercentDelta == null)
            {
                tacticalMoraleChanceModifier.m_NegativeMoraleChancePercentDelta = ContextValues.Constant(0);
            }

            tacticalMoraleChanceModifier.m_PositiveMoraleChancePercentDelta = positiveMoraleChancePercentDelta ?? tacticalMoraleChanceModifier.m_PositiveMoraleChancePercentDelta;
            if (tacticalMoraleChanceModifier.m_PositiveMoraleChancePercentDelta == null)
            {
                tacticalMoraleChanceModifier.m_PositiveMoraleChancePercentDelta = ContextValues.Constant(0);
            }

            return AddUniqueComponent(tacticalMoraleChanceModifier, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.Designers.Mechanics.Facts.TargetingBlind
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • BlindVictimBuff –43981a02863a5d34b9e1448d32000fd7
        public TBuilder AddTargetingBlind(Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail)
        {
            TargetingBlind component = new TargetingBlind();
            return AddUniqueComponent(component, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.Designers.Mechanics.Facts.BuffExtraEffects
        //
        // Parameters:
        //   checkedBuff:
        //     Blueprint of type BlueprintBuff. You can pass in the blueprint using:
        //     • A blueprint instance –
        //     • A blueprint reference –
        //     • A blueprint id as a string, Guid, or BlueprintGuid –
        //     • A blueprint name registered with BlueprintTool –
        //     See Blueprint for more details.
        //
        //   exceptionFact:
        //     Blueprint of type BlueprintUnitFact. You can pass in the blueprint using:
        //     • A blueprint instance –
        //     • A blueprint reference –
        //     • A blueprint id as a string, Guid, or BlueprintGuid –
        //     • A blueprint name registered with BlueprintTool –
        //     See Blueprint for more details.
        //
        //   extraEffectBuff:
        //     Blueprint of type BlueprintBuff. You can pass in the blueprint using:
        //     • A blueprint instance –
        //     • A blueprint reference –
        //     • A blueprint id as a string, Guid, or BlueprintGuid –
        //     • A blueprint name registered with BlueprintTool –
        //     See Blueprint for more details.
        //
        // Remarks:
        //     ComponentName: Add extra buff to buff
        //     • Used by
        //     • AeonBaneFeature –0b25e8d8b0488c84c9b5714e9ca0a204
        //     • HelmOfBattlefieldClarityFeature –7444d4913a1b1be459bac3b12c6a2933
        //     • WreckingBlowsFeature –5bccc86dd1f187a4a99f092dc054c755
        public TBuilder AddBuffExtraEffects(Blueprint<BlueprintBuffReference>? checkedBuff = null, Blueprint<BlueprintUnitFactReference>? exceptionFact = null, Blueprint<BlueprintBuffReference>? extraEffectBuff = null)
        {
            BuffExtraEffects buffExtraEffects = new BuffExtraEffects();
            buffExtraEffects.m_CheckedBuff = checkedBuff?.Reference ?? buffExtraEffects.m_CheckedBuff;
            if (buffExtraEffects.m_CheckedBuff == null)
            {
                buffExtraEffects.m_CheckedBuff = BlueprintTool.GetRef<BlueprintBuffReference>(null);
            }

            buffExtraEffects.m_ExceptionFact = exceptionFact?.Reference ?? buffExtraEffects.m_ExceptionFact;
            if (buffExtraEffects.m_ExceptionFact == null)
            {
                buffExtraEffects.m_ExceptionFact = BlueprintTool.GetRef<BlueprintUnitFactReference>(null);
            }

            buffExtraEffects.m_ExtraEffectBuff = extraEffectBuff?.Reference ?? buffExtraEffects.m_ExtraEffectBuff;
            if (buffExtraEffects.m_ExtraEffectBuff == null)
            {
                buffExtraEffects.m_ExtraEffectBuff = BlueprintTool.GetRef<BlueprintBuffReference>(null);
            }

            return AddComponent(buffExtraEffects);
        }

        //
        // Summary:
        //     Adds Kingmaker.Designers.Mechanics.Facts.HealResistance
        //
        // Remarks:
        //     • Used by
        //     • DLC3_SicknessHeavyDiseaseBuff –4a19a78fbde84e03b4547137b11ddc3a
        public TBuilder AddHealResistance(float? resistance = null)
        {
            HealResistance healResistance = new HealResistance();
            healResistance.Resistance = resistance ?? healResistance.Resistance;
            return AddComponent(healResistance);
        }

        //
        // Summary:
        //     Adds Kingmaker.Designers.Mechanics.Facts.MetamagicOnNextSpell
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        //   sourcerousReflex:
        //     InfoBox: Проверить что заклинание 1ого уровня или на 2 уровня ниже, чем макс.
        //     доступный уровень заклинаний
        //
        // Remarks:
        //     • Used by
        //     • BattleMagesHeadbandBuff –2c06b3504c3013e4ba7ea04b1d9201a3
        //     • QuickenedArcanaBuff –5a62abe6d8de2e24b8834493438b3e23
        //     • UniversalistSchoolReachBuff –8bc0ae0545e59f14aaef85f064fdc263
        public TBuilder AddMetamagicOnNextSpell(bool? doNotRemove = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail, Metamagic? metamagic = null, bool? sourcerousReflex = null)
        {
            MetamagicOnNextSpell metamagicOnNextSpell = new MetamagicOnNextSpell();
            metamagicOnNextSpell.DoNotRemove = doNotRemove ?? metamagicOnNextSpell.DoNotRemove;
            metamagicOnNextSpell.Metamagic = metamagic ?? metamagicOnNextSpell.Metamagic;
            metamagicOnNextSpell.SourcerousReflex = sourcerousReflex ?? metamagicOnNextSpell.SourcerousReflex;
            return AddUniqueComponent(metamagicOnNextSpell, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.Designers.Mechanics.Facts.MetamagicRodMechanics
        //
        // Parameters:
        //   abilitiesWhiteList:
        //     Blueprint of type BlueprintAbility. You can pass in the blueprint using:
        //     • A blueprint instance –
        //     • A blueprint reference –
        //     • A blueprint id as a string, Guid, or BlueprintGuid –
        //     • A blueprint name registered with BlueprintTool –
        //     See Blueprint for more details.
        //
        //   rodAbility:
        //     Blueprint of type BlueprintActivatableAbility. You can pass in the blueprint
        //     using:
        //     • A blueprint instance –
        //     • A blueprint reference –
        //     • A blueprint id as a string, Guid, or BlueprintGuid –
        //     • A blueprint name registered with BlueprintTool –
        //     See Blueprint for more details.
        //
        // Remarks:
        //     • Used by
        //     • AmberLoveRodBuff –3123ec8935850514aab8edccf7d8e942
        //     • MetamagicRodLesserMaximizeBuff –9799c61073256e747bcbbda5a565af8d
        //     • SkeletalFingerRodQuickenNormalBuff –2e03f85b3e759a743b2cae86b687ba4f
        public TBuilder AddMetamagicRodMechanics(List<Blueprint<BlueprintAbilityReference>>? abilitiesWhiteList = null, int? maxSpellLevel = null, Metamagic? metamagic = null, Blueprint<BlueprintActivatableAbilityReference>? rodAbility = null)
        {
            MetamagicRodMechanics metamagicRodMechanics = new MetamagicRodMechanics();
            metamagicRodMechanics.m_AbilitiesWhiteList = abilitiesWhiteList?.Select((Blueprint<BlueprintAbilityReference> bp) => bp.Reference)?.ToArray() ?? metamagicRodMechanics.m_AbilitiesWhiteList;
            if (metamagicRodMechanics.m_AbilitiesWhiteList == null)
            {
                metamagicRodMechanics.m_AbilitiesWhiteList = new BlueprintAbilityReference[0];
            }

            metamagicRodMechanics.MaxSpellLevel = maxSpellLevel ?? metamagicRodMechanics.MaxSpellLevel;
            metamagicRodMechanics.Metamagic = metamagic ?? metamagicRodMechanics.Metamagic;
            metamagicRodMechanics.m_RodAbility = rodAbility?.Reference ?? metamagicRodMechanics.m_RodAbility;
            if (metamagicRodMechanics.m_RodAbility == null)
            {
                metamagicRodMechanics.m_RodAbility = BlueprintTool.GetRef<BlueprintActivatableAbilityReference>(null);
            }

            return AddComponent(metamagicRodMechanics);
        }

        //
        // Summary:
        //     Adds Kingmaker.Designers.Mechanics.Facts.NeutralToFaction
        //
        // Parameters:
        //   faction:
        //     Blueprint of type BlueprintFaction. You can pass in the blueprint using:
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
        //
        // Remarks:
        //     • Used by
        //     • Charm –9dc29118addce3d48ae9b92be953b5b4
        public TBuilder AddNeutralToFaction(Blueprint<BlueprintFactionReference>? faction = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail)
        {
            NeutralToFaction neutralToFaction = new NeutralToFaction();
            neutralToFaction.m_Faction = faction?.Reference ?? neutralToFaction.m_Faction;
            if (neutralToFaction.m_Faction == null)
            {
                neutralToFaction.m_Faction = BlueprintTool.GetRef<BlueprintFactionReference>(null);
            }

            return AddUniqueComponent(neutralToFaction, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.Designers.Mechanics.Facts.SpecificSpellDamageBonus
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        //   spell:
        //     Blueprint of type BlueprintAbility. You can pass in the blueprint using:
        //     • A blueprint instance –
        //     • A blueprint reference –
        //     • A blueprint id as a string, Guid, or BlueprintGuid –
        //     • A blueprint name registered with BlueprintTool –
        //     See Blueprint for more details.
        //
        // Remarks:
        //     • Used by
        //     • SpellblastBombBuff –c783f23e678f71542995c01e36540206
        public TBuilder AddSpecificSpellDamageBonus(int? bonus = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail, List<Blueprint<BlueprintAbilityReference>>? spell = null)
        {
            SpecificSpellDamageBonus specificSpellDamageBonus = new SpecificSpellDamageBonus();
            specificSpellDamageBonus.Bonus = bonus ?? specificSpellDamageBonus.Bonus;
            specificSpellDamageBonus.m_Spell = spell?.Select((Blueprint<BlueprintAbilityReference> bp) => bp.Reference)?.ToArray() ?? specificSpellDamageBonus.m_Spell;
            if (specificSpellDamageBonus.m_Spell == null)
            {
                specificSpellDamageBonus.m_Spell = new BlueprintAbilityReference[0];
            }

            return AddUniqueComponent(specificSpellDamageBonus, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.Designers.Mechanics.Buffs.ACBonusAgainstCaster
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • DebilitatingInjuryBewilderedEffectBuff –22b1d98502050cb4cbdb3679ac53115e
        public TBuilder AddACBonusAgainstCaster(ModifierDescriptor? descriptor = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail, ContextValue? value = null)
        {
            ACBonusAgainstCaster aCBonusAgainstCaster = new ACBonusAgainstCaster();
            aCBonusAgainstCaster.Descriptor = descriptor ?? aCBonusAgainstCaster.Descriptor;
            aCBonusAgainstCaster.Value = value ?? aCBonusAgainstCaster.Value;
            if (aCBonusAgainstCaster.Value == null)
            {
                aCBonusAgainstCaster.Value = ContextValues.Constant(0);
            }

            return AddUniqueComponent(aCBonusAgainstCaster, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.Designers.Mechanics.Buffs.ACBonusAgainstTarget
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • ArmyChallengeBuff –8cd456875ab94e16a3b51d4c6e9fe7b7
        //     • HalfFiendSmiteGoodBuff –114af78efc58e5a4c86bb12ee1d907cc
        //     • SmiteEvilBuff_Scabbard –d0261b79ea01d73418eaf3307352792c
        public TBuilder AddACBonusAgainstTarget(bool? checkCaster = null, bool? checkCasterFriend = null, ModifierDescriptor? descriptor = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail, ContextValue? value = null)
        {
            ACBonusAgainstTarget aCBonusAgainstTarget = new ACBonusAgainstTarget();
            aCBonusAgainstTarget.CheckCaster = checkCaster ?? aCBonusAgainstTarget.CheckCaster;
            aCBonusAgainstTarget.CheckCasterFriend = checkCasterFriend ?? aCBonusAgainstTarget.CheckCasterFriend;
            aCBonusAgainstTarget.Descriptor = descriptor ?? aCBonusAgainstTarget.Descriptor;
            aCBonusAgainstTarget.Value = value ?? aCBonusAgainstTarget.Value;
            if (aCBonusAgainstTarget.Value == null)
            {
                aCBonusAgainstTarget.Value = ContextValues.Constant(0);
            }

            return AddUniqueComponent(aCBonusAgainstTarget, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds BlueprintCore.Blueprints.Configurators.UnitLogic.Buffs.BaseBuffConfigurator`2.AddAdditionalLimbIfHasFact(BlueprintCore.Utils.Blueprint{Kingmaker.Blueprints.BlueprintFeatureReference},System.Action{Kingmaker.Blueprints.BlueprintComponent,Kingmaker.Blueprints.BlueprintComponent},BlueprintCore.Blueprints.CustomConfigurators.ComponentMerge,BlueprintCore.Utils.Blueprint{Kingmaker.Blueprints.BlueprintItemWeaponReference})
        //
        // Parameters:
        //   checkedFact:
        //     Blueprint of type BlueprintFeature. You can pass in the blueprint using:
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
        //
        //   weapon:
        //     Blueprint of type BlueprintItemWeapon. You can pass in the blueprint using:
        //     • A blueprint instance –
        //     • A blueprint reference –
        //     • A blueprint id as a string, Guid, or BlueprintGuid –
        //     • A blueprint name registered with BlueprintTool –
        //     See Blueprint for more details.
        //
        // Remarks:
        //     • Used by
        //     • DefensiveStanceBuff –3dccdf27a8209af478ac71cded18a271
        //     • StonelordDefensiveStanceBuff –99ab5d010faa4c83b7d41bdd6b1afa83
        public TBuilder AddAdditionalLimbIfHasFact(Blueprint<BlueprintFeatureReference>? checkedFact = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail, Blueprint<BlueprintItemWeaponReference>? weapon = null)
        {
            AddAdditionalLimbIfHasFact addAdditionalLimbIfHasFact = new AddAdditionalLimbIfHasFact();
            addAdditionalLimbIfHasFact.m_CheckedFact = checkedFact?.Reference ?? addAdditionalLimbIfHasFact.m_CheckedFact;
            if (addAdditionalLimbIfHasFact.m_CheckedFact == null)
            {
                addAdditionalLimbIfHasFact.m_CheckedFact = BlueprintTool.GetRef<BlueprintFeatureReference>(null);
            }

            addAdditionalLimbIfHasFact.m_Weapon = weapon?.Reference ?? addAdditionalLimbIfHasFact.m_Weapon;
            if (addAdditionalLimbIfHasFact.m_Weapon == null)
            {
                addAdditionalLimbIfHasFact.m_Weapon = BlueprintTool.GetRef<BlueprintItemWeaponReference>(null);
            }

            return AddUniqueComponent(addAdditionalLimbIfHasFact, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds BlueprintCore.Blueprints.Configurators.UnitLogic.Buffs.BaseBuffConfigurator`2.AddStatBonusAbilityValue(System.Nullable{Kingmaker.Enums.ModifierDescriptor},System.Nullable{Kingmaker.EntitySystem.Stats.StatType},Kingmaker.UnitLogic.Mechanics.ContextValue)
        //
        // Remarks:
        //     ComponentName: Add stat bonus from ability value
        //     • Used by
        //     • ArcaneAccuracyBuff –dd2d0de63be31854794c006dc1077294
        //     • CommunityDomainGreaterBuff –672b0149e68d73943ad09ac35a9f5f30
        //     • ShatterConfidenceBuff –14225a2e4561bfd46874c9a4a97e7133
        public TBuilder AddStatBonusAbilityValue(ModifierDescriptor? descriptor = null, StatType? stat = null, ContextValue? value = null)
        {
            AddStatBonusAbilityValue addStatBonusAbilityValue = new AddStatBonusAbilityValue();
            addStatBonusAbilityValue.Descriptor = descriptor ?? addStatBonusAbilityValue.Descriptor;
            addStatBonusAbilityValue.Stat = stat ?? addStatBonusAbilityValue.Stat;
            addStatBonusAbilityValue.Value = value ?? addStatBonusAbilityValue.Value;
            if (addStatBonusAbilityValue.Value == null)
            {
                addStatBonusAbilityValue.Value = ContextValues.Constant(0);
            }

            return AddComponent(addStatBonusAbilityValue);
        }

        //
        // Summary:
        //     Adds Kingmaker.Designers.Mechanics.Buffs.AttackBonusAgainstCaster
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • DebilitatingInjuryDisorientedEffectBuff –1f1e42f8c06d7dc4bb70cc12c73dfb38
        public TBuilder AddAttackBonusAgainstCaster(ModifierDescriptor? descriptor = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail, ContextValue? value = null)
        {
            AttackBonusAgainstCaster attackBonusAgainstCaster = new AttackBonusAgainstCaster();
            attackBonusAgainstCaster.Descriptor = descriptor ?? attackBonusAgainstCaster.Descriptor;
            attackBonusAgainstCaster.Value = value ?? attackBonusAgainstCaster.Value;
            if (attackBonusAgainstCaster.Value == null)
            {
                attackBonusAgainstCaster.Value = ContextValues.Constant(0);
            }

            return AddUniqueComponent(attackBonusAgainstCaster, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.Designers.Mechanics.Buffs.AttackBonusAgainstTarget
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • ArmySmiteChaosBuff –61d753b863aa449e85fa34fb0374f071
        //     • FreebootersBaneBuff –76dabd40a1c1c644c86ce30e41ad5cab
        //     • VanguardAlliesStudyTargetBuff –261d47a0e2df6cf4fa6f02ec2cfb528a
        public TBuilder AddAttackBonusAgainstTarget(bool? checkCaster = null, bool? checkCasterFriend = null, bool? checkRangeType = null, ModifierDescriptor? descriptor = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail, WeaponRangeType? rangeType = null, ContextValue? value = null)
        {
            AttackBonusAgainstTarget attackBonusAgainstTarget = new AttackBonusAgainstTarget();
            attackBonusAgainstTarget.CheckCaster = checkCaster ?? attackBonusAgainstTarget.CheckCaster;
            attackBonusAgainstTarget.CheckCasterFriend = checkCasterFriend ?? attackBonusAgainstTarget.CheckCasterFriend;
            attackBonusAgainstTarget.CheckRangeType = checkRangeType ?? attackBonusAgainstTarget.CheckRangeType;
            attackBonusAgainstTarget.Descriptor = descriptor ?? attackBonusAgainstTarget.Descriptor;
            attackBonusAgainstTarget.RangeType = rangeType ?? attackBonusAgainstTarget.RangeType;
            attackBonusAgainstTarget.Value = value ?? attackBonusAgainstTarget.Value;
            if (attackBonusAgainstTarget.Value == null)
            {
                attackBonusAgainstTarget.Value = ContextValues.Constant(0);
            }

            return AddUniqueComponent(attackBonusAgainstTarget, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.Designers.Mechanics.Buffs.BuffInvisibility
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • DLC4_InvisibilityBuff_Cutscene –55dba144a4c54f2890a8f21f72886330
        //     • OracleRevelationInvisibilityBuff –a2b527cfac3f87244a133415a7cb5926
        //     • VanishBuff –e5b7ef8d49215314daaf0404349d42a6
        public TBuilder AddBuffInvisibility(ContextValue? chance = null, bool? dispelWithAChance = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail, bool? notDispellAfterOffensiveAction = null, int? stealthBonus = null)
        {
            BuffInvisibility buffInvisibility = new BuffInvisibility();
            buffInvisibility.Chance = chance ?? buffInvisibility.Chance;
            if (buffInvisibility.Chance == null)
            {
                buffInvisibility.Chance = ContextValues.Constant(0);
            }

            buffInvisibility.DispelWithAChance = dispelWithAChance ?? buffInvisibility.DispelWithAChance;
            buffInvisibility.NotDispellAfterOffensiveAction = notDispellAfterOffensiveAction ?? buffInvisibility.NotDispellAfterOffensiveAction;
            buffInvisibility.m_StealthBonus = stealthBonus ?? buffInvisibility.m_StealthBonus;
            return AddUniqueComponent(buffInvisibility, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.Designers.Mechanics.Buffs.BuffPoisonStatDamage
        //
        // Remarks:
        //     ComponentName: BuffMechanics/Add Random Stat Penalty
        //     • Used by
        //     • ApocalypseLocustPoisonBuff –9857d0c4417c47209dcb0ba64ad14c98
        //     • DiseaseMindFireBuff –2e13b4ac81ae4a745989a0e78a4c44e4
        //     • WyvernPoisonBuff –b5d9dc8671f8c9c4dab37f0ba52ab9d1
        public TBuilder AddBuffPoisonStatDamage(int? bonus = null, ModifierDescriptor? descriptor = null, bool? noEffectOnFirstTick = null, SavingThrowType? saveType = null, StatType? stat = null, int? succesfullSaves = null, int? ticks = null, DiceFormula? value = null)
        {
            BuffPoisonStatDamage buffPoisonStatDamage = new BuffPoisonStatDamage();
            buffPoisonStatDamage.Bonus = bonus ?? buffPoisonStatDamage.Bonus;
            buffPoisonStatDamage.Descriptor = descriptor ?? buffPoisonStatDamage.Descriptor;
            buffPoisonStatDamage.NoEffectOnFirstTick = noEffectOnFirstTick ?? buffPoisonStatDamage.NoEffectOnFirstTick;
            buffPoisonStatDamage.SaveType = saveType ?? buffPoisonStatDamage.SaveType;
            buffPoisonStatDamage.Stat = stat ?? buffPoisonStatDamage.Stat;
            buffPoisonStatDamage.SuccesfullSaves = succesfullSaves ?? buffPoisonStatDamage.SuccesfullSaves;
            buffPoisonStatDamage.Ticks = ticks ?? buffPoisonStatDamage.Ticks;
            buffPoisonStatDamage.Value = value ?? buffPoisonStatDamage.Value;
            return AddComponent(buffPoisonStatDamage);
        }

        //
        // Summary:
        //     Adds Kingmaker.Designers.Mechanics.Buffs.BuffPoisonStatDamageContext
        //
        // Remarks:
        //     ComponentName: BuffMechanics/Add Random Stat Penalty
        //     • Used by
        //     • AssassinCreatePoisonAbilityConBuffEffect –6542e025d84501a41b652bcdc57f6901
        //     • AssassinCreatePoisonAbilityDexBuffEffect –c766f0606ac12e24e8a9fdb8beabc6c2
        //     • AssassinCreatePoisonAbilityStrBuffEffect –285290cc80941bc4c97461d6f50aaad9
        public TBuilder AddBuffPoisonStatDamageContext(ContextValue? bonus = null, ModifierDescriptor? descriptor = null, bool? noEffectOnFirstTick = null, SavingThrowType? saveType = null, StatType? stat = null, ContextValue? succesfullSaves = null, ContextValue? ticks = null, ContextDiceValue? value = null)
        {
            BuffPoisonStatDamageContext buffPoisonStatDamageContext = new BuffPoisonStatDamageContext();
            buffPoisonStatDamageContext.Bonus = bonus ?? buffPoisonStatDamageContext.Bonus;
            if (buffPoisonStatDamageContext.Bonus == null)
            {
                buffPoisonStatDamageContext.Bonus = ContextValues.Constant(0);
            }

            buffPoisonStatDamageContext.Descriptor = descriptor ?? buffPoisonStatDamageContext.Descriptor;
            buffPoisonStatDamageContext.NoEffectOnFirstTick = noEffectOnFirstTick ?? buffPoisonStatDamageContext.NoEffectOnFirstTick;
            buffPoisonStatDamageContext.SaveType = saveType ?? buffPoisonStatDamageContext.SaveType;
            buffPoisonStatDamageContext.Stat = stat ?? buffPoisonStatDamageContext.Stat;
            buffPoisonStatDamageContext.SuccesfullSaves = succesfullSaves ?? buffPoisonStatDamageContext.SuccesfullSaves;
            if (buffPoisonStatDamageContext.SuccesfullSaves == null)
            {
                buffPoisonStatDamageContext.SuccesfullSaves = ContextValues.Constant(0);
            }

            buffPoisonStatDamageContext.Ticks = ticks ?? buffPoisonStatDamageContext.Ticks;
            if (buffPoisonStatDamageContext.Ticks == null)
            {
                buffPoisonStatDamageContext.Ticks = ContextValues.Constant(0);
            }

            buffPoisonStatDamageContext.Value = value ?? buffPoisonStatDamageContext.Value;
            if (buffPoisonStatDamageContext.Value == null)
            {
                buffPoisonStatDamageContext.Value = Constants.Empty.DiceValue;
            }

            return AddComponent(buffPoisonStatDamageContext);
        }

        //
        // Summary:
        //     Adds Kingmaker.Designers.Mechanics.Buffs.BuffSleeping
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • Sleeping –5e0cd801bac0e95429bb7e4d1bc61a23
        public TBuilder AddBuffSleeping(Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail, int? wakeupPerceptionDC = null)
        {
            BuffSleeping buffSleeping = new BuffSleeping();
            buffSleeping.WakeupPerceptionDC = wakeupPerceptionDC ?? buffSleeping.WakeupPerceptionDC;
            return AddUniqueComponent(buffSleeping, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.Designers.Mechanics.Buffs.ControlledProjectileHolder
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • BoneSpearProjectileBuff –a99e1f16c3f614b419e7722ae71a7459
        //     • HasControllableProjectileBuff –0e92c82297202bd4abac2c5a2ce2f9d3
        public TBuilder AddControlledProjectileHolder(Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail)
        {
            ControlledProjectileHolder component = new ControlledProjectileHolder();
            return AddUniqueComponent(component, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.Designers.Mechanics.Buffs.DamageBonusAgainstTarget
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • ArmyChallengeBuff –8cd456875ab94e16a3b51d4c6e9fe7b7
        //     • FreebootersBaneBuff –76dabd40a1c1c644c86ce30e41ad5cab
        //     • VanguardAlliesStudyTargetBuff –261d47a0e2df6cf4fa6f02ec2cfb528a
        public TBuilder AddDamageBonusAgainstTarget(bool? applyToSpellDamage = null, bool? checkCaster = null, bool? checkCasterFriend = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail, ContextValue? value = null)
        {
            DamageBonusAgainstTarget damageBonusAgainstTarget = new DamageBonusAgainstTarget();
            damageBonusAgainstTarget.ApplyToSpellDamage = applyToSpellDamage ?? damageBonusAgainstTarget.ApplyToSpellDamage;
            damageBonusAgainstTarget.CheckCaster = checkCaster ?? damageBonusAgainstTarget.CheckCaster;
            damageBonusAgainstTarget.CheckCasterFriend = checkCasterFriend ?? damageBonusAgainstTarget.CheckCasterFriend;
            damageBonusAgainstTarget.Value = value ?? damageBonusAgainstTarget.Value;
            if (damageBonusAgainstTarget.Value == null)
            {
                damageBonusAgainstTarget.Value = ContextValues.Constant(0);
            }

            return AddUniqueComponent(damageBonusAgainstTarget, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.Designers.Mechanics.Buffs.IgnoreTargetDR
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • ArmySmiteChaosBuff –61d753b863aa449e85fa34fb0374f071
        //     • HalfFiendSmiteGoodBuff –114af78efc58e5a4c86bb12ee1d907cc
        //     • StudentOfWarDeadlyBlowBuff –7795183a0e72ec14cb2e4d51acc53773
        public TBuilder AddIgnoreTargetDR(bool? checkCaster = null, bool? checkCasterFriend = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail)
        {
            IgnoreTargetDR ignoreTargetDR = new IgnoreTargetDR();
            ignoreTargetDR.CheckCaster = checkCaster ?? ignoreTargetDR.CheckCaster;
            ignoreTargetDR.CheckCasterFriend = checkCasterFriend ?? ignoreTargetDR.CheckCasterFriend;
            return AddUniqueComponent(ignoreTargetDR, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.Designers.Mechanics.Buffs.OverrideLocoMotion
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • Anevia_BrokenLeg –13e59e6d056c3ba4e8b44b9dce3b641a
        //     • DLC2_Player_Wounded_Buff –6f408add25eb4c6ca4d40d7e9f809d62
        //     • TorchAnimations_Buff –06d32871142844e5a14d43c6e0ca9bb4
        public TBuilder AddOverrideLocoMotion(UnitAnimationActionLocoMotion? locoMotion = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail)
        {
            OverrideLocoMotion overrideLocoMotion = new OverrideLocoMotion();
            Validate(locoMotion);
            overrideLocoMotion.LocoMotion = locoMotion ?? overrideLocoMotion.LocoMotion;
            return AddUniqueComponent(overrideLocoMotion, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.Designers.Mechanics.Buffs.RemovedByHeal
        //
        // Parameters:
        //   merge:
        //     If mergeBehavior is ComponentMerge.Merge and the component already exists, this
        //     expression is called to merge the components.
        //
        //   mergeBehavior:
        //     Handling if the component already exists since the component is unique. Defaults
        //     to ComponentMerge.Fail.
        //
        // Remarks:
        //     • Used by
        //     • ArmyBloodLustAuraEnemyBuff –da30e66cd0144a04ab8275666d34254d
        //     • DebilitatingInjuryHamperedEffectBuff –5bfefc22a68e736488b0c309d9c1c1d4
        //     • Gallu_Buff_RainOfBlood –1abd01485cd76784f8ca078e80132a76
        public TBuilder AddRemovedByHeal(Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail)
        {
            RemovedByHeal component = new RemovedByHeal();
            return AddUniqueComponent(component, mergeBehavior, merge);
        }

        //
        // Summary:
        //     Adds Kingmaker.Designers.Mechanics.Buffs.HalfOfPair.HalfOfPairComponent
        //
        // Parameters:
        //   buff:
        //     Blueprint of type BlueprintBuff. You can pass in the blueprint using:
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
        //
        // Remarks:
        //     • Used by
        //     • HalfOfPairedPendantBuff –066229a41ae97d6439fea81ebf141528
        public TBuilder AddHalfOfPairComponent(Blueprint<BlueprintBuffReference>? buff = null, int? distanceToActivateInFeet = null, Action<BlueprintComponent, BlueprintComponent>? merge = null, ComponentMerge mergeBehavior = ComponentMerge.Fail, float? sqrDistance = null)
        {
            HalfOfPairComponent halfOfPairComponent = new HalfOfPairComponent();
            halfOfPairComponent.m_Buff = buff?.Reference ?? halfOfPairComponent.m_Buff;
            if (halfOfPairComponent.m_Buff == null)
            {
                halfOfPairComponent.m_Buff = BlueprintTool.GetRef<BlueprintBuffReference>(null);
            }

            halfOfPairComponent.m_DistanceToActivateInFeet = distanceToActivateInFeet ?? halfOfPairComponent.m_DistanceToActivateInFeet;
            halfOfPairComponent.m_SqrDistance = sqrDistance ?? halfOfPairComponent.m_SqrDistance;
            return AddUniqueComponent(halfOfPairComponent, mergeBehavior, merge);
        }

        protected override void OnConfigureCompleted()
        {
            base.OnConfigureCompleted();
            if ((object)Blueprint.FxOnStart == null)
            {
                Blueprint.FxOnStart = Constants.Empty.PrefabLink;
            }

            if ((object)Blueprint.FxOnRemove == null)
            {
                Blueprint.FxOnRemove = Constants.Empty.PrefabLink;
            }

            if (Blueprint.ResourceAssetIds == null)
            {
                Blueprint.ResourceAssetIds = new string[0];
            }
        }
    }
}
#if false // Decompilation log
'34' items in cache
------------------
Resolve: 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\mscorlib.dll'
------------------
Resolve: 'Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'E:\Projects\VisualStudio\TransfiguredCasterArchetypes\lib\Assembly-CSharp.dll'
------------------
Resolve: 'System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Core.dll'
------------------
Resolve: 'System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.dll'
------------------
Resolve: 'Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'D:\Steam\steamapps\common\Pathfinder Second Adventure\Wrath_Data\Managed\Newtonsoft.Json.dll'
------------------
Resolve: '0Harmony, Version=2.0.4.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: '0Harmony, Version=2.2.2.0, Culture=neutral, PublicKeyToken=null'
WARN: Version mismatch. Expected: '2.0.4.0', Got: '2.2.2.0'
Load from: 'D:\Steam\steamapps\common\Pathfinder Second Adventure\Wrath_Data\Managed\UnityModManager\0Harmony.dll'
------------------
Resolve: 'Owlcat.Runtime.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'Owlcat.Runtime.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'D:\Steam\steamapps\common\Pathfinder Second Adventure\Wrath_Data\Managed\Owlcat.Runtime.Core.dll'
------------------
Resolve: 'Owlcat.Runtime.Validation, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'Owlcat.Runtime.Validation, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'D:\Steam\steamapps\common\Pathfinder Second Adventure\Wrath_Data\Managed\Owlcat.Runtime.Validation.dll'
------------------
Resolve: 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'D:\Steam\steamapps\common\Pathfinder Second Adventure\Wrath_Data\Managed\UnityEngine.CoreModule.dll'
------------------
Resolve: 'UnityEngine.AssetBundleModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'UnityEngine.AssetBundleModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'Owlcat.Runtime.Visual, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'Owlcat.Runtime.Visual, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'D:\Steam\steamapps\common\Pathfinder Second Adventure\Wrath_Data\Managed\Owlcat.Runtime.Visual.dll'
------------------
Resolve: 'Owlcat.Runtime.UI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'Owlcat.Runtime.UI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'D:\Steam\steamapps\common\Pathfinder Second Adventure\Wrath_Data\Managed\Owlcat.Runtime.UI.dll'
#endif
