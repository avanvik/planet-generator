using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TemporaryWorldGenerator : MonoBehaviour
{    
	public Material material;
	public int resolution;
	
	public ShapeSettings shapeSettings;

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
				Material = material,
				Resolution = resolution,
				ShapeSettings = shapeSettings
			};

			var world = WorldTerrainGenerator.GenerateWorldTerrain(terrainConfig);
			world.transform.parent = transform;
			world.transform.position = transform.position;
		}
	}
}
