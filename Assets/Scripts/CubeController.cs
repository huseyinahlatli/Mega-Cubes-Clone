using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CubeController : MonoBehaviour
{
	private static int _staticID = 0;
	[SerializeField] private TMP_Text[] numbersText;
	
	[HideInInspector] public int cubeID;
	[HideInInspector] public Color cubeColor;
	[HideInInspector] public int cubeNumber;
	[HideInInspector] public Rigidbody cubeRigidbody;
	[HideInInspector] public bool isMainCube;

	private MeshRenderer _cubeMeshRenderer;

	private void Awake()
	{
		cubeID = _staticID++;
		_cubeMeshRenderer = GetComponent<MeshRenderer>();
		cubeRigidbody = GetComponent<Rigidbody>();
	}

	public void SetColor(Color color)
	{
		cubeColor = color;
		_cubeMeshRenderer.material.color = color;
	}

	public void SetNumber(int number)
	{
		cubeNumber = number;
		for (int i = 0; i < 6; i++)
		{
			numbersText[i].text = number.ToString() ;
		}
	}
}