using HarmonyLib;
using Mono.Cecil.Cil;
using MonoMod.Cil;

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

	// Caps dodge to 70%
	[HarmonyPatch(typeof(DodgeRoller), nameof(DodgeRoller.Roll))]
	[HarmonyILManipulator]
	static void ManipulateRoll(ILContext il)
	{
		ILCursor c = new(il);

		c.GotoNext(
			MoveType.Before,
			x => x.Match(OpCodes.Ldloc_0),
			x => x.MatchLdcR4(0.0f));

		c.RemoveRange(16);
	}
}
