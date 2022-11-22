using HarmonyLib;
using Mono.Cecil.Cil;
using MonoMod.Cil;

using flanne;

namespace _20MinutesTillDawn.PowerScaling;

public static class StatModOverride
{
	[HarmonyPatch(typeof(StatMod), "Modify")]
	[HarmonyILManipulator]
	static void ManipulateModify(ILContext il)
	{
		// return  baseValue * _multiplierBonus + _flatBonus;

		ILCursor c = new(il);

		++c.Index;

		c.Remove();

		c.Index += 2;

		c.RemoveRange(4);
	}

	[HarmonyPatch(typeof(StatMod), "ModifyInverse")]
	[HarmonyILManipulator]
	static void ManipulateModifyInverse(ILContext il)
	{
		// 1 / (x * x * x) == 1 / x / x / x
		// return baseValue * (1f / _multiplierBonus) + _flatBonus;

		ILCursor c = new(il);

		c.GotoNext(MoveType.Before, x => x.MatchAdd());

		c.Remove();
		c.Emit(OpCodes.Div);

		c.RemoveRange(2);
		++c.Index;
		c.Remove();
	}

	[HarmonyPatch(typeof(StatMod), nameof(StatMod.AddMultiplierBonus))]
	[HarmonyILManipulator]
	static void ManipulateAddMultiplierBonus(ILContext il)
	{
		// _multiplierBonus *= 1f + value;

		ILCursor c = new(il);

		c.GotoNext(MoveType.After, x => x.Match(OpCodes.Ldfld));

		++c.Index;

		c.Emit(OpCodes.Ldc_R4, 1f);

		++c.Index;

		c.Emit(OpCodes.Mul);
	}

	[HarmonyPatch(typeof(StatMod), nameof(StatMod.AddMultiplierReduction))]
	[HarmonyILManipulator]
	static void ManipulateAddMultiplierReduction(ILContext il)
	{
		// _multiplierReduction -> _multiplierBonus

		ILCursor c = new(il);

		c.GotoNext(MoveType.Before, x => x.Match(OpCodes.Ldfld));
		c.Remove();
		c.Emit(
			OpCodes.Ldfld,
			AccessTools.DeclaredField(typeof(StatMod), "_multiplierBonus"));

		c.GotoNext(MoveType.Before, x => x.Match(OpCodes.Stfld));
		c.Remove();
		c.Emit(
			OpCodes.Stfld,
			AccessTools.DeclaredField(typeof(StatMod), "_multiplierBonus"));
	}
}

public static class StatModCtorOverride
{
	[HarmonyPatch(typeof(StatMod), MethodType.Constructor)]
	[HarmonyILManipulator]
	static void ManipulateConstructor(ILContext il)
	{
		// StatModCtorOverride::Constructor(ref _multiplierBonus);

		ILCursor c = new(il);

		c.Emit(OpCodes.Ldarg_0);

		c.Emit(
			OpCodes.Ldflda,
			AccessTools.DeclaredField(typeof(StatMod), "_multiplierBonus"));

		c.Emit(
			OpCodes.Call,
			AccessTools.Method(typeof(StatModCtorOverride), "Constructor"));
	}

	static void Constructor(ref float _multiplierBonus)
	{
		if(SelectedMap.MapData.endless)
			_multiplierBonus = 1f;
	}
}
