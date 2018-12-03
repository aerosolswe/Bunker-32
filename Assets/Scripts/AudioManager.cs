using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public static AudioManager instance = null;

	public AudioSource clickSource;

	void Awake() {
		if(instance != null) {
			Destroy(this.gameObject);
			return;
		}

		instance = this;

		DontDestroyOnLoad(this);
	}

	public void Start() {
		GetComponent<AudioSource>().Play();
	}

	public void PlayClickSource() {
		clickSource.Play();
	}
}
