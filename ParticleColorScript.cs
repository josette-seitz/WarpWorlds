using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleColorScript : MonoBehaviour {

	ParticleSystem particleSystem;

	// Use this for initialization
	void Start () {
		particleSystem = this.GetComponent<ParticleSystem>();

		GradientColorKey[] gradientColorKey = new GradientColorKey[5];
		gradientColorKey[0].color = Color.blue;
    	gradientColorKey[0].time = 0f;

    	gradientColorKey[1].color = Color.green;
    	gradientColorKey[1].time = 0.25f;

    	gradientColorKey[2].color = Color.magenta;
    	gradientColorKey[2].time = 0.5f;

		gradientColorKey[3].color = Color.yellow;;
    	gradientColorKey[3].time = 0.75f;

		gradientColorKey[4].color = Color.red;
    	gradientColorKey[4].time = 1f;


    	//Create Gradient alpha
   		GradientAlphaKey[] gradientAlphaKey = new GradientAlphaKey[5];
    	gradientAlphaKey[0].alpha = 1f;
    	gradientAlphaKey[0].time = 0.0f;

    	gradientAlphaKey[1].alpha = 1f;
    	gradientAlphaKey[1].time = 0.25f;

    	gradientAlphaKey[2].alpha = 1f;
    	gradientAlphaKey[2].time = 0.50f;

		gradientAlphaKey[3].alpha = 1f;
    	gradientAlphaKey[3].time = 0.75f;

		gradientAlphaKey[4].alpha = 1f;
    	gradientAlphaKey[4].time = 1f;


    	//Create Gradient
    	Gradient gradients = new Gradient();
    	gradients.SetKeys(gradientColorKey, gradientAlphaKey);

    	//Create Color from Gradient
    	ParticleSystem.MinMaxGradient colors = new ParticleSystem.MinMaxGradient();
    	colors.mode = ParticleSystemGradientMode.Gradient;
    	colors.gradient = gradients;

   		//Assign the color to particle
   		//To call startColor, store main in a temp variable first
    	ParticleSystem.MainModule assignColor = particleSystem.main;
    	assignColor.startColor = colors;

	}

}
