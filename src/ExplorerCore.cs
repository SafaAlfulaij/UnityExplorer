﻿using System.IO;
using System.Text.Json;

using UnityEngine;
using UnityExplorer.Config;
using UnityExplorer.ObjectExplorer;
using UnityExplorer.Runtime;
using UnityExplorer.UI;
using UnityExplorer.UI.Panels;
using UniverseLib.Input;

namespace UnityExplorer
{
    public static class ExplorerCore
    {
        public const string NAME = "UnityExplorer";
        public const string VERSION = "4.6.1";
        public const string AUTHOR = "Sinai";
        public const string GUID = "com.sinai.unityexplorer";

        public static IExplorerLoader Loader { get; private set; }

        public static HarmonyLib.Harmony Harmony { get; } = new HarmonyLib.Harmony(GUID);

        /// <summary>
        /// Initialize UnityExplorer with the provided Loader implementation.
        /// </summary>
        public static void Init(IExplorerLoader loader)
        {
            if (Loader != null)
            {
                LogWarning("UnityExplorer is already loaded!");
                return;
            }
            Loader = loader;

            Log($"{NAME} {VERSION} initializing...");

            if (!Directory.Exists(Loader.ExplorerFolder))
                Directory.CreateDirectory(Loader.ExplorerFolder);

            ConfigManager.Init(Loader.ConfigHandler);
            UERuntimeHelper.Init();
            ExplorerBehaviour.Setup();
            UnityCrashPrevention.Init();

            UniverseLib.Universe.Init(ConfigManager.Startup_Delay_Time.Value, LateInit, Log, new()
            {
                Disable_EventSystem_Override = ConfigManager.Disable_EventSystem_Override.Value,
                Force_Unlock_Mouse = ConfigManager.Force_Unlock_Mouse.Value,
                Unhollowed_Modules_Folder = loader.UnhollowedModulesFolder
            });
        }

        // Do a delayed setup so that objects aren't destroyed instantly.
        // This can happen for a multitude of reasons.
        // Default delay is 1 second which is usually enough.
        private static void LateInit()
        {
            Log($"Setting up late core features...");

            SceneHandler.Init();

            Log($"Creating UI...");

            UIManager.InitUI();

            Log($"{NAME} {VERSION} initialized for {UniverseLib.Universe.Context}.");

            //InspectorManager.Inspect(typeof(Tests.TestClass));
        }

        /// <summary>
        /// Should be called once per frame.
        /// </summary>
        public static void Update()
        {
            UIManager.Update();

            // check master toggle
            if (InputManager.GetKeyDown(ConfigManager.Master_Toggle.Value)) {
                Log($"Switching to {!UIManager.ShowMenu}...");
                Log($"main {Camera.main}");
                var output = JsonSerializer.Serialize(Camera.main, true);
                Log(output);
                Log($"current {Camera.current}");
                var output = JsonSerializer.Serialize(Camera.current, true);
                Log(output);
                Log($"canvas {UIManager.UICanvas}");
                var output = JsonSerializer.Serialize(UIManager.UICanvas, true);
                Log(output);
                Log(UIManager.UICanvas.renderMode);
                Log(UIManager.UICanvas.GetComponent<RectTransform>());
                UIManager.UICanvas.overrideSorting = true;
                UIManager.UICanvas.sortingOrder = 10000000;
                UIManager.ShowMenu = !UIManager.ShowMenu;
            }
        }

        #region LOGGING

        public static void Log(object message)
            => Log(message, LogType.Log);

        public static void LogWarning(object message)
            => Log(message, LogType.Warning);

        public static void LogError(object message)
            => Log(message, LogType.Error);

        public static void LogUnity(object message, LogType logType)
        {
            if (!ConfigManager.Log_Unity_Debug.Value)
                return;

            Log($"[Unity] {message}", logType);
        }

        private static void Log(object message, LogType logType)
        {
            string log = message?.ToString() ?? "";

            LogPanel.Log(log, logType);

            switch (logType)
            {
                case LogType.Assert:
                case LogType.Log:
                    Loader.OnLogMessage(log);
                    break;

                case LogType.Warning:
                    Loader.OnLogWarning(log);
                    break;

                case LogType.Error:
                case LogType.Exception:
                    Loader.OnLogError(log);
                    break;
            }
        }

        #endregion
    }
}
