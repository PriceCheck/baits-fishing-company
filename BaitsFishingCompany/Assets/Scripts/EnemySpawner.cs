using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public float interval = 10.0f;
    public float speedBoost = 0.0f;
    private float curTime;
    public GameObject thingToSpawn;

	// Use this for initialization
	void Start () {
        curTime = interval;
	}
	
	// Update is called once per frame
	void Update () {
        curTime -= Time.deltaTime;
        if(curTime < 0)
        {
            curTime = interval;
            if(interval > 0.2)
            {
                interval *= 0.9f;
            }

            if(speedBoost < 8)
            {
                speedBoost += 0.2f;
            }

            Vector3 spawnPos = new Vector3();

            switch (Random.Range(1, 4))
            {
                case 1:
                spawnPos.x = 30;
                spawnPos.y = Random.Range(-30, 30);
                break;

                case 2:
                    spawnPos.x = -30;
                    spawnPos.y = Random.Range(-30, 30);
                    break;

                case 3:
                    spawnPos.y = 30;
                    spawnPos.x = Random.Range(-30, 30);
                    break;

                case 4:
                    spawnPos.y = -30;
                    spawnPos.x = Random.Range(-30, 30);
                    break;

            }


            GameObject obj = Instantiate(thingToSpawn);
            obj.transform.position = spawnPos;
            obj.GetComponent<EnemyLogic>().hungerAcceleration += speedBoost;

        }

	}
}
