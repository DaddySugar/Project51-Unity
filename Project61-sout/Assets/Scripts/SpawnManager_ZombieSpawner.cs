using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SpawnManager_ZombieSpawner : NetworkBehaviour {

	[SerializeField] GameObject zombiePrefab;
	[SerializeField]  private GameObject zombieSpawns;
	private int counter;
	private int numberOfZombies = 20;
	private int maxNumberOfZombies = 80;
	private float waveRate = 10;
	private bool isSpawnActivated = true;

	public override void OnStartServer ()
	{
		//zombieSpawns = GameObject.FindGameObjectsWithTag("ZombieSpawn");
		StartCoroutine(ZombieSpawner());
	}

	IEnumerator ZombieSpawner()
	{
		//Debug.Log("111");
		for(;;)
		{
			yield return new WaitForSeconds(waveRate);
			GameObject[] zombies = GameObject.FindGameObjectsWithTag("Alien");
			if(zombies.Length < maxNumberOfZombies)
			{
				CommenceSpawn();
			}
		}
	}

	void CommenceSpawn()
	{
		if(isSpawnActivated)
		{
			for(int i = 0; i < numberOfZombies; i++)
			{
				//int randomIndex = Random.Range(0, zombieSpawns.Length);
				SpawnZombies(zombieSpawns.transform.position);
			}
		}
	}

	void SpawnZombies(Vector3 spawnPos)
	{
		//Debug.Log("Spaw");
		counter++;
		GameObject go = GameObject.Instantiate(zombiePrefab, spawnPos, Quaternion.identity) as GameObject;
		go.GetComponent<Alien_ID>().AlienID= "Zombie " + counter;
		NetworkServer.Spawn(go);
	}

}
