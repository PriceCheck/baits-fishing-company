using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeTheMouse : MonoBehaviour {
    public GameObject globals;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = globals.GetComponent<Raycaster>().mousePosition;
        transform.position =  new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
	}
}
