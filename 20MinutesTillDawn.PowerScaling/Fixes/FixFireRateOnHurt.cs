using HarmonyLib;

using flanne;
using flanne.PowerupSystem;

namespace _20MinutesTillDawn.PowerScaling.Fixes
{
public static class FixFireRateOnHurt
{
	[HarmonyPatch(typeof(FireRateOnHurt), "RemoveBoost")]
	[HarmonyPrefix]
	static bool RemoveBoost(float ___fireRateBoost, StatsHolder ___stats)
	{
		___stats[StatType.FireRate].AddMultiplierBonus(
			1f / (1f + ___fireRateBoost) - 1f);

		___stats[StatType.ReloadRate].AddMultiplierBonus(
			1f / (1f + ___fireRateBoost) - 1f);

		return false;
	}
}
}
