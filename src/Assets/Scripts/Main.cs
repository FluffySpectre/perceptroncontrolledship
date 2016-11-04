using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Main : MonoBehaviour {
	public Vehicle vehicle;
	public int numTargets = 8;
	public Material targetMarkerMaterial;

	private List<Vector2> mTargets;
	private GameObject targetMarker = null;
	
	// Use this for initialization
	void Awake() {
		vehicle.numForces = numTargets;
		vehicle.transform.position = new Vector3(Random.Range(-20, 20), Random.Range(-10, 10), 0);
		
		MakeTargets();
		
		SetTargetMarker (Vector2.zero);
	}
	
	// Update is called once per frame
	void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
		
		// set new target
		if (Input.GetMouseButtonDown(0)) {
			Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			vehicle.desiredPosition = new Vector2(targetPos.x, targetPos.y);
			SetTargetMarker(targetPos);
		}
		
		vehicle.Steer(mTargets);
	}
	
	void OnGUI() {
		if (GUI.Button(new Rect(Screen.width-100, 0, 100, 40), "Reset")) SceneManager.LoadScene(0);
	}
	
	private void MakeTargets() {
		mTargets = new List<Vector2>();
		for (int i=0; i<numTargets; i++) {
			mTargets.Add(new Vector2(Random.Range(-20, 20), Random.Range(-10, 10)));
			GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);
			g.transform.position = new Vector3(mTargets[i].x, mTargets[i].y, 10);
		}
	}
	
	private void SetTargetMarker(Vector2 pos) {
		if (targetMarker == null) {
			targetMarker = GameObject.CreatePrimitive(PrimitiveType.Cube);
			targetMarker.GetComponent<Renderer>().material = targetMarkerMaterial;	
		}

		targetMarker.transform.position = new Vector3(pos.x, pos.y, 10);
	}
}
