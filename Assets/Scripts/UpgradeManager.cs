using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using Assets.Scripts;

public class UpgradeManager : MonoBehaviour {

	public GameObject uBox;
	public Button range;
	public Button damage;
	public Button atkSpeed;
	public Text towerStat;
	private Tower clickedTower;

	bool managerActive = false;
	float atkSpeedUpgradeVal;
	float rangeUpgradeVal;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(clickedTower != null)
			UpdatePriceandStatusDisplay ();

		//If the range button is clicked
		if (Input.GetMouseButtonUp (0)) {
			range.onClick.RemoveAllListeners ();
			range.onClick.AddListener (UpgradeRange);
		}
		//If the Attack Speed button is clicked
		if (Input.GetMouseButtonUp (0)) {
			atkSpeed.onClick.RemoveAllListeners ();
			atkSpeed.onClick.AddListener (UpgradeAtkSpeed);
		}
		//If the Damage button is clicked
		if (Input.GetMouseButtonUp (0)) {
			damage.onClick.RemoveAllListeners ();
			damage.onClick.AddListener (UpgradeDamage);
		}
		//If the mouse is not over the upgrade panel
		if ( managerActive && Input.GetMouseButtonDown(0) && Input.mousePosition.x > 252 ) {
			uBox.SetActive (false);
			managerActive = false;
		}
	}

	/// <summary>
	/// Shows the upgrade panel
	/// </summary>
	/// <param name="twr">Twr.</param>
	public void ShowUpgrade(Tower twr)
	{
		clickedTower = twr;

		//For each different tower, the upgrade value will be different
		if (clickedTower.name == "Bunny(Clone)") {
			atkSpeedUpgradeVal = 0.15f;
			rangeUpgradeVal = 0.4f;
		} 
		else if (clickedTower.name == "Dragon(Clone)") {
			atkSpeedUpgradeVal = 0.8f;
			rangeUpgradeVal = 0.2f;
		} 
		else if (clickedTower.name == "Rider(Clone)") {
			atkSpeedUpgradeVal = 0.3f;
			rangeUpgradeVal = 0.4f;
		}
		uBox.SetActive (true);
		managerActive = true;
	}

	/// <summary>
	/// Update the price for upgrades and the tower information
	/// </summary>
	public void UpdatePriceandStatusDisplay()
	{
		range.GetComponentInChildren<Text> ().text = clickedTower.RangeUpgradePrice.ToString();
		atkSpeed.GetComponentInChildren<Text> ().text = clickedTower.AtkSpeedUpgradePrice.ToString();
		if (clickedTower.name == "Bunny(Clone)") {
			damage.GetComponentInChildren<Text> ().text = Constants.DamageUpgradeBunny.ToString();
			towerStat.text = "Damage: " + Constants.ArrowDamage + "\nAttack Speed: " + clickedTower.AtkSpeed.ToString() + "\nRange: " + clickedTower.ShootRange.ToString();
		} 
		else if (clickedTower.name == "Dragon(Clone)") {
			damage.GetComponentInChildren<Text>().text = Constants.DamageUpgradeDragon.ToString();
			towerStat.text = "Damage: " + Constants.FireballDamage + "\nAttack Speed: " + clickedTower.AtkSpeed.ToString() + "\nRange: " + clickedTower.ShootRange.ToString();
		} 
		else if (clickedTower.name == "Rider(Clone)") {
			towerStat.text = "Damage: " + Constants.IceArrowDamage + "\nAttack Speed: " + clickedTower.AtkSpeed.ToString() + "\nRange: " + clickedTower.ShootRange.ToString();
			damage.GetComponentInChildren<Text>().text = Constants.DamageUpgradeRaider.ToString();
		}

	}

	/// <summary>
	/// Upgrade the range.
	/// </summary>
	private void UpgradeRange()
	{
		if (GameManager.Instance.MoneyAvailable >= clickedTower.RangeUpgradePrice) {
			clickedTower.ShootRange += rangeUpgradeVal;
			GameManager.Instance.MoneyAvailable -= clickedTower.RangeUpgradePrice;
			clickedTower.RangeUpgradePrice += clickedTower.RangeUpgradePrice;
			AudioManager.Instance.PlayNormalUpgrade ();
		} 
		else
			AudioManager.Instance.PlayFailUpgrade ();
	}

	/// <summary>
	/// Upgrades the damage.
	/// </summary>
	private void UpgradeDamage()
	{
		//For each different tower, the upgrade value and price is differnet
		if (clickedTower.name == "Bunny(Clone)" && GameManager.Instance.MoneyAvailable >= Constants.DamageUpgradeBunny) {
			Constants.ArrowDamage += 10;
			GameManager.Instance.MoneyAvailable -= Constants.DamageUpgradeBunny;
			Constants.DamageUpgradeBunny += (Constants.DamageUpgradeBunny / 2);
			AudioManager.Instance.PlayGlobalUpgrade ();
		} 
		else if (clickedTower.name == "Dragon(Clone)" && GameManager.Instance.MoneyAvailable >= Constants.DamageUpgradeDragon) {
			Constants.FireballDamage += 50;
			GameManager.Instance.MoneyAvailable -= Constants.DamageUpgradeDragon;
			Constants.DamageUpgradeDragon += Constants.DamageUpgradeDragon / 2;
			AudioManager.Instance.PlayGlobalUpgrade ();
		} 
		else if (clickedTower.name == "Rider(Clone)" && GameManager.Instance.MoneyAvailable >= Constants.DamageUpgradeRaider) {
			GameManager.Instance.MoneyAvailable -= Constants.DamageUpgradeRaider;
			Constants.IceArrowDamage += 20;
			Constants.DamageUpgradeRaider += Constants.DamageUpgradeRaider / 2;
			AudioManager.Instance.PlayGlobalUpgrade ();
		}
		else
			AudioManager.Instance.PlayFailUpgrade ();

	}

	/// <summary>
	/// Upgrades the atk speed.
	/// </summary>
	private void UpgradeAtkSpeed()
	{
		if(GameManager.Instance.MoneyAvailable >= clickedTower.AtkSpeedUpgradePrice){
			clickedTower.AtkSpeed -= atkSpeedUpgradeVal;
			GameManager.Instance.MoneyAvailable -= clickedTower.AtkSpeedUpgradePrice;
			clickedTower.AtkSpeedUpgradePrice += clickedTower.AtkSpeedUpgradePrice;
			AudioManager.Instance.PlayNormalUpgrade ();
		} 
		else
			AudioManager.Instance.PlayFailUpgrade ();
	}

}
