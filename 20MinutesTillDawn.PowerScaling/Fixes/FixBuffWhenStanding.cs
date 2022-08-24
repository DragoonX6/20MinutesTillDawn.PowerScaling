using HarmonyLib;

using UnityEngine;

using flanne;
using flanne.PowerupSystem;

namespace _20MinutesTillDawn.PowerScaling.Fixes
{
public static class FixBuffWhenStanding
{
	[HarmonyPatch(typeof(BuffWhenStanding), "ResetBuff")]
	[HarmonyPrefix]
	static bool ResetBuff(
		StatsHolder ___stats,
		float ___damageBoostPerTick,
		ref int ____ticks,
		ref float ____timer)
	{
		___stats[StatType.BulletDamage].AddMultiplierBonus(
			Mathf.Pow(1f + (1f / ___damageBoostPerTick), ____ticks) - 1f);

		____ticks = 0;
		____timer = 0;

		return false;
	}
}
}
