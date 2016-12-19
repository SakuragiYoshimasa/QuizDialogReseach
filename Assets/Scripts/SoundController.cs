using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : Singleton<SoundController> {

	private AudioSource audioSource;

	void Start(){
		audioSource = this.GetComponent<AudioSource> ();
	}
	public bool playSound(AudioClip clip){
		if (audioSource.isPlaying) {
			return false;
		}
		audioSource.clip = clip;
		audioSource.Play ();
		return true;
	}
	public bool isPlaying(){
		return audioSource.isPlaying;
	}
	public void StopSound(){
		audioSource.Stop ();
	}
}
