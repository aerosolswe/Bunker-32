using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Entity {

	public static void Create(Vector3 position) {
		GameObject plObject = Instantiate(GameManager.instance.playerPrefab);
		plObject.transform.position = position;
		plObject.SetActive(true);
	}
	
	public static WaitForSeconds deathDelay = new WaitForSeconds(2);

	public static Player instance = null;
	private Rigidbody2D rb;

	public AudioSource hitSource;
	public AudioSource deadSource;
	public AudioSource fireSource;

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
		currentHealth = baseHealth;
		dead = false;
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
		fireSource.Play();
	}

	public virtual void RecieveDamage(HitInfo hitInfo) {
		Splat.Create(3, this.transform.position);

		hitSource.Play();

		Health -= hitInfo.damage;

		if(Health <= 0 && !dead) {
			StartCoroutine(Die());
		}
	}

	IEnumerator Die() {
		dead = true;

		deadSource.Play();

		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		GetComponent<Rigidbody2D>().angularVelocity = 0;

		animator.SetTrigger("die");
		
		CameraManager.instance.Zoom(3);

		// Squeeel
		yield return deathDelay;

		Splat.Create(10, this.transform.position, 75f);

		yield return deathDelay;
		// restart
		SceneManager.LoadScene("DeathScene");

	}

	void Update () {
		if(Dead) return;

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
	}
    
	public Vector3 WorldToScreenPoint {
		get {
			return CameraManager.instance.cam.WorldToScreenPoint(transform.position);
		}
	}
    
}
