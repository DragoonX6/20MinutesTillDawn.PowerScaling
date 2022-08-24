using UnityEngine.SceneManagement;

using BepInEx;
using HarmonyLib;
using HarmonyLib.Tools;

using _20MinutesTillDawn.PowerScaling.Fixes;

namespace _20MinutesTillDawn.PowerScaling
{
[BepInPlugin(
	"20MinutesTillDawn.PowerScaling",
	"20 Minutes Till Dawn Power Scaling Mod",
	"0.15.0")]
[BepInProcess("MinutesTillDawn.exe")]
public class PowerScaling: BaseUnityPlugin
{
	private Harmony instance = new Harmony("PowerScaling");

	public void Awake()
	{
		HarmonyFileLog.Enabled = true;

		instance.PatchAll(typeof(StatModOverride));
		instance.PatchAll(typeof(FixAdrenalinePowerup));
		instance.PatchAll(typeof(FixBuffDuringHolyShield));
		instance.PatchAll(typeof(FixBuffOnXP));
		instance.PatchAll(typeof(FixBuffPlayerStats));
		instance.PatchAll(typeof(FixBuffWhenStanding));
		instance.PatchAll(typeof(FixDamageUpOnReload));
		instance.PatchAll(typeof(FixDmgAndMSOnNotHurt));
		instance.PatchAll(typeof(FixExtraDamageEveryNthThunder));
		instance.PatchAll(typeof(FixFireRateOnHurt));
		instance.PatchAll(typeof(FixReloadRateUpOnKill));
		instance.PatchAll(typeof(ModifyPowerupTree));
		instance.PatchAll(typeof(ModifyEndlessSpawnSessions));
		instance.PatchAll(typeof(CapStats));

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
