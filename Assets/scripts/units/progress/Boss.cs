using UnityEngine;
using System.Collections;
using Progress;

public class Boss : Unit {

	private Vector3 initScale;
	private Vector3 targetScale;


	protected override void Fire() {
//		if (Mathf.Abs(gunShift) <= gunAmplitude) {
//			gunShift += gunStep;
//		} else {
//			gunStep *= -1f;
//			gunShift += gunStep;
//		}
//		gun.localScale = Vector3.Lerp(initScale, targetScale, 0);
	}

}
