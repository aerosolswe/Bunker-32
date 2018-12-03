using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splat : MonoBehaviour {

	public static void Create(float amount, Vector3 position, float force = 50) {
		float radius = 0.15f;
        for (int i = 0; i < amount; i++) {
            float angle = i * Mathf.PI * 2f / amount;
            angle += Random.Range(-1, 1f);
            Vector3 newPos = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            Vector3 dir = (position + newPos) - position;
            
			GameObject splat = Instantiate(GameManager.instance.splatPrefab);
			splat.transform.position = position;
			splat.GetComponent<Splat>().SetProperties(dir, force);
        }
	}

	public static WaitForSeconds disableDelay = new WaitForSeconds(2f);

	public Color[] randomColors;

	private Rigidbody2D rb;
	private SpriteRenderer spriteRenderer;

	public Vector3 direction = new Vector3();
	public float force = 5;

	IEnumerator Start() {
		force = force * Random.Range(0.95f, 1.05f);
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		rb = GetComponent<Rigidbody2D>();
        rb.AddForce(direction * force, ForceMode2D.Impulse);
        rb.angularVelocity = 50;

		Color selectedColor = randomColors[Random.Range(0, randomColors.Length)];
		spriteRenderer.color = selectedColor;

		yield return disableDelay;

		rb.velocity = Vector2.zero;
		rb.angularVelocity = 0;
		rb.bodyType = RigidbodyType2D.Static;
		GetComponentInChildren<Collider2D>().enabled = false;

		selectedColor.a = 0.4f;
		spriteRenderer.color = selectedColor;

	}

	public void SetProperties(Vector3 dir, float force)  {
		this.direction = dir;
		this.force = force;
	}
}
