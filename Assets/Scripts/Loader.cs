using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour {

	private WaitForSeconds delay = new WaitForSeconds(1);
	private static string levelToLoad = "";
	public static void LoadLevel(string level) {
		levelToLoad = level;
		SceneManager.LoadScene("Loading");
	}

	void Start() {
		StartCoroutine(DelayedLoadLevel());
	}
	
	IEnumerator DelayedLoadLevel() {
		yield return delay;

		SceneManager.LoadScene(levelToLoad);
	}

}
