using UnityEngine;

namespace Progress {
	public class Divan : Singleton<Divan>, Damagable {

		private Settings.Divan settings;
		private int health;

		void Awake() {
			settings = new Settings.Divan();
			health = MaxHealth();
		}

		public int Health() {
			return health; 
		}

		public int MaxHealth() {
			return settings.Hp; 
		}

		public int TakeDamage(Progress.Unit unit, float damage) {
			health -= (int)damage;
			return 0;
		}

	}
}
