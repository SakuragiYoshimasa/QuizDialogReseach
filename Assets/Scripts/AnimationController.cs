using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : Singleton<AnimationController> {

	[SerializeField]
	private GameObject model;

	void Update(){
		if(Input.GetKeyDown(KeyCode.M)){
			model.SetActive (!model.activeSelf);
		}
	}

}
