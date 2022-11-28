using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	private Transform transform, playerTransform, playerHealth;
	private CharacterParent healthScale;

	// Use this for initialization
	void Start () {
		transform = GetComponent<Transform>();
		playerTransform = GameObject.Find("Reu").GetComponent<Transform>();
		playerHealth = gameObject.transform.Find("Health").GetComponent<Transform>();
		healthScale = GameObject.Find("Reu").GetComponent<CharacterParent>();
	}
	
	// Update is called once per frame
	void Update () {
		//camera position
		Vector3 cameraPlace = playerTransform.position;
		//cameraPlace.y += 5;
		cameraPlace.z -= 10;
		transform.position = cameraPlace;
		
		Vector3 newHealth = playerHealth.localScale;
		newHealth.x = healthScale.health / 50;
		playerHealth.localScale = newHealth;
	}
}
