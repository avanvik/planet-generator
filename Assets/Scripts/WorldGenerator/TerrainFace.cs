using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace 
{
	public Mesh Mesh;
	public MinMax ElevationRange;
}

public static class TerrainFaceGenerator 
{
	public static TerrainFace GenerateTerrainFace (int resolution, Vector3 localUp, ShapeSettings settings) {

		var axisA = new Vector3(localUp.y, localUp.z, localUp.x);
		var axisB = Vector3.Cross(localUp, axisA);
		var shapeGenerator = new ShapeGenerator(settings);

		Vector3[] vertices = new Vector3[resolution * resolution];
		int[] triangles = new int[(resolution-1) * (resolution-1) * 6];
		int triIndex = 0;
		var minMax = new MinMax();

		for (int y = 0; y < resolution; y++) {
			for (int x = 0; x < resolution; x++)
			{	
				int i = x + y * resolution; // Same as doing int i = 0 outside loop, and i++ inside loop
				Vector2 percent = new Vector2(x,y) / (resolution-1);
				Vector3 pointOnUnitCube = localUp + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB;
				Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;

				var elevation = shapeGenerator.CalculateElevation(pointOnUnitSphere);
				var vertex = pointOnUnitSphere * elevation;
				minMax.AddValue(elevation);

				vertices[i] = vertex;

				if (x != resolution-1 && y != resolution-1) {
					triangles[triIndex] = i;
					triangles[triIndex + 1] = i + resolution + 1;
					triangles[triIndex + 2] = i + resolution;

					triangles[triIndex + 3] = i;
					triangles[triIndex + 4] = i + 1;
					triangles[triIndex + 5] = i + resolution + 1;

					triIndex += 6;
				}
			}
		}


		var mesh = new Mesh();

		mesh.Clear();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();

		return new TerrainFace() { Mesh = mesh, ElevationRange = minMax };
	} 

	
	
}
