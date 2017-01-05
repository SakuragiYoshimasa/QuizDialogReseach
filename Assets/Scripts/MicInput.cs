using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]	//AudioSourceは必須.
[DisallowMultipleComponent]		//複数アタッチさせない.
public class MicInput : MonoBehaviour {

	//外部から現在の音量を読み取る.
	//最大値は sensitivity による.
	public float GetLoudness() {
		return loudness;
	}

	public float sensitivity = 100;		//感度.音量の最大値.

	float loudness;				//音量.
	float lastLoudness;			//前フレームの音量.

	[Range(0,0.95f)]			//最大1にできてしまうと全く変動しなくなる.
	public float lastLoudnessInfluence;	//前フレームの影響度合い.

	void Start() {
		#if UNITY_WEBPLAYER
		//WebPlayerなら許可を取る.
		yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
		#endif
		InitRecord();
	}

	private AudioSource audio;


	/// <summary>
	/// マイク入力による録音を初期化をします.
	/// 
	/// Microphone.devices で現在接続されているマイクを検索し,それを用いて Start()で録音を開始します.
	/// しかし,指定しなくてもデフォルトのマイクを Unity が勝手に選んでくれるので普通に録音できます.便利です.
	///
	/// Start(string deviceName, bool loop, int lengthSec, int frequency);
	/// 
	/// deviceName 	-> マイク名.指定していない場合,勝手に選ばれるし,柔軟に切り替わります.勝手に切り替わると困る方は指定した方が良いです.
	/// loop 	-> ループさせるか.させないと1回の lengthSec で録音が終了します.
	/// lengthSec	-> 録音して吐き出す AudioClip の長さ.
	/// frequency 	-> 録音して吐き出す AudioClip のサンプルレート
	/// 
	/// </summary>
	void InitRecord() {
		audio = gameObject.GetComponent<AudioSource> ();
		//AudioSource の AudioClip を出力先にして,録音開始.
		//マイクで取り扱えるサンプルレートを調べて当てはめることもできますが,今回は一般的な 44100(44.1kHz) を指定しています.
		audio.clip = Microphone.Start(null, true, 10, 44100);

		//録音したデータを延々と取得するためにループさせます.
		audio.loop = true;

		//録音したデータは再生する必要がないのでミュートにします.
		//audio.mute = true;

		//録音が開始されるまで待ちます.
		while(!(Microphone.GetPosition("") > 0)){}
		Debug.Log ("Microphone valided");
		//データの中身を取得するために再生を始めます.
		audio.Play();
	}

	void Update () {
		CalcLoudness();
	}

	//現フレームの音量を計算します.
	//マイクの感度が良すぎる場合は, lastLoudnessInfluence を上げたりして調節しましょう.
	void CalcLoudness() {
		lastLoudness = loudness;
		Debug.Log (loudness);
		loudness = GetAveragedVolume() * sensitivity * ( 1 - lastLoudnessInfluence ) + lastLoudness * lastLoudnessInfluence;
	}

	//現フレームで再生されている AudioClip から平均的な音量を取得します.
	float GetAveragedVolume()
	{
		//AudioClip の情報を格納する配列.
		//256は適当です.少なすぎれば平均的なサンプルデータが得られなくなるかもしれず,
		//多すぎれば計算量が増えますので良い感じに...
		float[] data = new float[256];
		//最終的に返す音量データ.
		float a = 0;
		//AudioClipからデータを抽出します.
		audio.GetOutputData(data, 0);
		//音データを絶対値に変換します.
		foreach(float s in data)
		{
			a += Mathf.Abs(s);
		}
		//平均を返します.
		return a/256;
	}
}