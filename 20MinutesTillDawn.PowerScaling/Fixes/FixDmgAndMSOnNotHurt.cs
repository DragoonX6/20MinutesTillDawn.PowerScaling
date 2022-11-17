using System.Linq;
using System.Reflection;

using HarmonyLib;
using Mono.Cecil.Cil;
using MonoMod.Cil;

using flanne;

using UnityEngine;

namespace _20MinutesTillDawn.PowerScaling.Fixes;

public static class FixDmgAndMSOnNotHurt
{
	// Rewrite AddMultiplierBonus calls to the equation of:
	// Let v be the boost per tick
	// Let t be the amount of ticks
	// AMB((1 / (1 + v) ^ t) - 1)
	[HarmonyPatch(typeof(DmgAndMSOnNotHurt), "OnHurt")]
	[HarmonyILManipulator]
	static void FixOnHurt(ILContext il)
	{
		ILCursor c = new(il);

		while(c.TryGotoNext(
			MoveType.Before,
			x => x.Match(OpCodes.Ldc_I4_M1)))
		{
			c.Remove();

			c.Emit(OpCodes.Ldc_R4, 1f);
			c.Emit(OpCodes.Ldc_R4, 1f);

			// Save the load field instructions to survive variable renames.
			var saved1 = c.Instrs.Skip(c.Index).Take(2).ToArray();
			var saved2 = c.Instrs.Skip(c.Index + 4).Take(2).ToArray();

			c.RemoveRange(7);

			foreach(Instruction inst in saved2)
				c.Emit(inst.OpCode, inst.Operand);

			c.Emit(OpCodes.Add);
			c.Emit(OpCodes.Div);

			foreach(Instruction inst in saved1)
				c.Emit(inst.OpCode, inst.Operand);

			c.Emit(OpCodes.Conv_R4);

			c.Emit(
				OpCodes.Call,
				typeof(Mathf).GetMethod(
					nameof(Mathf.Pow),
					BindingFlags.Static | BindingFlags.Public));

			c.Emit(OpCodes.Ldc_R4, 1f);
			c.Emit(OpCodes.Sub);
		}
	}
}
