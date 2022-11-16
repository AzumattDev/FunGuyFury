using HarmonyLib;

namespace FunGuy_Fury.Patches;

[HarmonyPatch(typeof(ZoneSystem), nameof(ZoneSystem.Start))]
static class ZoneSystemStartPatch
{
    static void Prefix(ZoneSystem __instance)
    {
        var mushPickable = ZNetScene.instance.GetPrefab("Pickable_FlyAgaricMushroom");
        ZoneSystem.ZoneVegetation vegetation = new()
        {
            m_name = mushPickable.name,
            m_prefab = mushPickable,
            m_enable = true,
            m_min = 0.5f,
            m_max = 0.5f,
            m_forcePlacement = true,
            m_scaleMin = 1f,
            m_scaleMax = 1.75f,
            m_randTilt = 0,
            m_chanceToUseGroundTilt = 0,
            m_biome = (Heightmap.Biome.Swamp),
            m_biomeArea = Heightmap.BiomeArea.Everything,
            m_blockCheck = true,
            m_minAltitude = -1000f,
            m_maxAltitude = 1000f,
            m_minOceanDepth = 0,
            m_maxOceanDepth = 0,
            m_minTilt = 0,
            m_maxTilt = 90f,
            m_terrainDeltaRadius = 0,
            m_maxTerrainDelta = 2f,
            m_minTerrainDelta = 0,
            m_snapToWater = false,
            m_groundOffset = 0,
            m_groupSizeMin = 1,
            m_groupSizeMax = 1,
            m_groupRadius = 200,
            m_inForest = false,
            m_forestTresholdMin = 0,
            m_forestTresholdMax = 1f,
            m_foldout = false
        };
        __instance.m_vegetation.Add(vegetation);
    }
}