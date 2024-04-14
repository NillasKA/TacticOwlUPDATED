﻿using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;
using System.Collections.Generic;
using Settings = Tacticowl.ModSettings_Tacticowl;

namespace Tacticowl
{
    [HarmonyPatch(typeof(AttackTargetsCache), nameof(AttackTargetsCache.GetPotentialTargetsFor))]
    class Patch_GetPotentialTargetsFor
    {
        static bool Prepare()
		{
			return Settings.searchAndDestroyEnabled;
		}
        static void Postfix(IAttackTargetSearcher th, List<IAttackTarget> __result)
        {    
            if (th is not Pawn searchPawn || !searchPawn.SearchesAndDestroys()) return;

            for (int i = __result.Count; i-- > 0;)
            {
                var target = __result[i];
                if (target is Pawn targetPawn) if (!targetPawn.NonHumanlikeOrWildMan() || targetPawn.IsAttacking()) continue;
                
                __result.Remove(target);
            }
        }
    }
}