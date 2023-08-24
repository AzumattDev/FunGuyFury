using System;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using FunGuy_Fury.Util;
using HarmonyLib;
using ItemManager;
using JetBrains.Annotations;
using ServerSync;
using StatusEffectManager;

namespace FunGuy_Fury
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class FunGuy_FuryPlugin : BaseUnityPlugin
    {
        internal const string ModName = "FunGuy_Fury";
        internal const string ModVersion = "1.0.4";
        internal const string Author = "Azumatt";
        private const string ModGUID = Author + "." + ModName;
        private static string ConfigFileName = ModGUID + ".cfg";
        private static string ConfigFileFullPath = Paths.ConfigPath + Path.DirectorySeparatorChar + ConfigFileName;

        internal static string ConnectionError = "";

        private readonly Harmony _harmony = new(ModGUID);

        internal static FunGuy_FuryPlugin context = null!;

        public static readonly ManualLogSource FunGuyFuryLogger =
            BepInEx.Logging.Logger.CreateLogSource(ModName);

        private static readonly ConfigSync ConfigSync = new(ModGUID)
            { DisplayName = ModName, CurrentVersion = ModVersion, MinimumRequiredVersion = ModVersion };

        public enum Toggle
        {
            On = 1,
            Off = 0
        }

        public void Awake()
        {
            context = this;
            _serverConfigLocked = config("1 - General", "Lock Configuration", Toggle.On,
                "If on, the configuration is locked and can be changed by server admins only.");
            _ = ConfigSync.AddLockingConfigEntry(_serverConfigLocked);

            // Damage boost
            DamageBoost = config("1 - General", "Damage Boost", 2.0f,
                "Damage multiplier gained from eating the mushroom. Applies to creatures, players, and trees.");

            // cooldown config
            Cooldown = config("2 - Berserk Effect", "Cooldown", 15f,
                "Cooldown in seconds between each use of the effect. Prevents eating the food if the cooldown is not over. " +
                "Displays the cooldown when failed to eat the food. Tweak the Cooldown Message to change the message displayed.");
            // duration config
            Duration = config("2 - Berserk Effect", "Duration", 15f,
                "Duration in seconds of the berserker effect.");
            // Start message config
            StartMessage = textEntryConfig("2 - Berserk Effect", "Start Message",
                "Damage increased at the cost of health!",
                "Message displayed when the berserk effect starts.", false);
            // Stop message config
            StopMessage = textEntryConfig("2 - Berserk Effect", "Stop Message", "",
                "Message displayed when the berserk effect ends.", false);
            // Cooldown message config
            CooldownMessage = textEntryConfig("2 - Berserk Effect", "Cooldown Message",
                "You are still recovering from your last berserk rage! {0} remaining.",
                "Message displayed when the cooldown is not over. {0} is replaced by the remaining time in seconds.",
                false);

            // effect tooltip config
            EffectTooltip = textEntryConfig("2 - Berserk Effect", "Effect Tooltip",
                "<color=red>Increase damage x2, but at the cost of health loss over time</color>",
                "Tooltip shown when hovering over the Fly Agaric mushroom to describe the effect.", false);
            // Damage per second config
            DamagePerHit = config("2 - Berserk Effect", "Damage Per Hit", 5f,
                "Damage taken per hit while berserk.");
            // Damage interval config
            DamageInterval = config("2 - Berserk Effect", "Damage Interval", 1f,
                "Interval in seconds between each damage tick.");

            // On change of any of the variables, call the function to update the config for the status effect. Does not apply to active berserk effects.
            Cooldown.SettingChanged += Functions.UpdateConfig;
            Duration.SettingChanged += Functions.UpdateConfig;
            StartMessage.SettingChanged += Functions.UpdateConfig;
            StopMessage.SettingChanged += Functions.UpdateConfig;
            EffectTooltip.SettingChanged += Functions.UpdateConfig;
            DamagePerHit.SettingChanged += Functions.UpdateConfig;
            DamageInterval.SettingChanged += Functions.UpdateConfig;

            Item flyagaricHat = new("funguyfury", "FlyAgaricHat");
            flyagaricHat.Name.English("Fly Agaric Hat");
            flyagaricHat.Description.English("A amanita muscaria mushroom hat!");
            flyagaricHat.Crafting.Add(CraftingTable.Inventory, 3);
            flyagaricHat.RequiredItems.Add("FlyAgaricMushroom", 20);
            flyagaricHat.RequiredUpgradeItems.Add("FlyAgaricMushroom", 10);
            flyagaricHat.Snapshot();

            Item flyagaricMushrom = new("funguyfury", "FlyAgaricMushroom");
            flyagaricMushrom.Name.English("Fly Agaric Mushroom");
            flyagaricMushrom.Description.English("Amanita muscaria, commonly known as the fly agaric or fly amanita");
            flyagaricMushrom.Snapshot();
            flyagaricMushrom.DropsFrom.Add("Draugr", 0.1f, 1, 2); // A Draugr has a 10% chance, to drop 1-2 mushrooms.
            flyagaricMushrom.DropsFrom.Add("Draugr_Elite", 0.2f, 1, 2); // A Draugr_Elite has a 20% chance, to drop 1-2 mushrooms.
            flyagaricMushrom.DropsFrom.Add("Draugr_Ranged", 0.1f, 1, 2); // A Draugr_Ranged has a 10% chance, to drop 1-2 mushrooms.
            flyagaricMushrom.DropsFrom.Add("Dverger", 0.4f, 1, 2); // A Dverger has a 40% chance, to drop 1-2 mushrooms.
            flyagaricMushrom.DropsFrom.Add("DvergerMage", 0.4f, 1, 2); // A DvergerMage has a 40% chance, to drop 1-2 mushrooms.
            flyagaricMushrom.DropsFrom.Add("DvergerMageFire", 0.4f, 1, 2); // A DvergerMageFire has a 40% chance, to drop 1-2 mushrooms.
            flyagaricMushrom.DropsFrom.Add("DvergerMageIce", 0.4f, 1, 2); // A DvergerMageIce has a 40% chance, to drop 1-2 mushrooms.
            flyagaricMushrom.DropsFrom.Add("DvergerMageSupport", 0.4f, 1, 2); // A DvergerMageSupport has a 40% chance, to drop 1-2 mushrooms.

            CustomSE beserkSE = new("Berserk");
            beserkSE.Name.English("Berserk");
            beserkSE.Type = EffectType.Consume;
            beserkSE.Icon = "berserkerse.png";
            beserkSE.Effect.m_cooldown = 0f;
            beserkSE.Effect.m_activationAnimation = "gpower";
            beserkSE.Effect.m_ttl = Duration.Value;
            beserkSE.Effect.m_startMessageType = MessageHud.MessageType.Center;
            beserkSE.Effect.m_startMessage = "Damage increased at the cost of health";
            beserkSE.Effect.m_stopMessageType = MessageHud.MessageType.Center;
            beserkSE.Effect.m_stopMessage = "";
            beserkSE.Effect.m_tooltip =
                "<color=red>Increase damage x2, but at the cost of health loss over time</color>";
            // Add SE_Berserk to the variables available.
            beserkSE.Effect.m_damagePerHit = 2f;
            beserkSE.Effect.m_damageInterval = 1f;
            beserkSE.Effect.m_baseTTL = 1f;
            beserkSE.Effect.m_TTLPerDamage = 1f;
            beserkSE.Effect.m_TTLPerDamagePlayer = 5f;
            beserkSE.Effect.m_TTLPower = 0.5f;
            beserkSE.AddSEToPrefab(beserkSE, "FlyAgaricMushroom");


            PrefabManager.RegisterPrefab("funguyfury", "Pickable_FlyAgaricMushroom");
            PrefabManager.RegisterPrefab("funguyfury", "sfx_berserk_female");
            PrefabManager.RegisterPrefab("funguyfury", "sfx_berserk_male");

            Assembly assembly = Assembly.GetExecutingAssembly();
            _harmony.PatchAll(assembly);
            SetupWatcher();
        }

        private void OnDestroy()
        {
            Config.Save();
        }

        private void SetupWatcher()
        {
            FileSystemWatcher watcher = new(Paths.ConfigPath, ConfigFileName);
            watcher.Changed += ReadConfigValues;
            watcher.Created += ReadConfigValues;
            watcher.Renamed += ReadConfigValues;
            watcher.IncludeSubdirectories = true;
            watcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
            watcher.EnableRaisingEvents = true;
        }

        private void ReadConfigValues(object sender, FileSystemEventArgs e)
        {
            if (!File.Exists(ConfigFileFullPath)) return;
            try
            {
                FunGuyFuryLogger.LogDebug("ReadConfigValues called");
                Config.Reload();
            }
            catch
            {
                FunGuyFuryLogger.LogError($"There was an issue loading your {ConfigFileName}");
                FunGuyFuryLogger.LogError("Please check your config entries for spelling and format!");
            }
        }


        #region ConfigOptions

        private static ConfigEntry<Toggle> _serverConfigLocked = null!;
        internal static ConfigEntry<float> DamageBoost = null!;
        internal static ConfigEntry<float> Cooldown = null!;
        internal static ConfigEntry<float> Duration = null!;
        internal static ConfigEntry<string> StartMessage = null!;
        internal static ConfigEntry<string> StopMessage = null!;
        internal static ConfigEntry<string> CooldownMessage = null!;
        internal static ConfigEntry<string> EffectTooltip = null!;
        internal static ConfigEntry<float> DamagePerHit = null!;
        internal static ConfigEntry<float> DamageInterval = null!;

        internal static DateTime LastShroomTime = DateTime.MinValue;

        /*private static ConfigEntry<Toggle> _recipeIsActiveConfig = null!;*/

        private ConfigEntry<T> config<T>(string group, string name, T value, ConfigDescription description,
            bool synchronizedSetting = true)
        {
            ConfigDescription extendedDescription =
                new(
                    description.Description +
                    (synchronizedSetting ? " [Synced with Server]" : " [Not Synced with Server]"),
                    description.AcceptableValues, description.Tags);
            ConfigEntry<T> configEntry = Config.Bind(group, name, value, extendedDescription);
            //var configEntry = Config.Bind(group, name, value, description);

            SyncedConfigEntry<T> syncedConfigEntry = ConfigSync.AddConfigEntry(configEntry);
            syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

            return configEntry;
        }

        private ConfigEntry<T> config<T>(string group, string name, T value, string description,
            bool synchronizedSetting = true)
        {
            return config(group, name, value, new ConfigDescription(description), synchronizedSetting);
        }

        ConfigEntry<T> textEntryConfig<T>(string group, string name, T value, string desc,
            bool synchronizedSetting = true)
        {
            ConfigurationManagerAttributes attributes = new()
            {
                CustomDrawer = Functions.TextAreaDrawer
            };
            return config(group, name, value, new ConfigDescription(desc, null, attributes), synchronizedSetting);
        }

        private class ConfigurationManagerAttributes
        {
            [UsedImplicitly] public int? Order = null!;
            [UsedImplicitly] public bool? Browsable = null!;
            [UsedImplicitly] public string? Category = null!;
            [UsedImplicitly] public Action<ConfigEntryBase>? CustomDrawer = null!;
        }

        #endregion
    }
}