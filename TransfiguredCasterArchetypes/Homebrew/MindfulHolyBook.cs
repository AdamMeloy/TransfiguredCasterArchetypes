using System;
using UnityEngine;
using BlueprintCore.Blueprints.Configurators.Items.Weapons;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem.Converters;
using TransfiguredCasterArchetypes.Util;

namespace TransfiguredCasterArchetypes.Homebrew
{
    internal class MindfulHolyBook
    {
        internal const string Weapon = "MindfulHolyBookWeapon";
        internal const string WeaponName = "MindfulHolyBookWeapon.Name";
        internal const string BaseWeaponName = "HolyBookWeapon.Name";

        private static readonly Logging.Logger Logger = Logging.GetLogger(Weapon);

        internal static void Configure()
        {
            try
            {
                if (Settings.IsEnabled(Guids.LivingGrimoireArchetype))
                    if (Settings.IsEnabled(Guids.MindfulEnchantmentHomebrew))
                        if (Settings.IsEnabled(Guids.MindfulHolyBookWeapon))
                            ConfigureEnabled();
                        else ConfigureEnabled();
                    else ConfigureDisabled();
                else ConfigureDisabled();
            }
            catch (Exception e)
            {
                Logger.LogException("MindfulHolyBook.Configure", e);
            }
        }

        internal static void ConfigureDelayed()
        {
            try
            {
                if (Settings.IsEnabled(Guids.LivingGrimoireArchetype))
                    if (Settings.IsEnabled(Guids.MindfulEnchantmentHomebrew))
                        if (Settings.IsEnabled(Guids.MindfulHolyBookWeapon))
                            ConfigureEnabledDelayed();
                        else ConfigureEnabled();
                    else ConfigureDisabled();
                else ConfigureDisabled();
            }
            catch (Exception e)
            {
                Logger.LogException("MindfulHolyBook.ConfigureDelayed", e);
            }
        }

        private static void ConfigureDisabled()
        {
            Logger.Log($"Configuring {Weapon} (disabled)");

            ItemWeaponConfigurator.New(Weapon, Guids.MindfulHolyBookWeapon)
                .Configure();

        }

        private static void ConfigureEnabled()
        {
            Logger.Log($"Configuring {Weapon}");

            var weapon = ItemWeaponConfigurator.New(Weapon, Guids.MindfulHolyBookWeapon)
                .CopyFrom(ItemWeaponRefs.ColdIronLightMace.ToString())
                .SetDisplayNameText(BaseWeaponName)


                .SetIcon((Sprite)UnityObjectConverter.AssetList.Get("7ab85c5de2127eb49a1e3ba027ffb171", 21300000))

                .SetCost(-100000)
                .SetIsNotable(true)
                .SetDestructible(false)
                .SetCR(0)

                .SetWeight(0)
                .SetType(Guids.HolyBookWeaponType)
                .SetSize(Kingmaker.Enums.Size.Medium)
                .SetEnchantments(WeaponEnchantmentRefs.ColdIronWeaponEnchantment.ToString(), BlueprintTool.GetRef<BlueprintWeaponEnchantmentReference>(Guids.MindfulEnchantmentHomebrew).ToString())
                .Configure();
        }

        private static void ConfigureEnabledDelayed()
        {
            if (!Settings.IsTTTBaseEnabled())
                return;
        }
    }
}
