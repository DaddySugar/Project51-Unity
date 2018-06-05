using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScoreboard : MonoBehaviour {


	[SerializeField]
	GameObject playerScoreboardItem;

	[SerializeField]
	Transform playerScoreboardList;

	void OnEnable ()
	{
		Player[] players = GameManager.GetAllPlayers();
		int iiii = 0;
		foreach (Player player in players)
		{
			iiii++;
			GameObject itemGO = (GameObject)Instantiate(playerScoreboardItem, playerScoreboardList);
			PlayerScoreboardItem item = itemGO.GetComponent<PlayerScoreboardItem>();
			if (item != null)
			{
				item.Setup("Player " + iiii , player.kills, player.money, player.deaths);
			}
		}
	}


}
