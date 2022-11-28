using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BoardManager : MonoBehaviour {
	private Animator torch0, torch1, torch2, torch3;
	private Object demiDog;
	private GameObject terah, aDemiDog;
	private GameObject[] demiChildren;
	private Vector3 dogPos1, dogPos2, dogPos3, dogPos4;
	private SpriteRenderer nightCloak;
	private ReuController reu;
	
	private Text theText;
	private Image textImage, terahSprite, reuSprite;
	
	public float time, dogs, dogThreshold, ticker, dogPos, dogBound;
	public bool day;

	public NodeMap nodeMap = new NodeMap(Vector2.zero, 10, 10, 10, 10);

	// Use this for initialization
	void Start () {
		torch0 = gameObject.transform.Find("TorchTower").transform.Find("VillageSprites1_4").GetComponent<Animator>();
		torch1 = gameObject.transform.Find("TorchTower (1)").transform.Find("VillageSprites1_4").GetComponent<Animator>();
		torch2 = gameObject.transform.Find("TorchTower (2)").transform.Find("VillageSprites1_4").GetComponent<Animator>();
		torch3 = gameObject.transform.Find("TorchTower (3)").transform.Find("VillageSprites1_4").GetComponent<Animator>();
		demiDog = Resources.Load("DemiDog");
		terah = GameObject.Find("Terah");
		demiChildren = new GameObject[8] {GameObject.Find("DemiChild"), GameObject.Find("DemiChild1"), 
			GameObject.Find("DemiChild2"), GameObject.Find("DemiChild3"), GameObject.Find("DemiChild4"), 
			GameObject.Find("DemiChild5"), GameObject.Find("DemiChild6"), GameObject.Find("DemiChild7")};
		nightCloak = GameObject.Find("Main Camera").transform.Find("Night").GetComponent<SpriteRenderer>();
		//text box
		terahSprite = GameObject.Find("TerahSprite").GetComponent<Image>();
		reuSprite = GameObject.Find("ReuSprite").GetComponent<Image>();
		theText = GameObject.Find("Main Camera").GetComponentInChildren<Text>();
		textImage = GameObject.Find("Main Camera").GetComponentInChildren<Image>();
		reuSprite.enabled = false;
		terahSprite.enabled = false;
		theText.enabled = false;
		textImage.enabled = false;
		reu = GameObject.Find("Reu").GetComponent<ReuController>();
		
		dogPos1 = new Vector3(-1,1,9); dogPos2 = new Vector3(10,1,9);
		dogPos3 = new Vector3(-1,1,-3); dogPos4 = new Vector3(5,1,-5);
		time = 160;
		dogs = 0;
		dogThreshold = 5;
		dogPos = 1;
		dogBound = 0.5f;
		ticker = 20;
		day = true;
		nightCloak.enabled = false;
		PutOutTorches(day);
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if(time > 180){
			dogThreshold = 5; dogBound = 0.5f; day = !day; nightCloak.enabled = !day; PutOutTorches(day);
			if(day == true) {time = 150; ticker = 162;}
			else{time = 0; ticker = 12;}
		}
		
		if(!day){
			foreach(GameObject kid in demiChildren){
				kid.SetActive(false);
			}
			terah.SetActive(false);
			if(time > 135){dogThreshold = 150; dogBound = 0f;}//135 for 3 minutes, 225 for 5
			else if(time > 90){dogThreshold = 100;}//90 for 3 minutes, 150 for 5
			if(time > ticker){
				ticker += 6; // rate of dog spawning(12)
				if(dogs < dogThreshold){
					if(dogPos == 4){dogPos = 1; aDemiDog = (GameObject)Instantiate(demiDog, dogPos4, Quaternion.identity);}
					else if(dogPos == 3){dogPos += 1; aDemiDog = (GameObject)Instantiate(demiDog, dogPos3, Quaternion.identity);}
					else if(dogPos == 2){dogPos += 1; aDemiDog = (GameObject)Instantiate(demiDog, dogPos2, Quaternion.identity);}
					else if(dogPos == 1){dogPos += 1; aDemiDog = (GameObject)Instantiate(demiDog, dogPos1, Quaternion.identity);}
					dogs += 1;
				}
			}
		}
		else if(day){
			if(time > ticker){
				ticker += 300;
				foreach(GameObject kid in demiChildren){
					kid.SetActive(true);
				}
				terah.SetActive(true);
			}
		}
	}
	
	void FixedUpdate(){
		//reu fainting
		if(reu.health <= 0){
			time += 180;
			//day = true;nightCloak.enabled = !day; PutOutTorches(day);
			if(reu.HStrLv <= 0){
				
			}
			else{
				reu.HStrLv -= 1;
				reu.health = reu.HStrLv * 10;
			}
		}
	}
	
	void PutOutTorches(bool light){
		torch0.SetBool("Lit", light);
		torch1.SetBool("Lit", light);
		torch2.SetBool("Lit", light);
		torch3.SetBool("Lit", light);
	}
}
