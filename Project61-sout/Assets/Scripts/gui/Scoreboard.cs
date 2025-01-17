using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Scoreboard : MonoBehaviour {

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
				item.Setup("Player " + iiii , player.kills, player.deaths);
				Debug.Log("Player " + iiii + " : " + " player.isClient " + player.isClient + " player.isServer " + player.isServer + " player.isLocalPlayer " + player.isLocalPlayer);

			}
		}
	}

	void OnDisable ()
	{
		foreach (Transform child in playerScoreboardList)
		{
			Destroy(child.gameObject);
		}
	}

}
