using HarmonyLib;

using UnityEngine;

using flanne;
using flanne.PowerupSystems;

namespace _20MinutesTillDawn.PowerScaling.Fixes
{
public static class FixReloadRateUpOnKill
{
	// Share the stacks between multiples of the upgrades
	static int stacks = 0;

	[HarmonyPatch(typeof(ReloadRateUpOnKill), "OnReload")]
	[HarmonyPrefix]
	static bool OnReload(
		float ___bonusPerStack,
		StatsHolder ___stats,
		ref int ____stacks)
	{
		if(stacks > 0)
		{
			___stats[StatType.ReloadRate].AddMultiplierBonus(
				Mathf.Pow(1f / (1f + ___bonusPerStack), stacks) - 1f);

			stacks = 0;
		}

		____stacks = 0;

		return false;
	}

	[HarmonyPatch(typeof(ReloadRateUpOnKill), "OnDeath")]
	[HarmonyPrefix]
	static bool OnDeath(
		float ___bonusPerStack,
		StatsHolder ___stats,
		ref int ____stacks,
		object sender)
	{
		if((sender as Health).gameObject.tag == "Enemy" && stacks < 2000)
		{
			___stats[StatType.ReloadRate].AddMultiplierBonus(___bonusPerStack);
			++____stacks;
			++stacks;
		}

		return false;
	}
}
}
