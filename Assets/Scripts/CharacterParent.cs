using UnityEngine;
using System.Collections;

public class CharacterParent : MonoBehaviour {
	protected Transform transform;
	protected Rigidbody rb3d;
	protected Animator anime;
	protected Collider2D feet;
	
	public bool attackCooled, facingRight, grounded;
	public float health, speed, maxSpeed, atk, atkMultiplier;

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
}

// Animation Directions
// 0 = side
// 1 = front
// 2 = back