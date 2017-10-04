using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	//public GameObject RootGO;
	public GameObject MenuUI;
	bool playing;

	// Use this for initialization
	void Start () {
		
	}

	void Update()
	{

	}

	public void StartGame()
	{
		SceneManager.LoadScene (1);
	}

	public void Quit()
	{
		Application.Quit ();
	}

}
