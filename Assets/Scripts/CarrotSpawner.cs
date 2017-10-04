using UnityEngine;
using System.Collections;

public class CarrotSpawner : MonoBehaviour {

	/// <summary>
	/// Carrot prefab
	/// </summary>
	public GameObject Carrot;
	public GameObject purpleCarrot;
	public GameObject blackCarrot;
	static int x = 0;

	public void StartCarrotSpawning()
	{
		StartCoroutine(SpawnCarrots());
	}

	public void StopCarrotSpawning()
	{
		StopAllCoroutines();
	}
	private IEnumerator SpawnCarrots()
	{
		while (true)
		{
	
			//select a random position
			float X = Random.Range(100, Screen.width - 100);
			Vector3 randomPosition = Camera.main.ScreenToWorldPoint(new Vector3(X, 0, 0));
			//create and drop a carrot
			GameObject carrot = Instantiate(Carrot,
				new Vector3(randomPosition.x, transform.position.y, transform.position.z),
				Quaternion.identity) as GameObject;
			carrot.GetComponent<Carrot>().FallSpeed = Random.Range(1f, 3f);

			if (x % 2 == 0) {
				GameObject carrot2 = Instantiate (purpleCarrot,
					                    new Vector3 (randomPosition.x + 2, transform.position.y + 5, transform.position.z),
					                    Quaternion.identity) as GameObject;
				carrot2.GetComponent<Carrot> ().FallSpeed = Random.Range (3f, 4f);

			}

			if (x % 5 == 0) {
				GameObject carrot3 = Instantiate (blackCarrot,
					new Vector3 (randomPosition.x + 3, transform.position.y + 2, transform.position.z),
					Quaternion.identity) as GameObject;
				carrot3.GetComponent<Carrot> ().FallSpeed = Random.Range (1f, 3f);

			}
			x = x + 1;
			//wait for random seconds, based on level parameters
			yield return new WaitForSeconds
				(Random.Range(GameManager.Instance.MinCarrotSpawnTime, 
				GameManager.Instance.MaxCarrotSpawnTime));
		}
	}
	

}
