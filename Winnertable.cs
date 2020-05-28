using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Winnertable : NetworkBehaviour
{
	[SerializeField]
	GameObject winnerItemPrefab;

	[SerializeField]
	Transform listplace;

	// Use this for initialization
	public int maxkill = 0;
	public string winnername = "";
	GameObject go;
	public void Update()
	{
		Player[] players = GameManager.GetAllPlayers();

		foreach (Player player in players)
		{
			if (player.kills > maxkill)
			{
				CmdChangeWinner(player._playerName, player.kills);
			}
		}
	}
	
	public void ChangeWinner(string player, int kills)
	{
		Destroy(go);

		go = (GameObject)Instantiate(winnerItemPrefab, listplace);
		go.GetComponent<Winneritem>().Setup(player, kills);
	}
	
	[Command]
	void CmdChangeWinner(string player, int kills)
	{
		RpcChangeWinner(player,kills);
	}

	[ClientRpc]
	void RpcChangeWinner(string player, int kills)
	{
		maxkill = kills;
		winnername = player;
		Destroy(go);
		go = (GameObject)Instantiate(winnerItemPrefab, listplace);
		go.GetComponent<Winneritem>().Setup(winnername, maxkill);

	}
}
