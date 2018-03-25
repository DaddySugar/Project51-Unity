
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {

	[SerializeField]
	Behaviour[] componentsToDisable;

	[SerializeField]
	string remoteLayerName = "RemotePlayer";
	
	Camera sceneCamera;

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
		}
		RegistrePlayer();
	}

	void RegistrePlayer()
	{
		var _ID = "Player " + GetComponent<NetworkIdentity>().netId;
		transform.name = _ID;

	}

	void OnDisable()
	{

		if (sceneCamera != null)
		{
			sceneCamera.gameObject.SetActive (true);
		}
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

