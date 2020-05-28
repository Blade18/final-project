using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

	[SerializeField]
	GameObject pauseMenu;

	[SerializeField]
	GameObject scoreboard;

	[SerializeField]
	RectTransform healthBarFill;

	[SerializeField]
	Text ammoText;

	private Player player;
	private WeaponManager weaponManager;

	public void SetPlayer(Player _player)
	{
		player = _player;
		weaponManager = player.GetComponent<WeaponManager>();
	}

	void Start ()
	{
		PauseMenu.IsOn = false;
	}

	void Update ()
	{
		SetHealthAmount(player.GetHealthPct());
		SetAmmoAmount(weaponManager.GetCurrentWeapon().bullets);

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			TogglePauseMenu();
		}

		if (Input.GetKeyDown(KeyCode.Tab))
		{
			scoreboard.SetActive(true);
		}
		else if (Input.GetKeyUp(KeyCode.Tab))
		{
			scoreboard.SetActive(false);
		}
	}

	public void TogglePauseMenu ()
	{
		pauseMenu.SetActive(!pauseMenu.activeSelf);
		PauseMenu.IsOn = pauseMenu.activeSelf;

	}

	void SetHealthAmount(float _amount)
	{
		healthBarFill.localScale = new Vector3(1f, _amount, 1f);
	}

	void SetAmmoAmount(int _amount)
	{
		ammoText.text = _amount.ToString();
	}

}
