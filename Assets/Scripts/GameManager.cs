using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using System.Collections;
using System;

public class GameManager : MonoBehaviour
{
    //basic singleton implementation
    [HideInInspector]
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    //sprites can be found here: 
    //http://www.gameartguppy.com/shop/top-tower-defense-bunny-badgers-game-art-set/

    //enemies on screen
    public List<GameObject> Enemies;
    //prefabs
    public GameObject EnemyPrefab;
	//EXTRA ENEMY
	public GameObject EnemyPrefab2;
	public GameObject EnemyPrefab3;
	public GameObject EnemyBoss;
    public GameObject PathPrefab;
    public GameObject TowerPrefab;
    //list of waypoints in the current level
    public Transform[] Waypoints;
    private GameObject PathPiecesParent;
    private GameObject WaypointsParent;
    //file pulled from resources
    private LevelStuffFromXML levelStuffFromXML;
    //will spawn carrots on screen
    public CarrotSpawner CarrotSpawner;

    //helpful variables for our player
    [HideInInspector]
    public int MoneyAvailable { get; set; }
    [HideInInspector]
    public float MinCarrotSpawnTime;
    [HideInInspector]
    public float MaxCarrotSpawnTime;
    public int Lives = 10;
    private int currentRoundIndex = 0;
    [HideInInspector]
    public GameState CurrentGameState;
    public SpriteRenderer BunnyGeneratorSprite;
	public SpriteRenderer DragonGeneratorSprite;
	public SpriteRenderer RiderGeneratorSprite;
    [HideInInspector]
    public bool FinalRoundFinished;
    public GUIText infoText;
	public GameObject coins;
    private object lockerObject = new object();

    // Use this for initialization
    void Start()
    {
        IgnoreLayerCollisions();

        Enemies = new List<GameObject>();
        PathPiecesParent = GameObject.Find("PathPieces");
        WaypointsParent = GameObject.Find("Waypoints");
        levelStuffFromXML = Utilities.ReadXMLFile();

        CreateLevelFromXML();

        CurrentGameState = GameState.Start;

        FinalRoundFinished = false;
    }

    /// <summary>
    /// Will create necessary stuff from the object that has the XML stuff
    /// </summary>
    private void CreateLevelFromXML()
    {
        foreach (var position in levelStuffFromXML.Paths)
        {
            GameObject go = Instantiate(PathPrefab, position, 
                Quaternion.identity) as GameObject;
            go.GetComponent<SpriteRenderer>().sortingLayerName = "Path";
            go.transform.parent = PathPiecesParent.transform;
        }

        for (int i = 0; i < levelStuffFromXML.Waypoints.Count; i++)
        {
            GameObject go = new GameObject();
            go.transform.position = levelStuffFromXML.Waypoints[i];
            go.transform.parent = WaypointsParent.transform;
            go.tag = "Waypoint";
            go.name = "Waypoints" + i.ToString();
        }

        GameObject tower = Instantiate(TowerPrefab, levelStuffFromXML.Tower,
            Quaternion.identity) as GameObject;
        tower.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";

        Waypoints = GameObject.FindGameObjectsWithTag("Waypoint")
            .OrderBy(x => x.name).Select(x => x.transform).ToArray();

        MoneyAvailable = levelStuffFromXML.InitialMoney;
        MinCarrotSpawnTime = levelStuffFromXML.MinCarrotSpawnTime;
        MaxCarrotSpawnTime = levelStuffFromXML.MaxCarrotSpawnTime;
    }

