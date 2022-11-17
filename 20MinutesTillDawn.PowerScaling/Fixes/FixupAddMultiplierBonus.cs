using HarmonyLib;
using Mono.Cecil.Cil;
using MonoMod.Cil;

using flanne;
using flanne.PowerupSystem;
using flanne.PerkSystem.Actions;
using flanne.Player.Buffs;

namespace _20MinutesTillDawn.PowerScaling.Fixes;

public static class FixupAddMultiplierBonus
{
	[HarmonyPatch(typeof(TemporaryStatBuff), "OnUnattach")]
	[HarmonyPatch(typeof(ModStatAction), "DeActivate")]
	[HarmonyPatch(typeof(BuffDuringHolyShield), "Deactivate")]
	[HarmonyPatch(typeof(BuffOnXP), "StartBuffCR", MethodType.Enumerator)]
	[HarmonyILManipulator]
	static void FixupCallsite(ILContext il)
	{
		ILCursor c = new(il);

		while(c.TryGotoNext(
			MoveType.Before,
			x => x.MatchLdcR4(-1f)))
		{
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
