using System.Collections;
using System.Collections.Generic;
using System.Linq;

using HarmonyLib;

using flanne;
using flanne.Player;

using UnityEngine;

namespace _20MinutesTillDawn.PowerScaling
{
public static class ModifyEndlessSpawnSessions
{
	static int cycle = 2;

	static PlayerXP playerXP = null;

	static Dictionary<string, GameObject> monster =
		new Dictionary<string, GameObject>();

	public static void Reset()
	{
		cycle = 2;
	}

	// ID:		Monster name
	// 990:		BrainMonster
	// 992:		Lamprey
	// 1159:	BigBoomer
	// 1161:	Boomer
	// 1228:	ElderBrain
	// 1230:	SpawnerMonster
	// 1342:	EyeMonster
	// 1229:	WingedMonster

	// ID:		Boss name
	// 1195:	Shoggoth ; Laser boss
	// 1199:	ShubNiggurath ; Charging boss

	static IEnumerable<SpawnSession> GenerateInitialSessions()
	{
		yield return new SpawnSession() { monsterPrefab = monster["BrainMonster"],   HP = 24,    maximum = 20,  numPerSpawn = 4,  spawnCooldown = 3,  startTime = 0,    duration = 60,  isElite = false };
		yield return new SpawnSession() { monsterPrefab = monster["BrainMonster"],   HP = 24,    maximum = 50,  numPerSpawn = 10, spawnCooldown = 4,  startTime = 60,   duration = 60,  isElite = false };
		yield return new SpawnSession() { monsterPrefab = monster["Boomer"],         HP = 30,    maximum = 2,   numPerSpawn = 1,  spawnCooldown = 4,  startTime = 60,   duration = 60,  isElite = false };
		yield return new SpawnSession() { monsterPrefab = monster["BrainMonster"],   HP = 30,    maximum = 200, numPerSpawn = 7,  spawnCooldown = 2,  startTime = 120,  duration = 240, isElite = false };
		yield return new SpawnSession() { monsterPrefab = monster["Boomer"],         HP = 30,    maximum = 10,  numPerSpawn = 2,  spawnCooldown = 5,  startTime = 120,  duration = 240, isElite = false };
		yield return new SpawnSession() { monsterPrefab = monster["ElderBrain"],     HP = 1000,  maximum = 1,   numPerSpawn = 1,  spawnCooldown = 10, startTime = 180,  duration = 5,   isElite = true  };
		yield return new SpawnSession() { monsterPrefab = monster["BrainMonster"],   HP = 60,    maximum = 400, numPerSpawn = 12, spawnCooldown = 2,  startTime = 360,  duration = 120, isElite = false };
		yield return new SpawnSession() { monsterPrefab = monster["EyeMonster"],     HP = 400,   maximum = 2,   numPerSpawn = 2,  spawnCooldown = 10, startTime = 360,  duration = 120, isElite = false };
		yield return new SpawnSession() { monsterPrefab = monster["Boomer"],         HP = 60,    maximum = 1,   numPerSpawn = 1,  spawnCooldown = 1,  startTime = 420,  duration = 120, isElite = false };
		yield return new SpawnSession() { monsterPrefab = monster["BrainMonster"],   HP = 80,    maximum = 600, numPerSpawn = 16, spawnCooldown = 1,  startTime = 480,  duration = 120, isElite = false };
		yield return new SpawnSession() { monsterPrefab = monster["Lamprey"],        HP = 200,   maximum = 30,  numPerSpawn = 3,  spawnCooldown = 1,  startTime = 605,  duration = 55,  isElite = false };
		yield return new SpawnSession() { monsterPrefab = monster["EyeMonster"],     HP = 200,   maximum = 12,  numPerSpawn = 2,  spawnCooldown = 1,  startTime = 660,  duration = 120, isElite = false };
		yield return new SpawnSession() { monsterPrefab = monster["Lamprey"],        HP = 250,   maximum = 100, numPerSpawn = 5,  spawnCooldown = 1,  startTime = 660,  duration = 120, isElite = false };
		yield return new SpawnSession() { monsterPrefab = monster["Boomer"],         HP = 240,   maximum = 4,   numPerSpawn = 2,  spawnCooldown = 1,  startTime = 660,  duration = 120, isElite = false };
		yield return new SpawnSession() { monsterPrefab = monster["SpawnerMonster"], HP = 10000, maximum = 1,   numPerSpawn = 1,  spawnCooldown = 10, startTime = 680,  duration = 5,   isElite = true  };
		yield return new SpawnSession() { monsterPrefab = monster["Lamprey"],        HP = 400,   maximum = 300, numPerSpawn = 14, spawnCooldown = 1,  startTime = 780,  duration = 115, isElite = false };
		yield return new SpawnSession() { monsterPrefab = monster["EyeMonster"],     HP = 300,   maximum = 16,  numPerSpawn = 2,  spawnCooldown = 1,  startTime = 900,  duration = 60,  isElite = false };
		yield return new SpawnSession() { monsterPrefab = monster["Boomer"],         HP = 400,   maximum = 20,  numPerSpawn = 2,  spawnCooldown = 1,  startTime = 900,  duration = 60,  isElite = false };
		yield return new SpawnSession() { monsterPrefab = monster["WingedMonster"],  HP = 18000, maximum = 1,   numPerSpawn = 1,  spawnCooldown = 10, startTime = 960,  duration = 5,   isElite = true  };
		yield return new SpawnSession() { monsterPrefab = monster["BrainMonster"],   HP = 250,   maximum = 600, numPerSpawn = 26, spawnCooldown = 1,  startTime = 960,  duration = 120, isElite = false };
		yield return new SpawnSession() { monsterPrefab = monster["Lamprey"],        HP = 500,   maximum = 300, numPerSpawn = 20, spawnCooldown = 1,  startTime = 1080, duration = 119, isElite = false };
		yield return new SpawnSession() { monsterPrefab = monster["EyeMonster"],     HP = 50,    maximum = 10,  numPerSpawn = 1,  spawnCooldown = 1,  startTime = 1080, duration = 119, isElite = false };
	}