    /// <summary>
    /// Will make the arrow collide only with enemies!
    /// </summary>
    private void IgnoreLayerCollisions()
    {
		List<int> arrowIDs = new List<int> ();
		arrowIDs.Add (LayerMask.NameToLayer("Arrow"));
		arrowIDs.Add (LayerMask.NameToLayer("IceArrow"));
		arrowIDs.Add (LayerMask.NameToLayer("Fireball"));

		List<int> towerIDs = new List<int> ();
		towerIDs.Add (LayerMask.NameToLayer("Bunny"));
		towerIDs.Add (LayerMask.NameToLayer("Dragon"));
		towerIDs.Add (LayerMask.NameToLayer("Rider"));

		List<int> towerGeneratorIDs = new List<int> ();
		towerGeneratorIDs.Add (LayerMask.NameToLayer("BunnyGenerator"));
		towerGeneratorIDs.Add (LayerMask.NameToLayer("DragonGenerator"));
		towerGeneratorIDs.Add (LayerMask.NameToLayer("RiderGenerator"));
        
        int enemyLayerID = LayerMask.NameToLayer("Enemy");
        int backgroundLayerID = LayerMask.NameToLayer("Background");
        int pathLayerID = LayerMask.NameToLayer("Path");
        int towerLayerID = LayerMask.NameToLayer("Tower");
        int carrotLayerID = LayerMask.NameToLayer("Carrot");

		if (towerGeneratorIDs.Count != 0) {
			foreach (int i in towerGeneratorIDs)
				Physics2D.IgnoreLayerCollision (i, enemyLayerID); //Tower and Enemy (when dragging the tower)
		}

		if (arrowIDs.Count != 0) {
			foreach (int i in arrowIDs) {
				Console.WriteLine (i);
				Physics2D.IgnoreLayerCollision (i, backgroundLayerID); //Arrow and Background
				Physics2D.IgnoreLayerCollision (i, pathLayerID); //Arrow and Path
				Physics2D.IgnoreLayerCollision (i, towerLayerID); //Arrow and Tower
				Physics2D.IgnoreLayerCollision (i, carrotLayerID); //Arrow and Carrot

				foreach (int j in towerIDs)
					Physics2D.IgnoreLayerCollision (i, j); //Arrow and Tower

				foreach (int k in towerGeneratorIDs)
					Physics2D.IgnoreLayerCollision (i, k); //Arrow and TowerGenerator
			}  
		}
    }



    IEnumerator NextRound()
    {
        //give the player 2 secs to do stuff
        yield return new WaitForSeconds(2f);
        //get a reference to the next round details
        Round currentRound = levelStuffFromXML.Rounds[currentRoundIndex];
        for (int i = 0; i < currentRound.NoOfEnemies; i++)
        {//spawn a new enemy

			if (currentRoundIndex != 9) {
				
				//ADDITIONAL CODE
				if (i == 2 || i == 5 || i == 7 || i == 9) {
					GameObject enemy2 = Instantiate (EnemyPrefab2, Waypoints [0].position, Quaternion.identity) as GameObject;
					Enemy enemyComponent2 = enemy2.GetComponent<Enemy> ();
					enemyComponent2.Speed += Mathf.Clamp (currentRoundIndex, 1f, 2f);
					enemyComponent2.EnemyKilled += OnEnemyKilled;
					Enemies.Add (enemy2);
				}

				if (i == 1 || i == 4 || i == 6 || i == 8) {
					GameObject enemy3 = Instantiate (EnemyPrefab3, Waypoints [0].position, Quaternion.identity) as GameObject;
					Enemy enemyComponent3 = enemy3.GetComponent<Enemy> ();
					enemyComponent3.Speed += Mathf.Clamp (currentRoundIndex, 3f, 6f);
					enemyComponent3.EnemyKilled += OnEnemyKilled;
					Enemies.Add (enemy3);
				}

				GameObject enemy = Instantiate (EnemyPrefab, Waypoints [0].position, Quaternion.identity) as GameObject;
				Enemy enemyComponent = enemy.GetComponent<Enemy> ();

				//set speed and enemyKilled handler
				enemyComponent.Speed += Mathf.Clamp (currentRoundIndex, 1f, 5f);
				enemyComponent.EnemyKilled += OnEnemyKilled;

				//add it to the list and wait till you spawn the next one
				Enemies.Add (enemy);
			} else {
				GameObject boss = Instantiate (EnemyBoss, Waypoints [0].position, Quaternion.identity) as GameObject;
				Enemy bossComponent = boss.GetComponent<Enemy> ();
				bossComponent.Speed += Mathf.Clamp (currentRoundIndex, 1f, 1f);
				bossComponent.EnemyKilled += OnEnemyKilled;
				Enemies.Add (boss);
				break;
			}

            yield return new WaitForSeconds(1f / (currentRoundIndex == 0 ? 1 : currentRoundIndex));
        }

    }

    /// <summary>
    /// Handler for the enemy killed event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void OnEnemyKilled(object sender, EventArgs e)
    {
		bool startNewRound = false;
        //explicit lock, since this may occur any time by any enemy
        //not 100% that this is needed, but better safe than sorry!
        lock (lockerObject)
        {
            if (Enemies.Where(x => x != null).Count() == 0 && CurrentGameState == GameState.Playing)
            {
                startNewRound = true;
            }
        }
        if (startNewRound)
            CheckAndStartNewRound();
    }

