using System.Collections.Generic;
using System.Linq;

using BepInEx.Logging;

using Mono.Cecil;

namespace _20MinutesTillDawn.PowerScaling.Patcher;

public static class Patcher
{
	public static IEnumerable<string> TargetDlls { get; } =
		new[]{"Assembly-CSharp.dll"};

	private static ManualLogSource log = null;

	public static void Initialize()
	{
		log = Logger.CreateLogSource("20MinutesTillDawn.PowerScaling.Patcher");
	}

	public static void Patch(AssemblyDefinition assembly)
	{
		TypeDefinition powerupMenuStatetype = assembly.MainModule.Types
			.Where(t => t.Name.Contains("PowerupMenuState"))
			.First();

		MethodDefinition method = powerupMenuStatetype.Methods
			.Where(t => t.Name.Contains("PlayLevelUpAnimationCR"))
			.First();

		method.NoInlining = true;

		log.LogInfo("Added `noinlining` attribute to `PlayLevelUpAnimationCR`");
	}
}
