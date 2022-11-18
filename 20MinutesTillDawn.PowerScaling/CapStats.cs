using System;

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

	// Caps movement speed multiplier to 10
	[HarmonyPatch(typeof(PlayerController), "MovePlayer")]
	[HarmonyILManipulator]
	static void CapMovementSpeed(ILContext il)
	{
		ILCursor c = new(il);

		c.GotoNext(
			MoveType.After,
			x => x.MatchCallvirt(
				AccessTools.DeclaredMethod(typeof(StatMod), "Modify")));

		c.Emit(OpCodes.Ldc_R4, 0f);
		c.Emit(OpCodes.Ldc_R4, 10f);
		c.Emit(
			OpCodes.Call,
			AccessTools.DeclaredMethod(
				typeof(Mathf),
				"Clamp",
				new Type[]{ typeof(float), typeof(float), typeof(float) }));
	}
}
