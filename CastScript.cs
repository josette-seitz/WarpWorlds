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

	void Start ()
	{
    pathPoints.Add (Camera.main.transform);
		winterPS = bird.gameObject.transform.Find("Winter PS").gameObject;
		winterPS.SetActiveRecursively(false);
		birdSound = GetComponent<AudioSource> ();
		magic = GameObject.Find("Magical World");

		planets.gameObject.SetActive(false);

		for (int i = 0; i < worlds.Count; i++) {
			if (i != 0) {
				worlds[i].gameObject.SetActive(false);
			}
		}
	}
	
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
		
		//Scale new world
		iTween.ScaleTo (sphere, Vector3.one * 700f, 1.5f);
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
			sphere.GetComponent<CustomDissolve>().Dissolve();
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
	}

	void InvertedSphereDup ()
	{
		dupInvertedSphere = Instantiate(invertedSphere, new Vector3(0, 4f, -475f), invertedSphere.transform.rotation);
		dupInvertedSphere.transform.localScale = new Vector3(700, 700, 700);
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
		yield return new WaitForSeconds (10.5f);
		{
			warpWorldText.SetActive(true);
			starWarsText.SetActive(true);
		}
	}
}
