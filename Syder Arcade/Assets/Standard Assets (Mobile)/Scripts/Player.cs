using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

public int speed = 10;
bool runOnce = false;
Vector2 vel;
public float shootRate = 10f;
float shootTimer = 0f;
public int hp = 10;
public float damage = 5f;
int score = 0;
var timer = 0.0;
var firstFrame = true;
var intro = true;
var dead = false;
var restart = false;
var deathTime = 0.0;
var bulletsFired = 0;
var kills = 0;
var bulletTime = 100.0;
var inBulletTime = false;
var winTime = 0.0;

var topBound = 0.0;
var bottomBound = 0.0;
var rightBound = 0.0;
var leftBound = 0.0;

public GameObject bullet;
GameObject go;
GUISkin guiSkin;

function Start ()
{
	transform.position = Vector2.zero;
}

function Update ()
{
	timer += 1 / Time.deltaTime / 100;
	if (firstFrame)
	{
		topBound = GameObject.Find("Scripts").GetComponent(Bounds2).topBound - GameObject.Find("TopBound").transform.lossyScale.x - (transform.lossyScale.y / 2) - .5;
		bottomBound = GameObject.Find("Scripts").GetComponent(Bounds2).bottomBound + GameObject.Find("BottomBound").transform.lossyScale.x + (transform.lossyScale.y / 2) + .5;
		rightBound = GameObject.Find("Scripts").GetComponent(Bounds2).rightBound - GameObject.Find("RightBound").transform.lossyScale.x - (transform.lossyScale.x / 2) - .5; 
		leftBound = GameObject.Find("Scripts").GetComponent(Bounds2).leftBound + GameObject.Find("LeftBound").transform.lossyScale.x + (transform.lossyScale.x / 2) + .5;
	}
	GameObject.Find("ParticleSystem1").GetComponent<ParticleSystem>().particleSystem.enableEmission = false;
	GameObject.Find("ParticleSystem2").GetComponent<ParticleSystem>().particleSystem.enableEmission = false;
	if (!restart)
	{
		if (Input.GetAxis("Horizontal") == 0 || transform.position.x <= leftBound || transform.position.x >= rightBound)
			vel.x = 0;
		if (Input.GetAxis("Horizontal") == 1 && transform.position.x < rightBound)
		{
			vel.x = speed;
			if (Mathf.RoundToInt(transform.rotation.eulerAngles.y / 180) % 2 == 1)
			{
				GameObject.Find("ParticleSystem1").GetComponent(ParticleSystem).particleSystem.enableEmission = true;
				GameObject.Find("ParticleSystem2").GetComponent(ParticleSystem).particleSystem.enableEmission = true;
			}
		}
		else if (Input.GetAxis("Horizontal") == -1 && transform.position.x > leftBound)
		{
			vel.x = -speed;
			if (Mathf.RoundToInt(transform.rotation.eulerAngles.y / 180) % 2 == 0)
			{
				GameObject.Find("ParticleSystem1").GetComponent(ParticleSystem).particleSystem.enableEmission = true;
				GameObject.Find("ParticleSystem2").GetComponent(ParticleSystem).particleSystem.enableEmission = true;
			}
		}
		if (Input.GetAxis("Vertical") == 0 || transform.position.y <= bottomBound || transform.position.y >= topBound)
			vel.y = 0;
		if (Input.GetAxis("Vertical") == 1 && transform.position.y < topBound)
		{
			vel.y = speed;
		}
		else if (Input.GetAxis("Vertical") == -1 && transform.position.y > bottomBound)
		{
			vel.y = -speed;
		}
		if (Input.GetAxis("Jump") == 1 && runOnce)
		{
			transform.rotation.eulerAngles.y += 180;
			runOnce = false;
		}
		else if (Input.GetAxis("Jump") == 0)
			runOnce = true;
		vel = Vector2.ClampMagnitude(vel, speed);
		transform.position += vel;
		shootTimer ++;
		if (shootTimer > shootRate && Input.GetAxis("Fire1") == 1)
		{
			shootTimer = 0;
			if (Mathf.RoundToInt(transform.rotation.eulerAngles.y / 180) % 2 == 1)
				var go : GameObject = GameObject.Instantiate(bullet, Vector2(transform.position.x + .2375, transform.position.y), Quaternion.Euler(0, 270, 0));
			else
				var go2 : GameObject = GameObject.Instantiate(bullet, Vector2(transform.position.x - .2375, transform.position.y), Quaternion.Euler(0, 90, 0));
			bulletsFired ++;
		}
	}
	if (deathTime != 0 && timer > deathTime + .5)
	{
		if (Application.loadedLevel == 0)
			Application.LoadLevel(0);
		else if (Application.loadedLevel == 1)
			Application.LoadLevel(1);
	}
	firstFrame = false;
}

function OnTriggerEnter (other : Collider)
{
	if (other.gameObject.name.Contains("AlienBullet"))
	{
		if (other.gameObject.name.Contains("1"))
			hp -= other.gameObject.GetComponent(Bullet).damage;
		else if (other.gameObject.name.Contains("2"))
			hp -= other.gameObject.GetComponent(BulletNotAimed).damage;
		Destroy(other.gameObject);
	}
	else if (other.gameObject.name.Contains("AlienBomb") || other.gameObject.name.Contains("AlienSpaceship"))
	{
		hp -= other.gameObject.GetComponent(Enemy).damage;
		Destroy(other.gameObject);
	}
	if (hp <= 0)
	{
		dead = true;
		var components = GetComponentsInChildren(MeshRenderer);
		for (var c : Component in components)
			c.renderer.enabled = false;
		GameObject.Find("Camera").GetComponent(SmoothFollow2D).enabled = false;
	}
}

function OnGUI ()
{
	GUI.skin = guiSkin;
	GUI.color = Color.yellow;
	if (timer < 2.25)
	{
		if (Application.loadedLevel == 0)
			GUI.Label (Rect (10, Screen.height / 2 - 20, 750, 40), "                                              Level 1: 100 score or greater");
		if (Application.loadedLevel == 1)
			GUI.Label (Rect (10, Screen.height / 2 - 20, 750, 40), "                             Level 2: All kills (no crashes) with 6 or less shots fired");
	}
	else
	{
		GUI.Label (Rect (10, 10, 750, 40), "Score: " + score);
		GUI.Label (Rect (10, 35, 750, 40), "Game Time: " + Mathf.RoundToInt(levelTimer));
		GUI.Label (Rect (10, 60, 750, 40), "Health: " + hp);
	}
	if (Application.loadedLevel == 0 && score >= 100 && hp > 0)
	{
		Application.LoadLevel(1);
	}
	else if (Application.loadedLevel == 1 && bulletsFired <= 6 && kills == 4 && hp > 0)
	{
		Application.LoadLevel(2);
	}
	else if (Application.loadedLevel == 2 && GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && hp > 0)
	{
		GUI.Label (Rect (10, Screen.height / 2 - 20, 750, 40), "                                             CONGRATZ! Level 4 coming soon!");
	}
	if (!restart && ((Application.loadedLevel == 0 && GameObject.Find("AlienBomb") == null && GameObject.Find("AlienSpaceship") == null) || dead || (Application.loadedLevel == 1 && bulletsFired > 6)))
	{
		dead = false;
		restart = true;
		deathTime = timer;
	}
	if (restart)
	{
		GUI.Label (Rect (10, Screen.height / 2 + 20, 750, 40), "                                                     MISSION FAILED!");
	}
}