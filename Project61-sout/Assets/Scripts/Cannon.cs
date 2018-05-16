using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Cannon : NetworkBehaviour {

	[SerializeField] private int cost = 500;
	private int maxParts = 5;
	[SyncVar]
	public int currentpart;
	
	
	public GameObject BarPanel;
	public Image CannonBarFill; 

	void ShowMessage() {
		//BarPanel.SetActive(true);
		CannonBarFill.fillAmount = (float)currentpart / maxParts;
		//WavePanel.SetActive(false);
		
	}
	
	private void Start()
	{
		currentpart = 0; 
		//PlayerUI ui = GetComponent<PlayerUI>();
		//ui.SetCannon(GetComponent<Cannon>());
	}

	void Update()
	{
		ShowMessage();
	}


	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E) && other.GetComponent<Player>().money >= cost && currentpart < maxParts)
		{
			StartCoroutine(Pickup(other));
		}
	}

	public float Getpst()
	{
		return (float) currentpart / maxParts;
	}
	

	IEnumerator Pickup(Collider player)
	{
		Player stats = player.GetComponent<Player>();
		currentpart++;
		stats.money -= cost;
		
		yield return new WaitForSeconds(2);
	}
}
