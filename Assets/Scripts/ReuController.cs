using UnityEngine;
using System.Collections;

public class ReuController : CharacterParent {
	private Vector3 hitAt;
	private Collider demoHit;
	private SpriteRenderer hitGleam, boxProjectile;
	public float HStrLv, boxAmmo;
	public bool atkEnabled, controls2;

	// Use this for initialization
	void Start () {
		transform = GetComponent<Transform>();
		rb3d = GetComponent<Rigidbody>();
		anime = gameObject.transform.Find("Spriter").GetComponent<Animator>();
		demoHit = gameObject.transform.Find("DemoAttack").GetComponent<Collider>();
		hitGleam = gameObject.transform.Find("DemoAttack").GetComponentInChildren<SpriteRenderer>();
		boxProjectile = gameObject.transform.Find("BoxSprite").GetComponent<SpriteRenderer>();
		demoHit.enabled = false;
		hitGleam.enabled = false;
		boxProjectile.enabled = false;
		health = 90;
		speed = 10;
		maxSpeed = 2;
		atk = 5;
		atkMultiplier = 2;
		HStrLv = 9;
		boxAmmo = 0f;
		facingRight = true;
		atkEnabled = true;
		controls2 = false;
		hitAt = new Vector3(0, -0.2f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		if(atkEnabled){
			if(Input.GetKey(KeyCode.I)){
				anime.SetInteger("Direction", 2);
				hitAt.x = 0f; hitAt.z = 0.5f;//hit point
				if(rb3d.velocity.z > maxSpeed){float x = rb3d.velocity.x; rb3d.velocity = new Vector3(x,0,maxSpeed);}
				else{rb3d.AddForce(0,0,speed);}
			}
			if(Input.GetKey(KeyCode.J)){
				anime.SetInteger("Direction", 0); facingRight = false;
				hitAt.x = 0.25f; hitAt.z = 0f;
				if(rb3d.velocity.x < -maxSpeed){float z = rb3d.velocity.z; rb3d.velocity = new Vector3(-maxSpeed,0,z);}
				else{rb3d.AddForce(-speed,0,0);}
			}
			if((Input.GetKey(KeyCode.K) && !controls2)||(Input.GetKey(KeyCode.M) && controls2)){
				anime.SetInteger("Direction", 1);
				hitAt.x = 0f; hitAt.z = -0.5f;//hit point
				if(rb3d.velocity.z < -maxSpeed){float x = rb3d.velocity.x; rb3d.velocity = new Vector3(x,0,-maxSpeed);}
				else{rb3d.AddForce(0,0,-speed);}
			}
			if((Input.GetKey(KeyCode.L) && !controls2)||(Input.GetKey(KeyCode.K) && controls2)){
				anime.SetInteger("Direction", 0); facingRight = true;
				hitAt.x = 0.25f; hitAt.z = 0f;
				if(rb3d.velocity.x > maxSpeed){float z = rb3d.velocity.z; rb3d.velocity = new Vector3(maxSpeed,0,z);}
				else{rb3d.AddForce(speed,0,0);}
			}
		}
		
		if(Input.GetKey(KeyCode.F) && atkEnabled){
			demoHit.gameObject.GetComponent<Transform>().localPosition = hitAt;
			StartCoroutine(DemoHit());
		}
		
		if(Input.GetKeyDown(KeyCode.R) && atkEnabled){//grab
			//1
			//reach out in the direction indicated by Reu's animation
			//lineCast to see if there's anything in that range that's grabable
			//if there is, move object a ways above Reu's head, and always move if it's not centered on reu
			
			//2
			//use the mouse to click on the box, and deactivate the box while putting a box sprite over Reu's head, and having an ammo count for boxes.
			//on button click, spawn the box heading toward the enemy, or do an animation with a hit box instead.
		}
		
		if(boxAmmo < 0f){
			atkEnabled = false;
			boxProjectile.enabled = false;
			if(Input.GetKeyDown(KeyCode.U)){//spawn a box
				
			}
		}
	}
	
	void FixedUpdate(){
		//faint
		/*if(health <= 0){
			if(HStrLv <= 0){
				
			}
			else{
				HStrLv -= 1;
				health = HStrLv * 10;
			}
		}*/
		// flip
		if(transform.localScale.x > 0 && !facingRight){Flip();}
		if(transform.localScale.x < 0 && facingRight){Flip();}
	}
	
	public override void GotHit(){
	}
	
	IEnumerator DemoHit(){
		atkEnabled = false;
		yield return new WaitForSeconds(0.5f);
		hitGleam.enabled = true;
		demoHit.enabled = true;
		yield return new WaitForSeconds(0.25f);
		hitGleam.enabled = false;
		demoHit.enabled = false;
		atkEnabled = true;
	}
}

// Animation Directions
// 0 = side
// 1 = front
// 2 = back
