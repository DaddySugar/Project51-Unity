﻿
using System;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour {

	[SerializeField]
	Behaviour[] componentsToDisable;

	[SerializeField]
	string remoteLayerName = "RemotePlayer";
	
	Camera sceneCamera;
	
	[SerializeField]
	GameObject playerUIPrefab;
	
	//[HideInInspector]
	private GameObject playerUIInstance;

	void Start ()
	{
		// Disable components that should only be
		// active on the player that we control
		if (!isLocalPlayer) 
		{
			for (int i = 0; i < componentsToDisable.Length; i++) 
			{
				AssignRemoteLayer();
				DisableComponents();
			}
		} 
		else 
		{
			sceneCamera = Camera.main;
			if (sceneCamera != null) 
			{
				sceneCamera.gameObject.SetActive (false);
			}
			// Create PlayerUI
			playerUIInstance = Instantiate(playerUIPrefab);
			playerUIInstance.name = playerUIPrefab.name;

			// Configure PlayerUI
			PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
			if (ui == null)
				Debug.LogError("No PlayerUI component on PlayerUI prefab.");
			ui.SetPlayer(GetComponent<Player>());

			

			string _username = "Loading...";
			
			_username = transform.name;
			CmdSetUsername(transform.name, _username);
		}
		
		GetComponent<Player>().Setup();
	}
	


	[Command]
	void CmdSetUsername (string playerID, string username)
	{
		Player player = GameManager.GetPlayer(playerID);
		if (player != null)
		{
			player.username = username;	
		}
	}

	public override void OnStartClient()
	{
		base.OnStartClient();

		string _netID = GetComponent<NetworkIdentity>().netId.ToString();
		Player _player = GetComponent<Player>();

		GameManager.RegisterPlayer(_netID, _player);
	}


	void OnDisable()
	{

		//Destroy(playerUIInstance);
		
		if (sceneCamera != null)
		{
			sceneCamera.gameObject.SetActive (true);
		}

		GameManager.UnregisterPlayer(transform.name);
	}
	
	void AssignRemoteLayer ()
	{
		gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
	}

	void DisableComponents ()
	{
		for (int i = 0; i < componentsToDisable.Length; i++)
		{
			componentsToDisable[i].enabled = false;
		}
	}
}

