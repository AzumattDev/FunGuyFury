using System;
using HarmonyLib;

namespace FunGuy_Fury.Patches;

[HarmonyPatch(typeof(Player), nameof(Player.ConsumeItem))]
static class Player_ConsumeItem_Patch
{
    static bool Prefix(Player __instance, Inventory inventory, ItemDrop.ItemData item)
    {
        if (item.m_dropPrefab.name != "FlyAgaricMushroom") return true;
        if ((int)((FunGuy_FuryPlugin.Cooldown.Value + FunGuy_FuryPlugin.Duration.Value) -
                  (DateTime.Now - FunGuy_FuryPlugin.LastShroomTime).TotalSeconds) >=
            0)
        {
            //Player.m_localPlayer.Message(MessageHud.MessageType.Center, $"On cooldown for {(int)(FunGuy_FuryPlugin.Cooldown.Value - (DateTime.Now - FunGuy_FuryPlugin.LastShroomTime).TotalSeconds)} seconds");
            Player.m_localPlayer.Message(MessageHud.MessageType.Center,
                // ReSharper disable once UseStringInterpolation
                string.Format(FunGuy_FuryPlugin.CooldownMessage.Value,
                    (int)((FunGuy_FuryPlugin.Cooldown.Value + FunGuy_FuryPlugin.Duration.Value) -
                          (DateTime.Now - FunGuy_FuryPlugin.LastShroomTime).TotalSeconds)));
            return false;
        }

        return true;
    }
}