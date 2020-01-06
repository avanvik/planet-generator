using UnityEngine;
using System.Linq;


class WorldTerrainParameters 
{
	public int Resolution { get; set; }
	public int ColliderResolutionFraction { get; set; }
	public Material Material { get; set; }
	public ShapeSettings ShapeSettings { get; set; }
}

static class WorldTerrainGenerator 
{
	public class WorldFace 
	{
		public GameObject GameObject;
		public MinMax ElevationRange;
	}

	/// <summary>
	///	Generates face meshes for all six directions, attaches them to a root node, and returns the root node
	/// </summary>
	public static GameObject GenerateWorldTerrain(WorldTerrainParameters parameters) {
		var rootNode = new GameObject("Terrain");
		Vector3[] faceDirections = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
		
		var faces = faceDirections.Select(direction => 
		{
			var face = GenerateWorldFace(rootNode.transform, direction, parameters.Resolution, parameters.ColliderResolutionFraction, parameters.Material, parameters.ShapeSettings);
			face.GameObject.transform.parent = rootNode.transform;
			return face;
		});

		var combinedElevationMinMax = MinMax.Union(faces.Select(face => face.ElevationRange));
		parameters.Material.SetVector("_elevationMinMax", new Vector4(combinedElevationMinMax.Min, combinedElevationMinMax.Max));

		return rootNode;
	}

	/// <summary>
	///		Generates a face mesh with a given resolution and direction, with a collider
	/// </summary>
	private static WorldFace GenerateWorldFace(Transform parent, Vector3 direction, int resolution, int colliderResolutionFraction, Material material, ShapeSettings shapeSettings)
	{	
		var face = new GameObject("Mesh");
		var renderer = face.AddComponent<MeshRenderer>();
		renderer.sharedMaterial = material;

		// Face geometry
		var meshFilter = face.AddComponent<MeshFilter>();
		var terrainFace = TerrainFaceGenerator.GenerateTerrainFace(resolution, direction, shapeSettings);
		meshFilter.sharedMesh = terrainFace.Mesh;

		// Only generate colliders around planet
		if(direction != Vector3.forward && direction != Vector3.back)
		{
			// Collider geometry
			var collider = face.AddComponent<MeshCollider>();
			var collisionFace = TerrainFaceGenerator.GenerateTerrainFace(resolution / colliderResolutionFraction, direction, shapeSettings);
			collider.sharedMesh = collisionFace.Mesh;
		}

		// var combinedElevationMinMax = MinMax.Union(faces.Select(face => face.ElevationRange));
		renderer.sharedMaterial.SetVector("_elevationMinMax", new Vector4(0, 10));
		
		return new WorldFace() { GameObject = face, ElevationRange = terrainFace.ElevationRange };
	}
}
