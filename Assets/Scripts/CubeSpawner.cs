using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class CubeSpawner : MonoBehaviour
{
    // Singleton Class: CubeSpawner
    public static CubeSpawner Instance;

    Queue<CubeController> cubesQueue = new Queue<CubeController>();
    [SerializeField] private int cubesQueueCapacity = 20;
    [SerializeField] private bool autoQueueGrow = true;
   
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private Color[] cubeColors;
    
    [HideInInspector] public int maxCubeNumber; 
    // in our case it's 4096 | 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096

    private int _maxPower = 12;
    private Vector3 _defaultSpawnPosition;

    private void Awake()
    {
        Instance = this;

        _defaultSpawnPosition = transform.position;
        maxCubeNumber = (int)Mathf.Pow(2, _maxPower);

        InitializeCubesQueue();
    }

    private void InitializeCubesQueue()
    {
        for (int i = 0; i < cubesQueueCapacity; i++)
            AddCubeToQueue();
    }

    private void AddCubeToQueue()
    {
        CubeController cubeController = Instantiate(cubePrefab, _defaultSpawnPosition, Quaternion.identity, transform)
            .GetComponent<CubeController>();
        
        cubeController.gameObject.SetActive(false);
        cubeController.isMainCube = false;
        cubesQueue.Enqueue(cubeController);
    }

    public CubeController Spawn(int number, Vector3 position)
    {
        if (cubesQueue.Count == 0)
        {
            if (autoQueueGrow)
            {
                cubesQueueCapacity++;
                AddCubeToQueue();
            }
            else
            {
                Debug.LogError("[Cubes Queue] : no more cubes available in the pool");
                return null;
            }
        }

        CubeController cubeController = cubesQueue.Dequeue();
        cubeController.transform.position = position;
        cubeController.SetNumber(number);
        cubeController.SetColor(GetColor(number));
        cubeController.gameObject.SetActive(true);

        return cubeController;
    }

    public CubeController SpawnRandom()
    {
        return Spawn(GenerateRandomNumber(), _defaultSpawnPosition);
    }

    public void DestroyCube(CubeController cubeController)
    {
        cubeController.cubeRigidbody.velocity = Vector3.zero;
        cubeController.cubeRigidbody.angularVelocity = Vector3.zero;
        cubeController.transform.rotation = Quaternion.identity;
        cubeController.isMainCube = false;
        cubeController.gameObject.SetActive(false);
        cubesQueue.Enqueue(cubeController); 
    }
    
    public int GenerateRandomNumber()
    {
        return (int)Mathf.Pow(2, Random.Range(1, 6)); // 2, 4, 8, 16, 32
    }

    private Color GetColor(int number)
    {
        return cubeColors[(int)(Mathf.Log(number) / Mathf.Log(2)) - 1];
    }
}
