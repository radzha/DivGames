﻿using UnityEngine;
using System.Collections;

public class Boss : Enemy {

	private Vector3 initScale;
	private Vector3 targetScale;

	protected override void Awake() {
		gunStep = Time.deltaTime * 2 * (gunAmplitude - gun.localScale.x) * gunFreq;
		initScale = gun.localScale;
		targetScale = initScale * gunAmplitude;
	}

	protected override void Fire() {
		if (Mathf.Abs(gunShift) <= gunAmplitude) {
			gunShift += gunStep;
		} else {
			gunStep *= -1f;
			gunShift += gunStep;
		}
		gun.localScale = Vector3.Lerp(initScale, targetScale, 0);
	}

}