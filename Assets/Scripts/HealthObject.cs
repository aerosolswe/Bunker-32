using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthObject : MonoBehaviour {

	public int healthAmount = 50;
	public AudioSource pickupSource;

	public void OnTriggerEnter2D(Collider2D collider) {
		if(collider.tag == "Player") {
			pickupSource.Play();
			GetComponent<Collider2D>().enabled = false;
			GetComponent<SpriteRenderer>().enabled = false;
			Player.instance.Health += healthAmount;
			Destroy(this.gameObject, 0.3f);
		}
	}
}
