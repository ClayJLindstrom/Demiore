using UnityEngine;
using System.Collections;

public class DemiDogScript : CharacterParent {
	private Collider weapon;
	private SpriteRenderer weaponGleam;
	private BoardManager moon;
	public Transform theKid;
	public float boundary;
	public bool atkEnabled;

	// Use this for initialization
	void Start () {
		transform = GetComponent<Transform>();
		rb2d = GetComponent<Rigidbody2D>();
		theKid = GameObject.Find("Reu").GetComponent<Transform>();
		anime = gameObject.transform.Find("DogSpriter").GetComponent<Animator>();
		weapon = gameObject.transform.Find("Claw").GetComponent<Collider>();
		weaponGleam = gameObject.transform.Find("Claw").GetComponentInChildren<SpriteRenderer>();
		moon = GameObject.Find("HauntedNeighborhood").GetComponent<BoardManager>();
		weapon.enabled = false;
		weaponGleam.enabled = false;
		health = 25;
		atk = 10;
		atkMultiplier = 1;
		speed = 10f; //stalking speed is 5
		maxSpeed = 2f; //stalking max is 1
		boundary = 3f;
		facingRight = true;
		atkEnabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		newSpeed = rb2d.velocity;
		//follow
		float xDif = Mathf.Abs(theKid.position.x - transform.position.x); float zDif = Mathf.Abs(theKid.position.y - transform.position.z);
		if(xDif > boundary + moon.dogBound || zDif > boundary + moon.dogBound){maxSpeed = 2;}//if(xDif) > boundary + 0.5 || zDif > boundary + 0.5
		else if(xDif < boundary - moon.dogBound && zDif < boundary - moon.dogBound && Mathf.Sqrt((xDif * xDif) + (zDif * zDif)) > 1){maxSpeed = 1;}
		else{maxSpeed = 0;}
		if(atkEnabled){speed = 10;}
		else{speed = 0;}
		if(transform.position.x > theKid.position.x && maxSpeed != 0f){//go left
			facingRight = false;
			if(rb2d.velocity.x < -maxSpeed){newSpeed.x = -maxSpeed;}
			else { newSpeed.x -= speed * Time.deltaTime; }
		}
		if(transform.position.x < theKid.position.x && maxSpeed != 0f){//go right
			facingRight = true;
			if(rb2d.velocity.x > maxSpeed){newSpeed.x = maxSpeed;}
			else { newSpeed.x += speed * Time.deltaTime; }
		}
		if(transform.position.y > theKid.position.y && maxSpeed != 0f){//go up
			if(rb2d.velocity.y < -maxSpeed){newSpeed.y = -maxSpeed;}
			else { newSpeed.y -= speed * Time.deltaTime; }
		}
		if(transform.position.y < theKid.position.y && maxSpeed != 0f){// go down
			if(rb2d.velocity.y > maxSpeed){newSpeed.y = maxSpeed;}
			else { newSpeed.y += speed * Time.deltaTime; }
		}
		
		//attack
		if(Mathf.Sqrt((xDif * xDif) + (zDif * zDif)) < 1 && atkEnabled){
			Vector3 weaponAt = new Vector3(0,0,0);
			if(xDif > zDif){weaponAt.x = 0.3f; weaponAt.y = 0;}
			else{
				weaponAt.x = 0f;
				if(transform.position.y < theKid.position.y){weaponAt.y = 0.3f;}//down
				else if(transform.position.y > theKid.position.y){weaponAt.y = -0.3f;}//up
			}
			weapon.gameObject.GetComponent<Transform>().localPosition = weaponAt;
			StartCoroutine(LionSlash());
		}
		
		//animator
		if(atkEnabled){
			if(xDif > zDif){anime.SetInteger("Direction", 0);}//side
			else{
				if(transform.position.z < theKid.position.z){anime.SetInteger("Direction", 2);}//up
				else if(transform.position.z > theKid.position.z){anime.SetInteger("Direction", 1);}//down
			}
		}
		rb2d.velocity = newSpeed;
	}

	void FixedUpdate()
	{
		// Flip
		if (atkEnabled)
		{
			if (transform.localScale.x > 0 && !facingRight) { Flip(); }
			if (transform.localScale.x < 0 && facingRight) { Flip(); }
		}
		//dead
		if (health <= 0 || moon.day) { Dead(); moon.dogs -= 1; }
		if (newSpeed != Vector2.zero)
		{
			CorrectZ();//each time they move, we need to correct their z-axis.
		}
	}
	
	IEnumerator LionSlash(){
		atkEnabled = false;
		yield return new WaitForSeconds(1.5f);
		weaponGleam.enabled = true;
		weapon.enabled = true;
		yield return new WaitForSeconds(0.5f);
		weapon.enabled = false;
		weaponGleam.enabled = false;
		atkEnabled = true;
	}
	
	public override void GotHit(){
		
	}
}

// if they're outside the camera range, then they run toward the player. otherwise, they stalk toward them and attack when close enough.