	static IEnumerable<SpawnSession> GenerateNextSessions()
	{
		yield return new SpawnSession() { monsterPrefab = monster["BrainMonster"],   HP = 300,    maximum = 300, numPerSpawn = 4,  spawnCooldown = 1,  startTime = 0,    duration = 60,  isElite = false };
		yield return new SpawnSession() { monsterPrefab = monster["BrainMonster"],   HP = 300,    maximum = 500, numPerSpawn = 10, spawnCooldown = 1,  startTime = 60,   duration = 60,  isElite = false };
		yield return new SpawnSession() { monsterPrefab = monster["BigBoomer"],      HP = 10000,  maximum = 12,  numPerSpawn = 3,  spawnCooldown = 1,  startTime = 60,   duration = 60,  isElite = false };
		yield return new SpawnSession() { monsterPrefab = monster["BrainMonster"],   HP = 600,    maximum = 500, numPerSpawn = 7,  spawnCooldown = 1,  startTime = 120,  duration = 240, isElite = false };
		yield return new SpawnSession() { monsterPrefab = monster["BigBoomer"],      HP = 12000,  maximum = 10,  numPerSpawn = 3,  spawnCooldown = 1,  startTime = 120,  duration = 240, isElite = false };
		yield return new SpawnSession() { monsterPrefab = monster["ElderBrain"],     HP = 10000,  maximum = 1,   numPerSpawn = 1,  spawnCooldown = 2,  startTime = 300,  duration = 1,   isElite = true  };
		yield return new SpawnSession() { monsterPrefab = monster["Boomer"],         HP = 20000,  maximum = 6,   numPerSpawn = 2,  spawnCooldown = 1,  startTime = 300,  duration = 60,  isElite = false };
		yield return new SpawnSession() { monsterPrefab = monster["BrainMonster"],   HP = 900,    maximum = 400, numPerSpawn = 12, spawnCooldown = 1,  startTime = 360,  duration = 120, isElite = false };
		yield return new SpawnSession() { monsterPrefab = monster["EyeMonster"],     HP = 10000,  maximum = 20,  numPerSpawn = 2,  spawnCooldown = 10, startTime = 360,  duration = 120, isElite = false };
		yield return new SpawnSession() { monsterPrefab = monster["BigBoomer"],      HP = 20000,  maximum = 10,  numPerSpawn = 1,  spawnCooldown = 1,  startTime = 420,  duration = 120, isElite = false };
		yield return new SpawnSession() { monsterPrefab = monster["BrainMonster"],   HP = 1200,   maximum = 600, numPerSpawn = 16, spawnCooldown = 1,  startTime = 480,  duration = 120, isElite = false };
		yield return new SpawnSession() { monsterPrefab = monster["Lamprey"],        HP = 2000,   maximum = 300, numPerSpawn = 3,  spawnCooldown = 1,  startTime = 605,  duration = 55,  isElite = false };
		yield return new SpawnSession() { monsterPrefab = monster["EyeMonster"],     HP = 2000,   maximum = 24,  numPerSpawn = 2,  spawnCooldown = 1,  startTime = 660,  duration = 120, isElite = false };
		yield return new SpawnSession() { monsterPrefab = monster["Lamprey"],        HP = 3000,   maximum = 500, numPerSpawn = 5,  spawnCooldown = 1,  startTime = 660,  duration = 120, isElite = false };
		yield return new SpawnSession() { monsterPrefab = monster["Boomer"],         HP = 15000,  maximum = 40,  numPerSpawn = 2,  spawnCooldown = 1,  startTime = 660,  duration = 120, isElite = false };
		yield return new SpawnSession() { monsterPrefab = monster["SpawnerMonster"], HP = 100000, maximum = 1,   numPerSpawn = 1,  spawnCooldown = 10, startTime = 680,  duration = 5,   isElite = true  };
		yield return new SpawnSession() { monsterPrefab = monster["Lamprey"],        HP = 5000,   maximum = 600, numPerSpawn = 14, spawnCooldown = 1,  startTime = 780,  duration = 115, isElite = false };
		yield return new SpawnSession() { monsterPrefab = monster["EyeMonster"],     HP = 4000,   maximum = 16,  numPerSpawn = 2,  spawnCooldown = 1,  startTime = 900,  duration = 60,  isElite = false };
		yield return new SpawnSession() { monsterPrefab = monster["BigBoomer"],      HP = 25000,  maximum = 20,  numPerSpawn = 2,  spawnCooldown = 1,  startTime = 900,  duration = 60,  isElite = false };
		yield return new SpawnSession() { monsterPrefab = monster["WingedMonster"],  HP = 180000, maximum = 1,   numPerSpawn = 1,  spawnCooldown = 10, startTime = 960,  duration = 5,   isElite = true  };
		yield return new SpawnSession() { monsterPrefab = monster["BrainMonster"],   HP = 5000,   maximum = 600, numPerSpawn = 26, spawnCooldown = 1,  startTime = 960,  duration = 120, isElite = false };
		yield return new SpawnSession() { monsterPrefab = monster["Lamprey"],        HP = 7000,   maximum = 600, numPerSpawn = 20, spawnCooldown = 1,  startTime = 1080, duration = 119, isElite = false };
		yield return new SpawnSession() { monsterPrefab = monster["EyeMonster"],     HP = 2000,   maximum = 30,  numPerSpawn = 1,  spawnCooldown = 1,  startTime = 1080, duration = 119, isElite = false };
	}

