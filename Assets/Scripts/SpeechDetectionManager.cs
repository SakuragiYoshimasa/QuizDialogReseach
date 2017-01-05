using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechDetectionManager : Singleton<SpeechDetectionManager> {

	[SerializeField]
	private MicInput micInput;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ("Loudness:" + micInput.GetLoudness().ToString());
	}
}
