using UnityEngine;
using System.Linq;

class World 
{

}

class WorldTerrainConfig 
{
	public int Resolution { get; set; }
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

	public static World GenerateWorldTerrain(WorldTerrainConfig config) {
		var world = new World();
		var rootNode = new GameObject("Terrain");
		Vector3[] faceDirections = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
		
		var faces = faceDirections.Select(direction => 
		{
			var face = GenerateWorldFace(rootNode.transform, direction, config.Resolution, config.Material, config.ShapeSettings);
			face.GameObject.transform.parent = rootNode.transform;
			return face;
		});

		var combinedElevationMinMax = MinMax.Union(faces.Select(face=> face.ElevationRange));

		return world;
	}

	private static WorldFace GenerateWorldFace(Transform parent, Vector3 direction, int resolution, Material material, ShapeSettings shapeSettings)
	{	
		var face = new GameObject("Mesh");
		var renderer = face.AddComponent<MeshRenderer>();
		renderer.sharedMaterial = material;

		var meshFilter = face.AddComponent<MeshFilter>();
		var terrainFace = TerrainFaceGenerator.GenerateTerrainFace(resolution, direction, shapeSettings);

		meshFilter.sharedMesh = terrainFace.Mesh;

		return new WorldFace() { GameObject = face, ElevationRange = terrainFace.ElevationRange };
	}
}
