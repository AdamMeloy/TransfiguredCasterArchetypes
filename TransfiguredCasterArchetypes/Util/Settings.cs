using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using BlueprintCore.Utils;
using Kingmaker.Localization;
using UnityModManagerNet;
using ModMenu.Settings;
using Menu = ModMenu.ModMenu;

namespace TransfiguredCasterArchetypes.Util
{
    internal static class Settings
    {
        private static readonly string RootKey = "tca.settings";
        private static readonly string RootStringKey = "TCA.Settings";
        private const string VerboseLoggingKey = "enable-verbose-logs";

        private static readonly Logging.Logger Logger = Logging.GetLogger(nameof(Settings));

        internal static bool IsEnabled(string key)
        {
            return Menu.GetSettingValue<bool>(GetKey(key));
        }

        internal static bool IsTTTBaseEnabled()
        {
            return UnityModManager.modEntries.Where(
                mod => mod.Info.Id.Equals("TabletopTweaks-Base") && mod.Enabled && !mod.ErrorOnLoading)
              .Any();
        }

        internal static void Init()
        {
            Logger.Log("Initializing settings.");
            var settings =
              SettingsBuilder.New(RootKey, GetString("Title"))
                .AddToggle(
                  Toggle.New(GetKey(VerboseLoggingKey), defaultValue: false, GetString("VerboseLogging"))
                    .WithLongDescription(GetString("VerboseLogging.Description"))
                    .OnValueChanged(Logging.EnableVerboseLogging))
                .AddDefaultButton(OnDefaultsApplied);

            settings.AddSubHeader(GetString("Archetypes.Title"));
            foreach (var (guid, name) in Guids.Archetypes)
            {
                settings.AddToggle(
                  Toggle.New(GetKey(guid), defaultValue: GetDefault(guid), GetString(name, usePrefix: false))
                    .WithLongDescription(GetString("EnableFeature")));
            }

            /*settings.AddSubHeader(GetString("Classes.Title"));
            foreach (var (guid, name) in Guids.Classes)
            {
                settings.AddToggle(
                  Toggle.New(GetKey(guid), defaultValue: GetDefault(guid), GetString(name, usePrefix: false))
                    .WithLongDescription(GetString("EnableFeature")));
            }

            settings.AddSubHeader(GetString("ClassFeatures.Title"));
            foreach (var (guid, name) in Guids.ClassFeatures)
            {
                settings.AddToggle(
                  Toggle.New(GetKey(guid), defaultValue: GetDefault(guid), GetString(name, usePrefix: false))
                    .WithLongDescription(GetString("EnableFeature")));
            }

            settings.AddSubHeader(GetString("Feats.Title"));
            foreach (var (guid, name) in Guids.Feats)
            {
                settings.AddToggle(
                  Toggle.New(GetKey(guid), defaultValue: GetDefault(guid), GetString(name, usePrefix: false))
                    .WithLongDescription(GetString("EnableFeature")));
            }*/

            settings.AddSubHeader(GetString("Weapons.Title"));
            foreach (var (guid, name) in Guids.Weapons)
            {
                settings.AddToggle(
                  Toggle.New(GetKey(guid), defaultValue: GetDefault(guid), GetString(name, usePrefix: false))
                    .WithLongDescription(GetString("EnableFeature")));
            }

            settings.AddSubHeader(GetString("Homebrew.Title"));
            foreach (var (guid, name) in Guids.Homebrew)
            {
                settings.AddToggle(
                  Toggle.New(GetKey(guid), defaultValue: GetDefault(guid), GetString(name, usePrefix: false))
                    .WithLongDescription(GetString("EnableFeature")));
            }

            Menu.AddSettings(settings);
            Logging.EnableVerboseLogging(IsEnabled(VerboseLoggingKey));
        }

        private static void OnDefaultsApplied()
        {
            Logger.Log($"Default settings restored.");
        }

        private static LocalizedString GetString(string key, bool usePrefix = true)
        {
            var fullKey = usePrefix ? $"{RootStringKey}.{key}" : key;
            return LocalizationTool.GetString(fullKey);
        }

        private static string GetKey(string partialKey)
        {
            return $"{RootKey}.{partialKey}";
        }

        private static readonly List<string> DefaultDisabled = new() { Guids.InvestigatorClass };
        private static bool GetDefault(string key)
        {
            return !DefaultDisabled.Contains(key);
        }
    }
}