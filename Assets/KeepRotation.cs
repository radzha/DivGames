using UnityEngine;
using System.Collections;

public class KeepRotation : MonoBehaviour {

	Quaternion rotation;

	void Start () {
		rotation = transform.rotation;
	}
	
	void Update () {
		transform.rotation = rotation;
	}
}
