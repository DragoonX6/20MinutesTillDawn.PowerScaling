using HarmonyLib;

using flanne;

using UnityEngine;

namespace _20MinutesTillDawn.PowerScaling.Fixes
{
public static class FixDmgAndMSOnNotHurt
{
	[HarmonyPatch(typeof(DmgAndMSOnNotHurt), "OnHurt")]
	[HarmonyPrefix]
	static bool OnHurt(
		float ___damageBoostPerTick,
		float ___movespeedBoostPerTick,
		SpriteTrail ___spriteTrail,
		StatsHolder ___stats,
		ref int ____ticks,
		ref float ____timer)
	{
		___stats[StatType.BulletDamage].AddMultiplierBonus(
			Mathf.Pow(1f / (1f + ___damageBoostPerTick), ____ticks) - 1f);

		___stats[StatType.MoveSpeed].AddMultiplierBonus(
			Mathf.Pow(1f / (1f + ___movespeedBoostPerTick), ____ticks) - 1f);

		___spriteTrail.SetEnabled(enabled: false);

		____ticks = 0;
		____timer = 0;

		return false;
	}
}
}
