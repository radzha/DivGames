using UnityEngine;

namespace Progress {
	public class Fountain : Singleton<Fountain>, Damagable {

		private Settings.Fountain settings;

		void Awake() {
			settings = new Settings.Fountain();
		}

		public int TakeDamage(Unit unit, float damage) {
			return unit is MainCharacter ? settings.PlayerCureSpeed : settings.MignonCureSpeed;
		}
	}
}
