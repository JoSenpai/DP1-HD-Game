using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using Assets.Scripts;

public class TowerInfoDisplay : MonoBehaviour {

	public GameObject infoBox;
	public Text towerName;
	public Text towerPrice;
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.mousePosition.y >= 5 && Input.mousePosition.y <= 97) {
			// If the mouse is on top of Rider
			if (Input.mousePosition.x >= 110 && Input.mousePosition.x <= 205) {
				Vector2 temp = new Vector2 (110, 205);
				infoBox.transform.position = temp;
				towerPrice.text = "Cost: 75";
				infoBox.GetComponentInChildren<Text> ().text = "Attack Modifier: Slow\nDamage: 35\nAttack Speed: 2.5\nRange: 5";
				towerName.text = "Rider";

				infoBox.SetActive (true);
				Debug.Log ("Mouse On top of rider");
			} 
			//if the mouse is on top of Dragon
			else if (Input.mousePosition.x >= 230 && Input.mousePosition.x <= 325) {
				Vector2 temp = new Vector2 (230, 205);
				infoBox.transform.position = temp;
				towerPrice.text = "Cost: 100";
				infoBox.GetComponentInChildren<Text> ().text = "Attack Modifier: Normal\nDamage: 100\nAttack Speed: 6\nRange: 8";
				towerName.text = "Dragon";

				infoBox.SetActive (true);
				Debug.Log ("Mouse On top of dragons");
			} 
			//If the mouse is on top of Bunny
			else if (Input.mousePosition.x >= 350 && Input.mousePosition.x <= 435) {
				Vector2 temp = new Vector2 (310, 205);
				infoBox.transform.position = temp;
				towerPrice.text = "Cost: 50";
				infoBox.GetComponentInChildren<Text> ().text = "Attack Modifier: Normal\nDamage: 20\nAttack Speed: 1\nRange: 3";
				towerName.text = "Bunny";

				infoBox.SetActive (true);
				Debug.Log ("Mouse On top of bunny");
			}
			//If it is not on top of any tower generator
			else
				infoBox.SetActive (false);
		} else 
			infoBox.SetActive (false);
	}
}
