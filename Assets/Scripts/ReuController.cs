using UnityEngine;
using System.Collections;

public class ReuController : CharacterParent {
	private Vector3 destination; 
	private Vector3 hitAt;
	private Collider demoHit;
	private SpriteRenderer hitGleam, boxProjectile;
	public float HStrLv, boxAmmo;
	public bool controls2;

	// Use this for initialization
	void Start () {
		transform = GetComponent<Transform>();
		destination = new Vector3(12, 11, 0f);// = transform.position
		rb2d = GetComponent<Rigidbody2D>();
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
        if (Input.touchCount > 0)
        {
			Debug.Log("TouchCount activated!");
        }

		newSpeed = rb2d.velocity;
		if(atkEnabled){
			if(Vector2.Distance(destination, transform.position) >= 0.125f)
            {
				Debug.Log(Vector2.Distance(destination, transform.position));
				MoveTowards(destination);
				UpdateAnimeDirection();
            }
            else
            {
				SlowDown(speed);
            }
			if(Input.GetKey(KeyCode.I)){
				anime.SetInteger("Direction", 2);
				hitAt.x = 0f; hitAt.z = 0.5f;//hit point
				/*if(rb2d.velocity.y > maxSpeed){newSpeed.y = maxSpeed;}
				else{ newSpeed.y += speed * Time.deltaTime; }*/
				MoveTowards(Vector3.up + transform.position);
			}
			if(Input.GetKey(KeyCode.J)){
				anime.SetInteger("Direction", 0); facingRight = false;
				hitAt.x = 0.25f; hitAt.z = 0f;
				//if(rb2d.velocity.x < -maxSpeed){newSpeed.x = -maxSpeed;}
				//else { newSpeed.x -= speed * Time.deltaTime; }
				MoveTowards(Vector3.left + transform.position);
			}
			if((Input.GetKey(KeyCode.K) && !controls2)||(Input.GetKey(KeyCode.M) && controls2)){
				anime.SetInteger("Direction", 1);
				hitAt.x = 0f; hitAt.z = -0.5f;//hit point
											  //if(rb2d.velocity.y < -maxSpeed){newSpeed.y = -maxSpeed;}
											  //else { newSpeed.y -= speed * Time.deltaTime; }
				MoveTowards(Vector3.down + transform.position);
			}
			if((Input.GetKey(KeyCode.L) && !controls2)||(Input.GetKey(KeyCode.K) && controls2)){
				anime.SetInteger("Direction", 0); facingRight = true;
				hitAt.x = 0.25f; hitAt.z = 0f;
				//if(rb2d.velocity.x > maxSpeed){newSpeed.x = maxSpeed;}
				//else { newSpeed.x += speed * Time.deltaTime; }
				MoveTowards(Vector3.right + transform.position);
			}
			//slowing our character down!
            if(!Input.GetKey(KeyCode.J) && !Input.GetKey(KeyCode.L))
            {
				//SlowDownX(1);
			}
			if (!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.K))
			{
				//SlowDownY(1);
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
		rb2d.velocity = newSpeed;
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
		if (newSpeed != Vector2.zero) { CorrectZ(); }//each time they move, we need to correct their z-axis.
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

	void OnMouseUp()
    {
		Debug.Log(Event.current.mousePosition);
    }

}

