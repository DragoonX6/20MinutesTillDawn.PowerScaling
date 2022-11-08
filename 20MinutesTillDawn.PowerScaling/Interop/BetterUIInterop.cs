using System.Collections.Generic;
using System.Reflection.Emit;

using HarmonyLib;

using flanne;
using flanne.Core;

namespace _20MinutesTillDawn.PowerScaling.Interop
{
public static class BetterUIInterop
{
	const string fqtn = "BetterUI.InfoDisplay, BetterUI, "
		+ "Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";

	[HarmonyPatch(fqtn, "PauseStateEnterPostPatch")]
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

	[HarmonyPatch(typeof(GameController), "Start")]
	[HarmonyPostfix]
	static void StartPosftix(GameController __instance)
	{
		__instance.powerupListUI.transform.SetSiblingIndex(0);
	}
}
}
