using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Proyecto26;
public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public MatchSettings matchSettings;

	[SerializeField]
	private GameObject sceneCamera;

	public static double Killc= 0;
	public static double Deathc= 0;
	public static string playerName;

	public delegate void OnPlayerKilledCallback(string player, string source);
	public OnPlayerKilledCallback onPlayerKilledCallback;


	public delegate void OnPlayerJoinCallback(string player);
	public OnPlayerJoinCallback onPlayerJoinCallback;
	void Awake ()
	{
		if (instance != null)
		{
			Debug.LogError("More than one GameManager in scene.");
		} else
		{
			instance = this;
		}
	}

	public static void GetUser(User user)
	{
		Debug.Log(user.userName + " enter the lobby");
		Killc = user.Kill;
		Deathc = user.Death;
		playerName = user.userName;
	}

	public void SetSceneCameraActive (bool isActive)
	{
		if (sceneCamera == null)
			return;

		sceneCamera.SetActive(isActive);
	}

	public static void PlusDie()
	{
		Deathc++;
		RestClient.Put("https://finalproject-fb30d.firebaseio.com/" + playerName + "/Death.json", Deathc.ToString());
	}

	public static void PlusKill()
	{
		Killc++;
		Debug.Log("one down");
		RestClient.Put("https://finalproject-fb30d.firebaseio.com/" + playerName + "/Kill.json", Killc.ToString());
	}

	#region Player tracking

	private const string PLAYER_ID_PREFIX = "Player ";

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    public static void RegisterPlayer (string _netID, Player _player)
    {
        string _playerID = PLAYER_ID_PREFIX + _netID;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID;
	}

    public static void UnRegisterPlayer (string _playerID)
    {
        players.Remove(_playerID);
    }

    public static Player GetPlayer (string _playerID)
    {
        return players[_playerID];
    }

	public static Player[] GetAllPlayers ()
	{
		return players.Values.ToArray();
    }

	//void OnGUI ()
	//{
	//    GUILayout.BeginArea(new Rect(200, 200, 200, 500));
	//    GUILayout.BeginVertical();

	//    foreach (string _playerID in players.Keys)
	//    {
	//        GUILayout.Label(_playerID + "  -  " + players[_playerID].transform.name);
	//    }

	//    GUILayout.EndVertical();
	//    GUILayout.EndArea();
	//}

	#endregion

}
//https://finalproject-fb30d.firebaseio.com/try/Death
