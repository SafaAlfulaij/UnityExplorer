<p align="center">
  <img align="center" src="img/icon.png">
</p>

<p align="center">
  🔍 An in-game UI for exploring, debugging and modifying Unity games.
</p>
<p align="center">
  ✔️ Supports most Unity versions from 5.2 to 2021+ (IL2CPP and Mono).
</p>
<p align="center">
  ✨ Powered by <a href="https://github.com/sinai-dev/UniverseLib">UniverseLib</a>
</p>

# Releases  [![](https://img.shields.io/github/downloads/sinai-dev/UnityExplorer/total.svg)](../../releases)

[![](https://img.shields.io/github/release/sinai-dev/UnityExplorer.svg?label=version)](../../releases/latest) [![](https://img.shields.io/github/workflow/status/sinai-dev/UnityExplorer/Build%20UnityExplorer)](https://github.com/sinai-dev/UnityExplorer/actions) [![](https://img.shields.io/github/downloads/sinai-dev/UnityExplorer/latest/total.svg)](../../releases/latest)

⚡ Thunderstore releases: [BepInEx Mono](https://thunderstore.io/package/sinai-dev/UnityExplorer) | [BepInEx IL2CPP](https://gtfo.thunderstore.io/package/sinai-dev/UnityExplorer_IL2CPP) | [MelonLoader IL2CPP](https://boneworks.thunderstore.io/package/sinai-dev/UnityExplorer_IL2CPP_ML)

## BepInEx

| Release | IL2CPP | Mono |
| ------- | ------ | ---- |
| BIE 6.X | ✅ [link](https://github.com/sinai-dev/UnityExplorer/releases/latest/download/UnityExplorer.BepInEx.Il2Cpp.zip) | ✅ [link](https://github.com/sinai-dev/UnityExplorer/releases/latest/download/UnityExplorer.BepInEx6.Mono.zip) |
| BIE 5.X | ✖️ n/a | ✅ [link](https://github.com/sinai-dev/UnityExplorer/releases/latest/download/UnityExplorer.BepInEx5.Mono.zip) |

1. Take the `UnityExplorer.BIE.[version].dll` and the `UniverseLib.[version].dll` files and put them in `BepInEx\plugins\`

<i>Note: BepInEx 6 is obtainable via [BepisBuilds](https://builds.bepis.io/projects/bepinex_be)</i>

## MelonLoader

| Release | IL2CPP | Mono |
| ------- | ------ | ---- |
| ML 0.4+ | ✅ [link](https://github.com/sinai-dev/UnityExplorer/releases/latest/download/UnityExplorer.MelonLoader.Il2Cpp.zip) | ✅ [link](https://github.com/sinai-dev/UnityExplorer/releases/latest/download/UnityExplorer.MelonLoader.Mono.zip) | 

1. Take the `UnityExplorer.ML.[version].dll` file and put them in the `Mods\` folder created by MelonLoader, and then put the `UniverseLib.[version].dll` file in the `UserLibs\` folder.

## Standalone

| IL2CPP | Mono |
| ------ | ---- |
| ✅ [link](https://github.com/sinai-dev/UnityExplorer/releases/latest/download/UnityExplorer.Standalone.Il2Cpp.zip) | ✅ [link](https://github.com/sinai-dev/UnityExplorer/releases/latest/download/UnityExplorer.Standalone.Mono.zip) | 

The standalone release can be used with any injector or loader of your choice, but it requires you to load the dependencies manually.

1. Ensure the required libs are loaded - UniverseLib, HarmonyX and MonoMod
2. For IL2CPP, load Il2CppAssemblyUnhollower and start an [Il2CppAssemblyUnhollower runtime](https://github.com/knah/Il2CppAssemblyUnhollower#required-external-setup)
2. Load the UnityExplorer DLL
3. Create an instance of Unity Explorer with `UnityExplorer.ExplorerStandalone.CreateInstance();`
4. Optionally subscribe to the `ExplorerStandalone.OnLog` event to handle logging if you wish

## Unity Editor

1. Download the [`UnityExplorer.Editor`](https://github.com/sinai-dev/UnityExplorer/releases/latest/download/UnityExplorer.Editor.zip) release.
2. Install the package, either by using the Package Manager and importing the `package.json` file, or by manually dragging the folder into your `Assets` folder.
3. Drag the `Runtime/UnityExplorer` prefab into your scene, or create a GameObject and add the `Explorer Editor Behaviour` script to it.

# Common issues and solutions

Although UnityExplorer should work out of the box for most Unity games, in some cases you may need to tweak the settings for it to work properly.

To adjust the settings, open the config file:
* BepInEx: `BepInEx\config\com.sinai.unityexplorer.cfg`
* MelonLoader: `UserData\MelonPreferences.cfg`
* Standalone: `UnityExplorer\config.cfg`

Try adjusting the following settings and see if it fixes your issues:
* `Startup_Delay_Time` - increase to 5-10 seconds (or more as needed), can fix issues with UnityExplorer being destroyed or corrupted during startup.
* `Disable_EventSystem_Override` - if input is not working properly, try setting this to `true`.

If these fixes do not work, please create an issue in this repo and I'll do my best to look into it.

# Features

<p align="center">
  <a href="https://raw.githubusercontent.com/sinai-dev/UnityExplorer/master/img/preview.png">
    <img src="img/preview.png" />
  </a>
</p>

### Object Explorer

* Use the <b>Scene Explorer</b> tab to traverse the active scenes, as well as the DontDestroyOnLoad and HideAndDontSave objects.
  * The "HideAndDontSave" scene contains objects with that flag, as well as Assets and Resources which are not in any scene but behave the same way.
  * You can use the Scene Loader to easily load any of the scenes in the build (may not work for Unity 5.X games)
* Use the <b>Object Search</b> tab to search for Unity objects (including GameObjects, Components, etc), C# Singletons or Static Classes.
  * Use the UnityObject search to look for any objects which derive from `UnityEngine.Object`, with optional filters
  * The singleton search will look for any classes with a typical "Instance" field, and check it for a current value. This may cause unexpected behaviour in some IL2CPP games as we cannot distinguish between true properties and field-properties, so some property accessors will be invoked.

### Inspector

The inspector is used to see detailed information on objects of any type and manipulate their values, as well as to inspect C# Classes with static reflection.

* The <b>GameObject Inspector</b> (tab prefix `[G]`) is used to inspect a `GameObject`, and to see and manipulate its Transform and Components.
  * You can edit any of the input fields in the inspector (excluding readonly fields) and press <b>Enter</b> to apply your changes. You can also do this to the GameObject path as a way to change the GameObject's parent. Press the <b>Escape</b> key to cancel your edits.
  * <i>note: When inspecting a GameObject with a Canvas, the transform controls may be overridden by the RectTransform anchors.</i>
* The <b>Reflection Inspectors</b> (tab prefix `[R]` and `[S]`) are used for everything else
  * Automatic updating is not enabled by default, and you must press Apply for any changes you make to take effect.
  * Press the `▼` button to expand certain values such as strings, enums, lists, dictionaries, some structs, etc
  * Use the filters at the top to quickly find the members you are looking for
  * For `Texture2D` objects, there is a `View Texture` button at the top of the inspector which lets you view it and save it as a PNG file. Currently there are no other similar helpers yet, but I may add more at some point for Mesh, Sprite, Material, etc

### C# Console

* The C# Console uses the `Mono.CSharp.Evaluator` to define temporary classes or run immediate REPL code.
* You can execute a script automatically on startup by naming it `startup.cs` and placing it in the `UnityExplorer\Scripts\` folder (this folder will be created where you placed the DLL file).
* See the "Help" dropdown in the C# console menu for more detailed information.

### Hook Manager

* The Hooks panel allows you to hook methods at the click of a button for debugging purposes.
  * Simply enter any class (generic types not yet supported) and hook the methods you want from the menu. 
  * You can edit the source code of the generated hook with the "Edit Hook Source" button. Accepted method names are `Prefix` (which can return `bool` or `void`), `Postfix`, `Finalizer` (which can return `Exception` or `void`), and `Transpiler` (which must return `IEnumerable<HarmonyLib.CodeInstruction>`). You can define multiple patches if you wish.

### Mouse-Inspect

* The "Mouse Inspect" dropdown in the "Inspector" panel allows you to inspect objects under the mouse.
  * <b>World</b>: uses Physics.Raycast to look for Colliders
  * <b>UI</b>: uses GraphicRaycasters to find UI objects

### Clipboard

* The "Clipboard" panel allows you to see your current paste value, or clear it (resets it to `null`)
  * Can copy the value from any member in a Reflection Inspector, Enumerable or Dictionary, and from the target of any Inspector tab
  * Can paste values onto any member in a Reflection Inspector
  * Non-parsable arguments in Method/Property Evaluators allow pasting values
  * The C# Console has helper methods `Copy(obj)` and `Paste()` for accessing the Clipboard

### Settings

* You can change the settings via the "Options" tab of the menu, or directly from the config file.
  * BepInEx: `BepInEx\config\com.sinai.unityexplorer.cfg`
  * MelonLoader: `UserData\MelonPreferences.cfg`
  * Standalone `{DLL_location}\UnityExplorer\config.cfg`

# Building

0. Clone the repository and run `git submodule update --init --recursive` to get the submodules.
1. Open the `src\UnityExplorer.sln` project.
2. Build `mcs` (Release/AnyCPU, you may need to run `nuget restore mcs.sln`)
3. Build the UnityExplorer release(s) you want to use, either by selecting the config as the Active Config, or batch-building.

If you fork the repository on GitHub you can build using the [dotnet workflow](https://github.com/sinai-dev/UnityExplorer/blob/master/.github/workflows/dotnet.yml):

0. Click on the Actions tab and enable workflows in your repository
1. Click on the "Build UnityExplorer" workflow, then click "Run Workflow" and run it manually, or make a new commit to trigger the workflow.
2. Take the artifact from the completed run.

# Acknowledgments

* [ManlyMarco](https://github.com/ManlyMarco) for [Runtime Unity Editor](https://github.com/ManlyMarco/RuntimeUnityEditor) \[[license](THIRDPARTY_LICENSES.md#runtimeunityeditor-license)\], the ScriptEvaluator from RUE's REPL console was used as the base for UnityExplorer's C# console.
* [Geoffrey Horsington](https://github.com/ghorsington) for [mcs-unity](https://github.com/sinai-dev/mcs-unity) \[no license\], used as the `Mono.CSharp` reference for the C# Console.

### Disclaimer

UnityExplorer is in no way associated with Unity Technologies. "Unity", Unity logos, and other Unity trademarks are trademarks or registered trademarks of Unity Technologies or its affiliates in the U.S. and elsewhere.
