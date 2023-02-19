using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//The Dog Leader is here to take the mapping for the entire pack,
//and then relay it to the other dogs in its pack. It will
// - call dogs back to itself,
// - send them the Vector2 list to follow where the leader is going.
// - Will stay within a close-enough range of the player--its prey--, but will
// - let it's followers tear up Reu.
//OR
// - Just give the pack steps to follow in the leader's footsteps! (.. Nah).
public class DDogLeader : DemiDogScript 
{
    private List<DemiDogScript> followers;
    private Object theDogs;
	private int followerCount;
	//for helping the dogs get close enough to a node before moving to the next one
	private float closeEnough = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
		transform = GetComponent<Transform>();
		rb2d = GetComponent<Rigidbody2D>();
		theKid = GameObject.Find("Reu").GetComponent<Transform>();
		anime = gameObject.transform.Find("DogSpriter").GetComponent<Animator>();
		weapon = gameObject.transform.Find("Claw").GetComponent<Collider2D>();
		weaponGleam = gameObject.transform.Find("Claw").GetComponentInChildren<SpriteRenderer>();
		moon = GameObject.Find("Map").GetComponent<GameManager>();//HauntedNeighborhood
																  
		//for creating the followers!
		theDogs = Resources.Load("DemiDog");
		followerCount = 5;
		followers = new List<DemiDogScript>();
		/*for(int i = 0; i < followerCount; i++)
        {
			GameObject newDog = (GameObject)Instantiate(theDogs, transform.position + Vector3.right, Quaternion.identity);
			followers.Add(newDog.GetComponent<DemiDogScript>());
			followers[i].SetLeader(transform);
        }*/

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
    void Update()
	{
		newSpeed = rb2d.velocity;
		//The leader is in charge of getting a path and sending it to the dogs!
		if (Vector3.Distance(theKid.position, transform.position) > sight)
		{
			//so far, we have the dogs updating their path every 3 seconds, but there's
			//a lag that happens each time all 4 of them refresh their maps.
			//how do we fix this so that our dogs don't get stuck and we don't cause a lag?

			//if we don't have our heading
			if (currentCoro == null)//we update our map every three seconds, so that when the player moves, the monsters can move, too.
			{
				//Debug.Log("Null Path");
				//find it!
				//looking for the player

				currentCoro = StartCoroutine(NewPath(3f, theKid.position));
				//currentPath = moon.TracePath(transform.position, theKid.position);

			}
			if (currentPath == null || currentPath.Count < 1) { 
				currentPath = moon.TracePath(transform.position, theKid.position); 
			}
			//if we have our heading
			else
			{
				//Debug.Log("Following Path!");
				//here we go!
				if (Vector2.Distance(transform.position, currentPath[currentPath.Count - 1]) < closeEnough)
				{
					currentPath.Remove(currentPath[currentPath.Count - 1]);
				}
				else
				{
					MoveTowards2(currentPath[currentPath.Count - 1]);
				}
			}
		}
		//if we're in seeing distance of the kid.
        else
        {
			//reset our current path
			currentPath = null;
			//slow down for a moment.
			SlowDown(1);
        }


		UpdateAnimeDirection();
		rb2d.velocity = newSpeed;
	}
	//Trying a different approach
	public IEnumerator NewPath(float seconds, Vector3 newPos)
    {
		yield return new WaitForSeconds(seconds);
		currentPath = moon.TracePath(transform.position, newPos);
		currentCoro = null;
	}
}
