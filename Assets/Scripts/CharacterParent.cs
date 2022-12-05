using UnityEngine;
using System.Collections;

public class CharacterParent : MonoBehaviour {
	protected Transform transform;
	protected Rigidbody2D rb2d;
	protected Animator anime;
	protected Collider2D feet;
	
	public bool attackCooled, facingRight, grounded;
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
}

// Animation Directions
// 0 = side
// 1 = front
// 2 = back