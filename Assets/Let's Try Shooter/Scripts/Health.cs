using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public int value = 3;

	public void Damage(int damageAmount) {
		value -= damageAmount;

		if (value <= 0) {
			gameObject.SetActive (false);
		}
	}

}