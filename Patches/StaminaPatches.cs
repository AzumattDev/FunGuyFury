using HarmonyLib;

namespace FunGuy_Fury.Patches;

[HarmonyPatch(typeof(Player),nameof(Player.UseStamina))]
static class PlayerUseStamPatch
{
    static void Prefix(Player __instance, ref float v)
    {
        // If the player has the berserker status effect, reduce stamina usage by 100%
        if (__instance.GetSEMan().HaveStatusEffect("Berserk".GetStableHashCode()))
        {
            v = 0.0f;
        }
    }
}