using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class SoundController : Singleton<SoundController> {

	private AudioSource audioSource;
	private bool _isPlaying = false;


	void Start(){
		audioSource = this.GetComponent<AudioSource> ();

	}
	public bool playSound(AudioClip clip){
		if (audioSource.isPlaying) {
			return false;
		}
		audioSource.clip = clip;
		audioSource.Play ();
		_isPlaying = true;
		LogStartAudio ();
		return true;
	}
	public bool isPlaying(){
		return audioSource.isPlaying;
	}
	public void StopSound(){
		audioSource.Stop ();
	}

	void Update(){
		if(!audioSource.isPlaying && _isPlaying){
			_isPlaying = false;
			LogEndAudio();
		}
	}

	void LogStartAudio(){
		TimeStumpManager.I.WriteTimeStump ("START_AUDIO_" + audioSource.clip.name);

	}

	void LogEndAudio(){
		TimeStumpManager.I.WriteTimeStump ("END_AUDIO_" + audioSource.clip.name);
		SpeechDetectionManager.I.waitDetection = true;
	}
}
