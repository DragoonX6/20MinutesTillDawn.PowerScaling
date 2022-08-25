using System.Collections;
using System.Collections.Generic;

using HarmonyLib;

using flanne;

using UnityEngine;

namespace _20MinutesTillDawn.PowerScaling
{
public static class ModifyEndlessSpawnSessions
{
	static int cycle = 2;

	public static void Reset()
	{
		cycle = 2;
	}

	static IEnumerable<SpawnSession> GenerateSessions()
	{
		yield return new SpawnSession() { objectPoolTag = "BrainMonster",   HP = 24,    maximum = 20,  numPerSpawn = 4,  spawnCooldown = 3,  startTime = 0,    duration = 60,  isElite = false };
		yield return new SpawnSession() { objectPoolTag = "BrainMonster",   HP = 24,    maximum = 50,  numPerSpawn = 10, spawnCooldown = 4,  startTime = 60,   duration = 60,  isElite = false };
		yield return new SpawnSession() { objectPoolTag = "Boomer",         HP = 30,    maximum = 2,   numPerSpawn = 1,  spawnCooldown = 4,  startTime = 60,   duration = 60,  isElite = false };
		yield return new SpawnSession() { objectPoolTag = "BrainMonster",   HP = 30,    maximum = 200, numPerSpawn = 7,  spawnCooldown = 2,  startTime = 120,  duration = 240, isElite = false };
		yield return new SpawnSession() { objectPoolTag = "Boomer",         HP = 30,    maximum = 10,  numPerSpawn = 2,  spawnCooldown = 5,  startTime = 120,  duration = 240, isElite = false };
		yield return new SpawnSession() { objectPoolTag = "ElderBrain",     HP = 1000,  maximum = 1,   numPerSpawn = 1,  spawnCooldown = 10, startTime = 180,  duration = 5,   isElite = true };
		yield return new SpawnSession() { objectPoolTag = "BrainMonster",   HP = 60,    maximum = 400, numPerSpawn = 12, spawnCooldown = 2,  startTime = 360,  duration = 120, isElite = false };
		yield return new SpawnSession() { objectPoolTag = "EyeMonster",     HP = 400,   maximum = 2,   numPerSpawn = 2,  spawnCooldown = 10, startTime = 360,  duration = 120, isElite = false };
		yield return new SpawnSession() { objectPoolTag = "Boomer",         HP = 60,    maximum = 1,   numPerSpawn = 1,  spawnCooldown = 1,  startTime = 420,  duration = 120, isElite = false };
		yield return new SpawnSession() { objectPoolTag = "BrainMonster",   HP = 80,    maximum = 600, numPerSpawn = 16, spawnCooldown = 1,  startTime = 480,  duration = 120, isElite = false };
		yield return new SpawnSession() { objectPoolTag = "Lamprey",        HP = 200,   maximum = 30,  numPerSpawn = 3,  spawnCooldown = 1,  startTime = 605,  duration = 55,  isElite = false };
		yield return new SpawnSession() { objectPoolTag = "EyeMonster",     HP = 200,   maximum = 12,  numPerSpawn = 2,  spawnCooldown = 1,  startTime = 660,  duration = 120, isElite = false };
		yield return new SpawnSession() { objectPoolTag = "Lamprey",        HP = 250,   maximum = 100, numPerSpawn = 5,  spawnCooldown = 1,  startTime = 660,  duration = 120, isElite = false };
		yield return new SpawnSession() { objectPoolTag = "Boomer",         HP = 240,   maximum = 4,   numPerSpawn = 2,  spawnCooldown = 1,  startTime = 660,  duration = 120, isElite = false };
		yield return new SpawnSession() { objectPoolTag = "SpawnerMonster", HP = 10000, maximum = 1,   numPerSpawn = 1,  spawnCooldown = 10, startTime = 680,  duration = 5,   isElite = true };
		yield return new SpawnSession() { objectPoolTag = "Lamprey",        HP = 400,   maximum = 300, numPerSpawn = 14, spawnCooldown = 1,  startTime = 780,  duration = 115, isElite = false };
		yield return new SpawnSession() { objectPoolTag = "EyeMonster",     HP = 300,   maximum = 16,  numPerSpawn = 2,  spawnCooldown = 1,  startTime = 900,  duration = 60,  isElite = false };
		yield return new SpawnSession() { objectPoolTag = "Boomer",         HP = 400,   maximum = 20,  numPerSpawn = 2,  spawnCooldown = 1,  startTime = 900,  duration = 60,  isElite = false };
		yield return new SpawnSession() { objectPoolTag = "WingedMonster",  HP = 18000, maximum = 1,   numPerSpawn = 1,  spawnCooldown = 10, startTime = 960,  duration = 5,   isElite = true };
		yield return new SpawnSession() { objectPoolTag = "BrainMonster",   HP = 250,   maximum = 600, numPerSpawn = 26, spawnCooldown = 1,  startTime = 960,  duration = 120, isElite = false };
		yield return new SpawnSession() { objectPoolTag = "Lamprey",        HP = 500,   maximum = 300, numPerSpawn = 20, spawnCooldown = 1,  startTime = 1080, duration = 119, isElite = false };
		yield return new SpawnSession() { objectPoolTag = "EyeMonster",     HP = 50,    maximum = 10,  numPerSpawn = 1,  spawnCooldown = 1,  startTime = 1080, duration = 119, isElite = false };
	}

	[HarmonyPatch(typeof(MapInitializer), "Start")]
	[HarmonyPrefix]
	static void Start()
	{
		MapData mapData = SelectedMap.MapData;

		if(mapData.endless)
		{
			mapData.timeLimit = 1200;
			mapData.spawnSessions = new List<SpawnSession>();

			mapData.spawnSessions.AddRange(GenerateSessions());

			mapData.bossSpawns[1].timeToSpawn = 900;
		}
	}

	[HarmonyPatch(typeof(MapInitializer), "EndlessLoop")]
	[HarmonyPostfix]
	static IEnumerator EndlessLoop(
		IEnumerator result,
		HordeSpawner ___hordeSpawner,
		BossSpawner ___bossSpawner)
	{
		result.MoveNext();
		yield return result.Current;

		while(result.MoveNext())
		{
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
