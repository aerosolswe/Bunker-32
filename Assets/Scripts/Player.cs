using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public static void Create(Vector3 position) {
		GameObject plObject = Instantiate(GameManager.instance.playerPrefab);
		plObject.transform.position = position;
		plObject.SetActive(true);
	}

	public static Player instance = null;
	private Rigidbody2D rb;

	public Animator animator;
	public Transform bulletPos;
	private float vertical = 0;
	private float horizontal = 0;

	public float walkSpeed = 5;

	private bool firing = false;
	public float fireRate = 0.3f;
	public float fireTime = 0;

	private bool canPlay = false;

	void Awake() {
		instance = this;
	}

	void Start () {
		rb = GetComponent<Rigidbody2D>();
	}

	public void ActivatePlay() {
		canPlay = true;
	}

	public void InactivatePlay() {
		canPlay = false;
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

		HitInfo hit = new HitInfo();
		hit.sender = this.gameObject;
		hit.damage = 25;

		b.GetComponent<PlayerBullet>().hitInfo = hit;

		b.transform.position = bulletPos.position;
		
		b.SetActive(true);
	}

	public virtual void RecieveDamage(HitInfo hitInfo) {

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

		vertical = Input.GetAxisRaw("Vertical");
		horizontal = Input.GetAxisRaw("Horizontal");
		/*if(Input.GetKeyDown(KeyCode.W)) {
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
		}*/
	}

	public Vector3 WorldToScreenPoint {
		get {
			return CameraManager.instance.cam.WorldToScreenPoint(transform.position);
		}
	}

	public Vector2 MouseToWorldPoint {
		get {
			return CameraManager.instance.cam.ScreenToWorldPoint(Input.mousePosition);
		}
	}
}
