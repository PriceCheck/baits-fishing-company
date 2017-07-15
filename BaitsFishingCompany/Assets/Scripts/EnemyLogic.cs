using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour {

    public GameObject boat;
    public GameObject monster;

    public float hungerAcceleration = 2;
    public float fearAcceleration = 6;
    public float superFearAcceleration = 10;
    public float maxCaringDistance = 10;
    public float derpyVelocity = 5;
    public float derpTimeInterval = 2;
    public float derpRange = 10;

    public float slowyDownFactor = 0.98f;
    public float autoFleeRange = 2.0f;

    private float timeToNextDerpChange = 0;
    private Vector3 currentDerpGoal;
    private Vector3 derpDirection;

    float DistanceTo(GameObject obj)
    {
        return (transform.position - obj.transform.position).magnitude;
    }

    Vector3 GetNextDerpGoal()
    {
        Vector3 random = new Vector3(Random.Range(-derpRange, derpRange), Random.Range(-derpRange, derpRange), 0);
        return (random + currentDerpGoal) / 2;
    }

    Vector3 VectorTo(Vector3 vec)
    {
        return (vec - transform.position).normalized;
    }

	// Use this for initialization
	void Start () {
		
	}
	
    void LogicLogic()
    {
        if (DistanceTo(monster) > maxCaringDistance && DistanceTo(boat) > maxCaringDistance)
        {
            //GetComponent<Rigidbody>().velocity *= Mathf.Pow(slowyDownFactor, Time.deltaTime);
            return;
        }

        if(DistanceTo(monster) < autoFleeRange)
        {
            GetComponent<Rigidbody>().velocity -= VectorTo(monster.transform.position) * superFearAcceleration * Time.deltaTime;
        }
        else if ( DistanceTo(monster) < DistanceTo(boat))
        {
            GetComponent<Rigidbody>().velocity -= VectorTo(monster.transform.position) * fearAcceleration * Time.deltaTime;
        }
        else
        {
            GetComponent<Rigidbody>().velocity += VectorTo(boat.transform.position) * hungerAcceleration * Time.deltaTime;
        }
    }

    void DerpyLogic()
    {
        if(timeToNextDerpChange > 0)
        {
            timeToNextDerpChange -= Time.deltaTime;
        }
        else
        {
            currentDerpGoal = GetNextDerpGoal();
            timeToNextDerpChange = derpTimeInterval;
            derpDirection = VectorTo(currentDerpGoal);
        }
        transform.position += derpDirection * derpyVelocity * Time.deltaTime;

    }

	// Update is called once per frame
	void Update ()
    {
        LogicLogic();

        DerpyLogic();

	}
}
