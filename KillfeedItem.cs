using UnityEngine;
using UnityEngine.UI;

public class KillfeedItem : MonoBehaviour {

	[SerializeField]
	Text text;

	public void Setup (string player, string source)
	{
		text.text = "<b>" + source + "</b>" + " killed " + "<i>" + player + "</i>";
	}

	public void Respown(string player)
	{
		text.text = "<b>" + player + "</b>" + " just respawned ";
	}
}