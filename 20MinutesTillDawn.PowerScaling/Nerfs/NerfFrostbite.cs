using System;
using System.Collections.Generic;
using System.Reflection;

using HarmonyLib;

using flanne;
using flanne.PerkSystem.Actions;

using UnityEngine;

namespace _20MinutesTillDawn.PowerScaling.Nerfs
{
public class FrostbitePercentDamageAction: PercentDamageAction
{
	private HashSet<Health> targets = new HashSet<Health>();

	private bool subscribed = false;

	public FrostbitePercentDamageAction(PercentDamageAction wrapped)
	{
		FieldInfo damageTypeField = AccessTools
			.DeclaredField(typeof(PercentDamageAction), "damageType");

		FieldInfo percentDamageField = AccessTools
			.DeclaredField(typeof(PercentDamageAction), "percentDamage");

		FieldInfo championPercentDamageField = AccessTools
			.DeclaredField(typeof(PercentDamageAction), "championPercentDamage");

		DamageType damageType = (DamageType)damageTypeField.GetValue(wrapped);
		float percentDamage = (float)percentDamageField.GetValue(wrapped);

		float championPercentDamage = (float)championPercentDamageField
			.GetValue(wrapped);

		damageTypeField.SetValue(this, damageType);
		percentDamageField.SetValue(this, percentDamage);
		championPercentDamageField.SetValue(this, championPercentDamage);
	}

	public override void Activate(GameObject target)
	{
		if(!subscribed)
		{
			// We have to this in here, since the object gets copied at some
			// point elsewhere. So we have to subscribe for each copy...
			this.AddObserver(
				new Action<object, object>(this.RemoveTarget),
				Health.DeathEvent);

			subscribed = true;
		}

		Health health = target.GetComponent<Health>();

		if(health.isDead)
			return;

		if(!targets.Contains(health))
		{
			base.Activate(target);

			targets.Add(health);
		}
	}

	private void RemoveTarget(object sender, object args)
	{
		Health target = sender as Health;
		targets.Remove(target);
	}
}
}
