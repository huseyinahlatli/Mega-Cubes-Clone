using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedZone : MonoBehaviour
{
    public bool isGameOver = false;
    
    #region Singleton Class: RedZone

    public static RedZone Instance;

    private void Awake()
    {
        isGameOver = false;
        
        if (Instance == null)
            Instance = this;
    }

    #endregion

    private void OnTriggerStay(Collider other)
    {
        CubeController cubeController = other.GetComponent<CubeController>();
        if (cubeController != null)
        {
            if (!cubeController.isMainCube && cubeController.cubeRigidbody.velocity.magnitude < .1f)
                isGameOver = true;
        }
    }
}
