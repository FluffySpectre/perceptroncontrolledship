using UnityEngine;
using System.Collections.Generic;

public class Vehicle : MonoBehaviour {
	public Vector2 desiredPosition = Vector2.zero;
	public float learningRate = 0.001f;
	public int numForces = 32;
	public float maxForce = 0.1f;
	public float maxSpeed = 4f;
	
	private Perceptron mBrain;
	private Vector2 mVelocity;
	private Vector2 mAcceleration;
	
	// Use this for initialization
	void Start() {
		mBrain = new Perceptron(numForces, learningRate);
		mAcceleration = Vector2.zero;
		mVelocity = Vector2.zero;
	}
	
	// Update is called once per frame
	void Update() {
		mVelocity += mAcceleration;
		mVelocity = Limit(mVelocity, maxSpeed);
		
		Vector3 newPos = (transform.position + new Vector3(mVelocity.x, mVelocity.y, 0));
		newPos.x = Mathf.Clamp(newPos.x, -20, 20);
		newPos.y = Mathf.Clamp(newPos.y, -10, 10);
		
		transform.position = newPos;
		
		mAcceleration = Vector2.zero;
	}
	
	void OnGUI() {
		GUI.Label(new Rect(10, 10, 200, 30), "Training: " + mBrain.TrainingIterations);
		float[] weights = mBrain.GetWeights();
		string w = "";
		for (int i=0; i<weights.Length; i++) {
			w += "W" + (i+1) + ": " + weights[i] + "\n";
		}
		GUI.Label(new Rect(10, 40, 200, Screen.height), w);
	}
	
	private void ApplyForce(Vector2 force) {
		mAcceleration += force;
	}
	
	public void Steer(List<Vector2> targets) {
		Vector2[] forces = new Vector2[targets.Count];
		
		for (int i=0; i<forces.Length; i++) {
			forces[i] = Seek(targets[i]);
		}
		
		Vector2 result = mBrain.FeedForward(forces);
		
		ApplyForce(result);
		
		// calculate error and feed it back into the perceptron
		Vector2 error = desiredPosition - GetVehiclePos2D();
		mBrain.Train(forces, error);
	}
	
	private Vector2 Seek(Vector2 target) {
		Vector2 desired = target - GetVehiclePos2D();
		
		desired.Normalize();
		desired *= maxSpeed;
		
		Vector2 steer = desired - mVelocity;
		steer = Limit(steer, maxForce);
		
		return steer;
	}
	
	private Vector2 Limit(Vector2 v, float max) {
		if (v.x > max) v.x = max;
		if (v.y > max) v.y = max;
		
		return v;
	}
	
	private Vector2 GetVehiclePos2D() {
		return new Vector2(transform.position.x, transform.position.y);
	}
}
