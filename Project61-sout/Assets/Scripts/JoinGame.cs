using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class JoinGame : MonoBehaviour {

    private NetworkManager networkManager;
    List<GameObject> roomList = new List<GameObject>();
    
    [SerializeField]
    private Text status;
    
    
    void Start ()
    {
        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }

        RefreshRoomList();
    }
    
    public void RefreshRoomList ()
    {
        //ClearRoomList();

        if (networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }

     //   networkManager.matchMaker.ListMatches(0, 10, "", true, OnMatchList);
        status.text = "Loading...";
    }

    /*
    public void OnMatchList(Match )
    {
        status.text = ""; 
        if (matchList == null)
        {
            status.text = "Couldn't get room list.";
            return;
        }
    }
    
    */
}
