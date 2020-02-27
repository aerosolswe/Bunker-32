using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour {

	private Player player;
	private Animator animator;

	public float openDistance = 3;

	private bool lastOpen = false;
	private bool open = false;

	public bool nextLevelDoor = false;

	void Start () {
		animator = GetComponent<Animator>();
		
		if(!nextLevelDoor) {
			GetComponent<BoxCollider2D>().size = new Vector2(1, 1);
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if(!nextLevelDoor) return;

		Player p = coll.gameObject.GetComponent<Player>();

		if(p != null) {
			GetComponent<Collider2D>().enabled = false;
			GameManager.instance.CurrentLevel += 1;
			Loader.LoadLevel("Stairs");
		}
	}
	
	void Update () {
		if(!nextLevelDoor) return;

		if(player == null) {
			player = Player.instance;
			return;
		}
		
		float distance = Vector3.Distance(player.transform.position, transform.position);

		if(distance < openDistance) {
			if(open) {
				return;
			}
 
			open = true;
			animator.SetTrigger("Open");
		} else {
			if(open) {
				animator.SetTrigger("Close");
				open = false;
			}
		}
	}
}
