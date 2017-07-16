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

    public float tightDistance = 4;
    public float maxDistance = 4.1f;


    Rigidbody myRigidbody;
	// Use this for initialization
	void Start () {
        myRigidbody = GetComponent<Rigidbody>();
	}

    float DistanceTo(GameObject obj)
    {
        return (transform.position - obj.transform.position).magnitude;
    }

    Vector3 VectorTo(Vector3 vec)
    {
        return (vec - transform.position).normalized;
    }
    // Update is called once per frame
    void Update () {

        Vector3 pos = transform.position;
        pos.z = 1;
        transform.position = pos;

        if( DistanceTo(Monster) > maxDistance )
        {
            transform.position = Monster.transform.position - VectorTo(Monster.transform.position) * maxDistance;
        }

        if (DistanceTo(Monster) >= tightDistance)
        {

            var dir = Monster.transform.position - transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            //Quaternion Rot = Quaternion.LookRotation(Monster.transform.position - transform.position);
            //print(Rot);
            //Rot = Quaternion.Euler(new Vector3(0f, 1f, 0f));
            ////print(Rot);
            //print(Rot.eulerAngles);
            //transform.rotation = Quaternion.Slerp(transform.rotation, Rot, 1);
            //print(transform.rotation);

            //Vector3 toLookAt = Monster.transform.position;
            //toLookAt.z = transform.position.z;
            //transform.LookAt(toLookAt);


            if (Input.GetKey(KeyCode.A))
            {
                LineTight(true);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                LineTight(false);
            }
        }
        else
        {
            LineLoose();
        }
    }

    void LineLoose()
    {
        Vector3 moveDirection = Vector3.zero;
        float leftEngine = Input.GetKey(KeyCode.A) ? 1 : 0;
        float rightEngine = Input.GetKey(KeyCode.D) ? 1 : 0;
        moveDirection.y += leftEngine * moveSpeed;
        moveDirection.y -= rightEngine * moveSpeed;

        moveDirection = transform.TransformDirection(moveDirection);

        myRigidbody.velocity *= drag;
        myRigidbody.velocity += moveDirection;
        if (myRigidbody.velocity.magnitude > maxSpeed)
        {
            myRigidbody.velocity = myRigidbody.velocity.normalized * maxSpeed;
        }
    }
    void LineTight(bool goClockwise)
    {

        myRigidbody.velocity = Vector3.zero;

        Vector3 monsterLoc = Monster.transform.position;
        Vector3 playerLoc = transform.position;
        Vector3 connectingVector = playerLoc - monsterLoc;
        float direction = goClockwise ? 1.0f : -1.0f;

        transform.position = (Quaternion.AngleAxis(angularMoveSpeed * Time.deltaTime * direction, Vector3.back) * connectingVector) + monsterLoc;
    }
}
