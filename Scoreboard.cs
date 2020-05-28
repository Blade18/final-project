using UnityEngine;
using System.Collections;

public class Scoreboard : MonoBehaviour {

	[SerializeField]
	GameObject playerScoreboardItem;

	[SerializeField]
	Transform playerScoreboardList;

	void OnEnable ()
	{
		Player[] players = GameManager.GetAllPlayers();

		foreach (Player player in players)
		{
			GameObject itemGO = (GameObject)Instantiate(playerScoreboardItem, playerScoreboardList);
			PlayerScoreboardItem item = itemGO.GetComponent<PlayerScoreboardItem>();
			if (item != null)
			{
				Debug.Log(player._playerName + " k:" + player.kills.ToString() + "d:" + player.deaths.ToString());
				item.Setup(player._playerName, player.kills, player.deaths);
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
