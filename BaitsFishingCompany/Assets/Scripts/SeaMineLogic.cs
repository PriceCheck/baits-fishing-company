using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaMineLogic : MonoBehaviour {

    public float timer = 4.0f;
    bool exploading = false;
    public float killRange = 4.0f;

    float DistanceTo(GameObject obj)
    {
        return (transform.position - obj.transform.position).magnitude;
    }

    // Update is called once per frame
    void Update () {
        if (exploading)
        {
            timer -= Time.deltaTime;
        }

        if(timer <= 0)
        {
            EnemyLogic[] enemies = FindObjectsOfType<EnemyLogic>();
            foreach(var enemy in enemies)
            {
                if(DistanceTo(enemy.gameObject) <= killRange)
                {
                    Destroy(enemy.gameObject);
                }
            }

            GameObject boat = FindObjectOfType<BoatController>().gameObject;
            if (DistanceTo(boat) <= killRange)
            {
                FindObjectOfType<BoatController>().TakeDamage(1);
            }

            Destroy(gameObject);

        }
	}
        
    void OnCollision(GameObject obj)
    {
        if (obj.GetComponent<BoatController>() || obj.name == "MonsterPhysicsPrefab")
        {
            exploading = true;
        }
    }

}
