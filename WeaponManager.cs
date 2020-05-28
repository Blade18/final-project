using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class WeaponManager : NetworkBehaviour {

	[SerializeField]
	private string weaponLayerName = "Weapon";

	[SerializeField]
	private Transform weaponHolder;

	[SerializeField]
	private PlayerWeapon primaryWeapon;

	[SerializeField]
	public Animator anim;

	[SerializeField]
	GameObject glock, ak;


	private PlayerWeapon currentWeapon;
	private WeaponGraphics currentGraphics;

	public bool isReloading = false;
	public int akbullets = 15;
	public int glockbullets = 8;

	public void Update()
	{
		if (isLocalPlayer)
		{
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				if (string.Equals(currentWeapon.name, "AK"))
					return;
				Debug.Log("switch");
				glockbullets = currentWeapon.bullets;
				currentWeapon.name = "AK";
				currentWeapon.damage = 8f;
				currentWeapon.range = 120f;
				currentWeapon.fireRate = 8f;
				currentWeapon.bullets = akbullets;
				currentWeapon.maxBullets = 15;
				currentWeapon.reloadTime = 2f;
				CmdOnSwitchK();
			}

			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				if (string.Equals(currentWeapon.name, "Glock"))
					return;
				Debug.Log("switch2");
				akbullets = currentWeapon.bullets;
				currentWeapon.name = "Glock";
				currentWeapon.damage = 20f;
				currentWeapon.range = 100f;
				currentWeapon.fireRate = 0f;
				currentWeapon.bullets = glockbullets;
				currentWeapon.maxBullets = 8;
				currentWeapon.reloadTime = 1f;
				CmdOnSwitchG();
			}
		}
	}

	
	//Is called on the server when a player switch gun
	[Command]
	void CmdOnSwitchG()
	{
		RpcDoSwitch();
	}

	//Is called on all clients when we need to to switch gun
	[ClientRpc]
	void RpcDoSwitch()
	{
		glock.SetActive(true);
		ak.SetActive(false);
		currentGraphics = glock.GetComponent<WeaponGraphics>();
		if (currentGraphics == null)
			Debug.LogError("No WeaponGraphics component on the weapon object: " + glock.name);
	}


	//Is called on the server when a player switch gun
	[Command]
	void CmdOnSwitchK()
	{
		RpcDoSwitch2();
	}

	//Is called on all clients when we need to to switch gun
	[ClientRpc]
	void RpcDoSwitch2()
	{
		glock.SetActive(false);
		ak.SetActive(true);
		currentGraphics = ak.GetComponent<WeaponGraphics>();
		if (currentGraphics == null)
			Debug.LogError("No WeaponGraphics component on the weapon object: " + ak.name);
	}
	

	void Start ()
	{
		EquipWeapon(primaryWeapon);
	}

	
	public PlayerWeapon GetCurrentWeapon ()
	{
		return currentWeapon;
	}
	public WeaponGraphics GetCurrentGraphics()
	{
		return currentGraphics;
	}

	void EquipWeapon (PlayerWeapon _weapon)
	{

		currentWeapon = _weapon;

		//ak
		GameObject _weaponIns2 = (GameObject)Instantiate(_weapon.graphics2, weaponHolder.position, weaponHolder.rotation);
		ak = _weaponIns2;
		_weaponIns2.transform.SetParent(weaponHolder);

		currentGraphics = _weaponIns2.GetComponent<WeaponGraphics>();
		if (currentGraphics == null)
			Debug.LogError("No WeaponGraphics component on the weapon object: " + _weaponIns2.name);

		if (isLocalPlayer)
			Util.SetLayerRecursively(_weaponIns2, LayerMask.NameToLayer(weaponLayerName));

		//glock

		GameObject _weaponIns = (GameObject)Instantiate(_weapon.graphics, weaponHolder.position, weaponHolder.rotation);
		glock = _weaponIns;
		_weaponIns.transform.SetParent(weaponHolder);

		if (isLocalPlayer)
			Util.SetLayerRecursively(_weaponIns, LayerMask.NameToLayer(weaponLayerName));

		glock.SetActive(false);
	}


	
	public void Reload ()
	{
		if (isReloading)
			return;

		StartCoroutine(Reload_Coroutine());
	}

	private IEnumerator Reload_Coroutine ()
	{
		Debug.Log("Reloading...");

		isReloading = true;

		CmdOnReload();

		yield return new WaitForSeconds(currentWeapon.reloadTime);

		currentWeapon.bullets = currentWeapon.maxBullets;

		isReloading = false;
	}

	[Command]
	void CmdOnReload ()
	{
		RpcOnReload();
	}

	[ClientRpc]
	void RpcOnReload ()
	{
		if (anim != null)
		{
			anim.SetTrigger("Reload");
		}
	}
	
}
