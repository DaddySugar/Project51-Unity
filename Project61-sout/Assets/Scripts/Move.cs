using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Move : NetworkBehaviour {
	
	[SerializeField] 
	Transform Player;
	[SerializeField] 
	Camera cam;
	[SerializeField] 
	AudioListener audio;

	void Start()
	{
		if (!isLocalPlayer) {
			cam.gameObject.SetActive (false);
			audio.gameObject.SetActive (false);
		}
	}

	void Update()
	{
		if (!isLocalPlayer)
		{
			return;
		}
				
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			Player.localPosition += new Vector3 (-1, 0, 0);
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			Player.localPosition += new Vector3 (1, 0, 0);
		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			Player.localPosition += new Vector3 (0, 0, 1);
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			Player.localPosition += new Vector3 (0, 0, -1);
		}

	}
}
