using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreboardItem : MonoBehaviour {

	[SerializeField]
	Text usernameText;

	[SerializeField]
	Text killsText;

	[SerializeField]
	Text deathsText;
	
	[SerializeField]
	Text CashText;
	
	

	public void Setup (string username, int kills, int deaths, int cash)
	{
		usernameText.text = username + ":";
		killsText.text = "Kills: " + kills;
		deathsText.text = "Deaths: " + deaths;
		CashText.text = "Money: " + cash;
	}
	
	public void Setup (string username, int kills, int deaths)
	{
		usernameText.text = username + ":";
		killsText.text = "Kills: " + kills;
		deathsText.text = "Deaths: " + deaths;
	}


}
