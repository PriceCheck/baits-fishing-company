using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMineSpawner : MonoBehaviour
{

    public float interval = 5.0f;
    public int minePercent= 10;
    private float curTime;
    public GameObject minePrefab;

    public GameObject[] rockPrefabs;

    // Use this for initialization
    void Start()
    {
        curTime = interval;
    }

    // Update is called once per frame
    void Update()
    {
        curTime -= Time.deltaTime;
        if (curTime < 0)
        {
            curTime = interval;
            if (interval > 3)
            {
                interval *= 0.98f;
            }

            Vector3 spawnPos = new Vector3();
            spawnPos.y = 10;
            spawnPos.x = Random.Range(-10, 10);
            GameObject obj;
            if ( Random.Range(1,100) <= minePercent)
            {
                obj = Instantiate(minePrefab);
            }
            else
            {
                obj = Instantiate(rockPrefabs[Random.Range(0, 3)]);
            }
             
            obj.transform.position = spawnPos;
        }

    }
}
