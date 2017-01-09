using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechDetectionManager : Singleton<SpeechDetectionManager> {

	[SerializeField]
	private MicInput micInput;

	[SerializeField]
	private float threshold;

	public bool waitDetection;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ("Loudness:" + micInput.GetLoudness().ToString());
		if(waitDetection && threshold < micInput.GetLoudness()){
			waitDetection = false;
			//TimeStumpManager.I.WriteTimeStump ("USER_SPEECH_START", );
		}
	}
}
