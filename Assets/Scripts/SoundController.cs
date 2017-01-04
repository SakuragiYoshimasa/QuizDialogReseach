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
		streamWriter.WriteLine (GetTimeStampDate() +  " : START_AUDIO_" + audioSource.clip.name);
	}

	void LogEndAudio(){
		streamWriter.WriteLine (GetTimeStampDate() +  " : END_AUDIO_" + audioSource.clip.name);
	}

	void OnDisable(){
		streamWriter.Flush ();
		streamWriter.Close ();
	}

    string GetTimeStampDate() {
        string time = DateTime.Now.Year.ToString() 
            + "/" + DateTime.Now.Month.ToString().PadLeft(2, '0') 
            + "/" + DateTime.Now.Day.ToString().PadLeft(2, '0') 
            + " " + DateTime.Now.Hour.ToString().PadLeft(2, '0')
            + ":" + DateTime.Now.Minute.ToString().PadLeft(2, '0') 
            + ":" + DateTime.Now.Second.ToString().PadLeft(2, '0')
            + " " + DateTime.Now.Millisecond.ToString().PadLeft(3, '0');
        return time;

      //  2017 / 01 / 04 17:24:12 787 : TIMESTAMP_INTRODUCTION_START

    }
}
