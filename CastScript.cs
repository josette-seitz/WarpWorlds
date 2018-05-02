using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections.Generic;

public class CastScript : MonoBehaviour {

	public GameObject bird;
	public LayerMask objectToHit;

	AudioSource birdSound;

	public List<Transform> pathPoints;
	public int pathIndex;

	public List<WorldInfo> worlds;
	public int worldIndex;

	public GameObject currentParticleSystem;
	private GameObject winterPS;

	public GameObject planets;
	public GameObject invertedSphere;
	private GameObject dupInvertedSphere;
	private GameObject magic;

	private GameObject sphere;
	public WorldInfo currentWorld;

	public GameObject warpWorldText;
	public GameObject starWarsText;

	// Use this for initialization
	void Start ()
	{
        pathPoints.Add (Camera.main.transform);
		winterPS = bird.gameObject.transform.Find("Winter PS").gameObject;
		winterPS.SetActiveRecursively(false);
//		pathIndex = pathPoints.Count -2;
		birdSound = GetComponent<AudioSource> ();
		magic = GameObject.Find("Magical World");


		planets.gameObject.SetActive(false);

		for (int i = 0; i < worlds.Count; i++) {
			if (i != 0) {
				worlds[i].gameObject.SetActive(false);
			}
		}

//		particleSystem[0].gameObject.SetActive(false);
//		particleSystem[1].gameObject.SetActive(false);
//		particleSystem[2].gameObject.SetActive(false);

	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		RaycastHit hit;
		Vector3 fwd = transform.TransformDirection (Vector3.forward);
		Debug.DrawRay (this.transform.position, fwd * 1000, Color.red);

		if (Physics.SphereCast (transform.position, 200f, fwd, out hit, 1000f, objectToHit)) {
			Debug.Log(hit.collider.name);
			if (hit.collider.gameObject.name == "Eagle") {
				birdSound.Play();
				MoveBirdToNextPoint ();
			}			
		}
	}

	void MoveBirdToNextPoint ()
	{
		iTween.MoveTo (bird, iTween.Hash ("position", pathPoints [pathIndex].position, "oncompletetarget", this.gameObject,
			"oncomplete", "PathMoveComplete", "time", 5.5f, "looktarget", pathPoints [pathIndex].position));
		bird.GetComponent<Collider> ().enabled = false;
	}

	void PathMoveComplete ()
	{
		bird.GetComponent<Collider>().enabled = true;
		if (pathIndex == pathPoints.Count - 1) {
		 	//we hit players head
		 	Debug.Log("Transition to next world");
		 	TransitionToNextWorld();
		}

		else pathIndex++;
	}

	void TransitionToNextWorld ()
	{
		if (worlds [worldIndex] == worlds [1]) {
			bird.SetActive (false);
			Destroy (invertedSphere);
			Destroy (dupInvertedSphere);
			worlds [worldIndex].gameObject.SetActive (true);
		}

		worlds [worldIndex].transform.parent = null;
		sphere = worlds [worldIndex].gameObject.transform.Find ("SphereInvertedMesh").gameObject;
		sphere.transform.parent = null;

//		currentParticleSystem.gameObject.SetActive(false);
//		particleSystem[0].gameObject.SetActive(true);

		//Scale new world
		iTween.ScaleTo (sphere, Vector3.one * 5000f, 1.5f);

//		Destroy (currentParticleSystem.gameObject);
//		currentParticleSystem = particleSystem [worldIndex];

		//delete previous world
		Destroy (currentWorld.gameObject);
		currentWorld = worlds [worldIndex];

		currentWorld.transform.position = Vector3.zero;
		currentWorld.transform.eulerAngles = Vector3.zero;
		currentWorld.transform.localScale = Vector3.one;

		//set the skybox
		RenderSettings.skybox = currentWorld.newSkyBox;

		if (worlds [worldIndex] == worlds [1]) {
			bird.GetComponent<Collider> ().enabled = false;
			Destroy (currentParticleSystem.gameObject);
			currentParticleSystem = winterPS;
			//disable all planets when World is Winter
			Renderer[] childPlanets = planets.GetComponentsInChildren<Renderer> ();
			foreach (Renderer child in childPlanets) {
				child.enabled = false;
			}
			sphere.GetComponent<CustomDissolve> ().Dissolve ();
			StartCoroutine(AfterDissolve());
			Invoke("BlackDissolve", 45f);
		}


		if (worlds [worldIndex] == worlds [0]) {			
			bird.GetComponent<Collider> ().enabled = false;
			AudioSource spaceSound = magic.GetComponent<AudioSource> ();
			spaceSound.Play ();
			planets.gameObject.SetActive (true);
			bird.GetComponent<PathScript> ().enabled = true;

			Invoke("InvertedSphereDup", 10f);
			Invoke("EnableBird", 30f);
		}

//		//reset path points (create new)
//		pathPoints = currentWorld.newPathPoints;
//		pathPoints.Add (Camera.main.transform);
//		//set path index back to 0 (start over)
//		pathIndex = 0;
//
//		//make sure not end of world list
//		if (worldIndex + 1 < worlds.Count) {
//			worldIndex++;	
//		} else {
//			Debug.Log("We reach the end of world list");
//		}
//
//		worlds[worldIndex].gameObject.SetActive(true);
//
//		//bird at new start position
//		MoveBirdToNextPoint();
	}

	void InvertedSphereDup ()
	{
		dupInvertedSphere = Instantiate(invertedSphere, new Vector3(0, 10f, -500f), invertedSphere.transform.rotation);
		dupInvertedSphere.transform.localScale = new Vector3(1000, 1000, 1000);
		dupInvertedSphere.AddComponent<RotateScript>();
	}

	void EnableBird ()
	{
		bird.GetComponent<PathScript> ().enabled = false;
		bird.GetComponent<Collider> ().enabled = true;
		worldIndex++;
	}

	void BlackDissolve ()
	{
		sphere.GetComponent<Renderer>().material.color = Color.black;
		sphere.GetComponent<CustomDissolve>().Undissolve();
		Destroy(bird);
		StartCoroutine(FinalCredits());
	}

	private IEnumerator AfterDissolve ()
	{
		yield return new WaitForSeconds(12.5f);
		bird.GetComponent<PathScript> ().enabled = true;
		//Gets the position of the first child in planets GameObject
		bird.transform.position = planets.GetComponentInChildren<Transform>().position;
		bird.SetActive(true);
		currentParticleSystem.SetActiveRecursively(true);
	}

	private IEnumerator FinalCredits ()
	{
		//not sure if below code works (need to google)
		//float winterWorldAudioLength = worlds [1].GetComponent<AudioClip> ().length;

		yield return new WaitForSeconds (10.5f);
		{
			warpWorldText.SetActive(true);
			starWarsText.SetActive(true);
		}

	}
}


//RaycastHit[] sphereHit = Physics.SphereCastAll (transform.position, 1000f, Vector3.forward, Mathf.Infinity);
//				for (int i = 0; i < sphereHit.Length; i++) {
//						if (sphereHit[i].collider.name == "Eagle") {
//						iTween.MoveTo(bird, iTween.Hash("position", Camera.main.transform.position, 
//														"looktarget", Camera.main.transform.position, 
//														"time", 8.5));