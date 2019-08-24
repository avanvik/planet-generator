using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
[System.Serializable]
public class ShapeSettings : ScriptableObject
{
    [Range(0, 100)]
    public float planetRadius;
    public NoiseLayer[] noiseLayers;
	
	[System.Serializable]
	public class NoiseLayer {
		public bool enabled;
		public bool useFirstLayerAsMask;
		public NoiseSettings noiseSettings;
	}
}
