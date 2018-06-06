using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SpawnManager_ZombieSpawner : NetworkBehaviour
{
	public bool IsSpawnActivated
	{
		set { isSpawnActivated = value; }
	}

	[SerializeField] GameObject []zombiePrefab;
	private GameObject []zombieSpawns;
	private int counter = 0;
	private int qa = 0; 
	private int numberOfZombies = 3;
	private int maxNumberOfZombies = 17;
	private float waveRate = 5;
	private bool isSpawnActivated = true;
	private int _currentWave = 0;
	
	//wave text
	public GameObject WavePanel;
	public Text waveText;
	public Text _WaveNumb;

	public override void OnStartServer ()
	{
		zombieSpawns = GameObject.FindGameObjectsWithTag("ZombieSpawn");
		StartCoroutine(ZombieSpawner());
	}
	
	void Awake ()
	{
		WavePanel.SetActive(false);
	}
	
	IEnumerator ShowMessage (float delay) {

		
		WavePanel.SetActive(true);
		waveText.text = "Wave";
		_WaveNumb.text = _currentWave.ToString();
			
		Debug.Log("Text add");
		yield return new WaitForSeconds(delay);
		WavePanel.SetActive(false);
		Debug.Log("TextRemove");
		
		
	}

	void SetWaveText()
	{
		StartCoroutine(ShowMessage(3));
	}

	IEnumerator ZombieSpawner()
	{
		//Debug.Log("111");
		for(;;)
		{
			
			yield return new WaitForSeconds(waveRate);
			
		//	SetWaveText();
			GameObject[] zombies = GameObject.FindGameObjectsWithTag("Alien");
			if(IsSpawnPossible())
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
				int randomIndex = Random.Range(0, zombieSpawns.Length);
				SpawnZombies(zombieSpawns[randomIndex].transform.position);
			}
		}
	}

	void SpawnZombies(Vector3 spawnPos)
	{
		
		int randomIndex = Random.Range(0, zombiePrefab.Length);
		//Debug.Log("Spaw");
		counter++;
		if (nbofkills() - qa == maxNumberOfZombies)
		{
			qa = nbofkills();
			NextWave();
			SetWaveText();
		}
			
		GameObject go = GameObject.Instantiate(zombiePrefab[randomIndex], spawnPos, Quaternion.identity) as GameObject;
		go.GetComponent<Alien_ID>().AlienID= "Alien " + counter;
		NetworkServer.Spawn(go);
	}

	public bool IsSpawnPossible()
	{
		GameObject[] aa =  GameObject.FindGameObjectsWithTag("Alien");
		if (aa.Length < maxNumberOfZombies)
			return true;

		else
			return false;
	}

	private void NextWave()
	{
		_currentWave++;
	}

	int nbofkills()
	{
		int i = 0;
		Player[] aaa = FindObjectsOfType<Player>();
		foreach (var VARIABLE in aaa)
		{
			i += VARIABLE.kills;
		}

		return i; 
	}

}
