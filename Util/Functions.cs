using System;
using BepInEx.Configuration;
using FunGuy_Fury.Patches;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FunGuy_Fury.Util;

public class Functions
{
    public static void SetBerserkerZdo(long sender, ZDOID zdoid, bool isBerserker)
    {
        GameObject player = ZNetScene.instance.FindInstance(zdoid);
        if (player == null)
        {
            FunGuy_FuryPlugin.FunGuyFuryLogger.LogError("Player not found");
            return;
        }

        FunGuy_FuryPlugin.FunGuyFuryLogger.LogDebug($"Setting Berserker {isBerserker} for ZDOID: {zdoid}");

        if (isBerserker)
        {
            foreach (Renderer renderer in player.GetComponentsInChildren<Renderer>())
            {
                MaterialPropertyBlock block = new();
                renderer.GetPropertyBlock(block);
                block.SetColor("_Color", Color.red);
                block.SetColor("_EmissionColor", Color.red * 1.0f);
                renderer.SetPropertyBlock(block);
            }
        }
        else
        {
            var playerComp = player.GetComponent<Player>();
            ResetArmor(playerComp);
            player.GetComponent<VisEquipment>().UpdateEquipmentVisuals();
        }
    }

    private static void ResetArmor(Player p)
    {
        // cache the visequip
        var visEquip = p.GetComponent<VisEquipment>();

        visEquip.m_currentLeftItemHash = -1000;
        visEquip.m_currentRightItemHash = -1000;
        visEquip.m_currentChestItemHash = -1000;
        visEquip.m_currentLegItemHash = -1000;
        visEquip.m_currentHelmetItemHash = -1000;
        visEquip.m_currentShoulderItemHash = -1000;
        visEquip.m_currentBeardItemHash = -1000;
        visEquip.m_currentHairItemHash = -1000;
        visEquip.m_currentUtilityItemHash = -1000;
        visEquip.m_currentLeftBackItemHash = -1000;
        visEquip.m_currentRightBackItemHash = -1000;
    }

    internal static void RPC_BattleCryBerserkSE(long sender, ZDOID zdoid)
    {
        GameObject player = ZNetScene.instance.FindInstance(zdoid);
        if (player == null)
        {
            FunGuy_FuryPlugin.FunGuyFuryLogger.LogError("Player not found, cannot do effect");
            return;
        }

        var soundtoPlay = player.GetComponent<Player>().GetPlayerModel() == 1
            ? ZNetScene.instance.GetPrefab("sfx_berserk_female")
            : ZNetScene.instance.GetPrefab("sfx_berserk_male");
        var transform = player.transform;
        GameObject gameObject = Object.Instantiate(soundtoPlay, transform.position, transform.rotation);
        Object.Instantiate(soundtoPlay, player.transform, false);
    }

    internal static void TextAreaDrawer(ConfigEntryBase entry)
    {
        GUILayout.ExpandHeight(true);
        GUILayout.ExpandWidth(true);
        entry.BoxedValue = GUILayout.TextArea((string)entry.BoxedValue, GUILayout.ExpandWidth(true),
            GUILayout.ExpandHeight(true));
    }

    internal static void UpdateConfig(object o, EventArgs e)
    {
        bool saveOnConfigSet = FunGuy_FuryPlugin.context.Config.SaveOnConfigSet;
        FunGuy_FuryPlugin.context.Config.SaveOnConfigSet = false;
        foreach (SE_Berserk seBerserk in Resources.FindObjectsOfTypeAll<SE_Berserk>())
        {
            seBerserk.m_cooldown = 0f;
            seBerserk.m_activationAnimation = "gpower";
            seBerserk.m_ttl = FunGuy_FuryPlugin.Duration.Value;
            seBerserk.m_startMessageType = MessageHud.MessageType.Center;
            seBerserk.m_startMessage = FunGuy_FuryPlugin.StartMessage.Value;
            seBerserk.m_stopMessageType = MessageHud.MessageType.Center;
            seBerserk.m_stopMessage = FunGuy_FuryPlugin.StopMessage.Value;
            seBerserk.m_tooltip = FunGuy_FuryPlugin.EffectTooltip.Value;
            seBerserk.m_damagePerHit = FunGuy_FuryPlugin.DamagePerHit.Value;
            seBerserk.m_damageInterval = FunGuy_FuryPlugin.DamageInterval.Value;
        }
        if (!saveOnConfigSet) return;
        FunGuy_FuryPlugin.context.Config.SaveOnConfigSet = true;
        FunGuy_FuryPlugin.context.Config.Save();
    }

    internal static void CheckHitApplyModifier(HitData hit)
    {
        Character attacker = hit.GetAttacker();
        // If the player has the beserker status effect, increase damage by a factor defined in the config DamageBoost
        if (attacker == null || !attacker.IsPlayer() || !attacker.m_seman.HaveStatusEffect("Berserk")) return;
        hit.ApplyModifier(FunGuy_FuryPlugin.DamageBoost.Value);
    }
}