using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CircleDrawer : MonoBehaviour {
	public LineRenderer line;
	public static float radius = 3f;
	[Range(0, 256)]
	public int points = 32;
	public Vector3[] vertices;

	// Use this for initialization
	void Start () {
		vertices = new Vector3[points + 1];

		for (int i = 0; i < vertices.Length; i++) {
			float x = Mathf.Cos ((i / (float)points) * 2 * Mathf.PI);
			float y = Mathf.Sin ((i / (float)points) * 2 * Mathf.PI);
			vertices [i] = new Vector3 (x, y, -2) * radius;
		}
		vertices [vertices.Length - 1] = vertices [0];
		line.positionCount = vertices.Length;
		line.SetPositions (vertices);
	}

	// Update is called once per frame
	void Update () {
		vertices = new Vector3[points + 1];

		for (int i = 0; i < vertices.Length; i++) {
			float x = Mathf.Cos ((i / (float)points) * 2 * Mathf.PI);
			float y = Mathf.Sin ((i / (float)points) * 2 * Mathf.PI);
			vertices [i] = new Vector3 (x, y, -2) * radius;
		}
		vertices [vertices.Length - 1] = vertices [0];
		line.positionCount = vertices.Length;
		line.SetPositions (vertices);
	}
}
