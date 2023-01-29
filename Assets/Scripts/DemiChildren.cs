using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DemiChildren : CharacterParent {
	private Transform reu;
	private Button btn;
	private ReuController reusHealth;
	//text box
	private Image textImage, terahSprite, reuSprite;
	private Text theText;
	
	public bool talking;

	// Use this for initialization
	void Start () {
		//transform = GetComponent<Transform>();
		rectTransform = GetComponent<RectTransform>();
		reu = GameObject.Find("Reu").GetComponent<Transform>();
		anime = GetComponentInChildren<Animator>();
		btn = GetComponent<Button>();
		btn.onClick.AddListener(Respond);
		reusHealth = GameObject.Find("Reu").GetComponent<ReuController>();
		//text box
		terahSprite = GameObject.Find("TerahSprite").GetComponent<Image>();
		reuSprite = GameObject.Find("ReuSprite").GetComponent<Image>();
		theText = GameObject.Find("Canvas").GetComponentInChildren<Text>();
		textImage = GameObject.Find("Canvas").transform.Find("TextBox").GetComponent<Image>();

		health = 0;//using for direction
		speed = 0;//using this for pattern
		maxSpeed = 1;//using this for threshold
		facingRight = true;
		talking = false;

		//for Z-correcting (to potentially be changed later.
		CorrectRectZ();
	}
	
	// Update is called once per frame
	void Update () {
		//the pattern goes that the turn right twice, then turn left, then wait a bit, then repeat.
		//speed represents each state of the pattern. every second, I increase it's number. 2, turn right. 3, turn right. 5, turn left, and revert back to zero;
		if(!talking){
			speed += Time.deltaTime;
			if(speed >= maxSpeed){
				if(maxSpeed == 2 || maxSpeed == 3){health += 1;}
				else if(maxSpeed == 5){health -= 1;}
				maxSpeed += 1;
			}
			if(speed >= 5){
				speed = 0;
				maxSpeed = 1;
			}
		}
		else if(talking){
			float xDif = Mathf.Abs(reu.position.x - transform.position.x); 
			float zDif = Mathf.Abs(reu.position.z - transform.position.z);
			
			
			if(xDif > zDif){
				if(transform.position.x > reu.position.x){health = 3;}
				else if(transform.position.x < reu.position.x){health = 1;}
				anime.SetInteger("Direction", 0);
			}//side
			else{
				if(transform.position.z < reu.position.z){health = 0;}//up
				else if(transform.position.z > reu.position.z){health = 2;}//down
			}
		}
		if(health < 0){health = 3;}
		if(health > 3){health = 0;}
		//health represents direction. N = 0, E = 1, etc.
		if(health == 0){anime.SetInteger("Direction", 2);}//North
		if(health == 1){facingRight = true; anime.SetInteger("Direction", 0);}//East
		if(health == 2){anime.SetInteger("Direction", 1);}//South
		if(health == 3){facingRight = false; anime.SetInteger("Direction", 0);}//West
	}
	
	void FixedUpdate(){
		// Flip
		if(transform.localScale.x > 0 && !facingRight){Flip();}
		if(transform.localScale.x < 0 && facingRight){Flip();}
	}
	
	
	public void Respond(){
		if(!talking){
			if(gameObject.tag == "NPC"){StartCoroutine(Response("Hi."));}
			else{
				if(reusHealth.HStrLv < 10){
					StartCoroutine(TerahsResponseOne());
				}
				else{StartCoroutine(Response("Hi, Reu!"));}
			}
		}
	}
	
	IEnumerator Response(string sentence){
		talking = true;
		textImage.enabled = true;
		theText.enabled = true;
		theText.text = sentence;
		yield return new WaitForSeconds(2);
		talking = false;
		textImage.enabled = false;
		theText.enabled = false;
	}
	
	IEnumerator TerahsResponseOne(){
		talking = true;
		textImage.enabled = true;
		terahSprite.enabled = true;
		theText.enabled = true;
		theText.text = "Reu, I hope you're okay...";
		yield return new WaitForSeconds(4);
		reusHealth.HStrLv += 1;
		reusHealth.health += 10;
		talking = false;
		textImage.enabled = false;
		terahSprite.enabled = false;
		theText.enabled = false;
	}
}
