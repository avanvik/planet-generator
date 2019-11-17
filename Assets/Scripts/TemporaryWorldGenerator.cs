using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TemporaryWorldGenerator : MonoBehaviour
{
	[Range(1, 100)]
	public int resolution;
	
	public ShapeSettings shapeSettings;
	public ColorSettings colorSettings;

	private void Awake()
	{
		Generate();
	}

	private void OnValidate() {
		UnityEditor.EditorApplication.delayCall += () => Generate();
	}

	public void Generate() {
		if (this != null) // No idea why
		{
			var oldChild = transform.childCount > 0 ? transform.GetChild(0) : null;
			if (oldChild != null) {
				DestroyImmediate(oldChild.gameObject);
			}
		
			var terrainConfig = new WorldTerrainParameters() {
				Material = colorSettings.worldMaterial,
				Resolution = resolution,
				ShapeSettings = shapeSettings
			};

			var world = WorldTerrainGenerator.GenerateWorldTerrain(terrainConfig);
			world.transform.parent = transform;
			world.transform.position = transform.position;

			var colors = new ColorGenerator();
		}
	}
}
