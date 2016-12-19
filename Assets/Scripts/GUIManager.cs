using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : Singleton<GUIManager> {

	[SerializeField]
	private RawImage screen;

	public void SetScreenTexture(Texture2D texture){
		screen.texture = texture;
	}
}
