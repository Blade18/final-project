using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playerStats : MonoBehaviour
{
    [SerializeField]
    private Text Kill;

    [SerializeField]
    private Text Death;

    [SerializeField]
    private Text username;

    [SerializeField]
    private Text KD;
    public double kd = 0;

    public void Start()
    {
        kd = (GameManager.Killc / GameManager.Deathc);
        Kill.text = "KILLS: " + GameManager.Killc.ToString();
        Death.text = "DEATHS: " + GameManager.Deathc.ToString();
        username.text = GameManager.playerName;
        if (GameManager.Deathc == 0)
            KD.text = "K/D: " + GameManager.Killc.ToString();
        else if (kd % 1 == 0)
            KD.text = "K/D: " + kd.ToString();
        else
            KD.text = "K/D: " + kd.ToString("0.00");
    }

    public void logout()
    {
        SceneManager.LoadScene(0);
    }
}
