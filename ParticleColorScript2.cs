using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleColorScript2 : MonoBehaviour {

	private ParticleSystem ps;
	private Color psColor;
	private float duration = 8;
	public float time = 0;
	public Color startColor;
	public Color endColor;

	void Start () {
		ps = this.GetComponent<ParticleSystem>();
		Debug.Log(ps.name);
	}

	void Update ()
	{
		psColor = Color.Lerp (startColor, endColor, time);
		ps.startColor = psColor;

		if (time < 1) {
			time += Time.deltaTime/duration;
		}
		else time = 0;
	}
}
