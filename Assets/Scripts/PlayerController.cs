using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float pushForce;
    [SerializeField] private float cubeMaxPositionX;
    
    [Space]
    [SerializeField] private PlayerTouchSlider touchSlider;
    private CubeController _mainCube;    
    
    private bool _isPointerDown;
    private Vector3 _cubePosition;
    private bool _canMove;
    
    private void Start()
    {
        SpawnCube();
        _canMove = true;
        
        // Listen to slider events:
        touchSlider.OnPointerDownEvent += OnPointerDown;
        touchSlider.OnPointerDragEvent += OnPointerDrag;
        touchSlider.OnPointerUpEvent += OnPointerUp;
    }

    private void Update()
    {
        if (_isPointerDown)
            _mainCube.transform.position = Vector3.Lerp(
                _mainCube.transform.position,
                _cubePosition, 
                moveSpeed * Time.deltaTime
            );
    }

    private void OnPointerDown()
    {
        _isPointerDown = true;
    }
    
    private void OnPointerDrag(float xMovement)
    {
        if (_isPointerDown)
        {
            _cubePosition = _mainCube.transform.position;
            _cubePosition.x = xMovement * cubeMaxPositionX;
        }
    }
    
    private void OnPointerUp()
    {
        if (_isPointerDown && _canMove)
        {
            _isPointerDown = false;
            _canMove = false;
            
            // Push the cube:
            _mainCube.cubeRigidbody.AddForce(Vector3.forward * pushForce, ForceMode.Impulse);
            
            Invoke("SpawnNewCube", 0.3f);
        }
    }

    private void SpawnNewCube()
    {
        _mainCube.isMainCube = false;
        _canMove = true;
        SpawnCube();
    }
    
    private void SpawnCube()
    {
        _mainCube = CubeSpawner.Instance.SpawnRandom();
        _mainCube.isMainCube = true;
        
        // reset cubePosition variable
        _cubePosition = _mainCube.transform.position;
    }
    
    private void OnDestroy()
    {
        // Remove Listeners:
        touchSlider.OnPointerDownEvent -= OnPointerDown;
        touchSlider.OnPointerDragEvent -= OnPointerDrag; 
        touchSlider.OnPointerUpEvent -= OnPointerUp;
    }
}
