using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

class GravitySystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        var deltaTime = Time.DeltaTime;
        
        Entities.ForEach((GravityAffected ga, Rigidbody rb, Transform t) => {
            for (int i = 0; i < ga.gravitySources.Length; i++)
            {
                var gravitySource = ga.gravitySources[i];
                Vector3 gravityDirection = gravitySource.transform.position - t.position;
                Vector3 gravityForce = (gravityDirection * gravitySource.gravityStrength) / gravityDirection.sqrMagnitude;
                
                rb.AddForce(gravityForce * deltaTime, ForceMode.Acceleration);
            }
        });
    }
}