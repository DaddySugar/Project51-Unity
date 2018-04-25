using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HostSinglePlayer : MonoBehaviour {

	// We can add pasword 
	//private string roomPassword; 
	private NetworkManager _networkManager;

	private void Start()
	{
		_networkManager = NetworkManager.singleton;
		Debug.Log("3");
		if (_networkManager.matchMaker == null)
		{
			Debug.Log("1");
			_networkManager.StartMatchMaker();
			Debug.Log("2");
			
		}
	}


	public void CreateRoom ()
	{
		Debug.Log("4");
		_networkManager.matchMaker.CreateMatch("single", 1, true, "", "", "", 0, 0, _networkManager.OnMatchCreate);
	}
}