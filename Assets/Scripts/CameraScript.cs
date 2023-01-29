using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	private Transform transform, playerTransform, playerHealth;
	private CharacterParent healthScale;

	// Use this for initialization
	void Start () {
		transform = GetComponent<Transform>();
		playerTransform = GameObject.Find("Reu").GetComponent<Transform>();
		playerHealth = GameObject.Find("Canvas").transform.Find("Health").GetComponent<Transform>();
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

	//converting a ca,era space to a worldspace
	public Vector3 CameraToWorldSpace(Vector3 cameraSpace)
    {
		/*/First, we get the difference between the mouseClick and the middle of the screen )in pixels
		Vector3 worldSpace = cameraSpace - new Vector3(Screen.width / 2, Screen.height / 2, 0);
		Debug.Log("Mouse on Screen (pixels): " + worldSpace);
		//we then divide worldspace by either Screen.width or Screen.height, depending on which is bigger,
		//to get the coordinates into roughly units of the screen.
		if (Screen.width > Screen.height)
        {
			worldSpace /= Screen.width;
        }
        else
        {
			worldSpace /= Screen.height;
        }
		Debug.Log("Mouse on Screen (screen coords): " + worldSpace);
		//then times it by the camera size to get the worldspace.
		worldSpace *= 12.5f; // 5*5/2
		//and return the result.
		return worldSpace + transform.position; */
		return Camera.main.ScreenToWorldPoint(cameraSpace);
    }
}
