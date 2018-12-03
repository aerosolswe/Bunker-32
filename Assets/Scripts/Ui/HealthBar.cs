using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {

	public GameObject healthObject;

	void Update() {
		float hp = 1;

		if(Player.instance == null) return;

		hp = (float)Player.instance.Health / Player.instance.baseHealth;

		hp = Mathf.Clamp(hp, 0, 1);

		healthObject.transform.localScale = new Vector3(hp, 1, 1);
	}
}
