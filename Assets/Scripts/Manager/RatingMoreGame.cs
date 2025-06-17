using UnityEngine;
using System.Collections;

public class RatingMoreGame : MonoBehaviour {

	public UnityEngine.UI.Image Rating;      // button rating
	public UnityEngine.UI.Image MoreGame;      // button more game
	public string package;
	public string developer;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Rating
	/// </summary>
	public void GotoPlayStore()
	{		

		Application.OpenURL ("https://play.google.com/store/apps/details?id="+"");
	}



	// <summary>
	/// More game
	/// </summary>
	public void GotoMoreGame()
	{		

		Application.OpenURL ("https://play.google.com/store/apps/details?id="+Application.companyName+"");
	}
}
