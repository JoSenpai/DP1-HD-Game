using UnityEngine;
using System.Collections;
using System;

public class ObjectPoolerManager : MonoBehaviour {

    //we'll need pools for arrows and audio objects
    public ObjectPooler ArrowPooler;
	public ObjectPooler IceArrowPooler;
	public ObjectPooler FireballPooler;
    public ObjectPooler AudioPooler;

    public GameObject ArrowPrefab;
	public GameObject IceArrowPrefab;
	public GameObject FireballPrefab;


    //basic singleton implementation
    public static ObjectPoolerManager Instance {get;private set;}
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //just instantiate the pools
        if (ArrowPooler == null)
        {
            GameObject go = new GameObject("ArrowPooler");
            ArrowPooler = go.AddComponent<ObjectPooler>();
            ArrowPooler.PooledObject = ArrowPrefab;
            go.transform.parent = this.gameObject.transform;
            ArrowPooler.Initialize();
        }

		if (IceArrowPooler == null)
		{
			GameObject go = new GameObject("FireballPooler");
			FireballPooler = go.AddComponent<ObjectPooler>();
			FireballPooler.PooledObject = FireballPrefab;
			go.transform.parent = this.gameObject.transform;
			FireballPooler.Initialize();
		}

		if (IceArrowPooler == null)
		{
			GameObject go = new GameObject("IceArrowPooler");
			IceArrowPooler = go.AddComponent<ObjectPooler>();
			IceArrowPooler.PooledObject = IceArrowPrefab;
			go.transform.parent = this.gameObject.transform;
			IceArrowPooler.Initialize();
		}

        if (AudioPooler == null)
        {
            GameObject go = new GameObject("AudioPooler");
            AudioPooler = go.AddComponent<ObjectPooler>();
            go.transform.parent = this.gameObject.transform;
            AudioPooler.Initialize(typeof(AudioSource));
        }

        
    }

}
