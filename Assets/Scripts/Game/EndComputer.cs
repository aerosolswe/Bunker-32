using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndComputer : MonoBehaviour {

	private Player player;

	void OnCollisionEnter2D(Collision2D coll) {
		Player p = coll.gameObject.GetComponent<Player>();

		if(p != null) {
			GetComponent<Collider2D>().enabled = false;
			GameManager.instance.CurrentLevel = 0;
			SceneManager.LoadScene("EndScene");
		}
	}
	
}
