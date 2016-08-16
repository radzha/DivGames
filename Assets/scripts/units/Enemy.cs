﻿using Settings;
using UnityEngine;

public class Enemy : MonoBehaviour {
	private Unit settings;

	public Transform gun;

	public float gunAmplitude = 1f;
	public float gunFreq = 0.5f;

	protected float gunShift = 0f;
	protected float gunStep;
	protected bool firingMode = true;
	private Vector3 gunAxis;

	protected virtual void Awake() {
		gunStep = Time.deltaTime * 4 * gunAmplitude * gunFreq;
		var angle = Mathf.PI * (90f - gun.rotation.eulerAngles.x) / 180f;
		gunAxis = new Vector3(0f, -Mathf.Sin(angle), Mathf.Cos(angle)).normalized;
	}

	protected virtual void Update() {
		Move();
		if (firingMode) {
			Fire();
		}
	}

	protected virtual void Fire() {
		if (Mathf.Abs(gunShift) <= gunAmplitude) {
			gunShift += gunStep;
		} else {
			gunStep *= -1f;
			gunShift += gunStep;
		}
		gun.position += gunStep * gunAxis;
	}

	protected void Move() {
		var target = Divan.Instance.transform;
		var speed = 2f;
		var distance = Vector3.Distance(transform.position, target.position);
		transform.position = Vector3.Lerp(transform.position, target.position, speed * Time.deltaTime / distance);
	}
}