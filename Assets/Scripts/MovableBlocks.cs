using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableBlocks : MonoBehaviour
{
    protected Rigidbody2D rb2d;
	protected Vector2 newSpeed;
	protected float friction;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
		friction = 5 * rb2d.mass;
    }

	// Update is called once per frame
	void Update()
	{
		if (rb2d.velocity != Vector2.zero)
		{
			zCorrect();
		}
		SlowDown(1);
	}

    void zCorrect()
    {
        Vector3 newPose = transform.position;
        //we'll have the z-axis be equal to 10% the y-axis.
        newPose.z = newPose.y / 10;
        transform.position = newPose;
	}

	public virtual void SlowDown(float multiplier)
	{
		if (rb2d.velocity != Vector2.zero)
		{
			newSpeed = rb2d.velocity;
			//we use speed to slow ourselves down again.
			if (Mathf.Abs(newSpeed.x) < friction * Time.deltaTime * multiplier) //if we're close enough to stopping,
			{
				newSpeed.x = 0;
			}
			else if (newSpeed.x > 0) { newSpeed.x -= friction * Time.deltaTime * multiplier; }
			else if (newSpeed.x < 0) { newSpeed.x += friction * Time.deltaTime * multiplier; }
			if (Mathf.Abs(newSpeed.y) < friction * Time.deltaTime * multiplier) //if we're close enough to stopping,
			{
				newSpeed.y = 0;
			}
			else if (newSpeed.y > 0) { newSpeed.y -= friction * Time.deltaTime * multiplier; }
			else if (newSpeed.y < 0) { newSpeed.y += friction * Time.deltaTime * multiplier; }
			rb2d.velocity = newSpeed;
		}
	}
}
