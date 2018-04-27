using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class tmp : NetworkBehaviour
{

    [SerializeField] private GameObject zombie;
    [SerializeField] public GameObject spawnpos;

    private int counte; 
    private int nbofzm = 10 ;

    public override void OnStartServer()
    {
        for (int i = 0; i < nbofzm; i++)
        {
            SpawnZb();
        }
    }

    void SpawnZb()
    {
        GameObject go = GameObject.Instantiate(zombie, spawnpos.transform.position, Quaternion.identity) as GameObject;
        NetworkServer.Spawn(go);
    }
    
    
    
}
