using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy {
	public static WaitForSeconds deathDelay = new WaitForSeconds(2);

	private Player player;
	private Agent agent;
	private Rigidbody2D rigidbody;
	private Animator animator;

	private Vector3 startPosition;
	private Vector3 lookDir = Vector3.zero;

	public float aggroDst = 6f;
	public float shootDst = 8f;
	public float actionDelay = 1;
	private bool aggro = false;

	private int currentHealth;

	public LayerMask mask;

	void Start() {
		rigidbody = GetComponent<Rigidbody2D>();
		animator = GetComponentInChildren<Animator>();
		agent = GetComponent<Agent>();
		currentHealth = baseHealth;

		player = Player.instance;

		startPosition = transform.position;
	}

	public override void RecieveDamage(HitInfo hitInfo) {
		Debug.Log("Should recieve damage: " + hitInfo.damage + " from " + hitInfo.sender);

		Splat.Create(3, this.transform.position);

		Health -= hitInfo.damage;

		if(Health <= 0) {
			StartCoroutine(Die());
		}
	}

	void Update() {
		if(Health <= 0) return;
		
		fireTime -= Time.deltaTime;
		fireTime = Mathf.Clamp(fireTime, 0, 100);

		if(player == null) {
			player = Player.instance;
			return;
		}

		float dstToPlayer = Vector3.Distance(this.transform.position, player.transform.position);
		
		if(dstToPlayer > 6 && !aggro) {
			// Do idle / patroll
		} else {
			// If aggroed or close enough

			agent.MoveTo(player.transform.position);

			Vector3 dir = (player.transform.position - transform.position).normalized;
			RaycastHit2D hit2D = Physics2D.Raycast(transform.position, dir, 100, mask);
			
			bool rotatePath = true;
			if(hit2D.transform != null) {
				if(hit2D.transform.tag == "Player") {
					rotatePath = false;
				}
			}

			if(rotatePath) {
				dir = agent.velocity.normalized;
			} else {
				// FIRE
				Attack(dir);
			}

			lookDir = Vector3.Lerp(lookDir, dir, 10 * Time.deltaTime);
		}

	}

	public void RotateTowards(Vector2 dir) {
        float rot_z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0f, 0f, rot_z - 90);
	}

	void Attack(Vector3 dir) {
		if(fireTime <= 0) {
			fireTime = fireRate;

			CreateBullet(dir);
		}
	}

	void LateUpdate() {
		if(Health > 0) {
			RotateTowards(lookDir);
			float s = rigidbody.velocity.magnitude;
			animator.SetFloat("speed", s);
		}
	}
	IEnumerator Die() {
		agent.Stop();
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		GetComponent<Rigidbody2D>().angularVelocity = 0;
		GetComponent<Collider2D>().enabled = false;

		animator.SetTrigger("die");

		// Squeeel
		yield return deathDelay;

		Splat.Create(10, this.transform.position, 75f);
		this.enabled = false;
	}

	public int Health {
		get {
			return currentHealth;
		}
		set {
			currentHealth = value;
			currentHealth = Mathf.Clamp(currentHealth, 0, baseHealth);
		}
	}
}
