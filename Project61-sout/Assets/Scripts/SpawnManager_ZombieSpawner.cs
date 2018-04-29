using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SpawnManager_ZombieSpawner : NetworkBehaviour {

	[SerializeField] GameObject zombiePrefab;
	[SerializeField]  private GameObject zombieSpawns;
	private int counter;
	private int numberOfZombies = 3;
	private int maxNumberOfZombies = 30;
	private float waveRate = 10;
	private bool isSpawnActivated = true;
	private int _currentWave = 0;
	
	[SerializeField]
	private int _MaxWave;

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
			if(IsSpawnPossible(zombies.Length))
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
		go.GetComponent<Alien_ID>().AlienID= "Alien " + counter;
		NetworkServer.Spawn(go);
	}

	public bool IsSpawnPossible(int aliensList)
	{
		if (aliensList < maxNumberOfZombies)
			return true;

		else
			return false;
	}

	public bool IsWaveComplete()
	{
		GameObject[] aliensList = GameObject.FindGameObjectsWithTag("Alien");
		if (aliensList.Length >= 0 || aliensList == null)
		{
			NextWave();
			SpawnZombies(zombiePrefab.transform.position);
			return true;
			
		}

		else
			return false;
		
		
	}

	private void NextWave()
	{
		_currentWave++;
		if (_currentWave > _MaxWave)
		{
			_currentWave = 0;
		}
		
	}
	
	bool EnemyIsAlive()
	{
		if (GameObject.FindGameObjectWithTag("Alien") == null)
		{
			return false;
		}
	
	return true;
	}
}
