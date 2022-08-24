using HarmonyLib;

using UnityEngine;

using flanne;

namespace _20MinutesTillDawn.PowerScaling.Fixes
{
public static class FixBuffPlayerStats
{
	[HarmonyPatch(typeof(BuffPlayerStats), "RemoveBuff")]
	[HarmonyPrefix]
	static bool RemoveBuff(
		StatChange[] ___statChanges,
		StatsHolder ___stats,
		PlayerController ___player)
	{
		StatChange[] array = ___statChanges;
		for(int i = 0; i < array.Length; i++)
		{
			StatChange statChange = array[i];
			if(statChange.isFlatMod)
			{
				___stats[statChange.type].AddFlatBonus(-1 * statChange.flatValue);
			}
			else if(statChange.value > 0f)
			{
				___stats[statChange.type].AddMultiplierBonus(
					1f / (1f + statChange.value) - 1f);
			}

			switch(statChange.type)
			{
			case StatType.MaxHP:
			{
				___player.playerHealth.maxHP =
					Mathf.FloorToInt(___stats[statChange.type].Modify(
						___player.loadedCharacter.startHP));
			} break;
			case StatType.CharacterSize:
			{
				___player.playerSprite.transform.localScale =
					Vector3.one * ___stats[statChange.type].Modify(1f);
			} break;
			case StatType.PickupRange:
			{
				GameObject pickUpper =
					GameObject.FindGameObjectWithTag("Pickupper");

				pickUpper.transform.localScale =
					Vector3.one * ___stats[statChange.type].Modify(1f);
			} break;
			case StatType.VisionRange:
			{
				GameObject playerVision =
					GameObject.FindGameObjectWithTag("PlayerVision");

				playerVision.transform.localScale =
					Vector3.one * ___stats[statChange.type].Modify(1f);
			} break;
		}
		}

		return false;
	}
}
}
