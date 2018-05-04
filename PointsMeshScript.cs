using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsMeshScript : MonoBehaviour {
	void Start () {
		MeshRenderer pathPoints = this.GetComponent<MeshRenderer>();
		pathPoints.enabled = false;
	}
}
