using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	public GameObject playerPrefab;
	public GameObject splatPrefab;
	public GameObject speechPrefab;

	public string[] deathTexts;

	public int maxLevels = 4;

	void Awake() {
		if(instance != null) {
			Destroy(this.gameObject);
			return;
		}

		instance = this;

		DontDestroyOnLoad(this);
	}

	public string GetRandomDeathText() {
		return deathTexts[Random.Range(0, deathTexts.Length)];
	}
	
	public int CurrentLevel {
		get {
			return PlayerPrefs.GetInt("CURRENT_LEVEL", 0);
		}
		set {

			int level = value;
			level = Mathf.Clamp(level, 0, maxLevels);
			PlayerPrefs.SetInt("CURRENT_LEVEL", level);
			PlayerPrefs.Save();
		}
	}
}
