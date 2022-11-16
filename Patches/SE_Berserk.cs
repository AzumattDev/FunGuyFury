using UnityEngine;

namespace FunGuy_Fury.Patches;

public class SE_Berserk : StatusEffect
{
    public float m_damageInterval = 1f;
    public float m_baseTTL = 2f;
    public float m_TTLPerDamagePlayer = 2f;
    public float m_TTLPerDamage = 2f;
    public float m_TTLPower = 0.5f;
    public float m_timer;
    public float m_damageLeft;
    public float m_damagePerHit;

    public override void UpdateStatusEffect(float dt)
    {
        base.UpdateStatusEffect(dt);
        m_timer -= dt;
        if (m_timer > 0.0)
            return;
        m_timer = m_damageInterval;
        HitData hit = new();
        hit.m_point = m_character.GetCenterPoint();
        hit.m_damage.m_damage = m_damagePerHit;
        m_damageLeft -= m_damagePerHit;
        m_character.ApplyDamage(hit, true, false);
    }

    public void AddDamage(float damage)
    {
        if (damage < (double)m_damageLeft)
            return;
        m_damageLeft = damage;
        m_ttl = m_baseTTL +
                Mathf.Pow(
                    m_damageLeft * (m_character.IsPlayer()
                        ? m_TTLPerDamagePlayer
                        : m_TTLPerDamage), m_TTLPower);
        int num = (int)(m_ttl / (double)m_damageInterval);
        m_damagePerHit = m_damageLeft / num;
        FunGuy_FuryPlugin.FunGuyFuryLogger.LogDebug("Berserker damage: " + m_damageLeft + " ttl:" + m_ttl + " hits:" +
                                                     num + " dmg perhit:" + m_damagePerHit);
        ResetTime();
    }
}