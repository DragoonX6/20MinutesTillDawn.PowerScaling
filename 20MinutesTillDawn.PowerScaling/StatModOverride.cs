using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

using HarmonyLib;

using flanne;

using UnityEngine;

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

		List<Tuple<int, CodeInstruction>> orderedInstructions =
			new List<Tuple<int, CodeInstruction>>();

		int pos = 0;
		foreach(CodeInstruction inst in cm.Instructions())
		{
			if(inst.operand != null)
				pos += inst.operand.GetType().Equals(typeof(Label))
					? 1
					: inst.IsLdloc() || inst.IsStloc()
						? 1
						: 4;

			pos += inst.opcode.Size;

			orderedInstructions.Add(new Tuple<int, CodeInstruction>(pos, inst));
		}

		foreach(Tuple<int, CodeInstruction> pair in orderedInstructions)
		{
			CodeInstruction inst = pair.Item2;
			pos = pair.Item1;

			if(inst.operand != null
			   && inst.operand.GetType().Equals(typeof(Label)))
			{
				Label? outLabel;
				if(inst.Branches(out outLabel))
				{
					var any = orderedInstructions
						.Where(c => c.Item2.labels.Contains((Label)outLabel));

					int target = any.Count() > 0 ? any.First().Item1 : -1;

					Debug.Log($"IL_{pos:X4} {inst.opcode} {inst.operand} IL_{target:X4}");
				}
				else
					Debug.Log($"IL_{pos:X4} {inst.opcode} {inst.operand} IL_????");
			}
			else
				Debug.Log($"IL_{pos:X4} {inst.opcode} {inst.operand}");
		}

		return cm.InstructionEnumeration();
	}

	/*[HarmonyPatch(typeof(StatMod), nameof(StatMod.AddMultiplierBonus))]
	[HarmonyPrefix]
	static bool AddMultiplierBonus(
		StatMod __instance,
		EventHandler ___ChangedEvent,
		ref float ____multiplierBonus,
		float value)
	{
		Debug.Log($"Bonus: value: {value}, multiplier: {____multiplierBonus}");

		____multiplierBonus *= 1f + value;
		___ChangedEvent?.Invoke(__instance, null);

		return false;
	}*/

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
