using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public int baseHealth = 50;
	public int baseDamage = 20;
	
	//protected bool firing = false;
	public float fireRate = 0.3f;
	protected float fireTime = 0;

	public Transform bulletPos;

	public virtual void RecieveDamage(HitInfo hitInfo) {

	}

	protected void CreateBullet(Vector3 dir) {
		if(EnemyBulletPool.instance == null) return;

		GameObject b = EnemyBulletPool.instance.GetBullet();

		HitInfo hit = new HitInfo();
		hit.sender = this.gameObject;
		hit.damage = 25;
		hit.direction = dir;

		b.GetComponent<EnemyBullet>().hitInfo = hit;

		b.transform.position = bulletPos.position;
		
		b.SetActive(true);
	}

}
