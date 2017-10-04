using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract  class Tower : MonoBehaviour {
	protected GameObject targetedEnemy;
	protected UpgradeManager upgradeManager;
	//protected CircleDrawer rangeIndicator;
	protected float LastShootTime = 0f;
	protected float InitialArrowForce = 500f;
	protected float _shootWaitTime;
	protected float _shootRange;
	private int rangeUpgradePrice = 25;
	private int atkSpeedUpgradePrice = 25;

	public abstract void LookAndShoot();
	public abstract void Shoot (Vector2 dir);

	public float AtkSpeed{
		get { return _shootWaitTime;}
		set { _shootWaitTime = value;}
	}
 	
	public float ShootRange{
		get { return _shootRange;}
		set { _shootRange = value;}
	}
		
	public int AtkSpeedUpgradePrice{
		get { return atkSpeedUpgradePrice;}
		set { atkSpeedUpgradePrice = value;}
	}

	public int RangeUpgradePrice{
		get { return  rangeUpgradePrice;}
		set { rangeUpgradePrice = value; }
	} 
}
