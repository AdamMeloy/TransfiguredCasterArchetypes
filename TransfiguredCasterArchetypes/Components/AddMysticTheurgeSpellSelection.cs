using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Utility;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UI.MVVM._VM.CharGen.Phases.Spells;
using Kingmaker.PubSubSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core.Utilities;

namespace TransfiguredCasterArchetypes.Components
{
    [TypeId("0c06e7b89261403b8e25b9845886aea8")]
    internal class AddMysticTheurgeSpellSelection : UnitFactComponentDelegate, IUnitCompleteLevelUpHandler, ISubscriber, IGlobalSubscriber
    {
        [HarmonyPatch(typeof(SpellSelectionData), "CanSelectAnything", new Type[] { typeof(UnitDescriptor) })]
        private static class SpellSelectionData_CanSelectAnything_AdditionalSpellSelection_Patch
        {
            private static void Postfix(SpellSelectionData __instance, ref bool __result, UnitDescriptor unit)
            {
                Spellbook spellbook = unit.Spellbooks.FirstOrDefault((Spellbook s) => s.Blueprint == __instance.Spellbook);
                if (spellbook == null)
                {
                    __result = false;
                }

                if (!__instance.Spellbook.AllSpellsKnown || __instance.ExtraSelected == null || __instance.ExtraSelected.Length == 0 || !__instance.ExtraSelected.HasItem((BlueprintAbility i) => i == null) || __instance.ExtraByStat)
                {
                    return;
                }

                int level;
                for (level = 0; level <= __instance.ExtraMaxLevel; level++)
                {
                    if (__instance.SpellList.SpellsByLevel[level].SpellsFiltered.HasItem((BlueprintAbility sb) => !sb.IsCantrip && !__instance.SpellbookContainsSpell(spellbook, level, sb) && !Enumerable.Contains(__instance.ExtraSelected, sb)))
                    {
                        __result = true;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(CharGenSpellsPhaseVM), "DefinePhaseMode", new Type[]
        {
            typeof(SpellSelectionData),
            typeof(SpellSelectionData.SpellSelectionState)
        })]
        private static class CharGenSpellsPhaseVM_DefinePhaseMode_AdditionalSpellSelection_Patch
        {
            private static void Postfix(CharGenSpellsPhaseVM __instance, ref CharGenSpellsPhaseVM.SpellSelectorMode __result, SpellSelectionData selectionData)
            {
                if (selectionData.Spellbook.AllSpellsKnown && selectionData.ExtraSelected.Any() && !selectionData.ExtraByStat)
                {
                    __result = CharGenSpellsPhaseVM.SpellSelectorMode.AnyLevels;
                }
            }
        }

        [HarmonyPatch(typeof(CharGenSpellsPhaseVM), "OrderPriority", MethodType.Getter)]
        private static class CharGenSpellsPhaseVM_OrderPriority_AdditionalSpellSelection_Patch
        {
            private static void Postfix(CharGenSpellsPhaseVM __instance, ref int __result)
            {
                if (__instance?.m_SelectionData != null && __instance.m_SelectionData.Spellbook?.SpellList?.AssetGuid != __instance.m_SelectionData.SpellList?.AssetGuid)
                {
                    __result -= 500;
                }
            }
        }

        private List<SpellSelectionData> spellSelections = new List<SpellSelectionData>();

        public BlueprintSpellListReference m_SpellList;

        public BlueprintCharacterClassReference m_SpellCastingClass;

        public int MaxSpellLevel;

        public bool UseOffset;

        public int SpellLevelOffset;

        public int Count = 1;

        private Spellbook SpellBook => base.Owner.DemandSpellbook(m_SpellCastingClass);

        private BlueprintSpellList SpellList
        {
            get
            {
                BlueprintSpellListReference spellList = m_SpellList;
                return ProxyList((spellList != null) ? ((BlueprintSpellList)spellList) : SpellBook?.Blueprint?.SpellList);
            }
        }

        public int AdjustedMaxLevel
        {
            get
            {
                if (!UseOffset)
                {
                    return MaxSpellLevel;
                }

                return Math.Max((SpellBook?.MaxSpellLevel ?? 0) - SpellLevelOffset, 1);
            }
        }

        public override void OnActivate()
        {
            LevelUpController levelUpController = Game.Instance?.LevelUpController;
            if (levelUpController == null || SpellBook == null)
            {
                return;
            }

            int? num = (from f in levelUpController.State?.Selections?.Select((FeatureSelectionState s) => s.SelectedItem?.Feature)
                        where f == base.Fact.Blueprint
                        select f).Count();
            int i;
            for (i = 0; i < spellSelections.Count && i < num; i++)
            {
                levelUpController.State.SpellSelections.Add(spellSelections[i]);
                spellSelections[i].SetExtraSpells(Count, AdjustedMaxLevel);
            }

            for (; i < num; i++)
            {
                if (!(i >= num))
                {
                    SpellSelectionData spellSelectionData = levelUpController.State.DemandSpellSelection(SpellBook.Blueprint, SpellList);
                    spellSelectionData.SetExtraSpells(Count, AdjustedMaxLevel);
                    spellSelections.Add(spellSelectionData);
                }
            }
        }

        public override void OnTurnOff()
        {
            if (spellSelections.Empty())
            {
                return;
            }

            LevelUpController controller = Game.Instance?.LevelUpController;
            if (controller != null && SpellBook != null)
            {
                spellSelections.ForEach(delegate (SpellSelectionData selection)
                {
                    controller.State.SpellSelections.Remove(selection);
                });
            }
        }

        public void HandleUnitCompleteLevelup(UnitEntityData unit)
        {
            spellSelections.Clear();
        }

        private BlueprintSpellList ProxyList(BlueprintSpellList referenced)
        {
            return Helpers.CreateCopy(referenced, delegate (BlueprintSpellList bp)
            {
                bp.name += "Proxy";
            });
        }
    }
}
