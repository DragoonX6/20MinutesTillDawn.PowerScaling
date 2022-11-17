using HarmonyLib;

using flanne;

using UnityEngine;

namespace _20MinutesTillDawn.PowerScaling;

public static class CapStats
{
	// Should probably cap electro mastery

	[HarmonyPatch(typeof(Gun), nameof(Gun.GetProjectileRecipe))]
	[HarmonyPostfix]
	static void CapProjectileSpeed(ref ProjectileRecipe __result)
	{
		__result.projectileSpeed = Mathf.Min(__result.projectileSpeed, 100f);
	}
}
