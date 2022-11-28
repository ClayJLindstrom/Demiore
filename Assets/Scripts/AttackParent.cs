using UnityEngine;
using System.Collections;

public class AttackParent : MonoBehaviour {
	private CharacterParent user, target;
	public Collider weapon;

	// Use this for initialization
	void Start () {
		user = GetComponentInParent<CharacterParent>();
		weapon = GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(target != null){
			target.health -= user.atk * user.atkMultiplier;
			target.GotHit();
			target = null;
		}
	}
	
	void OnTriggerEnter(Collider hit){
		if(hit.gameObject.tag != gameObject.tag){target = hit.gameObject.GetComponent<CharacterParent>();}
		else if(hit.tag == gameObject.tag){Physics.IgnoreCollision(hit, weapon);}
	}
}
