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

	public bool boss = false;

	public LayerMask mask;

	public AudioSource hitSource;
	public AudioSource deadSource;
	public AudioSource fireSource;

	private bool dead = false;

	void Start() {
		rigidbody = GetComponent<Rigidbody2D>();
		animator = GetComponentInChildren<Animator>();
		agent = GetComponent<Agent>();

		currentHealth = baseHealth;
		damage = baseDamage;

		int randomDiff = Random.Range(0, 3);
		if(randomDiff == 0) {
			damage += 0;
			currentHealth += 0;
			fireRate -= 0;
		} else if(randomDiff == 1) {
			damage += 5;
			currentHealth += 0;
			fireRate -= 0.1f;
		} else if(randomDiff == 2) {
			damage += 15;
			currentHealth += 0;
			fireRate -= 0.4f;
		}
		

		player = Player.instance;

		startPosition = transform.position;
	}

	public override void RecieveDamage(HitInfo hitInfo) {
		aggro = true;

		Splat.Create(3, this.transform.position);

		hitSource.Play();

		Health -= hitInfo.damage;

		if(Health <= 0 && !dead) {
			StartCoroutine(Die());
		}
	}

	public void SetBoss() {
		baseDamage = 10;
		damage = 10;
		baseHealth = 200;
		currentHealth = 200;
		fireRate = 0.15f;
	}

	void Update() {
		if(!Grid.generated) return;
		if(Health <= 0) return;
		
		fireTime -= Time.deltaTime;
		fireTime = Mathf.Clamp(fireTime, 0, 100);
		
		idleTime -= Time.deltaTime;
		idleTime = Mathf.Clamp(idleTime, 0, 100);

		if(player == null) {
			player = Player.instance;
			return;
		}

		float dstToPlayer = Vector3.Distance(this.transform.position, player.transform.position);
		
		if(dstToPlayer > 10 && !aggro) {
			// Do idle / patroll / look around
			Idle();
		} else {
			// If aggroed or close enough
			if(!aggro) {
				if(!CanSeePlayer()) return;
			}
			if(dstToPlayer > 2) {
				agent.MoveTo(player.transform.position);
			}

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

	public void Idle() {
		lookDir = Vector3.Lerp(lookDir, agent.velocity.normalized, 10 * Time.deltaTime);

		if(idleTime <= 0) {
			idleTime = idleRate;
			int randomIdleIndex = Random.Range(0, 3);
			
			
			switch(randomIdleIndex) {
				case 0:
				break;
				case 1:
				agent.MoveTo(startPosition + new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), 0));
				break;
				case 2:
				break;
			}
		}

	}

	IEnumerator patrolRandom() {

		yield return null;
	}

	public bool CanSeePlayer() {
		Vector3 dir = (player.transform.position - transform.position).normalized;
		RaycastHit2D hit2D = Physics2D.Raycast(transform.position, dir, 100, mask);
		
		if(hit2D.transform != null) {
			if(hit2D.transform.tag == "Player") {
				return true;
			}
		}

		return false;
	}

	public void RotateTowards(Vector2 dir) {
        float rot_z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0f, 0f, rot_z - 90);
	}

	void Attack(Vector3 dir) {
		if(fireTime <= 0) {
			fireTime = fireRate;

			fireSource.Play();
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
		dead = true;
		agent.Stop();
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		GetComponent<Rigidbody2D>().angularVelocity = 0;
		GetComponent<Collider2D>().enabled = false;

		animator.SetTrigger("die");

		SpeechBubble.Create(GameManager.instance.GetRandomDeathText(), this.gameObject);

		// Squeeel
		yield return deathDelay;

		deadSource.Play();
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
