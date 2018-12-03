using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScene : MonoBehaviour {

	private WaitForSeconds delay = new WaitForSeconds(0.3f);
	
	public bool canClick = false;

	IEnumerator Start() {
		yield return delay;
		canClick = true;
	}

	void Update() {
		if(canClick) {
			if(Input.GetMouseButtonDown(0)) {
				canClick = false;
				AudioManager.instance.PlayClickSource();
				Loader.LoadLevel("Main");
			}
		}
	}

}
