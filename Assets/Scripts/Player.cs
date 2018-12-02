using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public static Player instance = null;
	private Rigidbody2D rb;

	public Animator animator;
	public Camera camera;
	public Transform bulletPos;
	private int vertical = 0;
	private int horizontal = 0;

	public float walkSpeed = 5;

	private bool firing = false;
	public float fireRate = 0.3f;
	public float fireTime = 0;

	void Awake() {
		instance = this;
		vertical = 0;
		horizontal = 0;
		rb.velocity = Vector2.zero;
	}

	void Start () {
		rb = GetComponent<Rigidbody2D>();
	}
	
	public void Fire() {
		firing = true;
	}

	public void StopFire() {
		firing = false;
	}

	void CreateBullet() {
		if(PlayerBulletPool.instance == null) return;

		GameObject b = PlayerBulletPool.instance.GetBullet();

		b.transform.position = bulletPos.position;
		
		b.SetActive(true);
	}

	void Update () {
		HandleInput();
		HandleRotation();

		fireTime -= Time.deltaTime;
		fireTime = Mathf.Clamp(fireTime, 0, 100);

		if(!firing) return;

		if(fireTime <= 0) {
			fireTime = fireRate;

			CreateBullet();
		}
	}

	void FixedUpdate() {
		Vector2 velocity = new Vector2(0, 0);

		velocity.x = walkSpeed * horizontal;
		velocity.y = walkSpeed * vertical;

		rb.velocity = velocity;
	}

	void LateUpdate() {
		float s = rb.velocity.magnitude;
		animator.SetFloat("speed", s);
	}

	void HandleRotation() {
		Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;
 
        Vector3 objectPos = WorldToScreenPoint;
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;
 
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        animator.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
	}

	void HandleInput() {
		if(Input.GetMouseButtonDown(0)) {
			Fire();
		}
		if(Input.GetMouseButtonUp(0)) {
			StopFire();
		}

		if(Input.GetKeyDown(KeyCode.W)) {
			vertical += 1;
		}
		if(Input.GetKeyUp(KeyCode.W)) {
			vertical -= 1;
		}
		if(Input.GetKeyDown(KeyCode.S)) {
			vertical -= 1;
		}
		if(Input.GetKeyUp(KeyCode.S)) {
			vertical += 1;
		}

		if(Input.GetKeyDown(KeyCode.D)) {
			horizontal += 1;
		}
		if(Input.GetKeyUp(KeyCode.D)) {
			horizontal -= 1;
		}
		if(Input.GetKeyDown(KeyCode.A)) {
			horizontal -= 1;
		}
		if(Input.GetKeyUp(KeyCode.A)) {
			horizontal += 1;
		}
	}

	public Vector3 WorldToScreenPoint {
		get {
			return camera.WorldToScreenPoint(transform.position);
		}
	}

	public Vector2 MouseToWorldPoint {
		get {
			return camera.ScreenToWorldPoint(Input.mousePosition);
		}
	}
}
