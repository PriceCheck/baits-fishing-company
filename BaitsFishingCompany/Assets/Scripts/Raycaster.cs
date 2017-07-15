using UnityEngine;
using System.Collections;

public class Raycaster : MonoBehaviour {
    Ray ray;
    RaycastHit hit;
    GameObject LastObjectHit;
    public bool objectHovering = false;
    public Vector3 mousePosition;
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (LastObjectHit != hit.collider.gameObject)
            {
                objectHovering = false;
                //if (LastObjectHit != null)
                //{
                //    //Unhighlight last target
                //    if (LastObjectHit.GetComponent<Target>())
                //    { LastObjectHit.GetComponent<Target>().OnLowlit();
                //    }
                //}
                //Highlight new target
                LastObjectHit = hit.collider.gameObject;
                //if (LastObjectHit.GetComponent<Target>())
                //{ LastObjectHit.GetComponent<Target>().OnHighlit();
                //    objectHovering = true;
                //}
            }
            LastObjectHit = hit.collider.gameObject;
            mousePosition = hit.point;
        }
        //if(Input.GetMouseButtonDown(0))
        //{
        //    if (LastObjectHit != null)
        //    {
        //        //Fire at target
        //        if (LastObjectHit.GetComponent<Target>())
        //        { LastObjectHit.GetComponent<Target>().OnShot(); }
        //    }

        //}
    }
}
