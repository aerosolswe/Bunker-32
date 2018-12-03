using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

	public static CameraManager instance = null;

	[HideInInspector]
	public Player player;
	
	[HideInInspector]
	public Camera cam;

	void Awake() {
		instance = this;
	}
	void Start () {
		cam = GetComponent<Camera>();
		player = Player.instance;	
	}
	
	void Update () {
		if(player == null) return;

		Vector3 middle = Vector3.Lerp(player.transform.position, player.MouseToWorldPoint, 0.5f);
		transform.position = new Vector3(middle.x, middle.y, transform.position.z);
	}
}
