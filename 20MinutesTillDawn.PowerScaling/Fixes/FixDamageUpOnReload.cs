using HarmonyLib;

using UnityEngine;

using flanne;
using flanne.PowerupSystems;

namespace _20MinutesTillDawn.PowerScaling.Fixes
{
public static class FixDamageUpOnReload
{
	[HarmonyPatch(typeof(DamageUpOnReload), "Update")]
	[HarmonyPrefix]
	static bool Update(
		float ___damageBonus,
		StatsHolder ___stats,
		ref float ____timer)
	{
		if(____timer > 0f)
		{
			____timer -= Time.deltaTime;

			if(____timer <= 0f)
			{
				___stats[StatType.BulletDamage].AddMultiplierBonus(
					1f / (1f + ___damageBonus) - 1f);
			}
		}

		return false;
	}
}
}
