using HarmonyLib;

using flanne;

namespace _20MinutesTillDawn.PowerScaling.Fixes
{
public static class FixBuffDuringHolyShield
{
	[HarmonyPatch(typeof(BuffDuringHolyShield), "Deactivate")]
	[HarmonyPrefix]
	static bool Deactivate(
		StatsHolder ___stats,
		float ___reloadRateMulti,
		float ___movespeedMulti)
	{
		___stats[StatType.ReloadRate].AddMultiplierBonus(
			1f / (1f + ___reloadRateMulti) - 1f);

		___stats[StatType.MoveSpeed].AddMultiplierBonus(
			1f / (1f + ___movespeedMulti) - 1f);

		return false;
	}
}
}
