using System.Linq;

using UnityEngine.SceneManagement;

using BepInEx;
using BepInEx.Bootstrap;

using HarmonyLib;

using _20MinutesTillDawn.PowerScaling.Fixes;
using _20MinutesTillDawn.PowerScaling.Interop;

namespace _20MinutesTillDawn.PowerScaling
{
[BepInPlugin(
	"20MinutesTillDawn.PowerScaling",
	"20 Minutes Till Dawn Power Scaling Mod",
	"0.15.0")]
[BepInProcess("MinutesTillDawn.exe")]
[BepInDependency("BetterUI", BepInDependency.DependencyFlags.SoftDependency)]
public class PowerScaling: BaseUnityPlugin
{
	private Harmony instance = new Harmony("PowerScaling");

	public void Awake()
	{
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

		Logger.LogInfo("Patching done.");

		SceneManager.sceneLoaded += OnSceneLoaded;
		Logger.LogInfo("Power scaling mod initialized.");
	}

	public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
	{
		Logger.LogInfo(scene.name);
		ModifyEndlessSpawnSessions.Reset();
	}
}
}
