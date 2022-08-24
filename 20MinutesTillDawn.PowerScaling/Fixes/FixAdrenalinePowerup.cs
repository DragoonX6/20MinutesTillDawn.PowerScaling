using System.Collections.Generic;
using System.Reflection.Emit;

using HarmonyLib;

using flanne.PowerupSystem;

namespace _20MinutesTillDawn.PowerScaling.Fixes
{
public static class FixAdrenalinePowerup
{
	[HarmonyPatch(
		typeof(AdrenalinePowerup),
		"StartAdrenaline",
		MethodType.Enumerator)]
	[HarmonyTranspiler]
	static IEnumerable<CodeInstruction> TranspileStartAdrenaline(
		IEnumerable<CodeInstruction> instructions,
		ILGenerator il)
	{
		Label loopCondition = il.DefineLabel();

		CodeMatcher cm = new CodeMatcher(instructions, il)
			.End()
			.MatchBack(false,
				new CodeMatch(OpCodes.Br))
			.SetAndAdvance(OpCodes.Br_S, loopCondition)
			.MatchForward(false,
			new CodeMatch(OpCodes.Ldc_R4, -1f))
			.InsertAndAdvance(
				new CodeInstruction(OpCodes.Ldc_R4, 1f),
				new CodeInstruction(OpCodes.Ldc_R4, 1f))
			.RemoveInstructions(1)
			.Advance(2)
			.InsertAndAdvance(
				new CodeInstruction(OpCodes.Add),
				new CodeInstruction(OpCodes.Div),
				new CodeInstruction(OpCodes.Ldc_R4, 1f),
				new CodeInstruction(OpCodes.Sub))
			.RemoveInstruction()
			.Advance(5);

		cm.Labels.Add(loopCondition);

		return cm.InstructionEnumeration();
	}
}
}
