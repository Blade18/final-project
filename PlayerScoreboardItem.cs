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
	Text KD;

	public double kd = 0;
	public void Setup (string username, double kills, double deaths)
	{
		kd = kills / deaths;
		usernameText.text = username;
		killsText.text = "Kills: " + kills;
		deathsText.text = "Deaths: " + deaths;

		if (deaths == 0)
			KD.text = "K/D: " + kills;
		else if (kd % 1 == 0)
			KD.text = "K/D: " + kd.ToString();
		else
			KD.text = "K/D: " + kd.ToString("0.00");
	}

}
