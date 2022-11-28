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
		rb3d = GetComponent<Rigidbody>();
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
		speed = 10; //stalking speed is 5
		maxSpeed = 2; //stalking max is 1
		boundary = 3f;
		facingRight = true;
		atkEnabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		//follow
		float xDif = Mathf.Abs(theKid.position.x - transform.position.x); float zDif = Mathf.Abs(theKid.position.z - transform.position.z);
		if(xDif > boundary + moon.dogBound || zDif > boundary + moon.dogBound){maxSpeed = 2;}//if(xDif) > boundary + 0.5 || zDif > boundary + 0.5
		else if(xDif < boundary - moon.dogBound && zDif < boundary - moon.dogBound && Mathf.Sqrt((xDif * xDif) + (zDif * zDif)) > 1){maxSpeed = 1;}
		else{maxSpeed = 0;}
		if(atkEnabled){speed = 10;}
		else{speed = 0;}
		if(transform.position.x > theKid.position.x && maxSpeed != 0){
			facingRight = false;
			if(rb3d.velocity.x < -maxSpeed){float z = rb3d.velocity.z; rb3d.velocity = new Vector3(-maxSpeed,0f,z);}
			else{rb3d.AddForce(-speed,0f,0f);}
		}
		if(transform.position.x < theKid.position.x && maxSpeed != 0){
			facingRight = true;
			if(rb3d.velocity.x > maxSpeed){float z = rb3d.velocity.z; rb3d.velocity = new Vector3(maxSpeed,0f,z);}
			else{rb3d.AddForce(speed,0f,0f);}
		}
		if(transform.position.z > theKid.position.z && maxSpeed != 0){
			if(rb3d.velocity.z < -maxSpeed){float x = rb3d.velocity.x; rb3d.velocity = new Vector3(x,0f,-maxSpeed);}
			else{rb3d.AddForce(0f,0f,-speed);}
		}
		if(transform.position.z < theKid.position.z && maxSpeed != 0){
			if(rb3d.velocity.z > maxSpeed){float x = rb3d.velocity.x; rb3d.velocity = new Vector3(x,0f,maxSpeed);}
			else{rb3d.AddForce(0f,0f,speed);}
		}
		
		//attack
		if(Mathf.Sqrt((xDif * xDif) + (zDif * zDif)) < 1 && atkEnabled){
			Vector3 weaponAt = new Vector3(0,0,0);
			if(xDif > zDif){weaponAt.x = 0.3f; weaponAt.z = 0;}
			else{
				weaponAt.x = 0f;
				if(transform.position.z < theKid.position.z){weaponAt.z = 0.3f;}//down
				else if(transform.position.z > theKid.position.z){weaponAt.z = -0.3f;}//up
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
	}
	
	void FixedUpdate(){
		// Flip
		if(atkEnabled){
			if(transform.localScale.x > 0 && !facingRight){Flip();}
			if(transform.localScale.x < 0 && facingRight){Flip();}
		}
		//dead
		if(health <= 0 || moon.day){Dead(); moon.dogs -= 1;}
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