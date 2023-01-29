using UnityEngine;
using System.Collections;

public class DemiDogScript : MonsterParent {

	protected Collider2D weapon;
	protected SpriteRenderer weaponGleam;
	protected Transform leader;
	public float boundary, cSpeed, cMaxSpeed; //to have a starting value of speed and maxSpeed.

	// Use this for initialization
	void Start () {
		transform = GetComponent<Transform>();
		rb2d = GetComponent<Rigidbody2D>();
		theKid = GameObject.Find("Reu").GetComponent<Transform>();
		anime = gameObject.transform.Find("DogSpriter").GetComponent<Animator>();
		weapon = gameObject.transform.Find("Claw").GetComponent<Collider2D>();
		weaponGleam = gameObject.transform.Find("Claw").GetComponentInChildren<SpriteRenderer>();
		moon = GameObject.Find("Map").GetComponent<GameManager>();//HauntedNeighborhood
																  //if (moon == null) { moon = GameObject.Find("Map").GetComponent<BoardManager>(); }
		currentPath = null;
		weapon.enabled = false;
		weaponGleam.enabled = false;
		health = 25;
		atk = 10;
		atkMultiplier = 1;
		speed = 10f; //stalking speed is 5
		maxSpeed = 1.8f; //stalking max is 1
		cSpeed = speed; cMaxSpeed = maxSpeed;
		boundary = 3f;
		sight = 5f;
		facingRight = true;
		atkEnabled = true;

		CorrectZ();//each time they move, we need to correct their z-axis.
	}
	
	// Update is called once per frame
	void Update () {
		newSpeed = rb2d.velocity;
		//follow
		float xDif = Mathf.Abs(theKid.position.x - transform.position.x); //distance(x)
		float zDif = Mathf.Abs(theKid.position.y - transform.position.y); //distance(y)
		//if we can't see the player yet, track them down!


		//if we're far away from the player
		if (Vector3.Distance(theKid.position, transform.position) > sight)
		{
			//if not, and we're out of reach of the Player, then we teleport!
			if (leader != null)
			{
				if (Vector3.Distance(theKid.position, leader.position) > sight * 2)
				{
					TeleportTo(leader.position +Vector3.right/2);
				}
				//If the leader is close enough, move toward the Leader.
				else if (Vector3.Distance(theKid.position, leader.position) > 1)
				{
					MoveTowards(leader.position);
				}
			}/*
			////if we don't have our heading
			if(currentPath == null)
            {
				//do nothing.
			}
			else if(currentPath.Count < 1) { currentPath = moon.TracePath(transform.position, theKid.position); }
			//if we have our heading
			else
            {
				//Debug.Log("Following Path!");
				//here we go!
				if(Vector2.Distance(transform.position, currentPath[currentPath.Count - 1]) < 1)
                {
					currentPath.Remove(currentPath[currentPath.Count-1]);
                }
                else
                {
					MoveTowards(currentPath[currentPath.Count - 1]);
                }
            }// */
		}
		//when we're close enough to the player.
		else
		{
			Debug.Log("Close!");
			currentPath = null;
			if (xDif > boundary + moon.dogBound || zDif > boundary + moon.dogBound) { maxSpeed = cMaxSpeed; }//if(xDif) > boundary + 0.5 || zDif > boundary + 0.5
			else if (xDif < boundary - moon.dogBound && zDif < boundary - moon.dogBound && Mathf.Sqrt((xDif * xDif) + (zDif * zDif)) > 1) { maxSpeed = cMaxSpeed / 2; }
			else { maxSpeed = 0; }
			if (atkEnabled) { speed = cSpeed; }
			else { speed = 0; }
			MoveTowards(theKid.position);

			//attack
			if (Mathf.Sqrt((xDif * xDif) + (zDif * zDif)) < 2 && atkEnabled) {
				Vector3 weaponAt = new Vector3(0, 0, 0);
				if (xDif > zDif) { weaponAt.x = 0.3f; weaponAt.y = 0; }
				else {
					weaponAt.x = 0f;
					if (transform.position.y < theKid.position.y) { weaponAt.y = 0.3f; }//down
					else if (transform.position.y > theKid.position.y) { weaponAt.y = -0.3f; }//up
				}
				weapon.gameObject.GetComponent<Transform>().localPosition = weaponAt;
				StartCoroutine(LionSlash());
			}
		}


		//animator
		/*if(atkEnabled){
			if(xDif > zDif){anime.SetInteger("Direction", 0);}//side
			else{
				if(transform.position.z < theKid.position.z){anime.SetInteger("Direction", 2);}//up
				else if(transform.position.z > theKid.position.z){anime.SetInteger("Direction", 1);}//down
			}
		}*/
		UpdateAnimeDirection();
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
		if (health <= 0 || moon.IsDay()) { Dead(); moon.dogs -= 1; }
		if (newSpeed != Vector2.zero)
		{
			CorrectZ();//each time they move, we need to correct their z-axis.
		}
	}
	
	IEnumerator LionSlash(){
		Debug.Log("Attack!");
		atkEnabled = false;
		yield return new WaitForSeconds(1.5f);
		weaponGleam.enabled = true;
		weapon.enabled = true;
		yield return new WaitForSeconds(0.5f);
		weapon.enabled = false;
		weaponGleam.enabled = false;
		atkEnabled = true;
	}
	
	//public override void GotHit(){
		
	//}

	public void OnCollisionEnter2D(Collision2D hit)
    {
		if (hit.gameObject.tag == "Player") { hit.gameObject.GetComponent<CharacterParent>().GotHit(atk); }
		//else if (hit.tag == gameObject.tag) { Physics2D.IgnoreCollision(hit, weapon); }
	}

	//for setting the Leader!
	public void SetLeader(Transform leaderPose)
    {
		leader = leaderPose;
    }

	//for teleporting back to the Leader!
	public void TeleportTo(Vector3 newPos)
    {
		transform.position = newPos;
		CorrectZ();
    }
}

// if they're outside the camera range, then they run toward the player. otherwise, they stalk toward them and attack when close enough.