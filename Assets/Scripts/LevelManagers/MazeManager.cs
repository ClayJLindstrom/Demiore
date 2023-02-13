using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : GameManager
{
    //dog Spawns
    private Vector3[] dogSpawns;
    private int spawnCount;
    // Start is called before the first frame update
    void Start()
    {
        //the node map (Vector2 the_position, float x_length, float y_length, int x_count, int y_count)
        nodeMap = new NodeMap(new Vector2(2, 2), 32, 32, 16, 16);
        InitialStart();
        demiDog = Resources.Load("DemiDogLeader");
        //dogSpawns
        dogSpawns = new Vector3[] {
            transform.Find("DogSpawn1").transform.position, transform.Find("DogSpawn2").transform.position,
            transform.Find("DogSpawn3").transform.position, transform.Find("DogSpawn4").transform.position
        };
        spawnCount = 4;
        dogs = 0;
        day = false;
		playerPosition = GameObject.Find("Reu").GetComponent<Transform>();
        nodeMap.SetQueensPath(playerPosition.position);
        StartCoroutine(SpawnDog());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator SpawnDog()
    {
        //Debug.Log("Spawning Dog");
        if (dogs<spawnCount)
        {
            if (dogPos == 3) { dogPos = 0; aDemiDog = (GameObject) Instantiate(demiDog, dogSpawns[0], Quaternion.identity); }
            else if (dogPos == 2) { dogPos += 1; aDemiDog = (GameObject)Instantiate(demiDog, dogSpawns[1], Quaternion.identity); }
            else if (dogPos == 1) { dogPos += 1; aDemiDog = (GameObject)Instantiate(demiDog, dogSpawns[2], Quaternion.identity); }
            else if (dogPos == 0) { dogPos += 1; aDemiDog = (GameObject)Instantiate(demiDog, dogSpawns[3], Quaternion.identity); }
            dogs += 1;
        }

        yield return new WaitForSeconds(0.2f);
        StartCoroutine(SpawnDog());
    }

    
	//For resetting the queen's path.
	public IEnumerator NewQueensPath(float seconds, Vector3 playerPos)
    {
		yield return new WaitForSeconds(seconds);
		nodeMap.SetQueensPath(playerPos);
		//currentCoro = null;
	}

	public override List<Vector2> TracePath(Vector3 start, Vector3 finish)
    {
		return nodeMap.FindCachePath(start, finish);
    }
}
