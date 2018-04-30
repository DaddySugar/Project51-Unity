using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	[SerializeField] public Slider healthbar;
	private Player player;
//	private PlayerController controller;
	

	public void SetPlayer (Player _player)
	{
		player = _player;
	//	controller = player.GetComponent<PlayerController>();
		Debug.Log("Player healthbar set");
	}

	
	public void SetHealthAmount (float _amount)
	{
		healthbar.value = _amount;
	}
}
