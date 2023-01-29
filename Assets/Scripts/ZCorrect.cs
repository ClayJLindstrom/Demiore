using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//for the items that don't move, but still need their z-axis to be corrected.
public class ZCorrect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 newPose = transform.position;
        //we'll have the z-axis be equal to 10% the y-axis.
        newPose.z = newPose.y / 10;
        transform.position = newPose;
        //Debug.Log("All good");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
