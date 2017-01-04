using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class SoundController : Singleton<SoundController> {

	private AudioSource audioSource;
	private bool _isPlaying = false;

	private StreamWriter streamWriter;

	void Start(){
		audioSource = this.GetComponent<AudioSource> ();
		streamWriter = new StreamWriter ("LogText_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" +  DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() +".txt", false);
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
		streamWriter.WriteLine (DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString()  + "_" +  DateTime.Now.Hour.ToString()  + "_" + DateTime.Now.Minute.ToString()  + "_" + 
			DateTime.Now.Second.ToString()  + "_" + DateTime.Now.Millisecond.ToString() +  ",START_AUDIO," + audioSource.clip.name);
	}

	void LogEndAudio(){
		streamWriter.WriteLine (DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString()  + "_" +  DateTime.Now.Hour.ToString()  + "_" + DateTime.Now.Minute.ToString()  + "_" + 
			DateTime.Now.Second.ToString()  + "_" + DateTime.Now.Millisecond.ToString() +  ",END_AUDIO," + audioSource.clip.name);
	}

	void OnDisable(){
		streamWriter.Flush ();
		streamWriter.Close ();
	}
}
