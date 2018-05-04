using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathScript : MonoBehaviour {
	public Transform [] myPath;
	public float pathDur; 
	private float startTime;
	public float currTime;
	public float percent;
	GameObject lookTarget;

	void Start () {
		startTime = Time.time;
		lookTarget = new GameObject();
	}

	void Update ()
	{
		currTime = Time.time;
		percent = ((Time.time - startTime) / pathDur);
		if (percent > 1.0) {
			percent = 0;
			startTime = Time.time;
		}
		iTween.PutOnPath (gameObject, myPath, percent);
		float lookPercent = percent + 0.01f;
		if (lookPercent > 1f) {
			lookPercent = lookPercent - 1f;
		}
		iTween.PutOnPath(lookTarget, myPath, lookPercent);
		this.transform.LookAt(lookTarget.transform, Vector3.up);
	}
}
