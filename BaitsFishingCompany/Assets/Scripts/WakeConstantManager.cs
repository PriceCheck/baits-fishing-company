using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakeConstantManager : MonoBehaviour {

    public bool onlyShowOnThump = false;
    public BoatController player;
    public Vector3 Offset = Vector3.zero;
    public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    TrailRenderer myTrail;

	// Use this for initialization
	void Start () {
        myTrail = GetComponent<TrailRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if(!player.isActiveAndEnabled)
        { return; }
        transform.position = player.transform.position + player.transform.TransformDirection(Offset);
        if (onlyShowOnThump && !player.thwamping)
        {
            myTrail.time = 0;
        }
        else
        {
            float currentTime = Mathf.Abs(player.currentDirecitalTime) / player.timeTilMaxSpeed;
            myTrail.time = curve.Evaluate(currentTime);
        }
	}

    
}
