using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastShoot : MonoBehaviour {
	public int gunDamage = 1;
	public float fireRate = 0.25f;
	public float weaponRange = 50;
	public float hitForce = 100;
	public Transform gunEnd;

	private Camera fpsCam;
	private WaitForSeconds shotDuration = new WaitForSeconds(.07f);
	private AudioSource gunAudio;
	private LineRenderer laserLine;
	private float nextFire;

	// Use this for initialization
	void Start () {
		laserLine = GetComponent<LineRenderer>();
		gunAudio = GetComponent<AudioSource>();
		fpsCam = GetComponentInParent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Fire1") && Time.time > nextFire) {
			Fire ();
		} else {
			DrawDebugRay ();
		}
	}

	private void Fire () {
		nextFire = Time.time + fireRate;
		StartCoroutine (ShotEffect ());
		FireRaycast ();
	}

	private void FireRaycast () {
		RaycastHit hit;
		laserLine.SetPosition (0, gunEnd.position);
		if (Physics.Raycast (getRayOrigin(), fpsCam.transform.forward, out hit, weaponRange)) {
			laserLine.SetPosition (1, hit.point);
			DamageTarget (hit);
			KnockbackTarget (hit);
		}
		else {
			laserLine.SetPosition (1, fpsCam.transform.forward * weaponRange);
		}
	}

	private void DrawDebugRay() {
		Debug.DrawRay (getRayOrigin (), fpsCam.transform.forward * weaponRange, Color.green);
	}

	private IEnumerator ShotEffect() {
		gunAudio.Play ();
		laserLine.enabled = true;

		yield return shotDuration;

		laserLine.enabled = false;
	}

	private void DamageTarget (RaycastHit hit) {
		Health target = hit.transform.GetComponent<Health> ();
		if (target) {
			target.Damage (gunDamage);
		}
	}

	private void KnockbackTarget (RaycastHit hit) {
		if (hit.rigidbody) {
			hit.rigidbody.AddForce (-hit.normal * hitForce);
		}
	}

	private Vector3 getRayOrigin() {
		return fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
	}
}
