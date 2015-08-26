using UnityEngine;
using System.Collections;

public class PatrolScript : MonoBehaviour
{

    public Transform[] waypoints;
    private float patrolSpeed = 2;
    private bool loop = true;
    private float dampingLook = 6.0f;
    private float pauseDuration = 0;

    private float curTime;
    private int currentWaypoint;
    private CharacterController character;

	void Start ()
	{
	    character = GetComponent<CharacterController>();
	}
	
	void Update () {
	    if (currentWaypoint < waypoints.Length)
	    {
	        Patrol();
	    }
	    else
	    {
	        if (loop)
	        {
	            currentWaypoint = 0;
	        }
	    }
	}

    void Patrol()
    {
        Vector3 target = waypoints[currentWaypoint].position;
        target.y = transform.position.y;
        Vector3 moveDirection = target - transform.position;

        if (moveDirection.magnitude < 0.5)
        {
            if (curTime == 0)
                curTime = Time.time;

            if ((Time.time - curTime) >= pauseDuration)
            {
                currentWaypoint++;
                curTime = 0;
            }
        }
        else
        {
            var rotation = Quaternion.LookRotation(target - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * dampingLook);
            character.Move(moveDirection.normalized*patrolSpeed*Time.deltaTime);
        }
    }
}
