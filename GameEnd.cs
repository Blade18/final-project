using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEnd : NetworkBehaviour
{

    private NetworkManager networkManager;
    bool isEnd = false;

    [SerializeField]
    Text EndText;

    [SerializeField]
    GameObject canvas;

    void Start()
    {
        networkManager = NetworkManager.singleton;
    }

    public void Update()
    {
        if (isEnd)
            return;
        Player[] players = GameManager.GetAllPlayers();

        foreach (Player player in players)
        {
            if (player.kills == 30)
            {
                isEnd = true;
                EndGame(player._playerName);
            }
        }
    }

    public void EndGame(string playername)
    {
        Player[] players = GameManager.GetAllPlayers();

        foreach (Player player in players)
        {
            canvas.SetActive(true);
            CmdUI(playername);
            StartCoroutine(wait());
        }
    }
    private IEnumerator wait()
    {
        yield return new WaitForSeconds(4);
        CmdEND();
    }


    [Command]
    void CmdEND()
    {
        RpcEnd();
    }

    [ClientRpc]
    void RpcEnd()
    {
        MatchInfo matchInfo = networkManager.matchInfo;
        networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
        networkManager.StopHost();
    }

    [Command]
    void CmdUI(string playername)
    {
        RpcUI(playername);
    }

    [ClientRpc]
    void RpcUI(string playername)
    {
        Debug.Log("just tell you who won " + playername);
        EndText.text = playername + " WON";
    }
    
}
