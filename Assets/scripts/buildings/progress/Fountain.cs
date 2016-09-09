using UnityEngine;

namespace Progress {
	public class Fountain : Singleton<Fountain> {

		private Settings.Fountain settings;

		void Awake() {
			settings = new Settings.Fountain();
		}
	}
}
