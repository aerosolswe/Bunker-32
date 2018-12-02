using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour {

	private Rigidbody2D rb;
	public SpriteRenderer spriteRenderer;
	public Vector3 target;

	public ParticleSystem trailPS;
	public ParticleSystem explodePS;
	public Vector2 direction = new Vector2();

	private float bulletSpeed = 12;

	private bool destroying = false;

	public void Start() {
		rb = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void OnEnable() {
		if(Player.instance == null) return;

		Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;
 
        Vector3 objectPos = Player.instance.WorldToScreenPoint;
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;
	

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

		direction = mousePos.normalized;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if(destroying) return;

		destroying = true;

		spriteRenderer.enabled = false;

		explodePS.gameObject.SetActive(true);
		trailPS.gameObject.SetActive(false);

		Invoke("Inactivate", 1);
	}

	void Inactivate() {
		gameObject.SetActive(false);
		Reset();
	}

	public void FixedUpdate() {
		if(destroying) {
			rb.velocity = Vector2.zero;
			return;
		}

		Vector2 velocity = new Vector2();
		velocity.x += bulletSpeed;
		velocity.y += bulletSpeed;

		velocity *= direction;

		rb.velocity = velocity;
	}

	public void Reset() {
		destroying = false;
		trailPS.gameObject.SetActive(true);
		explodePS.gameObject.SetActive(false);
		spriteRenderer.enabled = true;
	}
}
