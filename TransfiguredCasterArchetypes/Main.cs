using System;
using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.PubSubSystem;
using TabletopTweaks.Core.NewEvents;
using UnityModManagerNet;
using TransfiguredCasterArchetypes.Archetypes;
using TransfiguredCasterArchetypes.ClassFeatures;
using TransfiguredCasterArchetypes.Classes;
using TransfiguredCasterArchetypes.Feats;
using TransfiguredCasterArchetypes.Homebrew;
using TransfiguredCasterArchetypes.Weapons;
using TransfiguredCasterArchetypes.Util;

namespace TransfiguredCasterArchetypes
{
    public static class Main
    {
        private static readonly Logging.Logger Logger = Logging.GetLogger(nameof(Main));

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            try
            {
                var harmony = new Harmony(modEntry.Info.Id);
                harmony.PatchAll();

                EventBus.Subscribe(new BlueprintCacheInitHandler());

                Logger.Log("Finished patching.");
            }
            catch (Exception e)
            {
                Logger.LogException("Failed to patch", e);
            }
            return true;
        }

        class BlueprintCacheInitHandler : IBlueprintCacheInitHandler
        {
            private static bool Initialized = false;
            private static bool InitializeDelayed = false;

            public void AfterBlueprintCachePatches()
            {
                try
                {
                    if (InitializeDelayed)
                    {
                        Logger.Log("Already initialized blueprints cache.");
                        return;
                    }
                    InitializeDelayed = true;

                    //ConfigureFeatsDelayed();
                    ConfigureWeaponsDelayed();
                    ConfigureHomebrewDelayed();
                    //ConfigureClassFeaturesDelayed();
                    //ConfigureClassesDelayed();
                    ConfigureArchetypesDelayed();
                }
                catch (Exception e)
                {
                    Logger.LogException("Delayed blueprint configuration failed.", e);
                }
            }

            public void BeforeBlueprintCacheInit() { }

            public void BeforeBlueprintCachePatches() { }

            public void AfterBlueprintCacheInit()
            {
                try
                {
                    if (Initialized)
                    {
                        Logger.Log("Already initialized blueprints cache.");
                        return;
                    }
                    Initialized = true;

                    // First strings
                    LocalizationTool.LoadEmbeddedLocalizationPacks(
                        "TransfiguredCasterArchetypes.Strings.Archetypes.json",
                        "TransfiguredCasterArchetypes.Strings.Classes.json",
                        "TransfiguredCasterArchetypes.Strings.ClassFeatures.json",
                        "TransfiguredCasterArchetypes.Strings.Feats.json",
                        "TransfiguredCasterArchetypes.Strings.Homebrew.json",
                        "TransfiguredCasterArchetypes.Strings.Settings.json",
                        "TransfiguredCasterArchetypes.Strings.Weapons.json");

                    // Then settings
                    Settings.Init();

                    //ConfigureFeats();
                    ConfigureHomebrew();
                    ConfigureWeapons();
                    //ConfigureClassFeatures();
                    //ConfigureClasses();
                    ConfigureArchetypes();
                }
                catch (Exception e)
                {
                    Logger.LogException("Failed to initialize.", e);
                }
            }

            private static void ConfigureArchetypes()
            {
                Logger.Log("Configuring Archetypes.");

                LivingGrimoire.Configure();
            }

            private static void ConfigureArchetypesDelayed()
            {
                Logger.Log($"Configuring Archetypes delayed.");

                LivingGrimoire.ConfigureDelayed();
            }

            private static void ConfigureClasses()
            {
                Logger.Log("Configuring Classes.");


            }

            private static void ConfigureClassesDelayed()
            {
                Logger.Log("Configuring Classes delayed.");


            }

            private static void ConfigureClassFeatures()
            {
                Logger.Log("Configuring Class Features.");


            }

            private static void ConfigureClassFeaturesDelayed()
            {
                Logger.Log("Configuring Class Features delayed.");


            }

            private static void ConfigureWeapons()
            {
                Logger.Log("Configuring Weapons.");

                HolyBookType.Configure();
                HolyBook.Configure();
                MindfulHolyBook.Configure();
            }

            private static void ConfigureWeaponsDelayed()
            {
                Logger.Log("Configuring Weapons delayed.");

                HolyBookType.ConfigureDelayed();
                HolyBook.ConfigureDelayed();
                MindfulHolyBook.ConfigureDelayed();
            }

            private static void ConfigureFeats()
            {
                Logger.Log("Configuring Feats.");


            }

            private static void ConfigureFeatsDelayed()
            {
                Logger.Log($"Configuring Feats delayed.");

                
            }

            private static void ConfigureHomebrew()
            {
                Logger.Log("Configuring Homebrew.");

                MindfulEnchantment.Configure();
            }

            private static void ConfigureHomebrewDelayed()
            {
                Logger.Log($"Configuring Homebrew delayed.");

                MindfulEnchantment.ConfigureDelayed();
            }
        }
    }
}