using System;
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



namespace Bamboozled
{
	[BepInPlugin(ModGUID, ModName, ModVersion)]
	public class Bamboozled : BaseUnityPlugin
	{
		private const string ModName = "Bamboozled";
		private const string ModVersion = "1.0.12";
		private const string ModGUID = "org.bepinex.plugins.bamboozled";

        private static readonly ConfigSync configSync = new(ModName) { DisplayName = ModName, CurrentVersion = ModVersion, MinimumRequiredVersion = ModVersion };

        public static GameObject OP_Bamboo_Sapling;

		[HarmonyPatch(typeof(ZoneSystem), nameof(ZoneSystem.ValidateVegetation))]
		public class AddDestructiblesToZoneSystem
		{
			public static void Prefix(ZoneSystem __instance)
			{
				__instance.m_vegetation.Add(new ZoneSystem.ZoneVegetation
				{
					m_biome = Heightmap.Biome.Meadows,
					m_groupRadius = 6f,
					m_groupSizeMin = 2,
					m_groupSizeMax = 6,
					m_minAltitude = 0,
					m_maxAltitude =15,
					m_forcePlacement = true,
					m_max = 2,
					m_prefab = OP_Bamboo_Sapling
				});
			}
            private void Awake()
			{


                BuildPiece OP_Bamboo_Sapling = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Sapling", true, "Cultivator");
				OP_Bamboo_Sapling.Name.English("Odins Bamboo Sapling");
				OP_Bamboo_Sapling.Description.English("A strange tree");
				OP_Bamboo_Sapling.RequiredItems.Add("Wood", 1, true); ;
				OP_Bamboo_Sapling.Category.Add(BuildPieceCategory.Misc);

				Item OP_Bamboo_Wood = new("bamboo", "OP_Bamboo_Wood");
				OP_Bamboo_Wood.Crafting.Add(ItemManager.CraftingTable.Workbench, 10);
				OP_Bamboo_Wood.RequiredItems.Add("SwordCheat", 1);
				OP_Bamboo_Wood.CraftAmount = 1;
				OP_Bamboo_Wood.DropsFrom.Add("Greydwarf", 0.3f, 1, 2);


				Item OP_Bamboo_Hammer = new("bamboo", "OP_Bamboo_Hammer");  //assetbundle name, Asset Name
				OP_Bamboo_Hammer.Crafting.Add(ItemManager.CraftingTable.Workbench, 1);
				OP_Bamboo_Hammer.RequiredItems.Add("OP_Bamboo_Wood", 1);
				OP_Bamboo_Hammer.CraftAmount = 1;




				//pieces


				BuildPiece OP_Bamboo_Pole = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Pole", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Pole.Name.English("OP_Bamboo_Pole");
				OP_Bamboo_Pole.Description.English("A bamboozled piece");
				OP_Bamboo_Pole.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
				OP_Bamboo_Pole.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Pole_Light = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Pole_Light", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Pole_Light.Name.English("OP_Bamboo_Pole_Light");
				OP_Bamboo_Pole_Light.Description.English("A bamboozled piece");
				OP_Bamboo_Pole_Light.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
				OP_Bamboo_Pole_Light.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Thick_Pole = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Thick_Pole", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Thick_Pole.Name.English("OP_Bamboo_Thick_Pole");
				OP_Bamboo_Thick_Pole.Description.English("A bamboozled piece");
				OP_Bamboo_Thick_Pole.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
				OP_Bamboo_Thick_Pole.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Thick_Pole_Light = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Thick_Pole_Light", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Thick_Pole_Light.Name.English("OP_Bamboo_Thick_Pole_Light");
				OP_Bamboo_Thick_Pole_Light.Description.English("A bamboozled piece");
				OP_Bamboo_Thick_Pole_Light.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
				OP_Bamboo_Thick_Pole_Light.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Small_Pole_Dark = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Small_Pole_Dark", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Small_Pole_Dark.Name.English("OP_Bamboo_Small_Pole_Dark");
				OP_Bamboo_Small_Pole_Dark.Description.English("A Small Pole");
				OP_Bamboo_Small_Pole_Dark.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
				OP_Bamboo_Small_Pole_Dark.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Small_Pole_Light = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Small_Pole_Light", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Small_Pole_Light.Name.English("OP_Bamboo_Small_Pole_Light");
				OP_Bamboo_Small_Pole_Light.Description.English("A Small Pole");
				OP_Bamboo_Small_Pole_Light.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
				OP_Bamboo_Small_Pole_Light.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Thin_Small_Pole_Dark = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Thin_Small_Pole_Dark", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Thin_Small_Pole_Dark.Name.English("OP_Bamboo_Thin_Small_Pole_Dark");
				OP_Bamboo_Thin_Small_Pole_Dark.Description.English("A Small Pole");
				OP_Bamboo_Thin_Small_Pole_Dark.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
				OP_Bamboo_Thin_Small_Pole_Dark.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Thin_Small_Pole_Light = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Thin_Small_Pole_Light", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Thin_Small_Pole_Light.Name.English("OP_Bamboo_Thin_Small_Pole_Light");
				OP_Bamboo_Thin_Small_Pole_Light.Description.English("A Small Pole");
				OP_Bamboo_Thin_Small_Pole_Light.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
				OP_Bamboo_Thin_Small_Pole_Light.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Beam = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Beam", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Beam.Name.English("OP_Bamboo_Beam");
				OP_Bamboo_Beam.Description.English("A bamboozled piece");
				OP_Bamboo_Beam.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
				OP_Bamboo_Beam.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Beam_Light = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Beam_Light", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Beam_Light.Name.English("OP_Bamboo_Beam_Light");
				OP_Bamboo_Beam_Light.Description.English("A bamboozled piece");
				OP_Bamboo_Beam_Light.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
				OP_Bamboo_Beam_Light.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Thick_Beam = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Thick_Beam", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Thick_Beam.Name.English("OP_Bamboo_Thick_Beam");
				OP_Bamboo_Thick_Beam.Description.English("A bamboozled piece");
				OP_Bamboo_Thick_Beam.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
				OP_Bamboo_Thick_Beam.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Thick_Beam_Light = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Thick_Beam_Light", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Thick_Beam_Light.Name.English("OP_Bamboo_Thick_Beam_Light");
				OP_Bamboo_Thick_Beam_Light.Description.English("A bamboozled piece");
				OP_Bamboo_Thick_Beam_Light.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
				OP_Bamboo_Thick_Beam_Light.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Angle_Beam_26 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Angle_Beam_26", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Angle_Beam_26.Name.English("OP_Bamboo_Angle_Beam_26");
				OP_Bamboo_Angle_Beam_26.Description.English("A bamboozled piece");
				OP_Bamboo_Angle_Beam_26.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
				OP_Bamboo_Angle_Beam_26.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Angle_Beam_45 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Angle_Beam_45", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Angle_Beam_45.Name.English("OP_Bamboo_Angle_Beam_45");
				OP_Bamboo_Angle_Beam_45.Description.English("A bamboozled piece");
				OP_Bamboo_Angle_Beam_45.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
				OP_Bamboo_Angle_Beam_45.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Thin_Angle_Beam_45 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Thin_Angle_Beam_45", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Thin_Angle_Beam_45.Name.English("OP_Bamboo_Thin_Angle_Beam_45");
				OP_Bamboo_Thin_Angle_Beam_45.Description.English("A bamboozled piece");
				OP_Bamboo_Thin_Angle_Beam_45.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
				OP_Bamboo_Thin_Angle_Beam_45.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Thin_Angle_Light_Beam_45 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Thin_Angle_Light_Beam_45", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Thin_Angle_Light_Beam_45.Name.English("OP_Bamboo_Thin_Angle_Light_Beam_45");
				OP_Bamboo_Thin_Angle_Light_Beam_45.Description.English("A bamboozled piece");
				OP_Bamboo_Thin_Angle_Light_Beam_45.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
				OP_Bamboo_Thin_Angle_Light_Beam_45.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Thin_Angle_Beam_26 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Thin_Angle_Beam_26", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Thin_Angle_Beam_26.Name.English("OP_Bamboo_Thin_Angle_Beam_26");
				OP_Bamboo_Thin_Angle_Beam_26.Description.English("A bamboozled piece");
				OP_Bamboo_Thin_Angle_Beam_26.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
				OP_Bamboo_Thin_Angle_Beam_26.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Thin_Angle_Light_Beam_26 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Thin_Angle_Light_Beam_26", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Thin_Angle_Light_Beam_26.Name.English("OP_Bamboo_Thin_Angle_Light_Beam_26");
				OP_Bamboo_Thin_Angle_Light_Beam_26.Description.English("A bamboozled piece");
				OP_Bamboo_Thin_Angle_Light_Beam_26.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
				OP_Bamboo_Thin_Angle_Light_Beam_26.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Short_Beam_Dark = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Short_Beam_Dark", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Short_Beam_Dark.Name.English("OP_Bamboo_Short_Beam_Dark");
				OP_Bamboo_Short_Beam_Dark.Description.English("A bamboozled piece");
				OP_Bamboo_Short_Beam_Dark.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
				OP_Bamboo_Short_Beam_Dark.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Short_Beam_Light = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Short_Beam_Light", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Short_Beam_Light.Name.English("OP_Bamboo_Short_Beam_Light");
				OP_Bamboo_Short_Beam_Light.Description.English("A bamboozled piece");
				OP_Bamboo_Short_Beam_Light.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
				OP_Bamboo_Short_Beam_Light.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Short_Thin_Beam = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Short_Thin_Beam", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Short_Thin_Beam.Name.English("OP_Bamboo_Short_Thin_Beam");
				OP_Bamboo_Short_Thin_Beam.Description.English("A bamboozled piece");
				OP_Bamboo_Short_Thin_Beam.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
				OP_Bamboo_Short_Thin_Beam.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Short_Thin_Beam_Light = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Short_Thin_Beam_Light", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Short_Thin_Beam_Light.Name.English("OP_Bamboo_Short_Thin_Beam_Light");
				OP_Bamboo_Short_Thin_Beam_Light.Description.English("A bamboozled piece");
				OP_Bamboo_Short_Thin_Beam_Light.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
				OP_Bamboo_Short_Thin_Beam_Light.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Brush_Fence = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Brush_Fence", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Brush_Fence.Name.English("OP_Bamboo_Brush_Fence");
				OP_Bamboo_Brush_Fence.Description.English("A bamboozled piece");
				OP_Bamboo_Brush_Fence.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
				OP_Bamboo_Brush_Fence.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Brush_Fence_2 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Brush_Fence_2", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Brush_Fence_2.Name.English("OP_Bamboo_Brush_Fence_2");
				OP_Bamboo_Brush_Fence_2.Description.English("A bamboozled piece");
				OP_Bamboo_Brush_Fence_2.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
				OP_Bamboo_Brush_Fence_2.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Brush_Fence_Wide = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Brush_Fence_Wide", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Brush_Fence_Wide.Name.English("OP_Bamboo_Brush_Fence_Wide");
				OP_Bamboo_Brush_Fence_Wide.Description.English("A bamboozled piece");
				OP_Bamboo_Brush_Fence_Wide.RequiredItems.Add("OP_Bamboo_Wood", 4, true);
				OP_Bamboo_Brush_Fence_Wide.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Brush_Fence_Wide_2 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Brush_Fence_Wide_2", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Brush_Fence_Wide_2.Name.English("OP_Bamboo_Brush_Fence_Wide_2");
				OP_Bamboo_Brush_Fence_Wide_2.Description.English("A bamboozled piece");
				OP_Bamboo_Brush_Fence_Wide_2.RequiredItems.Add("OP_Bamboo_Wood", 4, true);
				OP_Bamboo_Brush_Fence_Wide_2.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Rope_Fence = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Rope_Fence", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Rope_Fence.Name.English("OP_Bamboo_Rope_Fence");
				OP_Bamboo_Rope_Fence.Description.English("A bamboozled rope fence");
				OP_Bamboo_Rope_Fence.RequiredItems.Add("OP_Bamboo_Wood", 4, true);
				OP_Bamboo_Rope_Fence.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Frame = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Frame", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Frame.Name.English("OP_Bamboo_Frame");
				OP_Bamboo_Frame.Description.English("A bamboozled piece");
				OP_Bamboo_Frame.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
				OP_Bamboo_Frame.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Frame_2 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Frame_2", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Frame_2.Name.English("OP_Bamboo_Frame_2");
				OP_Bamboo_Frame_2.Description.English("A bamboozled piece");
				OP_Bamboo_Frame_2.RequiredItems.Add("OP_Bamboo_Wood", 4, true);
				OP_Bamboo_Frame_2.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Halfwall = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Halfwall", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Halfwall.Name.English("OP_Bamboo_Halfwall");
				OP_Bamboo_Halfwall.Description.English("A bamboozled piece");
				OP_Bamboo_Halfwall.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
				OP_Bamboo_Halfwall.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Wall = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Wall", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Wall.Name.English("OP_Bamboo_Wall");
				OP_Bamboo_Wall.Description.English("A bamboozled piece");
				OP_Bamboo_Wall.RequiredItems.Add("OP_Bamboo_Wood", 4, true);
				OP_Bamboo_Wall.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Gate = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Gate", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Gate.Name.English("OP_Bamboo_Gate");
				OP_Bamboo_Gate.Description.English("A bamboozled piece");
				OP_Bamboo_Gate.RequiredItems.Add("OP_Bamboo_Wood", 8, true);
				OP_Bamboo_Gate.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Floor = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Floor", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Floor.Name.English("OP_Bamboo_Floor");
				OP_Bamboo_Floor.Description.English("A bamboozled piece");
				OP_Bamboo_Floor.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
				OP_Bamboo_Floor.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Stair = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Stair", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Stair.Name.English("OP_Bamboo_Stair");
				OP_Bamboo_Stair.Description.English("A bamboozled piece");
				OP_Bamboo_Stair.RequiredItems.Add("OP_Bamboo_Wood", 8, true);
				OP_Bamboo_Stair.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Door = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Door", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Door.Name.English("OP_Bamboo_Door");
				OP_Bamboo_Door.Description.English("A bamboozled piece");
				OP_Bamboo_Door.RequiredItems.Add("OP_Bamboo_Wood", 4, true);
				OP_Bamboo_Door.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Roof_Tile = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Roof_Tile", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Roof_Tile.Name.English("OP_Bamboo_Roof_Tile");
				OP_Bamboo_Roof_Tile.Description.English("A bamboozled piece");
				OP_Bamboo_Roof_Tile.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
				OP_Bamboo_Roof_Tile.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Straw_Gate = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Straw_Gate", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Straw_Gate.Name.English("OP_Bamboo_Straw_Gate");
				OP_Bamboo_Straw_Gate.Description.English("A bamboozled piece");
				OP_Bamboo_Straw_Gate.RequiredItems.Add("OP_Bamboo_Wood", 4, true);
				OP_Bamboo_Straw_Gate.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Straw_Gate_2 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Straw_Gate_2", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Straw_Gate_2.Name.English("OP_Bamboo_Straw_Gate_2");
				OP_Bamboo_Straw_Gate_2.Description.English("A bamboozled piece");
				OP_Bamboo_Straw_Gate_2.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
				OP_Bamboo_Straw_Gate_2.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Wall_Top_26 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Wall_Top_26", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Wall_Top_26.Name.English("OP_Bamboo_Wall_Top_26");
				OP_Bamboo_Wall_Top_26.Description.English("A bamboozled piece");
				OP_Bamboo_Wall_Top_26.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
				OP_Bamboo_Wall_Top_26.Category.Add(BuildPieceCategory.Misc);

                BuildPiece OP_Bamboo_Wall_Top_45 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Wall_Top_45", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Wall_Top_45.Name.English("OP_Bamboo_Wall_Top_45");
				OP_Bamboo_Wall_Top_45.Description.English("A bamboozled piece");
				OP_Bamboo_Wall_Top_45.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
				OP_Bamboo_Wall_Top_45.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Wall_26 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Wall_26", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Wall_26.Name.English("OP_Bamboo_Wall_26");
				OP_Bamboo_Wall_26.Description.English("A bamboozled piece");
				OP_Bamboo_Wall_26.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
				OP_Bamboo_Wall_26.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Wall_45 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Wall_45", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Wall_45.Name.English("OP_Bamboo_Wall_45");
				OP_Bamboo_Wall_45.Description.English("A bamboozled piece");
				OP_Bamboo_Wall_45.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
				OP_Bamboo_Wall_45.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Wall_Roof_45_Down = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Wall_Roof_45_Down", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Wall_Roof_45_Down.Name.English("OP_Bamboo_Wall_Roof_45_Down");
				OP_Bamboo_Wall_Roof_45_Down.Description.English("A bamboozled piece");
				OP_Bamboo_Wall_Roof_45_Down.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
				OP_Bamboo_Wall_Roof_45_Down.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Wall_Roof_Down = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Wall_Roof_Down", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Wall_Roof_Down.Name.English("OP_Bamboo_Wall_Roof_Down");
				OP_Bamboo_Wall_Roof_Down.Description.English("A bamboozled piece");
				OP_Bamboo_Wall_Roof_Down.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
				OP_Bamboo_Wall_Roof_Down.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Ladder = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Ladder", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Ladder.Name.English("OP_Bamboo_Ladder");
				OP_Bamboo_Ladder.Description.English("A ladder");
				OP_Bamboo_Ladder.RequiredItems.Add("OP_Bamboo_Wood", 4, true);
				OP_Bamboo_Ladder.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Ladder_3 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Ladder_3", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Ladder_3.Name.English("OP_Bamboo_Ladder_3");
				OP_Bamboo_Ladder_3.Description.English("A Large ladder");
				OP_Bamboo_Ladder_3.RequiredItems.Add("OP_Bamboo_Wood", 6, true);
				OP_Bamboo_Ladder_3.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Window = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Window", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Window.Name.English("OP_Bamboo_Window");
				OP_Bamboo_Window.Description.English("A bamboozled window");
				OP_Bamboo_Window.RequiredItems.Add("OP_Bamboo_Wood", 3, true);
				OP_Bamboo_Window.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Ladder_2 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Ladder_2", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Ladder_2.Name.English("OP_Bamboo_Ladder_2");
				OP_Bamboo_Ladder_2.Description.English("A bamboozled standing ladder");
				OP_Bamboo_Ladder_2.RequiredItems.Add("OP_Bamboo_Wood", 3, true);
				OP_Bamboo_Ladder_2.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Sign = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Sign", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Sign.Name.English("OP_Bamboo_Sign");
				OP_Bamboo_Sign.Description.English("A bamboozled sign");
				OP_Bamboo_Sign.RequiredItems.Add("OP_Bamboo_Wood", 3, true);
				OP_Bamboo_Sign.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Large_Sign = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Large_Sign", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Large_Sign.Name.English("OP_Bamboo_Large_Sign");
				OP_Bamboo_Large_Sign.Description.English("A bamboozled large sign");
				OP_Bamboo_Large_Sign.RequiredItems.Add("OP_Bamboo_Wood", 6, true);
				OP_Bamboo_Large_Sign.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Floor_2 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Floor_2", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Floor_2.Name.English("OP_Bamboo_Floor_2");
				OP_Bamboo_Floor_2.Description.English("A bamboozled bridge floor");
				OP_Bamboo_Floor_2.RequiredItems.Add("OP_Bamboo_Wood", 3, true);
				OP_Bamboo_Floor_2.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Roof_Top_45 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Roof_Top_45", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Roof_Top_45.Name.English("OP_Bamboo_Roof_Top_45");
				OP_Bamboo_Roof_Top_45.Description.English("A bamboozled piece");
				OP_Bamboo_Roof_Top_45.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
				OP_Bamboo_Roof_Top_45.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Roof_Top = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Roof_Top", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Roof_Top.Name.English("OP_Bamboo_Roof_Top");
				OP_Bamboo_Roof_Top.Description.English("A bamboozled piece");
				OP_Bamboo_Roof_Top.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
				OP_Bamboo_Roof_Top.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Roof_Sloped = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Roof_Sloped", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Roof_Sloped.Name.English("OP_Bamboo_Roof_Sloped");
				OP_Bamboo_Roof_Sloped.Description.English("A bamboozled piece");
				OP_Bamboo_Roof_Sloped.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
				OP_Bamboo_Roof_Sloped.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Roof_45 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Roof_45", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Roof_45.Name.English("OP_Bamboo_Roof_45");
				OP_Bamboo_Roof_45.Description.English("A bamboozled piece");
				OP_Bamboo_Roof_45.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
				OP_Bamboo_Roof_45.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Roof_Inner_Corner_45 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Roof_Inner_Corner_45", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Roof_Inner_Corner_45.Name.English("OP_Bamboo_Roof_Inner_Corner_45");
				OP_Bamboo_Roof_Inner_Corner_45.Description.English("A bamboozled piece");
				OP_Bamboo_Roof_Inner_Corner_45.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
				OP_Bamboo_Roof_Inner_Corner_45.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Roof_Inner_Corner = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Roof_Inner_Corner", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Roof_Inner_Corner.Name.English("OP_Bamboo_Roof_Inner_Corner");
				OP_Bamboo_Roof_Inner_Corner.Description.English("A bamboozled piece");
				OP_Bamboo_Roof_Inner_Corner.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
				OP_Bamboo_Roof_Inner_Corner.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Roof_Corner_45 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Roof_Corner_45", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Roof_Corner_45.Name.English("OP_Bamboo_Roof_Corner_45");
				OP_Bamboo_Roof_Corner_45.Description.English("A bamboozled piece");
				OP_Bamboo_Roof_Corner_45.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
				OP_Bamboo_Roof_Corner_45.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Bamboo_Roof_Corner = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Bamboo_Roof_Corner", true, "OP_Bamboo_Hammer");
				OP_Bamboo_Roof_Corner.Name.English("OP_Bamboo_Roof_Corner");
				OP_Bamboo_Roof_Corner.Description.English("A bamboozled piece");
				OP_Bamboo_Roof_Corner.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
				OP_Bamboo_Roof_Corner.Category.Add(BuildPieceCategory.Misc);

				//leaves

				BuildPiece OP_Leaves_1 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Leaves_1", true, "OP_Bamboo_Hammer");
				OP_Leaves_1.Name.English("OP_Leaves_2");
				OP_Leaves_1.Description.English("A bamboozled piece");
				OP_Leaves_1.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
				OP_Leaves_1.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Leaves_2 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Leaves_2", true, "OP_Bamboo_Hammer");
				OP_Leaves_2.Name.English("OP_Leaves_2");
				OP_Leaves_2.Description.English("A bamboozled piece");
				OP_Leaves_2.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
				OP_Leaves_2.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Leaves_3 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Leaves_3", true, "OP_Bamboo_Hammer");
				OP_Leaves_3.Name.English("OP_Leaves_3");
				OP_Leaves_3.Description.English("A bamboozled piece");
				OP_Leaves_3.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
				OP_Leaves_3.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Leaves_4 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Leaves_4", true, "OP_Bamboo_Hammer");
				OP_Leaves_4.Name.English("OP_Leaves_4");
				OP_Leaves_4.Description.English("A bamboozled piece");
				OP_Leaves_4.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
				OP_Leaves_4.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Leaves_5 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Leaves_5", true, "OP_Bamboo_Hammer");
				OP_Leaves_5.Name.English("OP_Leaves_5");
				OP_Leaves_5.Description.English("A bamboozled piece");
				OP_Leaves_5.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
				OP_Leaves_5.Category.Add(BuildPieceCategory.Misc);

				BuildPiece OP_Leaves_6 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Leaves_6", true, "OP_Bamboo_Hammer");
				OP_Leaves_6.Name.English("OP_Leaves_6");
				OP_Leaves_6.Description.English("A bamboozled piece");
				OP_Leaves_6.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
				OP_Leaves_6.Category.Add(BuildPieceCategory.Misc);

				//stone 

				BuildPiece OP_Ruin_Pillar_1 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Ruin_Pillar_1", true, "OP_Bamboo_Hammer");
				OP_Ruin_Pillar_1.Name.English("OP_Ruin_Pillar_1");
				OP_Ruin_Pillar_1.Description.English("A stone pillar");
				OP_Ruin_Pillar_1.RequiredItems.Add("Stone", 2, true);
				OP_Ruin_Pillar_1.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
				OP_Ruin_Pillar_1.Category.Add(BuildPieceCategory.Misc);


				BuildPiece OP_Ruin_Pillar_2 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Ruin_Pillar_2", true, "OP_Bamboo_Hammer");
				OP_Ruin_Pillar_2.Name.English("OP_Ruin_Pillar_2");
				OP_Ruin_Pillar_2.Description.English("A stone pillar");
				OP_Ruin_Pillar_2.RequiredItems.Add("Stone", 4, true);
				OP_Ruin_Pillar_2.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
				OP_Ruin_Pillar_2.Category.Add(BuildPieceCategory.Misc);


				BuildPiece OP_Ruin_Pillar_3 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Ruin_Pillar_3", true, "OP_Bamboo_Hammer");
				OP_Ruin_Pillar_3.Name.English("OP_Ruin_Pillar_3");
				OP_Ruin_Pillar_3.Description.English("A stone pillar");
				OP_Ruin_Pillar_3.RequiredItems.Add("Stone", 6, true);
				OP_Ruin_Pillar_3.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
				OP_Ruin_Pillar_3.Category.Add(BuildPieceCategory.Misc);


				BuildPiece OP_Rock_Formation_1 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Rock_Formation_1", true, "OP_Bamboo_Hammer");
				OP_Rock_Formation_1.Name.English("OP_Rock_Formation_1");
				OP_Rock_Formation_1.Description.English("A stone rock formation");
				OP_Rock_Formation_1.RequiredItems.Add("Stone", 4, true);
				OP_Rock_Formation_1.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
				OP_Rock_Formation_1.Category.Add(BuildPieceCategory.Misc);


				BuildPiece OP_Rock_Formation_2 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Rock_Formation_2", true, "OP_Bamboo_Hammer");
				OP_Rock_Formation_2.Name.English("OP_Rock_Formation_2");
				OP_Rock_Formation_2.Description.English("A stone rock formation");
				OP_Rock_Formation_2.RequiredItems.Add("Stone", 4, true);
				OP_Rock_Formation_2.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
				OP_Rock_Formation_2.Category.Add(BuildPieceCategory.Misc);


				BuildPiece OP_Rock_Formation_3 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Rock_Formation_3", true, "OP_Bamboo_Hammer");
				OP_Rock_Formation_3.Name.English("OP_Rock_Formation_3");
				OP_Rock_Formation_3.Description.English("A stone rock formation");
				OP_Rock_Formation_3.RequiredItems.Add("Stone", 4, true);
				OP_Rock_Formation_3.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
				OP_Rock_Formation_3.Category.Add(BuildPieceCategory.Misc);


				BuildPiece OP_Rock_Formation_4 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Rock_Formation_4", true, "OP_Bamboo_Hammer");
				OP_Rock_Formation_4.Name.English("OP_Rock_Formation_4");
				OP_Rock_Formation_4.Description.English("A stone rock formation");
				OP_Rock_Formation_4.RequiredItems.Add("Stone", 4, true);
				OP_Rock_Formation_4.RequiredItems.Add("OP_Bamboo_Wood", 2, true);
				OP_Rock_Formation_4.Category.Add(BuildPieceCategory.Misc);


				BuildPiece OP_Stone_Path_1 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Stone_Path_1", true, "OP_Bamboo_Hammer");
				OP_Stone_Path_1.Name.English("OP_Stone_Path_1");
				OP_Stone_Path_1.Description.English("A path stone");
				OP_Stone_Path_1.RequiredItems.Add("Stone", 1, true);
				OP_Stone_Path_1.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
				OP_Stone_Path_1.Category.Add(BuildPieceCategory.Misc);


				BuildPiece OP_Stone_Path_2 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Stone_Path_2", true, "OP_Bamboo_Hammer");
				OP_Stone_Path_2.Name.English("OP_Stone_Path_2");
				OP_Stone_Path_2.Description.English("A path stone");
				OP_Stone_Path_2.RequiredItems.Add("Stone", 1, true);
				OP_Stone_Path_2.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
				OP_Stone_Path_2.Category.Add(BuildPieceCategory.Misc);


				BuildPiece OP_Stone_Path_3 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Stone_Path_3", true, "OP_Bamboo_Hammer");
				OP_Stone_Path_3.Name.English("OP_Stone_Path_3");
				OP_Stone_Path_3.Description.English("A path stone");
				OP_Stone_Path_3.RequiredItems.Add("Stone", 1, true);
				OP_Stone_Path_3.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
				OP_Stone_Path_3.Category.Add(BuildPieceCategory.Misc);


				BuildPiece OP_Stone_Path_4 = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Stone_Path_4", true, "OP_Bamboo_Hammer");
				OP_Stone_Path_4.Name.English("OP_Stone_Path_4");
				OP_Stone_Path_4.Description.English("A path stone");
				OP_Stone_Path_4.RequiredItems.Add("Stone", 1, true);
				OP_Stone_Path_4.RequiredItems.Add("OP_Bamboo_Wood", 1, true);
				OP_Stone_Path_4.Category.Add(BuildPieceCategory.Misc);

				//bamboo forest walls

				BuildPiece OP_Wild_Bamboo_Wall = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Wild_Bamboo_Wall", true, "OP_Bamboo_Hammer");
				OP_Wild_Bamboo_Wall.Name.English("OP_Wild_Bamboo_Wall");
				OP_Wild_Bamboo_Wall.Description.English("A bamboozled piece");
				OP_Wild_Bamboo_Wall.RequiredItems.Add("OP_Bamboo_Wood", 12, true);
				OP_Wild_Bamboo_Wall.Category.Add(BuildPieceCategory.Misc);


				BuildPiece OP_Wild_Bamboo = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Wild_Bamboo", true, "OP_Bamboo_Hammer");
				OP_Wild_Bamboo.Name.English("OP_Wild_Bamboo");
				OP_Wild_Bamboo.Description.English("A bamboozled piece");
				OP_Wild_Bamboo.RequiredItems.Add("OP_Bamboo_Wood", 4, true);
				OP_Wild_Bamboo.Category.Add(BuildPieceCategory.Misc);


				BuildPiece OP_Wild_Bamboo_Light = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Wild_Bamboo_Light", true, "OP_Bamboo_Hammer");
				OP_Wild_Bamboo_Light.Name.English("OP_Wild_Bamboo_Light");
				OP_Wild_Bamboo_Light.Description.English("A bamboozled piece");
				OP_Wild_Bamboo_Light.RequiredItems.Add("OP_Bamboo_Wood", 4, true);
				OP_Wild_Bamboo_Light.Category.Add(BuildPieceCategory.Misc);


				BuildPiece OP_Wild_Bamboo_Dark = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Wild_Bamboo_Dark", true, "OP_Bamboo_Hammer");
				OP_Wild_Bamboo_Dark.Name.English("OP_Wild_Bamboo_Dark");
				OP_Wild_Bamboo_Dark.Description.English("A bamboozled piece");
				OP_Wild_Bamboo_Dark.RequiredItems.Add("OP_Bamboo_Wood", 4, true);
				OP_Wild_Bamboo_Dark.Category.Add(BuildPieceCategory.Misc);


				BuildPiece OP_Wild_Bamboo_Cluster = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Wild_Bamboo_Cluster", true, "OP_Bamboo_Hammer");
				OP_Wild_Bamboo_Cluster.Name.English("OP_Wild_Bamboo_Cluster");
				OP_Wild_Bamboo_Cluster.Description.English("A bamboozled piece");
				OP_Wild_Bamboo_Cluster.RequiredItems.Add("OP_Bamboo_Wood", 8, true);
				OP_Wild_Bamboo_Cluster.Category.Add(BuildPieceCategory.Misc);


				BuildPiece OP_Wild_Bamboo_Cluster_Dark = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Wild_Bamboo_Cluster_Dark", true, "OP_Bamboo_Hammer");
				OP_Wild_Bamboo_Cluster_Dark.Name.English("OP_Wild_Bamboo_Cluster_Dark");
				OP_Wild_Bamboo_Cluster_Dark.Description.English("A bamboozled piece");
				OP_Wild_Bamboo_Cluster_Dark.RequiredItems.Add("OP_Bamboo_Wood", 8, true);
				OP_Wild_Bamboo_Cluster_Dark.Category.Add(BuildPieceCategory.Misc);


				BuildPiece OP_Wild_Bamboo_Wall_Door = new(PiecePrefabManager.RegisterAssetBundle("bamboo"), "OP_Wild_Bamboo_Wall_Door", true, "OP_Bamboo_Hammer");
				OP_Wild_Bamboo_Wall_Door.Name.English("OP_Wild_Bamboo_Wall_Door");
				OP_Wild_Bamboo_Wall_Door.Description.English("A bamboozled piece");
				OP_Wild_Bamboo_Wall_Door.RequiredItems.Add("OP_Bamboo_Wood", 10, true);
				OP_Wild_Bamboo_Wall_Door.Category.Add(BuildPieceCategory.Misc);





				//trees
				GameObject OP_Bamboo_Log = ItemManager.PrefabManager.RegisterPrefab("bamboo", "OP_Bamboo_Log");

				GameObject OP_Bamboo_Log_Half = ItemManager.PrefabManager.RegisterPrefab("bamboo", "OP_Bamboo_Log_Half");

				GameObject OP_Bamboo_Stump = ItemManager.PrefabManager.RegisterPrefab("bamboo", "OP_Bamboo_Stump");

				GameObject OP_Bamboo_Tree_1 = ItemManager.PrefabManager.RegisterPrefab("bamboo", "OP_Bamboo_Tree_1");

				GameObject OP_Bamboo_Tree_2 = ItemManager.PrefabManager.RegisterPrefab("bamboo", "OP_Bamboo_Tree_2");


				//totem
				GameObject OP_Bamboo_Build_Totem = ItemManager.PrefabManager.RegisterPrefab("bamboo", "OP_Bamboo_Build_Totem");

                Assembly assembly = Assembly.GetExecutingAssembly();
                Harmony harmony = new(ModGUID);
                harmony.PatchAll(assembly);

            }
		}
	}
}




