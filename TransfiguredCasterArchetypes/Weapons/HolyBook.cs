using System;
using UnityEngine;
using BlueprintCore.Blueprints.Configurators.Items.Weapons;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.JsonSystem.Converters;
using TransfiguredCasterArchetypes.Util;

namespace TransfiguredCasterArchetypes.Weapons
{
    internal class HolyBook
    {
        internal const string Weapon = "HolyBookWeapon";
        internal const string WeaponName = "HolyBookWeapon.Name";

        private static readonly Logging.Logger Logger = Logging.GetLogger(Weapon);

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
            { Logger.LogException("HolyBook.Configure", e); }
        }

        internal static void ConfigureDelayed()
        {
            try
            {
                if (Settings.IsEnabled(Guids.LivingGrimoireArchetype))
                    ConfigureEnabledDelayed();
            }
            catch (Exception e)
            { Logger.LogException("HolyBook.ConfigureDelayed", e); }
        }

        private static void ConfigureDisabled()
        {
            Logger.Log($"Configuring {Weapon} (disabled)");
            ItemWeaponConfigurator.New(Weapon, Guids.HolyBookWeapon).Configure();
        }

        private static void ConfigureEnabled()
        {
            Logger.Log($"Configuring {Weapon}");
            ItemWeaponConfigurator.New(Weapon, Guids.HolyBookWeapon)
                .CopyFrom(ItemWeaponRefs.ColdIronLightMace.ToString())
                .SetDisplayNameText(WeaponName)
                .SetIcon((Sprite)UnityObjectConverter.AssetList.Get("7ab85c5de2127eb49a1e3ba027ffb171", 21300000))
                .SetCost(-100000)
                .SetIsNotable(true)
                .SetDestructible(false)
                .SetCR(0)
                .SetWeight(0)
                .SetType(Guids.HolyBookWeaponType)
                .SetSize(Kingmaker.Enums.Size.Medium)
                .Configure();
        }
        
        private static void ConfigureEnabledDelayed()
        {
            if (!Settings.IsTTTBaseEnabled())
                return;
        }
    }
}
