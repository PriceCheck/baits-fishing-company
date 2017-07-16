using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionRouter : MonoBehaviour {

    void Update()
    {
        Vector3 pos = gameObject.transform.position;
        pos.z = 0;
        gameObject.transform.position = pos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        gameObject.SendMessage("OnCollision", collision.gameObject, SendMessageOptions.DontRequireReceiver);
    }
    private void OnCollisionExit(Collision collision)
    {
        gameObject.SendMessage("OnCollision", collision.gameObject, SendMessageOptions.DontRequireReceiver);
    }
    private void OnCollisionStay(Collision collision)
    {
        gameObject.SendMessage("OnCollision", collision.gameObject, SendMessageOptions.DontRequireReceiver);
    }

    private void OnTriggerEnter(Collider other)
    {
        gameObject.SendMessage("OnCollision", other.gameObject, SendMessageOptions.DontRequireReceiver);
    }
    private void OnTriggerStay(Collider other)
    {
        gameObject.SendMessage("OnCollision", other.gameObject, SendMessageOptions.DontRequireReceiver);
    }
    private void OnTriggerExit(Collider other)
    {
        gameObject.SendMessage("OnCollision", other.gameObject, SendMessageOptions.DontRequireReceiver);
    }



}
