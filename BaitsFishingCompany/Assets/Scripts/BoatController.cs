using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;

public class BoatController : MonoBehaviour {
    public GameObject Monster;
    public float moveSpeed = 3;
    public float maxSpeed = 5;
    public float angularMoveSpeed = 180;
    public float drag = 0.9f;
    Rigidbody myRigidbody;
	// Use this for initialization
	void Start () {
        myRigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.A))
        {
            LineTight(true);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            LineTight(false);
        }
    }

    void LineLoose()
    {
        Vector3 moveDirection = Vector3.zero;
        float leftEngine = Input.GetKey(KeyCode.A) ? 1 : -1;
        float rightEngine = Input.GetKey(KeyCode.D) ? 1 : -1;
        moveDirection.x -= leftEngine * moveSpeed;
        moveDirection.x += rightEngine * moveSpeed;
        myRigidbody.velocity *= drag;
        myRigidbody.velocity += moveDirection;
        if (myRigidbody.velocity.magnitude > maxSpeed)
        {
            myRigidbody.velocity = myRigidbody.velocity.normalized * maxSpeed;
        }
    }
    void LineTight(bool goClockwise)
    {
        Vector3 monsterLoc = Monster.transform.position;
        Vector3 playerLoc = transform.position;
        Vector3 connectingVector = playerLoc - monsterLoc;
        float direction = goClockwise ? 1.0f : -1.0f;

        transform.position = (Quaternion.AngleAxis(angularMoveSpeed * Time.deltaTime * direction, Vector3.back) * connectingVector) + monsterLoc;
        print(transform.position);
    }
}
