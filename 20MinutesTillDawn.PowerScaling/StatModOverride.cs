using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

using HarmonyLib;

using flanne;

namespace _20MinutesTillDawn.PowerScaling
{
public static class StatModOverride
{
	[HarmonyPatch(typeof(StatMod), "Modify")]
	[HarmonyPrefix]
	static bool PrefixModify(
		ref float __result,
		int ____flatBonus,
		float ____multiplierBonus,
		float baseValue)
	{
		__result = baseValue * ____multiplierBonus + ____flatBonus;

		return false;
	}

	[HarmonyPatch(typeof(StatMod), "ModifyInverse")]
	[HarmonyPrefix]
	static bool PrefixModifyInverse(
		ref float __result,
		int ____flatBonus,
		float ____multiplierBonus,
		float baseValue)
	{
		// 1 / (x * x * x) == 1 / x / x / x
		__result = baseValue * (1f / ____multiplierBonus) + ____flatBonus;

		return false;
	}

	[HarmonyPatch(typeof(StatMod), nameof(StatMod.AddMultiplierBonus))]
	[HarmonyTranspiler]
	static IEnumerable<CodeInstruction> TranspileAddMultiplierBonus(
		IEnumerable<CodeInstruction> instructions,
		ILGenerator il)
	{
		Label label = il.DefineLabel();

		CodeMatcher cm = new CodeMatcher(instructions)
			.MatchForward(true, new CodeMatch(OpCodes.Ldfld))
			.Advance(1)
			.InsertAndAdvance(new CodeInstruction(OpCodes.Ldc_R4, 1f))
			.Advance(2)
			.InsertAndAdvance(new CodeInstruction(OpCodes.Mul))
			.MatchForward(false, new CodeMatch(OpCodes.Brtrue))
			.SetInstructionAndAdvance(
				new CodeInstruction(OpCodes.Brtrue_S, label))
			.MatchForward(false,
				new CodeMatch(OpCodes.Ldarg_0),
				new CodeMatch(OpCodes.Ldnull))
			.SetInstructionAndAdvance(
				new CodeInstruction(OpCodes.Ldarg_0) { labels = { label } });

		return cm.InstructionEnumeration();
	}

	[HarmonyPatch(typeof(StatMod), nameof(StatMod.AddMultiplierReduction))]
	[HarmonyTranspiler]
	static IEnumerable<CodeInstruction> TranspileAddMultiplierReduction(
		IEnumerable<CodeInstruction> instructions)
	{
		return new CodeMatcher(instructions)
			.MatchForward(false, new CodeMatch(OpCodes.Ldfld))
			.SetAndAdvance(
				OpCodes.Ldfld,
				typeof(StatMod).GetField(
					"_multiplierBonus",
					BindingFlags.NonPublic | BindingFlags.Instance))
			.MatchForward(false, new CodeMatch(OpCodes.Stfld))
			.SetAndAdvance(
				OpCodes.Stfld,
				typeof(StatMod).GetField(
					"_multiplierBonus",
					BindingFlags.NonPublic | BindingFlags.Instance))
			.InstructionEnumeration();
	}

	[HarmonyPatch(typeof(StatMod), MethodType.Constructor)]
	[HarmonyTranspiler]
	static IEnumerable<CodeInstruction> TranspileConstructor(
		IEnumerable<CodeInstruction> instructions)
	{
		return new CodeMatcher(instructions)
			.Start()
			.InsertAndAdvance(
				new CodeInstruction(OpCodes.Ldarg_0),
				new CodeInstruction(OpCodes.Ldc_R4, 1f),
				new CodeInstruction(
					OpCodes.Stfld,
					typeof(StatMod).GetField(
						"_multiplierBonus",
						BindingFlags.NonPublic | BindingFlags.Instance)))
			.InstructionEnumeration();
	}
}
}
