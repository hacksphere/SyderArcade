using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public Vector2 vel;
	public int speed = 10;
	public bool MADE_BY_PLAYER = true;
	public float damage = 1f;
	public GameObject shooter;

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		rigidbody.velocity = vel;
		if ((MADE_BY_PLAYER && Mathf.Abs(transform.position.x - GameObject.Find("Player").transform.position.x) > 4.5) || transform.position.x < GameObject.Find("Scripts").GetComponent<Bounds>().leftBound || transform.position.x > GameObject.Find("Scripts").GetComponent<Bounds>().rightBound || transform.position.y < GameObject.Find("Scripts").GetComponent<Bounds>().bottomBound || transform.position.y > GameObject.Find("Scripts").GetComponent<Bounds>().topBound)
			Destroy(gameObject);
	}
	/*
	void OnTriggerEnter (Collider other)
	{
		if (MADE_BY_PLAYER)
		{
			if (other.gameObject.name == "AlienBomb")
			{
				other.gameObject.GetComponent<Enemy>().hp -= damage;
				if (other.gameObject.GetComponent<Enemy>().hp <= 10)
			}
		}
	}
	*/
	void OnTriggerExit (Collider other)
	{
		collider.isTrigger = false;
	}
}
