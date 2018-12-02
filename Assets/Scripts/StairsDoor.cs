using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StairsDoor : MonoBehaviour {

	private bool hit = false;
	void OnTriggerEnter2D(Collider2D coll) {
		if(hit) return;

		if(coll.gameObject.GetComponent<Player>() != null) {
			hit = true;
			Loader.LoadLevel("Main");
		}
	}
}
