using UnityEngine;
using System.Collections;
using System.Threading;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WaveSpawner : NetworkBehaviour {

	public enum SpawnState { SPAWNING, WAITING, COUNTING };

	private NetworkManager networkManager;
	
	/*
	 * to spawn smth :
	 * create a function
	 * in the function create a local variable
	 * ex : GameObject go = Instantiate(objct to spawn, spawn position, rotation);
	 *NetworkServer.spawn(go)
	 *
	 *
	 * To destroy object :
	 * Destroy(object , time in sec)
	 *
	 * 	
	 */
	
	
	
	[System.Serializable]
	public class Wave
	{
		public string name;
		public Transform enemy;
		public int count;
		public float rate;
	}

	/*[SerializeField]
	private Text wavenb;
	[SerializeField]
	private Text wavetxt;*/
	
	
	
	public Wave[] waves;
	
	private int nextWave = 0;

	private void DisplayWaveCountDown()
	{
		/*wavenb.text = " " + (nextWave+1);
		wavenb.GetComponent<Text>().enabled = true;
		wavetxt.GetComponent<Text>().enabled = true;*/
	}
	
	private void DisableWaveCountDownText()
	{
		
		//wavenb.GetComponent<Text>().enabled = false;
		//wavetxt.GetComponent<Text>().enabled = false;
	}
	
	public int NextWave
	{
		
		get
		{
			DisplayWaveCountDown();
		 	Thread.Sleep(1500);
			DisableWaveCountDownText();
			return nextWave + 1; }
	}

	public Transform[] spawnPoints;

	public float timeBetweenWaves = 5f;
	private float waveCountdown;
	public float WaveCountdown
	{
		get { return waveCountdown; }
	}

	private float searchCountdown = 1f;

	private SpawnState state = SpawnState.COUNTING;
	public SpawnState State
	{
		get { return state; }
	}

	void Start()
	{
		DisplayWaveCountDown();
		Thread.Sleep(1500);
		Debug.Log("text");
		DisableWaveCountDownText();
		
		networkManager = NetworkManager.singleton;
		
		if (spawnPoints.Length == 0)
		{
			Debug.LogError("No spawn points referenced.");
		}

		waveCountdown = timeBetweenWaves;
	}

	void Update()
	{
		if (state == SpawnState.WAITING)
		{
			if (!EnemyIsAlive())
			{
				WaveCompleted();
			}
			else
			{
				return;
			}
		}

		if (waveCountdown <= 0)
		{
			if (state != SpawnState.SPAWNING)
			{
				StartCoroutine( SpawnWave ( waves[nextWave] ) );
			}
		}
		else
		{
			waveCountdown -= Time.deltaTime;
		}
	}

	void WaveCompleted()
	{
		Debug.Log("Wave Completed!");

		state = SpawnState.COUNTING;
		waveCountdown = timeBetweenWaves;

		if (nextWave + 1 > waves.Length - 1)
		{
			nextWave = 0;
			Debug.Log("ALL WAVES COMPLETE! Looping...");
		}
		else
		{
			nextWave++;
		}
	}

	bool EnemyIsAlive()
	{
		searchCountdown -= Time.deltaTime;
		if (searchCountdown <= 0f)
		{
			searchCountdown = 1f;
			if (GameObject.FindGameObjectWithTag("Alien") == null)
			{
				return false;
			}
		}
		return true;
	}

	IEnumerator SpawnWave(Wave _wave)
	{
		Debug.Log("Spawning Wave: " + _wave.name);
		state = SpawnState.SPAWNING;

		for (int i = 0; i < _wave.count; i++)
		{
			SpawnEnemy(_wave.enemy);
			yield return new WaitForSeconds( 1f/_wave.rate );
		}

		state = SpawnState.WAITING;

		yield break;
	}

	void SpawnEnemy(Transform _enemy)
	{
		Debug.Log("Spawning Enemy: " + _enemy.name);

		Transform _sp = spawnPoints[ Random.Range (0, spawnPoints.Length) ];
		Instantiate(_enemy, _sp.position, _sp.rotation);
	}

}
