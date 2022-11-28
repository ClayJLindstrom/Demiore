using UnityEngine;
using System.Collections;

public class ProjetileScript : MonoBehaviour {
	private ReuController thrower;

	// Use this for initialization
	void Start () {
		thrower = GameObject.Find("Reu").GetComponent<ReuController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter(Collision impact){
		
	}
}
