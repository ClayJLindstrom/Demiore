using UnityEngine;
using System.Collections;

public class ClockScript : MonoBehaviour {
	private Transform bigHand, littleHand;
	private float time;
	private Quaternion secondRotate, minuteRotate;

	// Use this for initialization
	void Start () {
		bigHand = gameObject.transform.Find("BigHand").GetComponent<Transform>();
		littleHand = gameObject.transform.Find("LittleHand").GetComponent<Transform>();
		time = 0;
	}
	
	// Update is called once per frame
	void Update () {
		secondRotate = bigHand.transform.localRotation;
		minuteRotate = littleHand.transform.localRotation;
		secondRotate.eulerAngles = new Vector3(0,0,time * 120);
		minuteRotate.eulerAngles = new Vector3(0,0,time * 10);
		bigHand.localRotation = secondRotate;
		littleHand.localRotation = minuteRotate;
		time += Time.deltaTime;
	}
}
