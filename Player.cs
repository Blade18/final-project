using UnityEngine.Networking;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerNet))]
public class Player : NetworkBehaviour
{

    [SyncVar]
    private bool isdead = false;
    public bool IsDead
    {
        get { return isdead; }
        protected set { isdead = value; }
    }
    
    [SerializeField]
    private float MaxHealth = 100f;

    [SyncVar]
    private float currentHealth;

    [SerializeField]
    private Behaviour[] disableonDeath;
    private bool[] wasEnable;

    [SerializeField]
    private GameObject[] disableGameObjectsOnDeath;

    [SerializeField]
    private GameObject deathEffect;

    [SerializeField]
    private GameObject spawnEffect;

    private bool firstSetup = true;
    private bool respown = true;

    [SyncVar]
    public int kills = 0;

    [SyncVar]
    public int deaths = 0;

    [SyncVar]
    public string _playerName = "";

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            kills++;
        }
    }
    
    public float GetHealthPct()
    {
        return (float)currentHealth / MaxHealth;
    }

    public void SetupPlayer()
    {
        if (isLocalPlayer)
        {
            //Switch cameras
            GameManager.instance.SetSceneCameraActive(false);
            GetComponent<PlayerNet>().playerUIInstance.SetActive(true);
        }

        CmdBroadCastNewPlayerSetup();
    }

    [Command]
    private void CmdBroadCastNewPlayerSetup()
    {
        RpcSetupPlayerOnAllClients();
    }

    [ClientRpc]
    private void RpcSetupPlayerOnAllClients()
    {
        if (firstSetup)
        {
            wasEnable = new bool[disableonDeath.Length];
            for (int i = 0; i < wasEnable.Length; i++)
            {
                wasEnable[i] = disableonDeath[i].enabled;
            }
            firstSetup = false;
        }
        SetDefaults();
    }

    [ClientRpc]
    public void RpcTakeDamage(float amount, string killer_Name)
    {
        if (IsDead)
            return;

        currentHealth -= amount;
        Debug.Log(transform.name + " have: " + currentHealth + " health left");
        if (currentHealth <= 0f)
        {
            Die(killer_Name);
        }
    }

    private void Die(string killer_Name)
    {

        if (isLocalPlayer)
        {
            GameManager.PlusDie();
            CmddeathCount(transform.name);
        }
        Player _player = GameManager.GetPlayer(killer_Name);
        _player.killcount(killer_Name);
        if (_player != null)
        {
            GameManager.instance.onPlayerKilledCallback.Invoke(_playerName, _player._playerName);
        }

        IsDead = true;

        //Disable components
        for (int i = 0; i < disableonDeath.Length; i++)
        {
            disableonDeath[i].enabled = false;
        }

        //Disable GameObjects
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(false);
        }

        //Disable the collider
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        //Spawn a death effect
        GameObject _gfxIns = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(_gfxIns, 3f);

        //Switch cameras
        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(true);
            GetComponent<PlayerNet>().playerUIInstance.SetActive(false);
        }

        Debug.Log(transform.name + " is dead");

        StartCoroutine(Respown(killer_Name));
    }

    private IEnumerator Respown(string killer_Name)
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);

        Transform Spawnpoint = NetworkManager.singleton.GetStartPosition();
        transform.position = Spawnpoint.position;
        transform.rotation = Spawnpoint.rotation;

        yield return new WaitForSeconds(0.1f);
        respown = true;
        SetupPlayer();

        Debug.Log(transform.name + " respawned");
        
    }


    [Command]
    void CmddeathCount(string playerID)
    {
        Player player = GameManager.GetPlayer(playerID);
        if (player != null)
        {
            player.deaths++;
        }
    }

    public void killcount(string killer_Name)
    {
        if (isLocalPlayer)
        { 
            GameManager.PlusKill();
            Cmdkillcount(killer_Name);
        }
    }


    [Command]
    void Cmdkillcount(string playerID)
    {
        Player player = GameManager.GetPlayer(playerID);
        if (player != null)
        {
            player.kills++;
        }
    }
    public void SetDefaults()
    {
        IsDead = false;

        currentHealth = MaxHealth;


        //Enable the components
        for (int i = 0; i < disableonDeath.Length; i++)
        {
            disableonDeath[i].enabled = wasEnable[i];
        }

        //Enable the gameobjects
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(true);
        }

        //Enable the collider
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }

        //Create spawn effect
        GameObject _gfxIns = (GameObject)Instantiate(spawnEffect, transform.position, Quaternion.identity);
        Destroy(_gfxIns, 3f);
        if (isLocalPlayer)
        {
            if (!respown)
                return;
            respown = false;
            CmdRes(GameManager.playerName);
        }
    }

    [Command]
    void CmdRes(string playername)
    {
        RpcRes(playername);
    }

    [ClientRpc]
    void RpcRes(string playername)
    {
        GameManager.instance.onPlayerJoinCallback.Invoke(playername);
    }
}
