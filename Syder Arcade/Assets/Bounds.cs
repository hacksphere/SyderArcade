using UnityEngine;
using System.Collections;

public class Bounds : MonoBehaviour {
	public float topBound = 0f;
	public float bottomBound = 0f;
	public float rightBound = 0f;
	public float leftBound = 0f;

	void Awake ()
	{
		topBound = GameObject.Find("TopBound").transform.localPosition.y + (GameObject.Find("TopBound").transform.lossyScale.x / 2);
		bottomBound = GameObject.Find("BottomBound").transform.localPosition.y - (GameObject.Find("BottomBound").transform.lossyScale.x / 2);
		rightBound = GameObject.Find("RightBound").transform.localPosition.x + (GameObject.Find("RightBound").transform.lossyScale.x / 2);
		leftBound = GameObject.Find("LeftBound").transform.localPosition.x - (GameObject.Find("LeftBound").transform.lossyScale.x / 2);
	}

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
