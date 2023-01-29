using UnityEngine;
using System.Collections;

public class AttackParent : MonoBehaviour {
	private CharacterParent user, target;
	protected Collider2D weapon;

	// Use this for initialization
	void Start () {
		user = GetComponentInParent<CharacterParent>();
		weapon = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(target != null){
			//target.health -= user.atk * user.atkMultiplier;
			target.GotHit(user.atk * user.atkMultiplier);
			target = null;
		}
	}
	
	void OnTriggerEnter2D(Collider2D hit){
		if(hit.gameObject.tag != gameObject.tag){target = hit.gameObject.GetComponent<CharacterParent>();}
		//else if(hit.gameObject.tag == gameObject.tag){Physics2D.IgnoreCollision(hit, weapon);}
	}
}
