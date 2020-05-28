using UnityEngine;

[System.Serializable]
public class PlayerWeapon
{

	public string name = "AK";

	public float damage = 8f;
	public float range = 120f;

	public float fireRate = 8f;

	public int maxBullets = 15;

	public int bullets;

	public float reloadTime = 2f;

	public GameObject graphics;
	public GameObject graphics2;

	public PlayerWeapon()
	{
		bullets = maxBullets;
	}

}
