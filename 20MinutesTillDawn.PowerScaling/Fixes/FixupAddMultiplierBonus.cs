using System.Reflection;

using HarmonyLib;
using Mono.Cecil.Cil;
using MonoMod.Cil;

using flanne.PerkSystem.Actions;
using flanne.Player.Buffs;

namespace _20MinutesTillDawn.PowerScaling.Fixes
{
public static class FixupAddMultiplierBonus
{
	[HarmonyPatch(typeof(TemporaryStatBuff), "OnUnattach")]
	[HarmonyPatch(typeof(ModStatAction), "DeActivate")]
	[HarmonyILManipulator]
	static void FixupCallsite(ILContext il, MethodBase method)
	{
		ILCursor c = new ILCursor(il);

		c.GotoNext(
			MoveType.Before,
			x => x.MatchLdcR4(-1f));

		c.Remove();

		c.Emit(OpCodes.Ldc_R4, 1f);
		c.Emit(OpCodes.Ldc_R4, 1f);

		c.Index += 2;

		c.Remove();

		c.Emit(OpCodes.Add);
		c.Emit(OpCodes.Div);
		c.Emit(OpCodes.Ldc_R4, 1f);
		c.Emit(OpCodes.Sub);
	}
}
}
