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
	
	void FixedUpdate() {
		if(player == null) return;

		Vector3 middle = Vector3.Lerp(player.transform.position, MouseToWorldPoint, 0.5f);

		if(player.Dead) {
			middle = player.transform.position;
		}
		
		transform.position = new Vector3(middle.x, middle.y, transform.position.z);
	}

	public void Zoom(float size) {
		StartCoroutine(zoom(size));
	}

	IEnumerator zoom(float size) {
		bool zooming = true;

		float currentSize = cam.orthographicSize;
		float sizeTo = size;

		float lerpTime = 1;
		float lerp = 0;

		while(zooming) {
			lerp += Time.deltaTime;

        	lerp += Time.deltaTime;
        	if (lerp > lerpTime) {
            	lerp = lerpTime;
        	}
 
       		float t = lerp / lerpTime;
			float s = Mathf.Lerp(currentSize, sizeTo, t);
			cam.orthographicSize = s;

			if(t >= 1) {
				zooming = false;
			}

			yield return null;
		}
	}

    public Vector2 MouseToWorldPoint {
        get {
            Vector3 mousePos = Input.mousePosition;
            mousePos.x = Mathf.Clamp(mousePos.x, 0, Screen.width);
            mousePos.y = Mathf.Clamp(mousePos.y, 0, Screen.height);
            return CameraManager.instance.cam.ScreenToWorldPoint(mousePos);
        }
    }
}
