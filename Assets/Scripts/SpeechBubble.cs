using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class SpeechBubble : MonoBehaviour {

	public static void Create(string text, GameObject parent) {
		GameObject go = Instantiate(GameManager.instance.speechPrefab);
		SpeechBubble sb = go.GetComponent<SpeechBubble>();
		sb.textObject.text = text;
		sb.followTransform = parent.transform;
	}

	public static WaitForSeconds removeDelay = new WaitForSeconds(2);

	IEnumerator Start() {
		yield return removeDelay;

		Destroy(this.gameObject);
	}

	public TextMeshPro textObject;
	public SpriteRenderer spriteRenderer;
	public Transform followTransform;

	public float width = 1;
	public float height = 1;

	public void Update() {
		transform.position = followTransform.position + new Vector3(0.5f, 0.5f, 0);

		float textWidth = textObject.textBounds.size.x;
		float textHeight = textObject.textBounds.size.y;

		width = textWidth + 0.15f;
		height = textHeight + 0.25f;

		Vector2 size = new Vector2(width, height);
		spriteRenderer.size = size;
	}
}
