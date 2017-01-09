using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class TimeStumpManager : Singleton<TimeStumpManager> {

	private StreamWriter streamWriter;

	private string startTime;

	void Start(){
		streamWriter = new StreamWriter ("LogText_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" +  DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() +".txt", false);
	}

	void OnDisable(){
		streamWriter.Flush ();
		streamWriter.Close ();

	}


	public void WriteTimeStump(string eventName, bool start){
		if (start) {
			startTime = GetTimeStampDate ();
		} else {
			streamWriter.WriteLine (startTime +  ", " + GetTimeStampDate() + ", " + eventName);
		}
	}

	string GetTimeStampDate() {
		string time = DateTime.Now.Year.ToString() 
			+ "/" + DateTime.Now.Month.ToString().PadLeft(2, '0') 
			+ "/" + DateTime.Now.Day.ToString().PadLeft(2, '0') 
			+ "," + DateTime.Now.Hour.ToString().PadLeft(2, '0')
			+ ":" + DateTime.Now.Minute.ToString().PadLeft(2, '0') 
			+ ":" + DateTime.Now.Second.ToString().PadLeft(2, '0')
			+ "." + DateTime.Now.Millisecond.ToString().PadLeft(3, '0');
		return time;
	}
}

