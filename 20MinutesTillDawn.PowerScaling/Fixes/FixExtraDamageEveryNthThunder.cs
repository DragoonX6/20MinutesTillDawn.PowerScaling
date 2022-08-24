using HarmonyLib;

using flanne;

namespace _20MinutesTillDawn.PowerScaling.Fixes
{
public static class FixExtraDamageEveryNthThunder
{
	[HarmonyPatch(typeof(ExtraDamageEveryNthThunder), "OnThunderHit")]
	[HarmonyPrefix]
	static bool OnThunderHit(
		float ___damageBonus,
		int ___activationThreshold,
		ref int ____counter,
		ThunderGenerator ___TG,
		Ammo ___ammo)
	{
		++____counter;

		if(____counter == ___activationThreshold - 1)
		{
			___TG.damageMod.AddMultiplierBonus(___damageBonus);
		}
		else if(____counter >= ___activationThreshold)
		{
			____counter = 0;
			___TG.damageMod.AddMultiplierBonus(1f / (1f + ___damageBonus) - 1f);
			___ammo.GainAmmo();
		}

		return false;
	}
}
}