    /// <summary>
    /// Starts a new round (if available) and sets the FinalRound flag
    /// </summary>
    private void CheckAndStartNewRound()
    {
        if (currentRoundIndex < levelStuffFromXML.Rounds.Count - 1)
        {
            currentRoundIndex++;
            StartCoroutine(NextRound());
        }
        else
        {
            FinalRoundFinished = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (CurrentGameState)
        {
            //start state, on tap, start the game and spawn carrots!
            case GameState.Start:
                if (Input.GetMouseButtonUp(0))
                {
                    CurrentGameState = GameState.Playing;
                    StartCoroutine(NextRound());
                    CarrotSpawner.StartCarrotSpawning();
                }
                break;
            case GameState.Playing:
                if (Lives == 0) //we lost
                {
                    //no more rounds
                    StopCoroutine(NextRound());
                    DestroyExistingEnemiesAndCarrots();
                    CarrotSpawner.StopCarrotSpawning();
                    CurrentGameState = GameState.Lost;
                }
                else if (FinalRoundFinished && Enemies.Where(x => x != null).Count() == 0)
                {
                    DestroyExistingEnemiesAndCarrots();
                    CarrotSpawner.StopCarrotSpawning();
                    CurrentGameState = GameState.Won;
                }
                break;
            case GameState.Won:
                if (Input.GetMouseButtonUp(0))
                {//restart
                    Application.LoadLevel(Application.loadedLevel);
                }
                break;
            case GameState.Lost:
                if (Input.GetMouseButtonUp(0))
                {//restart
                    Application.LoadLevel(Application.loadedLevel);
                }
                break;
            default:
                break;
        }
    }

    private void DestroyExistingEnemiesAndCarrots()
    {
        //get all the enemies
        foreach (var item in Enemies)
        {
            if (item != null)
                Destroy(item.gameObject);
        }
        //get all the carrots
        var carrots = GameObject.FindGameObjectsWithTag("Carrot");
        foreach (var item in carrots)
        {
            Destroy(item);
        }
    }

    /// <summary>
    /// Increase or decrease money available
    /// </summary>
    /// <param name="money"></param>
    public void AlterMoneyAvailable(int money)
    {
        MoneyAvailable += money;
        //we're also modifying the BunnyGenerator alpha color
        //yeah, I know, I could use an event for that, next time!
        if (MoneyAvailable < Constants.BunnyCost)
        {
            Color temp = BunnyGeneratorSprite.color;
            temp.a = 0.3f;
            BunnyGeneratorSprite.color = temp;
        }
        else
        {
            Color temp = BunnyGeneratorSprite.color;
            temp.a = 1.0f;
            BunnyGeneratorSprite.color = temp;
        }

		if (MoneyAvailable < Constants.DragonCost)
		{
			Color temp = DragonGeneratorSprite.color;
			temp.a = 0.3f;
			DragonGeneratorSprite.color = temp;
		}
		else
		{
			Color temp = DragonGeneratorSprite.color;
			temp.a = 1.0f;
			DragonGeneratorSprite.color = temp;
		}

		if (MoneyAvailable < Constants.RiderCost)
		{
			Color temp = RiderGeneratorSprite.color;
			temp.a = 0.3f;
			RiderGeneratorSprite.color = temp;
		}
		else
		{
			Color temp = BunnyGeneratorSprite.color;
			temp.a = 1.0f;
			RiderGeneratorSprite.color = temp;
		}
    }

    /// <summary>
    /// Show GUI stuff with the deprecated way
    /// Long live Unity 4.6!
    /// </summary>
    void OnGUI()
    {
        Utilities.AutoResize(800, 480);
        switch (CurrentGameState)
        {
			case GameState.Start:
				infoText.text = "Tap to start!";
				coins.SetActive (false);
                break;
			case GameState.Playing:
				coins.SetActive (true);
                infoText.text = "Money:    " + MoneyAvailable.ToString() + "\n"
                    + "Life: " + Lives.ToString() + "\n" +
                    string.Format("round {0} of {1}", currentRoundIndex + 1, levelStuffFromXML.Rounds.Count);
                break;
            case GameState.Won:
                infoText.text = "Won! Tap to restart!";
                break;
            case GameState.Lost:
                infoText.text = "Lost :( Tap to restart!";
                break;
            default:
                break;
        }
    }
}