	[HarmonyPatch(typeof(MapInitializer), "Start")]
	[HarmonyPrefix]
	static void Start()
	{
		MapData mapData = SelectedMap.MapData;

		if(mapData.endless)
		{
			KeyValuePair<string, GameObject> selector(SpawnSession s)
			{
				return new KeyValuePair<string, GameObject>(
					s.monsterPrefab.name,
					s.monsterPrefab);
			};

			var ssNames = mapData.spawnSessions
				.Select(selector)
				.GroupBy(kvp => kvp.Key)
				.Select(ig => ig.First())
				.ToDictionary(ks => ks.Key, v => v.Value);

			var esNames = mapData.endlessSpawnSessions
				.Select(selector)
				.GroupBy(kvp => kvp.Key)
				.Select(ig => ig.First())
				.ToDictionary(ks => ks.Key, v => v.Value);

			monster = monster
				.Union(ssNames)
				.GroupBy(kvp => kvp.Key)
				.ToDictionary(kvp => kvp.Key, kvp => kvp.First().Value);

			monster = monster
				.Union(esNames)
				.GroupBy(kvp => kvp.Key)
				.ToDictionary(kvp => kvp.Key, kvp => kvp.First().Value);

			mapData.spawnSessions = new List<SpawnSession>();
			mapData.spawnSessions.AddRange(GenerateInitialSessions());

			mapData.endlessLoopTime = 1200;
			mapData.endlessSpawnSessions = new List<SpawnSession>();
			mapData.endlessSpawnSessions.AddRange(GenerateNextSessions());

			for(int i = 2; i < 5; ++i)
				mapData.endlessBossSpawn[i].timeToSpawn = 900;
		}
	}

	[HarmonyPatch(typeof(MapInitializer), "EndlessLoop")]
	[HarmonyPostfix]
	static IEnumerator EndlessLoop(
		IEnumerator result,
		HordeSpawner ___hordeSpawner,
		BossSpawner ___bossSpawner)
	{
		playerXP = GameObject.FindObjectOfType<PlayerXP>();

		result.MoveNext();
		yield return result.Current;

		while(result.MoveNext())
		{
			playerXP.xpMultiplier.AddMultiplierBonus(1.2f);

			// *2 lags the game lol
			___hordeSpawner.spawnRateMulitplier *= 1.25f;
			___hordeSpawner.speedMultiplier *= 1.2f;
			___bossSpawner.healthMultiplier = 1f + Mathf.Pow(cycle, 2f);
			++cycle;

			yield return result.Current;
		}
	}
}
}
