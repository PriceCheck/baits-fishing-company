using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;

public class BoatController : MonoBehaviour {
    public GameObject Monster;
    float maxSpeed = 10;
    float thwampSpeed = 9.5f;
    bool thwamping = false;
    public int health = 10;

    public float angularMoveSpeed = 180;
    public float drag = 0.9f;

    public float tightDistance = 4;
    public float maxDistance = 4.1f;

    public AnimationCurve AccelerationRate = AnimationCurve.EaseInOut(0, 0, 1, 1);
    float currentSpeed = 0;
    float timeTilMaxSpeed = 0.8f;

    public float rotLerpTime = 0;
    public float rotLerpTimeTotal = 1;
    public AnimationCurve rotLerpCurve;
    private Quaternion startRot;
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
        float leftEngine = Input.GetKey(KeyCode.A) ? 1 : 0;
        float rightEngine = Input.GetKey(KeyCode.D) ? 1 : 0;
        currentSpeed += (leftEngine * Time.deltaTime) - (rightEngine * Time.deltaTime);
        currentSpeed *= drag;
        currentSpeed = Mathf.Clamp(currentSpeed, -timeTilMaxSpeed, timeTilMaxSpeed);

        if(currentSpeed >= thwampSpeed)
        {
            thwamping = true;
        }

        Vector3 pos = transform.position;
        pos.z = 1;
        transform.position = pos;

        if( DistanceTo(Monster) > maxDistance )
        {
            int n = 30;
            transform.position = ((transform.position * (n - 1) + transform.position + VectorTo(Monster.transform.position) * maxDistance)) / n;
        }

        if (DistanceTo(Monster) >= tightDistance)
        {
            rotLerpTime += Time.deltaTime;
            float lerpValue = rotLerpCurve.Evaluate(rotLerpTime / rotLerpTimeTotal);

            var dir = Monster.transform.position - transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion goalDirection = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Lerp(startRot, goalDirection, lerpValue);

            //Quaternion Rot = Quaternion.LookRotation(Monster.transform.position - transform.position);
            //Rot = Quaternion.Euler(new Vector3(0f, 1f, 0f));
            //print(Rot.eulerAngles);
            //transform.rotation = Quaternion.Slerp(transform.rotation, Rot, 1);

            LineTight();
        }
        else
        {
            startRot = transform.rotation;
            rotLerpTime = 0;
            LineLoose();
        }
    }

    void LineLoose()
    {
        Vector3 moveDirection = Vector3.zero;

        float direction = currentSpeed < 0 ? -1 : 1;
        moveDirection.y = AccelerationRate.Evaluate(Mathf.Abs(currentSpeed) / timeTilMaxSpeed) * direction;

        moveDirection = transform.TransformDirection(moveDirection);

        transform.position += moveDirection * maxSpeed * Time.deltaTime;
   
    }
    void LineTight()
    {
        myRigidbody.velocity = Vector3.zero;

        Vector3 monsterLoc = Monster.transform.position;
        Vector3 playerLoc = transform.position;
        Vector3 connectingVector = playerLoc - monsterLoc;
        float direction = currentSpeed < 0 ? -1 : 1;

        transform.position = (Quaternion.AngleAxis(angularMoveSpeed * AccelerationRate.Evaluate(Mathf.Abs(currentSpeed) / timeTilMaxSpeed) * Time.deltaTime * direction, Vector3.back) * connectingVector) + monsterLoc;
    }

    private void OnCollision(Collider other)
    {
        if (other.gameObject.name == "Enemy")
        {
            if(!thwamping)
            {
                --health;
                if(health <= 0)
                    Destroy(gameObject);
            }
            else
            {
                other.gameObject.GetComponent<EnemyLogic>().Thwamp();
                currentSpeed = 1.0f;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        OnCollision(other);
    }

    void OnTriggerStay(Collider other)
    {
        OnCollision(other);
    }

    void OnTriggerEnter(Collider other)
    {
        OnCollision(other);
    }
}
