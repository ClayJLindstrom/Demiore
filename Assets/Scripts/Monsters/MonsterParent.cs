using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterParent : CharacterParent
{
    //The monsters will all need these things
    //the game manager
    protected GameManager moon;
    //the player
    protected Transform theKid;
    //a danger level (so the weaker run from the stronger, and the stronger prey on the weaker.
    protected int threatLevel;
    //for navigation
    public List<Vector2> currentPath;//making this public just to debug.
    //how far ahead we can see.
    protected float sight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FindTarget(Vector3 targetPos)
    {
        moon.TracePath(transform.position, targetPos);
    }
}
