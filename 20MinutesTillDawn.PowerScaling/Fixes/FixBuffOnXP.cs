using System.Collections.Generic;
using System.Reflection.Emit;

using HarmonyLib;

using flanne.PowerupSystem;

namespace _20MinutesTillDawn.PowerScaling.Fixes
{
	public static class FixBuffOnXP
	{
		[HarmonyPatch(typeof(BuffOnXP), "StartBuffCR", MethodType.Enumerator)]
		[HarmonyTranspiler]
		static IEnumerable<CodeInstruction> TranspileStartBuffCR(
			IEnumerable<CodeInstruction> instructions)
		{
			return new CodeMatcher(instructions)
				.End()
				.MatchBack(false,
					new CodeMatch(OpCodes.Ldc_R4, -1f))
				.SetOperandAndAdvance(1f)
				.InsertAndAdvance(new CodeInstruction(OpCodes.Ldc_R4, 1f))
				.Advance(2)
				.SetOpcodeAndAdvance(OpCodes.Add)
				.InsertAndAdvance(
					new CodeInstruction(OpCodes.Div),
					new CodeInstruction(OpCodes.Ldc_R4, 1f),
					new CodeInstruction(OpCodes.Sub))
				.InstructionEnumeration();
		}
	}
}
