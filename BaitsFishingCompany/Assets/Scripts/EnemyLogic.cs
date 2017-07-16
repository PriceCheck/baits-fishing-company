using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour {

    private GameObject boat;
    private GameObject monster;

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

    public float stunTime = 4.0f;
    private float curStunTime = 0.0f;

    float DistanceTo(GameObject obj)
    {
        if (!obj || !obj.activeInHierarchy)
        {
            return float.MaxValue;
        }

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
        monster = GameObject.Find("MonsterPhysicsPrefab");
        boat = GameObject.Find("BoatPhysicsPrefab");

    }

    public void Thwamp()
    {
        curStunTime = stunTime;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    void LogicLogic()
    {
        if (!GetComponent<ImBig>())
        {
            ImBig[] bigins = FindObjectsOfType<ImBig>();
            foreach(ImBig bigly in bigins)
            {
                if (DistanceTo(bigly.gameObject) < autoFleeRange + 2f)
                {
                    GetComponent<Rigidbody>().velocity -= VectorTo(bigly.gameObject.transform.position) * superFearAcceleration * Time.deltaTime;
                    return;
                }
            }

        }


        if (DistanceTo(monster) > maxCaringDistance && DistanceTo(boat) > maxCaringDistance)
        {
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

    void OnCollision(GameObject other)
    {
        if (other.name == "MonsterPhysicsPrefab")
        {
            FindObjectOfType<ScoreDisplay>().thingsKilled++;
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (curStunTime > 0)
            return;

        LogicLogic();

        DerpyLogic();

	}
}
