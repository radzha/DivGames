using UnityEngine;

public class Divan : MonoBehaviour {

	private Settings.Divan settings;

	void Awake() {
		if (Instance == null) {
			Instance = this;
		}
		settings = new Settings.Divan(10);
	}

	public static Divan Instance {
		get; private set;
	}
}
