using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX : MonoBehaviour
{
    [SerializeField] private ParticleSystem cubeExplosionFX;
    private ParticleSystem.MainModule _cubeExplosionFxMainModule;
    public static FX Instance;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void Start()
    {
        _cubeExplosionFxMainModule = cubeExplosionFX.main;
    }

    public void PlayCubeExplosionFX(Vector3 position, Color color)
    {
        _cubeExplosionFxMainModule.startColor = new ParticleSystem.MinMaxGradient(color);
        cubeExplosionFX.transform.position = position;
        cubeExplosionFX.Play();
    }
}
