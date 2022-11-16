using FunGuy_Fury.Util;
using HarmonyLib;

namespace FunGuy_Fury.Patches;

[HarmonyPatch(typeof(Character), nameof(Character.RPC_Damage))]
static class PlayerDamagePatch
{
    static void Prefix(Character __instance, ref HitData hit)
    {
        if (__instance.IsDebugFlying() || !__instance.m_nview.IsOwner() || __instance.GetHealth() <= 0.0 ||
            __instance.IsDead() || __instance.IsTeleporting() || __instance.InCutscene() ||
            hit.m_dodgeable && __instance.IsDodgeInvincible())
            return; 
        
        Functions.CheckHitApplyModifier(hit);
    }
}

[HarmonyPatch(typeof(TreeLog), nameof(TreeLog.RPC_Damage))]
static class TreeLogRPCDamagePatch
{
    static void Prefix(TreeLog __instance, ref HitData hit)
    {
        Functions.CheckHitApplyModifier(hit);
    }
}

[HarmonyPatch(typeof(TreeBase),nameof(TreeBase.RPC_Damage))]
static class TreeBasePatch
{
    static void Prefix(TreeBase __instance, ref HitData hit)
    {
        Functions.CheckHitApplyModifier(hit);
    }
}