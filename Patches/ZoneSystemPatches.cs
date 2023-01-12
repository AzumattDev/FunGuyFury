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
            m_max = 0.65f,
            m_forcePlacement = true,
            m_scaleMin = 1f,
            m_scaleMax = 1.75f,
            m_chanceToUseGroundTilt = 0,
            m_biome = Heightmap.Biome.Mistlands | Heightmap.Biome.Swamp,
            m_biomeArea = Heightmap.BiomeArea.Everything,
            m_blockCheck = true,
            m_minAltitude = 0.01f,
            m_maxAltitude = 1000f,
            m_groupSizeMin = 1,
            m_groupSizeMax = 3,
            m_groupRadius = 32,
            m_inForest = false,
            m_forestTresholdMin = 0,
            m_forestTresholdMax = 1f,
            m_foldout = false
        };

        __instance.m_vegetation.Add(vegetation);
    }
}

[HarmonyPatch(typeof(ZoneSystem), nameof(ZoneSystem.ValidateVegetation))]
static class ZoneSystemValidatePatch
{
    static void Prefix(ZoneSystem __instance)
    {
        var mushPickable = ZNetScene.instance.GetPrefab("Pickable_FlyAgaricMushroom");
        ZoneSystem.ZoneVegetation vegetation = new()
        {
            m_name = mushPickable.name,
            m_prefab = mushPickable,
            m_enable = true,
            m_max = 0.65f,
            m_forcePlacement = true,
            m_scaleMin = 1f,
            m_scaleMax = 1.75f,
            m_chanceToUseGroundTilt = 0,
            m_biome = Heightmap.Biome.Mistlands | Heightmap.Biome.Swamp,
            m_biomeArea = Heightmap.BiomeArea.Everything,
            m_blockCheck = true,
            m_minAltitude = 0.01f,
            m_maxAltitude = 1000f,
            m_groupSizeMin = 1,
            m_groupSizeMax = 3,
            m_groupRadius = 32,
            m_inForest = false,
            m_forestTresholdMin = 0,
            m_forestTresholdMax = 1f,
            m_foldout = false
        };

        __instance.m_vegetation.Add(vegetation);
    }
}