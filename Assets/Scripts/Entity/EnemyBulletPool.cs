using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletPool : MonoBehaviour {

	public static EnemyBulletPool instance = null;

	private List<GameObject> bullets = new List<GameObject>();

	public GameObject bulletObject;

	public int startAmount = 50;

	void Awake() {
		instance = this;
	}

	void Start () {
		bulletObject.SetActive(false);

		for(int i = 0; i < startAmount; i++) {
			bullets.Add(CreateBullet());
		}
	}
	public GameObject GetBullet() {
		foreach(GameObject go in bullets) {
			if(!go.activeInHierarchy) {
				return go;
			}
		}

		return CreateBullet();
	}

	GameObject CreateBullet() {
		GameObject go = Instantiate(bulletObject);
		go.SetActive(true);
		go.transform.parent = this.transform;
		go.transform.localPosition = Vector3.zero;
		go.transform.localScale = Vector3.one;
		go.transform.localRotation = Quaternion.identity;

		EnemyBullet eb = go.GetComponent<EnemyBullet>();
		eb.Reset();

		go.SetActive(false);

		return go;
	}
	
}
