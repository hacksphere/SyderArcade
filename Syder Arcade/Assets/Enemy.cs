using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
	public int speed = 5;
	public float shootRate = 3f;
	float shootTimer = 0f;
	public float hp = 4f;
	float damage = 5f;
	bool goingLeft;
	bool bomb = false;
	bool follower = false;
	float reward = hp;
	float rotateX = 0f;
	float rotateY = 0f;
	float rotateZ = 0f;
	public int bulletNum = 8;
	bool firstFrame = true;
	
	float topBound = 0.0;
	float bottomBound = 0.0;
	float rightBound = 0.0;
	float leftBound = 0.0;
	
	public GameObject bullet;
	GameObject go;
	public int range = 0;
	
	float rotateSpeedMax = 5f;

	// Use this for initialization
	void Start ()
	{
		shootTimer = Time.timeSinceLevelLoad + shootRate;
		goingLeft = true;
		if (transform.position.x < 0)
			goingLeft = false;
		if (bomb)
		{
			transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
			rotateX = Random.Range(-rotateSpeedMax, rotateSpeedMax);
			rotateY = Random.Range(-rotateSpeedMax, rotateSpeedMax);
			rotateZ = Random.Range(-rotateSpeedMax, rotateSpeedMax);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (firstFrame)
		{
			topBound = GameObject.Find("Scripts").GetComponent(Bounds2).topBound;
			bottomBound = GameObject.Find("Scripts").GetComponent(Bounds2).bottomBound;
			rightBound = GameObject.Find("Scripts").GetComponent(Bounds2).rightBound;
			leftBound = GameObject.Find("Scripts").GetComponent(Bounds2).leftBound;
		}
		if (shootRate != -1 && transform.position.x > leftBound && transform.position.x < rightBound && transform.position.y > bottomBound && transform.position.y < topBound)
		{
			if (Time.timeSinceLevelLoad > shootTimer)
			{
				shootTimer += shootRate;
				if (Mathf.RoundToInt(transform.rotation.eulerAngles.y / 180) % 2 == 0)
				{
					go = (GameObject) GameObject.Instantiate(bullet, Vector2(transform.position.x - 0.1375, transform.position.y - 0.01586717), Quaternion.Euler(0, 90, 0));
					go.GetComponent(Bullet).shooter = gameObject;
					go.GetComponent(Bullet).MADE_BY_PLAYER = false;
				}
				else
				{
					go = (GameObject) GameObject.Instantiate(bullet, Vector2(transform.position.x + 0.1375, transform.position.y + 0.01586717), Quaternion.Euler(0, 270, 0));
					go.GetComponent(Bullet).shooter = gameObject;
					go.GetComponent(Bullet).MADE_BY_PLAYER = false;
				}
			}
		}
		if (follower)
		{
			var direction = GameObject.Find("Player").transform.position - transform.position;
			direction = Vector2.ClampMagnitude(direction, speed);
			if (Vector3.Distance(transform.position, GameObject.Find("Player").transform.position) > range)
				transform.position += direction;
			transform.forward = GameObject.Find("Player").transform.position - transform.position;
			transform.Rotate(Vector3(-45, 0, 0));
		}
		else
			transform.position.x += speed;
		if (bomb)
		{
			transform.rotation.eulerAngles.x += rotateX;
			transform.rotation.eulerAngles.y += rotateY;
			transform.rotation.eulerAngles.z += rotateZ;
		}
		if ((transform.position.x > rightBound + 25 && !goingLeft) || (transform.position.x < leftBound - 25 && goingLeft))
			Destroy(gameObject);
		firstFrame = false;
	}

	function OnTriggerEnter (Collider other)
	{
		if (other.gameObject.name.Contains("PlayerBullet"))
		{
			hp -= other.gameObject.GetComponent(Bullet).damage;
			GameObject.Find("Player").GetComponent(Player).score ++;
			Destroy(other.gameObject);
		}
		else if (other.gameObject.name.Contains("AlienBullet1") && other.gameObject.GetComponent(Bullet).shooter != gameObject)
		{
			hp -= other.gameObject.GetComponent(Bullet).damage;
			Destroy(other.gameObject);
		}
		else if (other.gameObject.name.Contains("AlienBullet2") && other.gameObject.GetComponent<BulletNotAimed>().shooter != gameObject)
		{
			hp -= other.gameObject.GetComponent<BulletNotAimed>().damage;
			Destroy(other.gameObject);
		}
		if (hp <= 0)
		{
			Destroy(gameObject);
			GameObject.Find("Player").GetComponent(Player).score += reward;
			GameObject.Find("Player").GetComponent(Player).kills ++;
			if (bomb)
			{
				go = (GameObject) GameObject.Instantiate(bullet, Vector2(transform.position.x, transform.position.y), Quaternion.identity);
				go.GetComponent<BulletNotAimed>().vel.x = go.GetComponent<BulletNotAimed>().MOVE_SPEED;
				go.GetComponent<BulletNotAimed>().vel.y = 0;
				go.GetComponent<BulletNotAimed>().vel = Vector2.ClampMagnitude(go.GetComponent<BulletNotAimed>().vel, go.GetComponent<BulletNotAimed>().MOVE_SPEED);
				go = (GameObject) GameObject.Instantiate(bullet, Vector2(transform.position.x, transform.position.y), Quaternion.identity);
				go.GetComponent<BulletNotAimed>().vel.x = 0;
				go.GetComponent<BulletNotAimed>().vel.y = go.GetComponent<BulletNotAimed>().MOVE_SPEED;
				go.GetComponent<BulletNotAimed>().vel = Vector2.ClampMagnitude(go.GetComponent<BulletNotAimed>().vel, go.GetComponent<BulletNotAimed>().MOVE_SPEED);
				go = (GameObject) GameObject.Instantiate(bullet, Vector2(transform.position.x, transform.position.y), Quaternion.identity);
				go.GetComponent<BulletNotAimed>().vel.x = go.GetComponent<BulletNotAimed>().MOVE_SPEED;
				go.GetComponent<BulletNotAimed>().vel.y = go.GetComponent<BulletNotAimed>().MOVE_SPEED;
				go.GetComponent<BulletNotAimed>().vel = Vector2.ClampMagnitude(go.GetComponent<BulletNotAimed>().vel, go.GetComponent<BulletNotAimed>().MOVE_SPEED);
				go = (GameObject) GameObject.Instantiate(bullet, Vector2(transform.position.x, transform.position.y), Quaternion.identity);
				go.GetComponent<BulletNotAimed>().vel.x = -go.GetComponent<BulletNotAimed>().MOVE_SPEED;
				go.GetComponent<BulletNotAimed>().vel.y = -go.GetComponent<BulletNotAimed>().MOVE_SPEED;
				go.GetComponent<BulletNotAimed>().vel = Vector2.ClampMagnitude(go.GetComponent<BulletNotAimed>().vel, go.GetComponent<BulletNotAimed>().MOVE_SPEED);
				go = (GameObject) GameObject.Instantiate(bullet, Vector2(transform.position.x, transform.position.y), Quaternion.identity);
				go.GetComponent<BulletNotAimed>().vel.x = -go.GetComponent<BulletNotAimed>().MOVE_SPEED;
				go.GetComponent<BulletNotAimed>().vel.y = go.GetComponent<BulletNotAimed>().MOVE_SPEED;
				go.GetComponent<BulletNotAimed>().vel = Vector2.ClampMagnitude(go.GetComponent<BulletNotAimed>().vel, go.GetComponent<BulletNotAimed>().MOVE_SPEED);
				go = (GameObject) GameObject.Instantiate(bullet, Vector2(transform.position.x, transform.position.y), Quaternion.identity);
				go.GetComponent<BulletNotAimed>().vel.x = go.GetComponent<BulletNotAimed>().MOVE_SPEED;
				go.GetComponent<BulletNotAimed>().vel.y = -go.GetComponent<BulletNotAimed>().MOVE_SPEED;
				go.GetComponent<BulletNotAimed>().vel = Vector2.ClampMagnitude(go.GetComponent<BulletNotAimed>().vel, go.GetComponent<BulletNotAimed>().MOVE_SPEED);
				go = (GameObject) GameObject.Instantiate(bullet, Vector2(transform.position.x, transform.position.y), Quaternion.identity);
				go.GetComponent<BulletNotAimed>().vel.x = 0;
				go.GetComponent<BulletNotAimed>().vel.y = -go.GetComponent<BulletNotAimed>().MOVE_SPEED;
				go.GetComponent<BulletNotAimed>().vel = Vector2.ClampMagnitude(go.GetComponent<BulletNotAimed>().vel, go.GetComponent<BulletNotAimed>().MOVE_SPEED);
				go = (GameObject) GameObject.Instantiate(bullet, Vector2(transform.position.x, transform.position.y), Quaternion.identity);
				go.GetComponent<BulletNotAimed>().vel.x = -go.GetComponent<BulletNotAimed>().MOVE_SPEED;
				go.GetComponent<BulletNotAimed>().vel.y = 0;
				go.GetComponent<BulletNotAimed>().vel = Vector2.ClampMagnitude(go.GetComponent<BulletNotAimed>().vel, go.GetComponent<BulletNotAimed>().MOVE_SPEED);
			}
		}
	}
}
