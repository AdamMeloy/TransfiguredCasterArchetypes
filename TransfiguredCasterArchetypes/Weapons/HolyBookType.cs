using System;
using UnityEngine;
using BlueprintCore.Blueprints.Configurators.Items.Weapons;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.JsonSystem.Converters;
using Kingmaker.RuleSystem;
using TransfiguredCasterArchetypes.Util;

namespace TransfiguredCasterArchetypes.Weapons
{
    internal class HolyBookType
    {
        internal const string WeaponType = "HolyBookWeaponType";
        internal const string WeaponTypeName = "HolyBookWeaponType.Name";

        private static readonly Logging.Logger Logger = Logging.GetLogger(WeaponType);

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
                Logger.LogException("HolyBookType.Configure", e);
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
                Logger.LogException("HolyBookType.ConfigureDelayed", e);
            }
        }

        private static void ConfigureDisabled()
        {
            Logger.Log($"Configuring {WeaponType} (disabled)");

            WeaponTypeConfigurator.New(WeaponType, Guids.HolyBookWeaponType)
                .SetDefaultNameText(WeaponTypeName)
                .Configure();
        }

        private static void ConfigureEnabled()
        {
            Logger.Log($"Configuring {WeaponType}");

            var weaponType = WeaponTypeConfigurator.New(WeaponType, Guids.HolyBookWeaponType)
                .CopyFrom(WeaponTypeRefs.LightMace.ToString())
                .SetTypeNameText(WeaponTypeName)
                .SetDefaultNameText(WeaponTypeName)

                .SetIcon((Sprite)UnityObjectConverter.AssetList.Get("7ab85c5de2127eb49a1e3ba027ffb171", 21300000))

                //.SetVisualParameters() //fix when get model

                .SetWeight(0)
                .SetBaseDamage(new(1, DiceType.D4))
                .SetDestructible(false)
                .Configure();
        }

        private static void ConfigureEnabledDelayed()
        {
            if (!Settings.IsTTTBaseEnabled())
                return;
        }
    }
}
