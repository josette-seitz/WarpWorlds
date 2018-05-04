using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScript : MonoBehaviour {
	void Update () {
		transform.Rotate(new Vector3(5,30,20) * Time.deltaTime);
	}
}
