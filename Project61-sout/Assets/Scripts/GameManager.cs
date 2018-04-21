using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    
    public MatchSettings _MatchSettings;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("more than 1 game manager");
        }
        else
            instance = this;
    }


    #region PlayerTracking 

    

    
    private static Dictionary<string, Player> players= new Dictionary<string, Player>();
    private const string PLAYER_ID_PREFIX = "Player ";

    public static void RegisterPlayer(string _netID, Player _player)
    {
        string _playerID = PLAYER_ID_PREFIX + _netID;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID;
    }

    public static void UnregisterPlayer(string _playerID)
    {
        players.Remove(_playerID);
    }

    public static Player GetPlayer(string playerid)
    {
        return players[playerid];
    }
    
    #endregion 
    
    
    
}
