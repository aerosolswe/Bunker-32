using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

	private Player player;

	void Start () {
		player = Player.instance;	
	}
	
	void Update () {
		Vector3 middle = Vector3.Lerp(player.transform.position, player.MouseToWorldPoint, 0.5f);
		transform.position = new Vector3(middle.x, middle.y, transform.position.z);
	}
}
