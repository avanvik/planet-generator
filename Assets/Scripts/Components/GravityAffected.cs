using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class GravityAffected : MonoBehaviour
{

    public GravitySource[] gravitySources;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        gravitySources = FindObjectsOfType<GravitySource>();
        rb = GetComponent<Rigidbody>();
    }
}



