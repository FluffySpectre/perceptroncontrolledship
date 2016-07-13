using UnityEngine;

public class Perceptron {
	public int TrainingIterations { get; private set; }
	
	private float[] mWeights;
	private float mLearningRate;
	
	public Perceptron(int numWeights, float learningRate) {
		mWeights = new float[numWeights];
		mLearningRate = learningRate;
		TrainingIterations = 0;
		
		// init weights with random values
		for (int i=0; i<mWeights.Length; i++) {
			mWeights[i] = Random.Range(0.0f, 1.0f);
		}
	}
	
	public void Train(Vector2[] forces, Vector2 error) {
		for (int i=0; i<mWeights.Length; i++) {
			mWeights[i] += mLearningRate * error.x * forces[i].x;
			mWeights[i] += mLearningRate * error.y * forces[i].y;
			mWeights[i] = Mathf.Clamp01(mWeights[i]);
		}
		
		TrainingIterations++;
	}
	
	public Vector2 FeedForward(Vector2[] forces) {
		Vector2 sum = Vector2.zero;
		
		for (int i=0; i<mWeights.Length; i++) {
			forces[i] *= mWeights[i];
			sum += forces[i];
		}
		
		return sum;
	}
	
	public float[] GetWeights() {
		return mWeights;
	}
}
