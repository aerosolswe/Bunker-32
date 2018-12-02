using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StretchSprite : MonoBehaviour {

	void Start () {
		var sr = GetComponent<SpriteRenderer>();

    	if (sr == null) return;
     
    	transform.localScale = new Vector3(1,1,1);
     
    	var width = sr.sprite.bounds.size.x;
    	var height = sr.sprite.bounds.size.y;
     
    	var worldScreenHeight = Camera.main.orthographicSize * 2.0f;
    	var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

		Vector3 scale = transform.localScale;
    	scale.x = worldScreenWidth / width;
    	scale.y = worldScreenHeight / height;
		transform.localScale = scale;
	}
}
