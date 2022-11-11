using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using HarmonyLib;

using flanne;
using flanne.Core;
using flanne.UI;

using UnityEngine;
using UnityEngine.UI;

namespace _20MinutesTillDawn.PowerScaling.UI;

public static class BanishSystem
{
	static BanishButton banishButton = null;

	[HarmonyPatch(typeof(GameController), "Awake")]
	[HarmonyPostfix]
	static void InsertBanishButton(GameController __instance)
	{
		Transform powerupMenu = __instance.powerupMenu.gameObject.transform;
		Transform menuMover = powerupMenu.transform.GetChild(1);
		GameObject rerollButton = menuMover.GetChild(3).gameObject;

		GameObject banishObj = GameObject.Instantiate(rerollButton);
		banishButton = banishObj.AddComponent<BanishButton>();

		banishButton.Init(
			__instance.powerupGenerator,
			__instance.powerupMenu,
			rerollButton);

		banishObj.transform.SetParent(menuMover.transform, false);
		banishObj.SetActive(SelectedMap.MapData.endless);
	}

	[HarmonyPatch(typeof(PowerupMenuState), "PlayLevelUpAnimationCR")]
	[HarmonyPostfix]
	static IEnumerator PlayLevelUpAnimationCR(
		IEnumerator result,
		PowerupMenuState __instance)
	{
		banishButton.state = __instance;

		while(result.MoveNext())
			yield return result.Current;

		banishButton.SetPosition();

		yield break;
	}
}

public class BanishButton: MonoBehaviour
{
	static LocalizedString buttonKey = "banish_button_text";
	static string buttonTextEN = "Banish Upgrade";
	static bool textAdded = false;

	private PowerupGenerator powerupGenerator = null;
	private PowerupMenu powerupMenu = null;
	private GameObject rerollButton = null;
	private RectTransform rerollTransform = null;

	public PowerupMenuState state = null;

	private FieldInfo powerupPool = AccessTools
		.DeclaredField(typeof(PowerupGenerator), "powerupPool");

	public void Init(
		PowerupGenerator powerupGenerator,
		PowerupMenu powerupMenu,
		GameObject rerollButton)
	{
		this.powerupGenerator = powerupGenerator;
		this.powerupMenu = powerupMenu;
		this.rerollButton = rerollButton;
		rerollTransform = rerollButton.GetComponent<RectTransform>();

		rerollButton
			.GetComponent<Button>()
			.onClick
			.AddListener(new(OnReroll));

		Button button = GetComponent<Button>();
		button.onClick.AddListener(new(OnClick));

		if(!textAdded)
		{
			for(int i = 0;
				i < Enum.GetNames(typeof(LocalizationSystem.Language)).Length;
				++i)
			{
				FieldInfo dictField = AccessTools
					.DeclaredField(typeof(LocalizationSystem), i + 1);

				Dictionary<string, string> localized = dictField
					.GetValue(null) as Dictionary<string, string>;

				localized.Add(buttonKey.key, buttonTextEN);

				dictField.SetValue(null, localized);
			}

			textAdded = true;
		}

		TextLocalizerUI textLocalizer =
			GetComponentInChildren<TextLocalizerUI>(true);

		FieldInfo localizedString = AccessTools
			.DeclaredField(typeof(TextLocalizerUI), "localizedString");

		localizedString.SetValue(textLocalizer, buttonKey);
	}

	public void SetPosition(bool noforce = true)
	{
		if(rerollButton.activeInHierarchy && noforce)
		{
			transform.localPosition = new(
				0,
				rerollTransform.anchoredPosition.y -35f);
		}
		else
		{
			transform.localPosition = new(
				0f,
				rerollTransform.anchoredPosition.y);
		}
	}

	private void OnClick()
	{
		state.StartCoroutine("EndLevelUpAnimationCR");
		Powerup powerup = powerupMenu.toggledData;

		List<PowerupPoolItem> pool = powerupPool
			.GetValue(powerupGenerator) as List<PowerupPoolItem>;

		PowerupPoolItem item = pool.Find(x => x.powerup == powerup);
		if(item != null)
		{
			PowerScaling.Log.LogDebug($"Banished {powerup.name}");
			pool.Remove(item);

			powerupPool.SetValue(powerupGenerator, pool);
		}

		// Maybe call reroll?
	}

	private void OnReroll()
	{
		SetPosition(false);
	}
}
