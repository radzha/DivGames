using UnityEngine;

namespace Progress {
	public class Divan : Singleton<Divan> {

		private Settings.Divan settings;

		void Awake() {
			settings = new Settings.Divan();
		}
	}
}
