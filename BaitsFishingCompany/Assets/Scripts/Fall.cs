using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (GetComponent<Rigidbody>().velocity.y <= -0.2f)
            return;

        GetComponent<Rigidbody>().velocity += new Vector3(0, -0.2f, 0) * Time.deltaTime;
    }
}
