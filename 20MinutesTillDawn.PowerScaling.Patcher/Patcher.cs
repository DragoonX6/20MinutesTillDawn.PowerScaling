using System.Collections.Generic;
using Mono.Cecil;

namespace _20MinutesTillDawn.PowerScaling.Patcher;

public static class Patcher
{
	public static IEnumerable<string> TargetDlls { get; } =
		new[]{"Assembly-CSharp.dll"};

	public static void Patch(AssemblyDefinition assembly)
	{
	}
}
