using System.Collections.Generic;
using System.Reflection.Emit;

using HarmonyLib;

using flanne;

namespace _20MinutesTillDawn.PowerScaling.Interop
{
public static class BetterUIInterop
{
	const string fqtn = "BetterUI.StatsPanel, BetterUI, "
		+ "Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";

	[HarmonyPatch(fqtn, "UpdatePanel")]
	[HarmonyAfter("BetterUI")]
	[HarmonyTranspiler]
	static IEnumerable<CodeInstruction> PatchReloadSpeed(
		IEnumerable<CodeInstruction> instructions)
	{
		return new CodeMatcher(instructions)
			.MatchForward(false, new CodeMatch(OpCodes.Div))
			.MatchBack(
				false,
				new CodeMatch(
					OpCodes.Callvirt,
					AccessTools.DeclaredMethod(typeof(StatMod), "Modify")))
			.SetOperandAndAdvance(
				AccessTools.DeclaredMethod(typeof(StatMod), "ModifyInverse"))
			.InstructionEnumeration();
	}
}
}
