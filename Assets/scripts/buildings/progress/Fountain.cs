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

		public int TakeDamage(Unit unit, float damage, float slow, float attackSlow) {
			return TakeDamage(unit, damage);
		}

		public int Health() {
			return int.MaxValue;
		}

		public int MaxHealth() {
			return int.MaxValue;
		}

		public void Die() {
			// Фонтан неразрушим.
		}

		public bool IsDead() {
			return false;
		}

		/// <summary>
		/// Юнит подошел к фонтану.
		/// </summary>
		private void OnTriggerEnter(Collider other) {
			var unit = other.gameObject.GetComponent<Unit>();
			if (unit != null && this.Equals(unit.target.aim)) {
				unit.AimTriggered = true;
			}
		}

		/// <summary>
		/// Юнит ушел из фонтана.
		/// </summary>
		private void OnTriggerExit(Collider other) {
			var unit = other.gameObject.GetComponent<Unit>();
			if (unit != null) {
				unit.AimTriggered = false;
			}
		}
	}
}
