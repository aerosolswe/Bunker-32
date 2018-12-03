using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	public GameObject playerPrefab;
	public GameObject splatPrefab;

	void Awake() {
		if(instance != null) {
			Destroy(this.gameObject);
			return;
		}

		instance = this;

		DontDestroyOnLoad(this);
	}
	
	public int CurrentLevel {
		get {
			return PlayerPrefs.GetInt("CURRENT_LEVEL", 0);
		}
		set {
			PlayerPrefs.SetInt("CURRENT_LEVEL", value);
		}
	}
}
