﻿using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Harmony;
using Verse;
using UnityEngine;

namespace RT_SolarFlareShield
{
	[HarmonyPatch(typeof(PowerNet))]
	[HarmonyPatch("PowerNetTick")]
	static class Patch_PowerNetTick
	{
		public static IEnumerable<CodeInstruction> Transpiler(
			MethodBase original,
			IEnumerable<CodeInstruction> instructions)
		{
			return instructions.MethodReplacer(
				AccessTools.Method(typeof(GameConditionManager), "ConditionIsActive"),
				AccessTools.Method(typeof(Patch_PowerNetTick), "ConditionIsActive"));
		}

		public static bool ConditionIsActive(this GameConditionManager instance, GameConditionDef def)
		{
			bool result = instance.ConditionIsActive(def);
			if (result && def == GameConditionDefOf.SolarFlare)
			{
				return !instance.map.GetShieldCoordinator().hasActiveShield;
			}
			return result;
		}
	}
}
