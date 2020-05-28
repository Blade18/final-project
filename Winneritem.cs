using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Winneritem : MonoBehaviour
{
	[SerializeField]
	Text text;

	public void Setup(string player, int kills)
	{
		text.text = "<b>" + player + "</b>" + " with " + "<i>" + kills.ToString() + "</i>";
	}
}
