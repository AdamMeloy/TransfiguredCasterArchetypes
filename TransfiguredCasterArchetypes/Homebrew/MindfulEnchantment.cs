using BlueprintCore.Blueprints.Configurators.Items.Ecnchantments;
using BlueprintCore.Blueprints.References;
using Kingmaker.EntitySystem.Stats;
using System;
using TransfiguredCasterArchetypes.Util;
using UnityEngine;

namespace TransfiguredCasterArchetypes.Homebrew
{
    internal class MindfulEnchantment
    {
        internal const string Enchantment = "Mindful";
        internal const string EnchantmentName = "MindfulEnchantment.Name";
        internal const string EnchantmentDescription = "MindfulEnchantment.Description";

        private static readonly Logging.Logger Logger = Logging.GetLogger(Enchantment);

        internal static void Configure()
        {
            try
            {
                if (Settings.IsEnabled(Guids.MindfulEnchantmentHomebrew))
                    ConfigureEnabled();
                else
                    ConfigureDisabled();
            }
            catch (Exception e)
            {
                Logger.LogException("Mindful.Configure", e);
            }
        }

        internal static void ConfigureDelayed()
        {
            try
            {
                if (Settings.IsEnabled(Guids.MindfulEnchantmentHomebrew))
                    ConfigureEnabledDelayed();
            }
            catch (Exception e)
            {
                Logger.LogException("Mindful.ConfigureDelayed", e);
            }
        }

        private static void ConfigureDisabled()
        {
            Logger.Log($"Configuring {Enchantment} (disabled)");

            WeaponEnchantmentConfigurator.New(Enchantment, Guids.MindfulEnchantmentHomebrew)
                .SetEnchantName(EnchantmentName)
                .Configure();
        }

        private static void ConfigureEnabled()
        {
            Logger.Log($"Configuring {Enchantment}");

            var enchantment = WeaponEnchantmentConfigurator.New(Enchantment, Guids.MindfulEnchantmentHomebrew)
                .CopyFrom(WeaponEnchantmentRefs.Agile)
                .SetEnchantName(EnchantmentName)
                .SetDescription(EnchantmentDescription)
                .SetPrefix(Enchantment)
                .AddWeaponDamageStatReplacement(default, default, false, StatType.Intelligence)
                .SetWeaponFxPrefab(BlueprintCore.Utils.Constants.Empty.PrefabLink) //new, might break things
                .Configure();
        }

        private static void ConfigureEnabledDelayed()
        {
            if (!Settings.IsTTTBaseEnabled())
                return;
        }


    }
}
