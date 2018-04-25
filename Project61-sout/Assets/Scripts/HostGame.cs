using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HostGame : MonoBehaviour
{

	[SerializeField]
	private uint roomSize = 2; 
	
	
	private string roomName;
	// We can add pasword 
	//private string roomPassword; 
	private NetworkManager _networkManager;

	private void Start()
	{
		_networkManager = NetworkManager.singleton;
		if (_networkManager.matchMaker == null)
		{
			_networkManager.StartMatchMaker();
		}
		
		Debug.Log("start creating " + roomName);
	}

	public void SetRoomName (string _name)
	{
		roomName = _name;
	}

	public void CreateRoom ()
	{
		if (roomName != "" && roomName != null)
		{
			Debug.Log("Creating Room: " + roomName + " with room for " + roomSize + " players.");
			//We crete a room : with roomName + size 
			_networkManager.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, _networkManager.OnMatchCreate);
		}
	}
}
