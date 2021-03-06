﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour {
    public bool isActive = true;
    public GameObject toFollow;
    public AnimationCurve speedCurve;
    public float minDistance = 1;
    public float maxDistance = 5;
    public float maxSpeed = 10.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LateUpdate()
    {
        if(!isActive) {
            if (GetComponent<Rigidbody>())
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            return;
        }
        Vector3 toObject =  toFollow.transform.position - transform.position;
        toObject.z = 0;
        float distanceToObject = toObject.magnitude;

        if (distanceToObject <= minDistance)
        {
            if (GetComponent<Rigidbody>())
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            return;
        }

        toObject /= toObject.magnitude;
        float currentSpeed = speedCurve.Evaluate(distanceToObject / maxDistance) * maxSpeed;
        Vector3 currentVelocity = toObject * currentSpeed;

        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().velocity = toObject * currentSpeed;
        }
        else
        {
            transform.position += toObject * currentSpeed * Time.deltaTime;
        }
    }
}
