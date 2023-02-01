using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	//for enemy navigation. They will share a nodemap (which we will need to reset after each use.
	public NodeMap nodeMap;// = new NodeMap(Vector2.zero, 10, 10, 10, 10);
	//making it nighttime.
	protected Image nightCloak;
	//the player's script
	protected ReuController reu;
	
	//the canvas items (UI aka textbox and images)
	protected Text theText;
	protected Image textImage, terahSprite, reuSprite;

	//characters/enemies that exist in the scene
	protected Object demiDog;
	protected GameObject terah, aDemiDog;
	protected GameObject[] demiChildren;
	protected DemiDogScript[] demiDogs;
	protected Transform playerPosition;

	//don't really need, I think
	public float time, dogs, dogThreshold, ticker, dogPos, dogBound;
	protected bool day;


	// Use this for initialization
	public void InitialStart () {
		//the node map (Vector2 the_position, float x_length, float y_length, int x_count, int y_count)
		//nodeMap = new NodeMap(Vector2.zero, 16, 16, 16, 16);
		demiDog = Resources.Load("DemiDog");
		terah = GameObject.Find("Terah");
		nightCloak = GameObject.Find("Canvas").transform.Find("Night").GetComponent<Image>();
		nightCloak.enabled = false;
		//text box
		terahSprite = GameObject.Find("TerahSprite").GetComponent<Image>();
		reuSprite = GameObject.Find("ReuSprite").GetComponent<Image>();
		theText = GameObject.Find("Canvas").GetComponentInChildren<Text>();
		textImage = GameObject.Find("Canvas").transform.Find("TextBox").GetComponent<Image>();
		reuSprite.enabled = false;
		terahSprite.enabled = false;
		theText.enabled = false;
		textImage.enabled = false;
		//the player
		reu = GameObject.Find("Reu").GetComponent<ReuController>();
		
		time = 160;
		dogs = 0;
		dogThreshold = 5;
		dogPos = 1;
		dogBound = 0.5f;
		ticker = 20;
		day = true;
	}
	
	public void ReuDeath(){
		//reu fainting
		//if(reu.health <= 0){
			time += 180;
			//day = true;nightCloak.enabled = !day; PutOutTorches(day);
			if(reu.HStrLv <= 0){
				//he's officially dead
			}
			else{
				//he starts again, with less health.
				reu.HStrLv -= 1;
				reu.health = reu.HStrLv * 10;
			}
		//}
	}
	
	//returns true/false on whether it is day or not.
	public bool IsDay()
    {
		return day;
    }

	//for finding our path from pointA to pointB
	public virtual List<Vector2> TracePath(Vector3 start, Vector3 finish)
    {
		return nodeMap.TracePath(start, finish);
    }

	//for resetting our Queen's Path.
	public virtual void ResetQueensPath(Vector3 thePlayer){
        nodeMap.SetQueensPath(thePlayer);
	}
}
