﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using ItemManager;
using UnityEngine;
using PieceManager;
using ServerSync;
using HarmonyLib;
using TMPro;
using UnityEngine.UI;




namespace Bamboozled
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class Bamboozled : BaseUnityPlugin
    {
        private const string ModName = "Bamboozled";
        private const string ModVersion = "1.0.23";
        private const string ModGUID = "org.bepinex.plugins.bamboozled";



            private static readonly ConfigSync configSync = new(ModName) { DisplayName = ModName, CurrentVersion = ModVersion, MinimumRequiredVersion = ModVersion };

            private static ConfigEntry<Toggle> serverConfigLocked = null!;

            private ConfigEntry<T> config<T>(string group, string name, T value, ConfigDescription description, bool synchronizedSetting = true)
            {
                ConfigEntry<T> configEntry = Config.Bind(group, name, value, description);

                SyncedConfigEntry<T> syncedConfigEntry = configSync.AddConfigEntry(configEntry);
                syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

                return configEntry;
            }

            private ConfigEntry<T> config<T>(string group, string name, T value, string description, bool synchronizedSetting = true) => config(group, name, value, new ConfigDescription(description), synchronizedSetting);

            private enum Toggle
            {
                On = 1,
                Off = 0
            }
           
        public static GameObject OP_Bamboo_Tree_2_GameObject;

        public class Vegetation
        {
            [HarmonyPatch(typeof(ZoneSystem), nameof(ZoneSystem.Start))]
            static class ZoneSystemStartPatch
            {
                static void Prefix(ZoneSystem __instance)
                {
                    var OP_Bamboo_Tree_2_GameObject = ZNetScene.instance.GetPrefab("OP_Bamboo_Tree_2");
                    ZoneSystem.ZoneVegetation OP_Bamboo_Tree_2 = new()
                    {
                        m_name = OP_Bamboo_Tree_2_GameObject.name,
                        m_prefab = OP_Bamboo_Tree_2_GameObject,
                        m_enable = true,
                        m_min = 1f,
                        m_max = 6f,
                        m_forcePlacement = true,
                        m_scaleMin = 1f,
                        m_scaleMax = 1f,
                        m_randTilt = 0,
                        m_chanceToUseGroundTilt = 0,
                        m_biome = (Heightmap.Biome.Meadows),
                        m_biomeArea = Heightmap.BiomeArea.Everything,
                        m_blockCheck = true,
                        m_minAltitude = 0f,
                        m_maxAltitude = 1000f,
                        m_minOceanDepth = 0,
                        m_maxOceanDepth = 0,
                        m_minTilt = 0,
                        m_maxTilt = 20f,
                        m_terrainDeltaRadius = 0,
                        m_maxTerrainDelta = 2f,
                        m_minTerrainDelta = 0,
                        m_snapToWater = false,
                        m_groundOffset = 0,
                        m_groupRadius = 6f,
                        m_groupSizeMin = 1,
                        m_groupSizeMax = 6,
                        m_inForest = true,
                        m_forestTresholdMin = 0,
                        m_forestTresholdMax = 1f,
                        m_foldout = false
                    };
                    __instance.m_vegetation.Add(OP_Bamboo_Tree_2);
                }
            }
        }

        public void Awake()
            {

            serverConfigLocked = config("1 - General", "Lock Configuration", Toggle.On, "If on, the configuration is locked and can be changed by server admins only.");
            configSync.AddLockingConfigEntry(serverConfigLocked);

            GameObject OP_Bamboo_Tree_2 = ItemManager.PrefabManager.RegisterPrefab("bamboo", "OP_Bamboo_Tree_2");
            OP_Bamboo_Tree_2_GameObject = OP_Bamboo_Tree_2.gameObject;
            MaterialReplacer.RegisterGameObjectForShaderSwap(OP_Bamboo_Tree_2, MaterialReplacer.ShaderType.VegetationShader);

            BuildPiece OP_Bamboo_Sapling = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Sapling");
            OP_Bamboo_Sapling.Tool.Add("Cultivator");
            OP_Bamboo_Sapling.Name.English("Odins Bamboo Sapling");
            OP_Bamboo_Sapling.Description.English("A strange tree");
            OP_Bamboo_Sapling.RequiredItems.Add("Wood", 1, true); ;
            OP_Bamboo_Sapling.Category.Set(BuildPieceCategory.Misc);

            Item OP_Bamboo_Wood = new("bamboo", "OP_Bamboo_Wood");
            OP_Bamboo_Wood.Name.English("Bamboo Wood");
            OP_Bamboo_Wood.Description.English("A strange wood");
            OP_Bamboo_Wood.DropsFrom.Add("Greydwarf", 0.3f, 1, 2);

            Item OP_Bamboo_Hammer = new("bamboo", "OP_Bamboo_Hammer");  //assetbundle name, Asset Name
            OP_Bamboo_Hammer.Crafting.Add(ItemManager.CraftingTable.Workbench, 1);
            OP_Bamboo_Hammer.Name.English("Bamboo Hammer");
            OP_Bamboo_Hammer.Description.English("A bamboo hammer");
            OP_Bamboo_Hammer.RequiredItems.Add("OP_Bamboo_Wood", 1);
            OP_Bamboo_Hammer.CraftAmount = 1;

            BuildPiece OP_Bamboo_Build_Totem = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Build_Totem");
            OP_Bamboo_Build_Totem.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Build_Totem.Name.English("Bamboo Totem");
            OP_Bamboo_Build_Totem.Description.English("A bamboozled piece restriction");
            OP_Bamboo_Build_Totem.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
            OP_Bamboo_Build_Totem.Category.Set(BuildPieceCategory.Misc);

            //pieces


            BuildPiece OP_Bamboo_Pole = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Pole");
            OP_Bamboo_Pole.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Pole.Name.English("Bamboo Pole");
            OP_Bamboo_Pole.Description.English("A bamboozled piece");
            OP_Bamboo_Pole.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
            OP_Bamboo_Pole.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Pole_Light = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Pole_Light");
            OP_Bamboo_Pole_Light.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Pole_Light.Name.English("Bamboo Pole Light");
            OP_Bamboo_Pole_Light.Description.English("A bamboozled piece");
            OP_Bamboo_Pole_Light.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
            OP_Bamboo_Pole_Light.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Thick_Pole = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Thick_Pole");
            OP_Bamboo_Thick_Pole.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Thick_Pole.Name.English("Bamboo Thick Pole");
            OP_Bamboo_Thick_Pole.Description.English("A bamboozled piece");
            OP_Bamboo_Thick_Pole.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
            OP_Bamboo_Thick_Pole.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Thick_Pole_Light = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Thick_Pole_Light");
            OP_Bamboo_Thick_Pole_Light.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Thick_Pole_Light.Name.English("Bamboo Thick Pole Light");
            OP_Bamboo_Thick_Pole_Light.Description.English("A bamboozled piece");
            OP_Bamboo_Thick_Pole_Light.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
            OP_Bamboo_Thick_Pole_Light.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Small_Pole_Dark = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Small_Pole_Dark");
            OP_Bamboo_Small_Pole_Dark.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Small_Pole_Dark.Name.English("Bamboo Small Pole");
            OP_Bamboo_Small_Pole_Dark.Description.English("A Small Pole");
            OP_Bamboo_Small_Pole_Dark.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
            OP_Bamboo_Small_Pole_Dark.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Small_Pole_Light = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Small_Pole_Light");
            OP_Bamboo_Small_Pole_Light.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Small_Pole_Light.Name.English("Bamboo Small Pole Light");
            OP_Bamboo_Small_Pole_Light.Description.English("A Small Pole");
            OP_Bamboo_Small_Pole_Light.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
            OP_Bamboo_Small_Pole_Light.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Thin_Small_Pole_Dark = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Thin_Small_Pole_Dark");
            OP_Bamboo_Thin_Small_Pole_Dark.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Thin_Small_Pole_Dark.Name.English("Bamboo Thin Small Pole");
            OP_Bamboo_Thin_Small_Pole_Dark.Description.English("A Small Pole");
            OP_Bamboo_Thin_Small_Pole_Dark.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
            OP_Bamboo_Thin_Small_Pole_Dark.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Thin_Small_Pole_Light = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Thin_Small_Pole_Light");
            OP_Bamboo_Thin_Small_Pole_Light.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Thin_Small_Pole_Light.Name.English("Bamboo Thin Small Pole Light");
            OP_Bamboo_Thin_Small_Pole_Light.Description.English("A Small Pole");
            OP_Bamboo_Thin_Small_Pole_Light.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
            OP_Bamboo_Thin_Small_Pole_Light.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Beam = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Beam");
            OP_Bamboo_Beam.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Beam.Name.English("Bamboo Beam");
            OP_Bamboo_Beam.Description.English("A bamboozled piece");
            OP_Bamboo_Beam.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
            OP_Bamboo_Beam.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Beam_Light = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Beam_Light");
            OP_Bamboo_Beam_Light.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Beam_Light.Name.English("Bamboo Beam Light");
            OP_Bamboo_Beam_Light.Description.English("A bamboozled piece");
            OP_Bamboo_Beam_Light.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
            OP_Bamboo_Beam_Light.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Thick_Beam = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Thick_Beam");
            OP_Bamboo_Thick_Beam.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Thick_Beam.Name.English("Bamboo Thick Beam");
            OP_Bamboo_Thick_Beam.Description.English("A bamboozled piece");
            OP_Bamboo_Thick_Beam.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
            OP_Bamboo_Thick_Beam.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Thick_Beam_Light = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Thick_Beam_Light");
            OP_Bamboo_Thick_Beam_Light.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Thick_Beam_Light.Name.English("Bamboo Thick Beam Light");
            OP_Bamboo_Thick_Beam_Light.Description.English("A bamboozled piece");
            OP_Bamboo_Thick_Beam_Light.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
            OP_Bamboo_Thick_Beam_Light.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Angle_Beam_26 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Angle_Beam_26");
            OP_Bamboo_Angle_Beam_26.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Angle_Beam_26.Name.English("Bamboo Angle Beam 26");
            OP_Bamboo_Angle_Beam_26.Description.English("A bamboozled piece");
            OP_Bamboo_Angle_Beam_26.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
            OP_Bamboo_Angle_Beam_26.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Angle_Beam_45 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Angle_Beam_45");
            OP_Bamboo_Angle_Beam_45.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Angle_Beam_45.Name.English("Bamboo Angle Beam 45");
            OP_Bamboo_Angle_Beam_45.Description.English("A bamboozled piece");
            OP_Bamboo_Angle_Beam_45.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
            OP_Bamboo_Angle_Beam_45.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Thin_Angle_Beam_45 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Thin_Angle_Beam_45");
            OP_Bamboo_Thin_Angle_Beam_45.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Thin_Angle_Beam_45.Name.English("Bamboo Thin Angle Beam 45");
            OP_Bamboo_Thin_Angle_Beam_45.Description.English("A bamboozled piece");
            OP_Bamboo_Thin_Angle_Beam_45.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
            OP_Bamboo_Thin_Angle_Beam_45.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Thin_Angle_Light_Beam_45 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Thin_Angle_Light_Beam_45");
            OP_Bamboo_Thin_Angle_Light_Beam_45.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Thin_Angle_Light_Beam_45.Name.English("Bamboo Thin Angle Light Beam 45");
            OP_Bamboo_Thin_Angle_Light_Beam_45.Description.English("A bamboozled piece");
            OP_Bamboo_Thin_Angle_Light_Beam_45.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
            OP_Bamboo_Thin_Angle_Light_Beam_45.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Thin_Angle_Beam_26 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Thin_Angle_Beam_26");
            OP_Bamboo_Thin_Angle_Beam_26.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Thin_Angle_Beam_26.Name.English("Bamboo Thin Angle Beam 26");
            OP_Bamboo_Thin_Angle_Beam_26.Description.English("A bamboozled piece");
            OP_Bamboo_Thin_Angle_Beam_26.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
            OP_Bamboo_Thin_Angle_Beam_26.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Thin_Angle_Light_Beam_26 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Thin_Angle_Light_Beam_26");
            OP_Bamboo_Thin_Angle_Light_Beam_26.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Thin_Angle_Light_Beam_26.Name.English("Bamboo Thin Angle Light Beam 26");
            OP_Bamboo_Thin_Angle_Light_Beam_26.Description.English("A bamboozled piece");
            OP_Bamboo_Thin_Angle_Light_Beam_26.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
            OP_Bamboo_Thin_Angle_Light_Beam_26.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Short_Beam_Dark = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Short_Beam_Dark");
            OP_Bamboo_Short_Beam_Dark.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Short_Beam_Dark.Name.English("Bamboo Short Beam Dark");
            OP_Bamboo_Short_Beam_Dark.Description.English("A bamboozled piece");
            OP_Bamboo_Short_Beam_Dark.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
            OP_Bamboo_Short_Beam_Dark.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Short_Beam_Light = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Short_Beam_Light");
            OP_Bamboo_Short_Beam_Light.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Short_Beam_Light.Name.English("Bamboo Short Beam Light");
            OP_Bamboo_Short_Beam_Light.Description.English("A bamboozled piece");
            OP_Bamboo_Short_Beam_Light.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
            OP_Bamboo_Short_Beam_Light.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Short_Thin_Beam = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Short_Thin_Beam");
            OP_Bamboo_Short_Thin_Beam.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Short_Thin_Beam.Name.English("Bamboo Short Thin Beam");
            OP_Bamboo_Short_Thin_Beam.Description.English("A bamboozled piece");
            OP_Bamboo_Short_Thin_Beam.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
            OP_Bamboo_Short_Thin_Beam.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Short_Thin_Beam_Light = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Short_Thin_Beam_Light");
            OP_Bamboo_Short_Thin_Beam_Light.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Short_Thin_Beam_Light.Name.English("Bamboo Short Thin Beam Light");
            OP_Bamboo_Short_Thin_Beam_Light.Description.English("A bamboozled piece");
            OP_Bamboo_Short_Thin_Beam_Light.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
            OP_Bamboo_Short_Thin_Beam_Light.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Brush_Fence = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Brush_Fence");
            OP_Bamboo_Brush_Fence.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Brush_Fence.Name.English("Bamboo Brush Fence");
            OP_Bamboo_Brush_Fence.Description.English("A bamboozled piece");
            OP_Bamboo_Brush_Fence.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
            OP_Bamboo_Brush_Fence.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Brush_Fence_2 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Brush_Fence_2");
            OP_Bamboo_Brush_Fence_2.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Brush_Fence_2.Name.English("Bamboo Brush Fence 2");
            OP_Bamboo_Brush_Fence_2.Description.English("A bamboozled piece");
            OP_Bamboo_Brush_Fence_2.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
            OP_Bamboo_Brush_Fence_2.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Brush_Fence_Wide = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Brush_Fence_Wide");
            OP_Bamboo_Brush_Fence_Wide.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Brush_Fence_Wide.Name.English("Bamboo Brush Fence Wide");
            OP_Bamboo_Brush_Fence_Wide.Description.English("A bamboozled piece");
            OP_Bamboo_Brush_Fence_Wide.RequiredItems.Add("OP_Bamboo_Wood", 4, true);
            OP_Bamboo_Brush_Fence_Wide.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Brush_Fence_Wide_2 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Brush_Fence_Wide_2");
            OP_Bamboo_Brush_Fence_Wide_2.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Brush_Fence_Wide_2.Name.English("Bamboo Brush Fence Wide 2");
            OP_Bamboo_Brush_Fence_Wide_2.Description.English("A bamboozled piece");
            OP_Bamboo_Brush_Fence_Wide_2.RequiredItems.Add("OP_Bamboo_Wood", 4, true);
            OP_Bamboo_Brush_Fence_Wide_2.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Rope_Fence = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Rope_Fence");
            OP_Bamboo_Rope_Fence.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Rope_Fence.Name.English("Bamboo Rope Fence");
            OP_Bamboo_Rope_Fence.Description.English("A bamboozled rope fence");
            OP_Bamboo_Rope_Fence.RequiredItems.Add("OP_Bamboo_Wood", 4, true);
            OP_Bamboo_Rope_Fence.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Frame = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Frame");
            OP_Bamboo_Frame.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Frame.Name.English("Bamboo Frame");
            OP_Bamboo_Frame.Description.English("A bamboozled piece");
            OP_Bamboo_Frame.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
            OP_Bamboo_Frame.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Frame_2 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Frame_2");
            OP_Bamboo_Frame_2.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Frame_2.Name.English("Bamboo Frame 2");
            OP_Bamboo_Frame_2.Description.English("A bamboozled piece");
            OP_Bamboo_Frame_2.RequiredItems.Add("OP_Bamboo_Wood", 4, true);
            OP_Bamboo_Frame_2.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Halfwall = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Halfwall");
            OP_Bamboo_Halfwall.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Halfwall.Name.English("Bamboo Halfwall");
            OP_Bamboo_Halfwall.Description.English("A bamboozled piece");
            OP_Bamboo_Halfwall.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
            OP_Bamboo_Halfwall.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Wall = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Wall");
            OP_Bamboo_Wall.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Wall.Name.English("Bamboo Wall");
            OP_Bamboo_Wall.Description.English("A bamboozled piece");
            OP_Bamboo_Wall.RequiredItems.Add("OP_Bamboo_Wood", 4, true);
            OP_Bamboo_Wall.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Gate = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Gate");
            OP_Bamboo_Gate.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Gate.Name.English("Bamboo Gate");
            OP_Bamboo_Gate.Description.English("A bamboozled piece");
            OP_Bamboo_Gate.RequiredItems.Add("OP_Bamboo_Wood", 8, true);
            OP_Bamboo_Gate.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Floor = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Floor");
            OP_Bamboo_Floor.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Floor.Name.English("Bamboo Floor");
            OP_Bamboo_Floor.Description.English("A bamboozled piece");
            OP_Bamboo_Floor.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
            OP_Bamboo_Floor.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Stair = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Stair");
            OP_Bamboo_Stair.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Stair.Name.English("Bamboo Stair");
            OP_Bamboo_Stair.Description.English("A bamboozled piece");
            OP_Bamboo_Stair.RequiredItems.Add("OP_Bamboo_Wood", 8, true);
            OP_Bamboo_Stair.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Door = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Door");
            OP_Bamboo_Door.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Door.Name.English("Bamboo Door");
            OP_Bamboo_Door.Description.English("A bamboozled piece");
            OP_Bamboo_Door.RequiredItems.Add("OP_Bamboo_Wood", 4, true);
            OP_Bamboo_Door.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Roof_Tile = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Roof_Tile");
            OP_Bamboo_Roof_Tile.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Roof_Tile.Name.English("Bamboo Roof Tile");
            OP_Bamboo_Roof_Tile.Description.English("A bamboozled piece");
            OP_Bamboo_Roof_Tile.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
            OP_Bamboo_Roof_Tile.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Straw_Gate = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Straw_Gate");
            OP_Bamboo_Straw_Gate.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Straw_Gate.Name.English("Bamboo Straw Gate");
            OP_Bamboo_Straw_Gate.Description.English("A bamboozled piece");
            OP_Bamboo_Straw_Gate.RequiredItems.Add("OP_Bamboo_Wood", 4, true);
            OP_Bamboo_Straw_Gate.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Straw_Gate_2 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Straw_Gate_2");
            OP_Bamboo_Straw_Gate_2.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Straw_Gate_2.Name.English("Bamboo Straw Gate 2");
            OP_Bamboo_Straw_Gate_2.Description.English("A bamboozled piece");
            OP_Bamboo_Straw_Gate_2.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
            OP_Bamboo_Straw_Gate_2.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Wall_Top_26 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Wall_Top_26");
            OP_Bamboo_Wall_Top_26.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Wall_Top_26.Name.English("Bamboo Wall Top 26");
            OP_Bamboo_Wall_Top_26.Description.English("A bamboozled piece");
            OP_Bamboo_Wall_Top_26.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
            OP_Bamboo_Wall_Top_26.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Wall_Top_45 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Wall_Top_45");
            OP_Bamboo_Wall_Top_45.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Wall_Top_45.Name.English("Bamboo Wall Top 45");
            OP_Bamboo_Wall_Top_45.Description.English("A bamboozled piece");
            OP_Bamboo_Wall_Top_45.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
            OP_Bamboo_Wall_Top_45.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Wall_26 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Wall_26");
            OP_Bamboo_Wall_26.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Wall_26.Name.English("Bamboo Wall 26");
            OP_Bamboo_Wall_26.Description.English("A bamboozled piece");
            OP_Bamboo_Wall_26.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
            OP_Bamboo_Wall_26.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Wall_45 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Wall_45");
            OP_Bamboo_Wall_45.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Wall_45.Name.English("Bamboo Wall 45");
            OP_Bamboo_Wall_45.Description.English("A bamboozled piece");
            OP_Bamboo_Wall_45.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
            OP_Bamboo_Wall_45.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Wall_Roof_45_Down = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Wall_Roof_45_Down");
            OP_Bamboo_Wall_Roof_45_Down.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Wall_Roof_45_Down.Name.English("Bamboo Wall Roof 45 Down");
            OP_Bamboo_Wall_Roof_45_Down.Description.English("A bamboozled piece");
            OP_Bamboo_Wall_Roof_45_Down.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
            OP_Bamboo_Wall_Roof_45_Down.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Wall_Roof_Down = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Wall_Roof_Down");
            OP_Bamboo_Wall_Roof_Down.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Wall_Roof_Down.Name.English("Bamboo Wall Roof Down");
            OP_Bamboo_Wall_Roof_Down.Description.English("A bamboozled piece");
            OP_Bamboo_Wall_Roof_Down.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
            OP_Bamboo_Wall_Roof_Down.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Ladder = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Ladder");
            OP_Bamboo_Ladder.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Ladder.Name.English("Bamboo Ladder");
            OP_Bamboo_Ladder.Description.English("A ladder");
            OP_Bamboo_Ladder.RequiredItems.Add("OP_Bamboo_Wood", 4, true);
            OP_Bamboo_Ladder.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Ladder_3 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Ladder_3");
            OP_Bamboo_Ladder_3.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Ladder_3.Name.English("Bamboo Ladder 3");
            OP_Bamboo_Ladder_3.Description.English("A Large ladder");
            OP_Bamboo_Ladder_3.RequiredItems.Add("OP_Bamboo_Wood", 6, true);
            OP_Bamboo_Ladder_3.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Window = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Window");
            OP_Bamboo_Window.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Window.Name.English("Bamboo Window");
            OP_Bamboo_Window.Description.English("A bamboozled window");
            OP_Bamboo_Window.RequiredItems.Add("OP_Bamboo_Wood", 3, true);
            OP_Bamboo_Window.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Ladder_2 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Ladder_2");
            OP_Bamboo_Ladder_2.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Ladder_2.Name.English("Bamboo Ladder 2");
            OP_Bamboo_Ladder_2.Description.English("A bamboozled standing ladder");
            OP_Bamboo_Ladder_2.RequiredItems.Add("OP_Bamboo_Wood", 3, true);
            OP_Bamboo_Ladder_2.Category.Set(BuildPieceCategory.Misc);

           // BuildPiece OP_Bamboo_Sign = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Sign");
           // OP_Bamboo_Sign.Tool.Add("OP_Bamboo_Hammer");
           // OP_Bamboo_Sign.Name.English("Bamboo Sign");
           // OP_Bamboo_Sign.Description.English("A bamboozled sign");
           // OP_Bamboo_Sign.RequiredItems.Add("OP_Bamboo_Wood", 3, true);
           // OP_Bamboo_Sign.Category.Set(BuildPieceCategory.Misc);

          //  BuildPiece OP_Bamboo_Large_Sign = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Large_Sign");
          //  OP_Bamboo_Large_Sign.Tool.Add("OP_Bamboo_Hammer");
          //  OP_Bamboo_Large_Sign.Name.English("Bamboo Large Sign");
          //  OP_Bamboo_Large_Sign.Description.English("A bamboozled large sign");
          //  OP_Bamboo_Large_Sign.RequiredItems.Add("OP_Bamboo_Wood", 6, true);
          //  OP_Bamboo_Large_Sign.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Floor_2 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Floor_2");
            OP_Bamboo_Floor_2.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Floor_2.Name.English("Bamboo Floor 2");
            OP_Bamboo_Floor_2.Description.English("A bamboozled bridge floor");
            OP_Bamboo_Floor_2.RequiredItems.Add("OP_Bamboo_Wood", 3, true);
            OP_Bamboo_Floor_2.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Roof_Top_45 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Roof_Top_45");
            OP_Bamboo_Roof_Top_45.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Roof_Top_45.Name.English("Bamboo Roof Top 45");
            OP_Bamboo_Roof_Top_45.Description.English("A bamboozled piece");
            OP_Bamboo_Roof_Top_45.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
            OP_Bamboo_Roof_Top_45.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Roof_Top = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Roof_Top");
            OP_Bamboo_Roof_Top.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Roof_Top.Name.English("Bamboo Roof Top");
            OP_Bamboo_Roof_Top.Description.English("A bamboozled piece");
            OP_Bamboo_Roof_Top.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
            OP_Bamboo_Roof_Top.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Roof_Sloped = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Roof_Sloped");
            OP_Bamboo_Roof_Sloped.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Roof_Sloped.Name.English("Bamboo Roof Sloped");
            OP_Bamboo_Roof_Sloped.Description.English("A bamboozled piece");
            OP_Bamboo_Roof_Sloped.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
            OP_Bamboo_Roof_Sloped.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Roof_45 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Roof_45");
            OP_Bamboo_Roof_45.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Roof_45.Name.English("Bamboo Roof 45");
            OP_Bamboo_Roof_45.Description.English("A bamboozled piece");
            OP_Bamboo_Roof_45.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
            OP_Bamboo_Roof_45.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Roof_Inner_Corner_45 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Roof_Inner_Corner_45");
            OP_Bamboo_Roof_Inner_Corner_45.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Roof_Inner_Corner_45.Name.English("Bamboo Roof Inner Corner 45");
            OP_Bamboo_Roof_Inner_Corner_45.Description.English("A bamboozled piece");
            OP_Bamboo_Roof_Inner_Corner_45.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
            OP_Bamboo_Roof_Inner_Corner_45.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Roof_Inner_Corner = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Roof_Inner_Corner");
            OP_Bamboo_Roof_Inner_Corner.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Roof_Inner_Corner.Name.English("Bamboo Roof Inner Corner");
            OP_Bamboo_Roof_Inner_Corner.Description.English("A bamboozled piece");
            OP_Bamboo_Roof_Inner_Corner.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
            OP_Bamboo_Roof_Inner_Corner.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Roof_Corner_45 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Roof_Corner_45");
            OP_Bamboo_Roof_Corner_45.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Roof_Corner_45.Name.English("Bamboo Roof Corner 45");
            OP_Bamboo_Roof_Corner_45.Description.English("A bamboozled piece");
            OP_Bamboo_Roof_Corner_45.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
            OP_Bamboo_Roof_Corner_45.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Bamboo_Roof_Corner = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Roof_Corner");
            OP_Bamboo_Roof_Corner.Tool.Add("OP_Bamboo_Hammer");
            OP_Bamboo_Roof_Corner.Name.English("Bamboo Roof Corner");
            OP_Bamboo_Roof_Corner.Description.English("A bamboozled piece");
            OP_Bamboo_Roof_Corner.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
            OP_Bamboo_Roof_Corner.Category.Set(BuildPieceCategory.Misc);

            //leaves

            BuildPiece OP_Leaves_1 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Leaves_1");
            OP_Leaves_1.Tool.Add("OP_Bamboo_Hammer");
            OP_Leaves_1.Name.English("Bamboo Leaves 1");
            OP_Leaves_1.Description.English("A bamboozled piece");
            OP_Leaves_1.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
            OP_Leaves_1.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Leaves_2 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Leaves_2");
            OP_Leaves_2.Tool.Add("OP_Bamboo_Hammer");
            OP_Leaves_2.Name.English("Bamboo Leaves 2");
            OP_Leaves_2.Description.English("A bamboozled piece");
            OP_Leaves_2.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
            OP_Leaves_2.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Leaves_3 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Leaves_3");
            OP_Leaves_3.Tool.Add("OP_Bamboo_Hammer");
            OP_Leaves_3.Name.English("Bamboo Leaves 3");
            OP_Leaves_3.Description.English("A bamboozled piece");
            OP_Leaves_3.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
            OP_Leaves_3.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Leaves_4 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Leaves_4");
            OP_Leaves_4.Tool.Add("OP_Bamboo_Hammer");
            OP_Leaves_4.Name.English("Bamboo Leaves 4");
            OP_Leaves_4.Description.English("A bamboozled piece");
            OP_Leaves_4.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
            OP_Leaves_4.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Leaves_5 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Leaves_5");
            OP_Leaves_5.Tool.Add("OP_Bamboo_Hammer");
            OP_Leaves_5.Name.English("Bamboo Leaves 5");
            OP_Leaves_5.Description.English("A bamboozled piece");
            OP_Leaves_5.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
            OP_Leaves_5.Category.Set(BuildPieceCategory.Misc);

            BuildPiece OP_Leaves_6 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Leaves_6");
            OP_Leaves_6.Tool.Add("OP_Bamboo_Hammer");
            OP_Leaves_6.Name.English("Bamboo Leaves 6");
            OP_Leaves_6.Description.English("A bamboozled piece");
            OP_Leaves_6.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
            OP_Leaves_6.Category.Set(BuildPieceCategory.Misc);

            //stone 

            BuildPiece OP_Ruin_Pillar_1 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Ruin_Pillar_1");
            OP_Ruin_Pillar_1.Tool.Add("OP_Bamboo_Hammer");
            OP_Ruin_Pillar_1.Name.English("Ruin Pillar 1");
            OP_Ruin_Pillar_1.Description.English("A stone pillar");
            OP_Ruin_Pillar_1.RequiredItems.Add("Stone", 2, true);
            OP_Ruin_Pillar_1.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
            OP_Ruin_Pillar_1.Category.Set(BuildPieceCategory.Misc);


            BuildPiece OP_Ruin_Pillar_2 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Ruin_Pillar_2");
            OP_Ruin_Pillar_2.Tool.Add("OP_Bamboo_Hammer");
            OP_Ruin_Pillar_2.Name.English("Ruin Pillar 2");
            OP_Ruin_Pillar_2.Description.English("A stone pillar");
            OP_Ruin_Pillar_2.RequiredItems.Add("Stone", 4, true);
            OP_Ruin_Pillar_2.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
            OP_Ruin_Pillar_2.Category.Set(BuildPieceCategory.Misc);


            BuildPiece OP_Ruin_Pillar_3 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Ruin_Pillar_3");
            OP_Ruin_Pillar_3.Tool.Add("OP_Bamboo_Hammer");
            OP_Ruin_Pillar_3.Name.English("Ruin Pillar 3");
            OP_Ruin_Pillar_3.Description.English("A stone pillar");
            OP_Ruin_Pillar_3.RequiredItems.Add("Stone", 6, true);
            OP_Ruin_Pillar_3.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
            OP_Ruin_Pillar_3.Category.Set(BuildPieceCategory.Misc);


            BuildPiece OP_Rock_Formation_1 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Rock_Formation_1");
            OP_Rock_Formation_1.Tool.Add("OP_Bamboo_Hammer");
            OP_Rock_Formation_1.Name.English("Rock Formation 1");
            OP_Rock_Formation_1.Description.English("A stone rock formation");
            OP_Rock_Formation_1.RequiredItems.Add("Stone", 4, true);
            OP_Rock_Formation_1.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
            OP_Rock_Formation_1.Category.Set(BuildPieceCategory.Misc);


            BuildPiece OP_Rock_Formation_2 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Rock_Formation_2");
            OP_Rock_Formation_2.Tool.Add("OP_Bamboo_Hammer");
            OP_Rock_Formation_2.Name.English("Rock Formation 2");
            OP_Rock_Formation_2.Description.English("A stone rock formation");
            OP_Rock_Formation_2.RequiredItems.Add("Stone", 4, true);
            OP_Rock_Formation_2.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
            OP_Rock_Formation_2.Category.Set(BuildPieceCategory.Misc);


            BuildPiece OP_Rock_Formation_3 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Rock_Formation_3");
            OP_Rock_Formation_3.Tool.Add("OP_Bamboo_Hammer");
            OP_Rock_Formation_3.Name.English("Rock Formation 3");
            OP_Rock_Formation_3.Description.English("A stone rock formation");
            OP_Rock_Formation_3.RequiredItems.Add("Stone", 4, true);
            OP_Rock_Formation_3.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
            OP_Rock_Formation_3.Category.Set(BuildPieceCategory.Misc);


            BuildPiece OP_Rock_Formation_4 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Rock_Formation_4");
            OP_Rock_Formation_4.Tool.Add("OP_Bamboo_Hammer");
            OP_Rock_Formation_4.Name.English("Rock Formation 4");
            OP_Rock_Formation_4.Description.English("A stone rock formation");
            OP_Rock_Formation_4.RequiredItems.Add("Stone", 4, true);
            OP_Rock_Formation_4.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
            OP_Rock_Formation_4.Category.Set(BuildPieceCategory.Misc);


            BuildPiece OP_Stone_Path_1 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Stone_Path_1");
            OP_Stone_Path_1.Tool.Add("OP_Bamboo_Hammer");
            OP_Stone_Path_1.Name.English("Stone Path 1");
            OP_Stone_Path_1.Description.English("A path stone");
            OP_Stone_Path_1.RequiredItems.Add("Stone", 1, true);
            OP_Stone_Path_1.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
            OP_Stone_Path_1.Category.Set(BuildPieceCategory.Misc);


            BuildPiece OP_Stone_Path_2 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Stone_Path_2");
            OP_Stone_Path_2.Tool.Add("OP_Bamboo_Hammer");
            OP_Stone_Path_2.Name.English("Stone Path 2");
            OP_Stone_Path_2.Description.English("A path stone");
            OP_Stone_Path_2.RequiredItems.Add("Stone", 1, true);
            OP_Stone_Path_2.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
            OP_Stone_Path_2.Category.Set(BuildPieceCategory.Misc);


            BuildPiece OP_Stone_Path_3 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Stone_Path_3");
            OP_Stone_Path_3.Tool.Add("OP_Bamboo_Hammer");
            OP_Stone_Path_3.Name.English("Stone Path 3");
            OP_Stone_Path_3.Description.English("A path stone");
            OP_Stone_Path_3.RequiredItems.Add("Stone", 1, true);
            OP_Stone_Path_3.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
            OP_Stone_Path_3.Category.Set(BuildPieceCategory.Misc);


            BuildPiece OP_Stone_Path_4 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Stone_Path_4");
            OP_Stone_Path_4.Tool.Add("OP_Bamboo_Hammer");
            OP_Stone_Path_4.Name.English("Stone Path 4");
            OP_Stone_Path_4.Description.English("A path stone");
            OP_Stone_Path_4.RequiredItems.Add("Stone", 1, true);
            OP_Stone_Path_4.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
            OP_Stone_Path_4.Category.Set(BuildPieceCategory.Misc);

            //bamboo forest walls

            BuildPiece OP_Wild_Bamboo_Wall = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Wild_Bamboo_Wall");
            OP_Wild_Bamboo_Wall.Tool.Add("OP_Bamboo_Hammer");
            OP_Wild_Bamboo_Wall.Name.English("Wild Bamboo Wall");
            OP_Wild_Bamboo_Wall.Description.English("A bamboozled piece");
            OP_Wild_Bamboo_Wall.RequiredItems.Add("OP_Bamboo_Wood", 12, true);
            OP_Wild_Bamboo_Wall.Category.Set(BuildPieceCategory.Misc);


            BuildPiece OP_Wild_Bamboo = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Wild_Bamboo");
            OP_Wild_Bamboo.Tool.Add("OP_Bamboo_Hammer");
            OP_Wild_Bamboo.Name.English("Wild Bamboo");
            OP_Wild_Bamboo.Description.English("A bamboozled piece");
            OP_Wild_Bamboo.RequiredItems.Add("OP_Bamboo_Wood", 4, true);
            OP_Wild_Bamboo.Category.Set(BuildPieceCategory.Misc);


            BuildPiece OP_Wild_Bamboo_Light = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Wild_Bamboo_Light");
            OP_Wild_Bamboo_Light.Tool.Add("OP_Bamboo_Hammer");
            OP_Wild_Bamboo_Light.Name.English("Wild Bamboo Light");
            OP_Wild_Bamboo_Light.Description.English("A bamboozled piece");
            OP_Wild_Bamboo_Light.RequiredItems.Add("OP_Bamboo_Wood", 4, true);
            OP_Wild_Bamboo_Light.Category.Set(BuildPieceCategory.Misc);


            BuildPiece OP_Wild_Bamboo_Dark = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Wild_Bamboo_Dark");
            OP_Wild_Bamboo_Dark.Tool.Add("OP_Bamboo_Hammer");
            OP_Wild_Bamboo_Dark.Name.English("Wild Bamboo Dark");
            OP_Wild_Bamboo_Dark.Description.English("A bamboozled piece");
            OP_Wild_Bamboo_Dark.RequiredItems.Add("OP_Bamboo_Wood", 4, true);
            OP_Wild_Bamboo_Dark.Category.Set(BuildPieceCategory.Misc);


            BuildPiece OP_Wild_Bamboo_Cluster = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Wild_Bamboo_Cluster");
            OP_Wild_Bamboo_Cluster.Tool.Add("OP_Bamboo_Hammer");
            OP_Wild_Bamboo_Cluster.Name.English("Wild Bamboo Cluster");
            OP_Wild_Bamboo_Cluster.Description.English("A bamboozled piece");
            OP_Wild_Bamboo_Cluster.RequiredItems.Add("OP_Bamboo_Wood", 8, true);
            OP_Wild_Bamboo_Cluster.Category.Set(BuildPieceCategory.Misc);


            BuildPiece OP_Wild_Bamboo_Cluster_Dark = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Wild_Bamboo_Cluster_Dark");
            OP_Wild_Bamboo_Cluster_Dark.Tool.Add("OP_Bamboo_Hammer");
            OP_Wild_Bamboo_Cluster_Dark.Name.English("Wild Bamboo Cluster Dark");
            OP_Wild_Bamboo_Cluster_Dark.Description.English("A bamboozled piece");
            OP_Wild_Bamboo_Cluster_Dark.RequiredItems.Add("OP_Bamboo_Wood", 8, true);
            OP_Wild_Bamboo_Cluster_Dark.Category.Set(BuildPieceCategory.Misc);


            BuildPiece OP_Wild_Bamboo_Wall_Door = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Wild_Bamboo_Wall_Door");
            OP_Wild_Bamboo_Wall_Door.Tool.Add("OP_Bamboo_Hammer");
            OP_Wild_Bamboo_Wall_Door.Name.English("Wild Bamboo Wall Door");
            OP_Wild_Bamboo_Wall_Door.Description.English("A bamboozled piece");
            OP_Wild_Bamboo_Wall_Door.RequiredItems.Add("OP_Bamboo_Wood", 10, true);
            OP_Wild_Bamboo_Wall_Door.Category.Set(BuildPieceCategory.Misc);





            //trees
            GameObject OP_Bamboo_Log = ItemManager.PrefabManager.RegisterPrefab("bamboo", "OP_Bamboo_Log");

            GameObject OP_Bamboo_Log_Half = ItemManager.PrefabManager.RegisterPrefab("bamboo", "OP_Bamboo_Log_Half");

            GameObject OP_Bamboo_Stump = ItemManager.PrefabManager.RegisterPrefab("bamboo", "OP_Bamboo_Stump");

            //GameObject OP_Bamboo_Tree_1 = ItemManager.PrefabManager.RegisterPrefab("bamboo", "OP_Bamboo_Tree_1");




            Assembly assembly = Assembly.GetExecutingAssembly();
            Harmony harmony = new(ModGUID);
            harmony.PatchAll(assembly);

        }
    }
}