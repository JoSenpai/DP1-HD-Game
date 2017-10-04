using UnityEngine;
using System.Collections;
using Assets.Scripts;
using System;

public class Enemy : MonoBehaviour
{
    //death sound found here
    //https://www.freesound.org/people/psychentist/sounds/168567/

    public int Health;
    int nextWaypointIndex = 0;
	bool hitByIcedArrow = false;
	int slowTime = 0;
	Color originalColor;
	float originalSpeed;
    public float Speed = 1f;

    // Use this for initialization
    void Start()
    {
		originalColor = GetComponent<Enemy> ().GetComponentInChildren<SpriteRenderer> ().color;
		originalSpeed = GetComponent<Enemy> ().Speed;
    }

    // Update is called once per frame
    void Update()
    {
		//Used to calculate the time left for the slow
		//inflicted by the Rider
		if (hitByIcedArrow){
			if(slowTime > 0)
				slowTime -= 1;
			else {
				hitByIcedArrow = false;
				Speed = originalSpeed;
				GetComponent<Enemy>().GetComponentInChildren<SpriteRenderer>().color = originalColor;

			}
		}

        //calculate the distance between current position
        //and the target waypoint
        if (Vector2.Distance(transform.position,
            GameManager.Instance.Waypoints[nextWaypointIndex].position) < 0.01f)
        {
            //is this waypoint the last one?
            if (nextWaypointIndex == GameManager.Instance.Waypoints.Length - 1)
            {
                RemoveAndDestroy();
                GameManager.Instance.Lives--;
            }
            else
            {
                //our enemy will go to the next waypoint
                nextWaypointIndex++;
                //our simple AI, enemy is looking at the next waypoint
                transform.LookAt(GameManager.Instance.Waypoints[nextWaypointIndex].position,
                    -Vector3.forward);
                //only in the z axis
                transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
            }
        }
        
        //enemy is moved towards the next waypoint
        transform.position = Vector2.MoveTowards(transform.position,
            GameManager.Instance.Waypoints[nextWaypointIndex].position,
            Time.deltaTime * Speed);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
		if (col.gameObject.tag == "Arrow" || col.gameObject.tag == "IceArrow" || col.gameObject.tag == "Fireball")
        {//if we're hit by an arrow
            if (Health > 0)
            {
                //decrease enemy health
				if (col.gameObject.tag == "Arrow")
					Health -= Constants.ArrowDamage;
				else if (col.gameObject.tag == "IceArrow") {
					Health -= Constants.IceArrowDamage;
					if (!hitByIcedArrow) {
						Speed = Speed * 0.5f;
						hitByIcedArrow = true;
						slowTime = 75;
					}
				}
				else if (col.gameObject.tag == "Fireball")
					Health -= Constants.FireballDamage;
					
                if (Health <= 0)
                {
                    RemoveAndDestroy();
                }
            }
			col.gameObject.SetActive(false);

			if (hitByIcedArrow) {
				GetComponent<Enemy>().GetComponentInChildren<SpriteRenderer>().color = Color.cyan;
			}
        }
    }

    public event EventHandler EnemyKilled;

    void RemoveAndDestroy()
    {
		if (this.gameObject.tag == "Enemy") {
			AudioManager.Instance.PlayDeathSound();
		}
       
		if (this.gameObject.tag == "Golem") {
			AudioManager.Instance.PlayDeathGolem ();
		}

		if (this.gameObject.tag == "Bug") {
			AudioManager.Instance.PlayDeathBug ();
		}
        //remove it from the enemy list
        GameManager.Instance.Enemies.Remove(this.gameObject);
        Destroy(this.gameObject);
        //notify interested parties that we died
        if (EnemyKilled != null)
            EnemyKilled(this, EventArgs.Empty);
    }
}
