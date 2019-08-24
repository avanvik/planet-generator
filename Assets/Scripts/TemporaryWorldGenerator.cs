using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryWorldGenerator : MonoBehaviour
{    
	public Material material;
	public int resolution;

	private void Awake()
	{
		var terrainConfig = new WorldTerrainConfig() {
			Material = material,
			Resolution = resolution,
			ShapeSettings = new ShapeSettings()
		};
		var terrain = WorldTerrainGenerator.GenerateWorldTerrain(terrainConfig);
	}
}
