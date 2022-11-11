using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using HarmonyLib;

using flanne;
using flanne.PerkSystem;
using flanne.PerkSystem.Actions;

using UnityEngine;

using _20MinutesTillDawn.PowerScaling.Nerfs;

namespace _20MinutesTillDawn.PowerScaling
{
public static class ModifyPowerupTree
{
	static FieldInfo statChangesField = AccessTools
		.DeclaredField(typeof(Powerup), "statChanges");

	static FieldInfo nameStringID = AccessTools
		.DeclaredField(typeof(Powerup), "nameStrID");

	static FieldInfo effectsField = AccessTools
			.DeclaredField(typeof(Powerup), "effects");

	static FieldInfo actionField = AccessTools
			.DeclaredField(typeof(PerkEffect), "action");

	static string GetPowerupKey(Powerup powerup)
	{
		LocalizedString nameString =
			(LocalizedString)nameStringID.GetValue(powerup);

		return nameString.key;
	}

	[HarmonyPatch(
		typeof(PowerupGenerator),
		nameof(PowerupGenerator.InitPowerupPool))]
	[HarmonyPostfix]
	static void InitPowerupPoolPostfix(List<PowerupPoolItem> ___powerupPool)
	{
		List<string> noRepeatNames = new List<string>
		{
			//"ritual_name",
			"take_aim_name",
			"assassin_name",
			"focal_point_name",
			"light_weaponry_name",
			"kill_clip_name",
			"frostbite_name"
		};

		List<string> infRepeatNames = new List<string>
		{
			"sniper_name",
			"reaper_rounds",
			"aged_dragon",
			"rapid_fire_name",
			"sharpen_name",
			"vitality_name",
			"electro_mastery_name",
			"intense_burn_name",
			"armed_and_ready_name",
			"holy_might_name",
			"dual_wield_name"
		};

		___powerupPool.Do(p =>
		{
			noRepeatNames
				.Where(n => n.Equals(GetPowerupKey(p.powerup)))
				.Do(_ => p.numTimeRepeatable = 1);

			infRepeatNames
				.Where(n => n.Equals(GetPowerupKey(p.powerup)))
				.Do(_ => p.numTimeRepeatable = int.MaxValue);

			switch(GetPowerupKey(p.powerup))
			{
			case "sharpen_name":
			{
				StatChange[] statChanges =
					(StatChange[])statChangesField.GetValue(p.powerup);

				statChanges[0].value = 0.15f;
			} break;
			case "ritual_name":    NerfRitual(p.powerup);    break;
			case "frostbite_name": NerfFrostbite(p.powerup); break;
			case "shatter_name":   NerfShatter(p.powerup);   break;
			}
		});

		// power_shot_name
		// big_shot_name
		// reaper_rounds
		// splinter_name

		// light_bullets_name
		// rapid_fire_name
		// rubber_bullets_name
		// siege_name

		// energetic_friends
		// ghost_friend_name
		// in_sync_name
		// vengeful_ghost_name

		// anger_point_name
		// giant_name
		// regeneration_name
		// vitality_name

		// divine_blessing_name
		// divine_wrath_name
		// holy_shield_name
		// stalwart_shield_name

		// electro_bug_name
		// electro_mage_name
		// electro_mastery_name
		// energized_name

		// fire_starter_name
		// intense_burn_name
		// pyro_mage_name
		// soothing_warmth_name

		// blazing_speed_name
		// haste_name
		// in_the_wind_name
		// run_and_gun_name

		// fan_fire_name
		// double_shot_name
		// fusillade_name
		// split_fire_name

		// armed_and_ready_name
		// fresh_clip_name
		// kill_clip_name
		// quick_hands_name

		// dual_wield_name
		// heavy_weaponry_name
		// light_weaponry_name
		// sharpen_name

		// frostbite_name
		// frost_mage_name
		// ice_shards_name
		// shatter_name

		// magnetism_name
		// excitement_name
		// recharge_name
		// watch_and_learn_name

		// dragon_egg_name
		// aged_dragon
		// trained_dragon_name
		// dragon_bond_name

		// assassin_name
		// penetration_name
		// sniper_name
		// take_aim_name

		// glare_name
		// sight_magic_name
		// intense_glare_name
		// saccade_name

		// magic_lens_name
		// refraction_name
		// igniting_lens_name
		// focal_point_name

		// dark_arts_name
		// doom_name
		// wither_name
		// ritual_name

		// angelic_name
		// holy_arts_name
		// justice_name
		// holy_might_name

		// Synergies
		// death_rounds_name
		// frost_fire_name
		// gun_mastery_name
		// mini_clip_name
		// overload_name
		// stand_your_ground_name
		// summon_mastery
		// death_plague_name
		// generator_name
		// kunoichi_name
		// sword_and_shield_name
		// titan_name
	}

	[HarmonyPatch(typeof(PowerupGenerator), "SetCharacterPowerupPool")]
	[HarmonyPostfix]
	static void SetCharacterPowerupPoolPostfix(
		List<PowerupPoolItem> ___characterPool)
	{
		___characterPool.Do(p =>
		{
			switch(GetPowerupKey(p.powerup))
			{
			case "shana_perk_ascension_name":  p.numTimeRepeatable = 1; break;
			case "shana_perk_specialize_name": p.numTimeRepeatable = 5; break;
			}
		});

		// shana_perk_ascension_name
		// shana_perk_quick_learner_name
		// shana_perk_specialize_name
	}

	// Nerf ritual to only give 0.1% per 10 kills
	static void NerfRitual(Powerup p)
	{
		PerkEffect[] perkEffects = effectsField.GetValue(p) as PerkEffect[];

		ModStatAction action = actionField.GetValue(perkEffects[0]) as ModStatAction;

		FieldInfo actionStatChangesField = AccessTools
			.DeclaredField(typeof(ModStatAction), "statChanges");

		StatChange[] statChanges = actionStatChangesField
			.GetValue(action) as StatChange[];

		statChanges[0].value = 0.001f;
	}

	// Nerf electro mastery's area to 5% size increase. I honestly may have to
	// even lower here.
	[HarmonyPatch(typeof(ModThunderAreaAction), "Activate")]
	[HarmonyPrefix]
	static void NerfThunderSize(GameObject target, ref float ___thunderAOEMod)
	{
		___thunderAOEMod = 0.05f;
	}

	static void NerfFrostbite(Powerup p)
	{
		PerkEffect[] perkEffects = effectsField.GetValue(p) as PerkEffect[];

		PercentDamageAction action = actionField
			.GetValue(perkEffects[0]) as PercentDamageAction;

		FrostbitePercentDamageAction fpda =
			new FrostbitePercentDamageAction(action);

		actionField.SetValue(perkEffects[0], fpda);
	}

	static void NerfShatter(Powerup p)
	{
		PerkEffect[] perkEffects = effectsField.GetValue(p) as PerkEffect[];

		ShatterFrozenAction action = actionField
			.GetValue(perkEffects[0]) as ShatterFrozenAction;

		FieldInfo shatterPercentDamage = AccessTools
			.DeclaredField(typeof(ShatterFrozenAction), "shatterPercentDamage");

		shatterPercentDamage.SetValue(action, 0.01f);
	}
}
}
