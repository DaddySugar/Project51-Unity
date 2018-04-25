﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraIgnore : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		Camera camera = GetComponent<Camera>();
		camera.cullingMask = -1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}