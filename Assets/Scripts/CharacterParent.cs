using UnityEngine;
using System.Collections;
using UnityEngine.iOS;

public class CharacterParent : MonoBehaviour {
	protected Transform transform;
	protected RectTransform rectTransform;
	protected Rigidbody2D rb2d;
	protected Animator anime;
	protected Collider2D feet;
	
	public bool atkEnabled, attackCooled, facingRight, grounded;
	public float health, speed, maxSpeed, atk, atkMultiplier;
	public Vector2 newSpeed;//for easy alteration of the speed.

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void FixedUpdate(){
		
	}
	
	public void Flip(){
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	
	public void Dead(){
		Destroy(gameObject);
	}
	
	public virtual void GotHit(){
		
	}

	public void CorrectZ()
    {
		if (rb2d.velocity != Vector2.zero)
		{
			Vector3 newPose = transform.position;
			//we'll have the z-axis be equal to 10% the y-axis.
			newPose.z = newPose.y / 10;
			transform.position = newPose;
		}
	}
	public void CorrectRectZ()
	{
		if (rb2d.velocity != Vector2.zero)
		{
			Vector3 newPose = rectTransform.position;
			//we'll have the z-axis be equal to 10% the y-axis.
			newPose.z = newPose.y / 10;
			rectTransform.position = newPose;
		}
	}

	public virtual void SlowDown(float multiplier)
	{
		if(rb2d.velocity != Vector2.zero)
		{
			newSpeed = rb2d.velocity;
			//we use speed to slow ourselves down again.
			if (Mathf.Abs(newSpeed.x) < speed * Time.deltaTime * multiplier) //if we're close enough to stopping,
			{
				newSpeed.x = 0;
			}
			else if (newSpeed.x > 0) { newSpeed.x -= speed * Time.deltaTime * multiplier; }
			else if (newSpeed.x < 0) { newSpeed.x += speed * Time.deltaTime * multiplier; }
			if (Mathf.Abs(newSpeed.y) < speed * Time.deltaTime * multiplier) //if we're close enough to stopping,
			{
				newSpeed.y = 0;
			}
			else if (newSpeed.y > 0) { newSpeed.y -= speed * Time.deltaTime * multiplier; }
			else if (newSpeed.y < 0) { newSpeed.y += speed * Time.deltaTime * multiplier; }
			rb2d.velocity = newSpeed;
		}
	}
	public virtual void SlowDownX(float multiplier)
	{
		if (rb2d.velocity.x != 0)
		{
			newSpeed = rb2d.velocity;
			//we use speed to slow ourselves down again.
			if (Mathf.Abs(newSpeed.x) < speed * Time.deltaTime * multiplier) //if we're close enough to stopping,
			{
				newSpeed.x = 0;
			}
			else if (newSpeed.x > 0) { newSpeed.x -= speed * Time.deltaTime * multiplier; }
			else if (newSpeed.x < 0) { newSpeed.x += speed * Time.deltaTime * multiplier; }
			rb2d.velocity = newSpeed;
		}
	}
	public virtual void SlowDownY(float multiplier)
	{
		if (rb2d.velocity.y != 0)
		{
			newSpeed = rb2d.velocity;
			//we use speed to slow ourselves down again.
			if (Mathf.Abs(newSpeed.y) < speed * Time.deltaTime * multiplier) //if we're close enough to stopping,
			{
				newSpeed.y = 0;
			}
			else if (newSpeed.y > 0) { newSpeed.y -= speed * Time.deltaTime * multiplier; }
			else if (newSpeed.y < 0) { newSpeed.y += speed * Time.deltaTime * multiplier; }
			rb2d.velocity = newSpeed;
		}
	}

	//the player taps on the screen, and this is where the player goes.
	//This will also use the SlowDown functions above.
	public virtual void MoveTowards(Vector3 location)
    {
		//getting the xy distances from the target
		Vector2 displacement = location - transform.position;
		//to get the hypotenuse distance from the target
		float distance = Vector2.Distance(location, transform.position);
		if(distance == 0) { distance = 0.001f; }
		//Now, to know how much we need to increase the 

		//for our max velocity y, it's maxSpeed * Mathf.Abs(y/hyp) * y/MAthf.Abs(y)
		//								max speed,	our ratio,		positive/negative.

		//OH! we'll add some friction!
		//if we're too fast in the Y-direction,
		if (Mathf.Abs(rb2d.velocity.y) > maxSpeed * Mathf.Abs(displacement.y / distance)){
			//decelerate y-wise.
			SlowDownY(speed);
        }
        else
		{
			newSpeed.y += speed * Time.deltaTime * (displacement.y / distance);
		}

		//if we're too fast in the Y-direction,
		if (Mathf.Abs(rb2d.velocity.x) > maxSpeed * Mathf.Abs(displacement.x / distance)){
			//decelerate y-wise.
			SlowDownX(speed);
		}
		else
		{
			newSpeed.x += speed * Time.deltaTime * (displacement.x / distance);
		}
		//facing left or right.
		if(displacement.x < 0) { facingRight = false; }
        else { facingRight = true; }
	}

	public void UpdateAnimeDirection()
    {
		//animator
		if (atkEnabled)
		{
			if (rb2d.velocity.x > rb2d.velocity.y) { anime.SetInteger("Direction", 0); }//side
			else
			{
				if (rb2d.velocity.y > 0) { anime.SetInteger("Direction", 2); }//up
				else{ anime.SetInteger("Direction", 1); }//down
			}
		}
	}
}

// Animation Directions
// 0 = side
// 1 = front
// 2 = back