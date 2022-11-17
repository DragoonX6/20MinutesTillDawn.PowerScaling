using HarmonyLib;

using flanne;
using flanne.Core;

namespace _20MinutesTillDawn.PowerScaling;

public static class AdjustDifficulty
{
	[HarmonyPatch(typeof(DifficultyInitializer), "Start")]
	[HarmonyPostfix]
	static void Start(GameController ___gameController)
	{
		___gameController.playerXP.xpMultiplier.AddMultiplierBonus(2f);
	}
}
