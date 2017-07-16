using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Messages;

public class BoatController : MonoBehaviour {

    public float damageTime = 0.5f;
    private float curDamageTime = 0;
    public float reverseCooldown = 0.5f;
    public float curReverseCooldown = 0.0f;

    public GameObject Monster;
    float maxSpeed = 10;
    [HideInInspector]
    public bool thwamping = false;
    public int health = 10;

    public float angularMoveSpeed = 180;
    public float drag = 0.9f;

    public float tightDistance = 4;
    public float maxDistance = 4.1f;

    public AnimationCurve AccelerationRate = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [HideInInspector]
    public float currentDirecitalTime = 0;
    
    public float timeTilMaxSpeed = 5.0f;

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
        if (curDamageTime > 0)
            curDamageTime -= Time.deltaTime;
        if (curReverseCooldown > 0)
            curReverseCooldown -= Time.deltaTime;

        FindObjectOfType<HealthDisplay>().curHealth = health;
        DrawLine();

        float leftEngine = Input.GetKey(KeyCode.A) ? 1 : 0;
        float rightEngine = Input.GetKey(KeyCode.D) ? 1 : 0;
        currentDirecitalTime += (leftEngine * Time.deltaTime) - (rightEngine * Time.deltaTime);
        currentDirecitalTime *= drag;
        currentDirecitalTime = Mathf.Clamp(currentDirecitalTime, -timeTilMaxSpeed, timeTilMaxSpeed);

        thwamping = Mathf.Abs(currentDirecitalTime) >= Mathf.Abs(timeTilMaxSpeed) - 0.05;
        Monster.GetComponent<FollowObject>().isActive = !thwamping;

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

    void DrawLine()
    {
        Vector3 offset = Vector3.right * 0.85f;
        offset = transform.TransformDirection(offset);

        GetComponent<LineRenderer>().SetPosition(0, transform.position + offset);
        GetComponent<LineRenderer>().SetPosition(1, Monster.transform.position);
    }

    void LineLoose()
    {
        Vector3 moveDirection = Vector3.zero;

        float direction = currentDirecitalTime < 0 ? -1 : 1;
        moveDirection.y = AccelerationRate.Evaluate(Mathf.Abs(currentDirecitalTime) / timeTilMaxSpeed) * direction;

        moveDirection = transform.TransformDirection(moveDirection);

        transform.position += moveDirection * maxSpeed * Time.deltaTime;
   
    }
    void LineTight()
    {
        myRigidbody.velocity = Vector3.zero;

        Vector3 monsterLoc = Monster.transform.position;
        Vector3 playerLoc = transform.position;
        Vector3 connectingVector = playerLoc - monsterLoc;
        float direction = currentDirecitalTime < 0 ? -1 : 1;

        transform.position = (Quaternion.AngleAxis(angularMoveSpeed * AccelerationRate.Evaluate(Mathf.Abs(currentDirecitalTime) / timeTilMaxSpeed) * Time.deltaTime * direction, Vector3.back) * connectingVector) + monsterLoc;
    }

    private void OnCollision(GameObject other)
    {
        if (other.GetComponent<EnemyLogic>() || other.GetComponent<Fall>() /*rocks*/ )
        {
            if(!thwamping || other.GetComponent<Fall>())
            {
                TakeDamage(1);
            }

            if (other.GetComponent<EnemyLogic>() && thwamping)
            {
                thwamping = false;
                other.GetComponent<EnemyLogic>().Thwamp();
            }

            if (curReverseCooldown <= 0)
            {
                curReverseCooldown = reverseCooldown;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                currentDirecitalTime = -currentDirecitalTime;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (curDamageTime > 0)
            return;
        FindObjectOfType<HealthDisplay>().curHealth = health;
        curDamageTime = damageTime;
        health -= damage;
        if (health <= 0)
            Destroy(gameObject);
    }
}
