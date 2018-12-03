using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour {

	public float time = 1.5f;

	IEnumerator Start () {
		yield return new WaitForSeconds(time);

		Destroy(this.gameObject);
	}
	
}
