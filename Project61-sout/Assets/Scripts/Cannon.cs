using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Cannon : NetworkBehaviour {

	[SerializeField] private int cost = 500;
	private const int maxParts = 5;
	[SyncVar]
	public int currentpart;

	private float timeToWait = 0f;
	//private PlayerUI ui; 
	private string _buymsg; 
	
	
	/*public GameObject BarPanel;
	public Image CannonBarFill; */

	/*public float ShowMessage() {
		//BarPanel.SetActive(true);
		//CannonBarFill.fillAmount = (float)currentpart / maxParts;
		//WavePanel.SetActive(false);
		return (float)currentpart / maxParts;
		
	}*/
	
	private void Start()
	{
		//ui = GetComponent<PlayerUI>();
		currentpart = 0;
		_buymsg = "Parts for " + cost; 
		//PlayerUI ui = GetComponent<PlayerUI>();
		//ui.SetCannon(GetComponent<Cannon>());
	}

	void Update()
	{
		//ShowMessage();
	}


	private void OnTriggerStay(Collider other)
	{
		
		if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E) && other.GetComponent<Player>().money >= cost && currentpart < maxParts && Time.time > timeToWait)
		{
			Pickup(other);
			
		}
	}

	public float Getpst()
	{
		return (float) currentpart / maxParts;
	}
	

	private void Pickup(Collider player)
	{
		Player stats = player.GetComponent<Player>();
		currentpart++;
//		Debug.Log(currentpart);
		timeToWait = Time.time + 0.1f;
		stats.money -= cost;
		
	}
}
