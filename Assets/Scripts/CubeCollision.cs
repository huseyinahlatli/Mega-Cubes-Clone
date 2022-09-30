using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class CubeCollision : MonoBehaviour
{
    public CubeController otherCube;

    // Singleton Class: CubeCollision
    public static CubeCollision Instance;

    private CubeController _cubeController;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        _cubeController = GetComponent<CubeController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        otherCube = collision.gameObject.GetComponent<CubeController>();
        if (otherCube != null && _cubeController.cubeID > otherCube.cubeID)  // check if contacted with other cube
        {   
            if (_cubeController.cubeNumber == otherCube.cubeNumber) // check if both cubes have same number
            {
                Vector3 contactPoint = collision.contacts[0].point;
                if (otherCube.cubeNumber < CubeSpawner.Instance.maxCubeNumber) // spawn a new cube as a result
                {
                    CubeController newCube = CubeSpawner.Instance.Spawn(_cubeController.cubeNumber * 2, contactPoint + Vector3.up * 1.6f);
                    float _pushForce = 2.5f;
                    newCube.cubeRigidbody.AddForce(new Vector3(0, .3f, 1f) * _pushForce, ForceMode.Impulse); 
                    //push the new cube up and forward:
                        
                    var randomValue = Random.Range(-20f, 20f);
                    Vector3 randomDirection = Vector3.one * randomValue;
                    newCube.cubeRigidbody.AddTorque(randomDirection);
                }
                
                // the explosion should affect surrounded cubes too:
                Collider[] surroundCubes = Physics.OverlapSphere(contactPoint, 2f);
                float explosionForce = 400f;
                float explosionRadius = 1.5f;

                foreach (Collider collider in surroundCubes)
                {
                    if (collider.attachedRigidbody != null)
                        collider.attachedRigidbody.AddExplosionForce(explosionForce, contactPoint, explosionRadius);
                }

                FX.Instance.PlayCubeExplosionFX(contactPoint, _cubeController.cubeColor);

                UI.Instance.addedPoints.Add(otherCube.cubeNumber);
                
                // Destroy the two cubes:
                CubeSpawner.Instance.DestroyCube(_cubeController);
                CubeSpawner.Instance.DestroyCube(otherCube);
            }
        }
    }
}
