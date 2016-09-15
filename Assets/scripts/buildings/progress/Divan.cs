using UnityEngine;

namespace Progress {
	public class Divan : Singleton<Divan>, Damagable {

		public static bool gameStop;

		public delegate void onGameEnd(bool win);

		public event onGameEnd OnGameEnd;

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

		public Unit.Profit TakeDamage(Unit unit, float damage) {
			if (IsDead()) {
				return new Unit.Profit(0, 0, 0);
			}
			health -= (int)damage;
			if (health <= 0) {
				Die();
			}
			return new Unit.Profit(0, 0, 0);
		}

		public Unit.Profit TakeDamage(Unit unit, float damage, float slow, float attackSlow, float duration) {
			return TakeDamage(unit, damage);
		}

		public void Die() {
			gameObject.SetActive(false);
			OnGameEnd(false);
			gameStop = true;
		}

		public bool IsDead() {
			return health <= 0;
		}

	}
}
