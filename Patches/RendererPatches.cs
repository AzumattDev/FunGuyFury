using System;
using FunGuy_Fury.Util;
using HarmonyLib;
using UnityEngine;

namespace FunGuy_Fury.Patches;

[HarmonyPatch(typeof(Game), nameof(Game.Start))]
static class GameStartPatch
{
    static void Postfix(Game __instance)
    {
        ZRoutedRpc.s_instance.Register("SetBerserkerZdo", new Action<long, ZDOID, bool>(Functions.SetBerserkerZdo));
        ZRoutedRpc.s_instance.Register("BattleCryBerserkSE", new Action<long, ZDOID>(Functions.RPC_BattleCryBerserkSE));
    }
}

[HarmonyPatch(typeof(StatusEffect), nameof(StatusEffect.IsDone))]
static class StatusEffectIsDonePatch
{
    static void Postfix(StatusEffect __instance, ref bool __result)
    {
        if (__instance.name == "Berserk" && __result)
        {
            ZRoutedRpc.s_instance.InvokeRoutedRPC(ZRoutedRpc.Everybody, "SetBerserkerZdo",
                __instance.m_character.GetZDOID(), false);
        }
    }
}

[HarmonyPatch(typeof(StatusEffect), nameof(StatusEffect.Setup))]
static class StatusEffectSetUpPatch
{
    static void Postfix(StatusEffect __instance, Character character)
    {
        if (__instance.name == "Berserk")
        {
            FunGuy_FuryPlugin.LastShroomTime = DateTime.Now;
            __instance.m_character.m_zanim.SetTrigger("gpower");
            ZRoutedRpc.s_instance.InvokeRoutedRPC(ZRoutedRpc.Everybody, "SetBerserkerZdo", __instance.m_character.GetZDOID(), true);
            ZRoutedRpc.s_instance.InvokeRoutedRPC(ZRoutedRpc.Everybody, "BattleCryBerserkSE", __instance.m_character.GetZDOID());
        }
    }
}