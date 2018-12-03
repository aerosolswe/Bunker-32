using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashManager : MonoBehaviour {

	bool clicked = false;

	void Update() {
		if(clicked) return;

		if(Input.GetMouseButtonDown(0)) {
			clicked = true;

			AudioManager.instance.PlayClickSource();
			SceneManager.LoadScene("Menu");
		}
	}
}
