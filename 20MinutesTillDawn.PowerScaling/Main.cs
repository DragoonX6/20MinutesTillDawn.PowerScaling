using System.Linq;

using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;

using HarmonyLib;

using UnityEngine.SceneManagement;

using flanne;

using _20MinutesTillDawn.PowerScaling.Fixes;
using _20MinutesTillDawn.PowerScaling.Interop;

namespace _20MinutesTillDawn.PowerScaling;

[BepInPlugin(
	"20MinutesTillDawn.PowerScaling",
	"20 Minutes Till Dawn Power Scaling Mod",
	"0.15.0")]
[BepInProcess("MinutesTillDawn.exe")]
[BepInDependency("BetterUI", BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency("20MinutesTillDawn.Fixes", "3.0.0.0")]
public class PowerScaling: BaseUnityPlugin
{
	private Harmony instance = new("PowerScaling");
	private Harmony always = new("PowerScaling.Always");

	public static ManualLogSource Log = null;

	private bool patched = false;

	public void Awake()
	{
		Log = Logger;

		// This one needs to be patched at all times.
		always.PatchAll(typeof(StatModCtorOverride));

		SceneManager.sceneLoaded += OnSceneLoaded;

		Logger.LogInfo("Power scaling mod initialized.");
	}

	public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
	{
		Logger.LogDebug(scene.name);

		if(scene.name != "Battle")
		{
			Unpatch();
			return;
		}

		if(SelectedMap.MapData != null && !SelectedMap.MapData.endless)
		{
			Unpatch();
			return;
		}

		Patch();

		ModifyEndlessSpawnSessions.Reset();
	}

	private void Unpatch()
	{
		if(!patched)
			return;

		Logger.LogDebug("Unpatching...");

		instance.UnpatchSelf();

		Logger.LogDebug("Unpatched.");

		patched = false;
	}

	private void Patch()
	{
		if(patched)
			return;

		Logger.LogDebug("Patching...");

		instance.PatchAll(typeof(StatModOverride));
		instance.PatchAll(typeof(FixDmgAndMSOnNotHurt));
		instance.PatchAll(typeof(FixReloadRateUpOnKill));
		instance.PatchAll(typeof(FixupAddMultiplierBonus));
		instance.PatchAll(typeof(ModifyPowerupTree));
		instance.PatchAll(typeof(CapStats));
		instance.PatchAll(typeof(AdjustDifficulty));
		instance.PatchAll(typeof(ModifyEndlessSpawnSessions));

		if(Chainloader.PluginInfos.Where(kv => kv.Key == "BetterUI").Any())
			instance.PatchAll(typeof(BetterUIInterop));

		Logger.LogDebug("Patched.");

		patched = true;
	}
}
