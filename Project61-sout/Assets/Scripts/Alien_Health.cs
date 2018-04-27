using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Alien_Health : NetworkBehaviour {

	private int health = 50;

	public void DeductHealth (int dmg)
	{
		health -= dmg;
		CheckHealth();
		Debug.Log("zombie " + health);
	}

	void CheckHealth()
	{
		if(health <= 0)
		{
			Destroy(gameObject);
		}
	}
}
