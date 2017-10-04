using UnityEngine;
using System.Collections;
using System.Linq;
using Assets.Scripts;

public class Dragon : Tower
{
	//arrow sound found here
	//https://www.freesound.org/people/Erdie/sounds/65734/

	public Transform ArrowSpawnPosition;
	public GameObject FireballPrefab;
	private GameObject display;
	public GameObject rangeIndicator;

	//Use this for initialization
	void Start()
	{
		ShootRange = 8;
		AtkSpeed = 6f;
		upgradeManager = FindObjectOfType<UpgradeManager>();
		State = BunnyState.Inactive;
		//find where we're shooting from
		ArrowSpawnPosition = transform.Find("ArrowSpawnPosition");
		CircleDrawer.radius = 7f;
		display = Instantiate (rangeIndicator, this.transform);
	}

	/// <summary>
	/// When the mouse hover the Dragon, this method
	/// will be called and shoot range will be displayed.
	/// </summary>
	public void OnMouseEnter()
	{
		display.SetActive (true);
	}

	/// <summary>
	/// This method will be continuously called when the mouse
	/// is on top of the Dragon. On left click, call ShowUpgrade 
	/// to display the upgrade panel
	/// </summary>
	public void OnMouseOver ()
	{
		if (Input.GetMouseButtonUp (0)) {
			Debug.Log ("Dragon clicked");
			AudioManager.Instance.PlayDragonClick ();
			upgradeManager.ShowUpgrade (this);
		}
	}

	/// <summary>
	/// This method will be called when the mouse is 
	/// not over the Dragon anymore and the range display
	/// will be removed.
	/// </summary>
	public void OnMouseExit()
	{
		display.SetActive (false);
	}
	// Update is called once per frame
	void Update()
	{
		//if we're in the last round and we've killed all enemies, do nothing
		if (GameManager.Instance.FinalRoundFinished &&
			GameManager.Instance.Enemies.Where(x => x != null).Count() == 0)
			State = BunnyState.Inactive;

		//searching for an enemy
		if (State == BunnyState.Searching)
		{
			if (GameManager.Instance.Enemies.Where(x => x != null).Count() == 0) return;

			//find the closest enemy
			//aggregate method proposed here
			//http://unitygems.com/linq-1-time-linq/
			targetedEnemy = GameManager.Instance.Enemies.Where(x => x != null)
				.Aggregate((current, next) => Vector2.Distance(current.transform.position, transform.position)
					< Vector2.Distance(next.transform.position, transform.position)
					? current : next);

			//if there is an enemy and is close to us, target it
			if (targetedEnemy != null && targetedEnemy.activeSelf
				&& Vector3.Distance(transform.position, targetedEnemy.transform.position)
				< ShootRange)
			{
				State = BunnyState.Targeting;
			}

		}
		else if (State == BunnyState.Targeting)
		{
			//if the targeted enemy is still close to us, look at it and shoot!
			if (targetedEnemy != null 
				&& Vector3.Distance(transform.position, targetedEnemy.transform.position)
				< ShootRange)
			{
				LookAndShoot();
			}
			else //enemy has left our shooting range, so look for another one
			{
				State = BunnyState.Searching;
			}
		}
	}

	public override void LookAndShoot()
	{
		//look at the enemy
		Quaternion diffRotation = Quaternion.LookRotation
			(transform.position - targetedEnemy.transform.position, Vector3.forward);
		transform.rotation = Quaternion.RotateTowards
			(transform.rotation, diffRotation, Time.deltaTime * 2000);
		transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);

		//make sure we're almost looking at the enemy before start shooting
		Vector2 direction = targetedEnemy.transform.position - transform.position;
		float axisDif = Vector2.Angle(transform.up, direction);
		//shoot only if we have 20 degrees rotation difference to the enemy
		if (axisDif <= 20f)
		{
			if (Time.time - LastShootTime > AtkSpeed)
			{
				Shoot(direction);
				LastShootTime = Time.time;
			}

		}
	}


	public override void Shoot(Vector2 dir)
	{
		//if the enemy is still close to us
		if (targetedEnemy != null && targetedEnemy.activeSelf
			&& Vector3.Distance(transform.position, targetedEnemy.transform.position)
			< ShootRange)
		{
			//create a new arrow
			GameObject go = ObjectPoolerManager.Instance.FireballPooler.GetPooledObject();
			go.transform.position = ArrowSpawnPosition.position;
			go.transform.rotation = transform.rotation;
			go.SetActive(true);
			//SHOOT IT!
			go.GetComponent<Rigidbody2D>().AddForce(dir * InitialArrowForce);
			AudioManager.Instance.PlayFireballSound();
		}
		else//find another enemy
		{
			State = BunnyState.Searching;
		}


	}

	private BunnyState State;


	public void Activate()
	{
		State = BunnyState.Searching;
	}	
}


