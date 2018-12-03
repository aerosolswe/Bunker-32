using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour {

	public void Clicked() {
		AudioManager.instance.PlayClickSource();
		Loader.LoadLevel("Menu");
	}
}
