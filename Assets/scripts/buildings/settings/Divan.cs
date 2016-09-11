namespace Settings {
	/// <summary>
	/// Настройки дивана
	/// </summary>
	public class Divan: Damagable {
		/// <summary>
		/// Количество жизней
		/// </summary>
		public int Hp {
			get;
			private set;
		}

		public int TakeDamage(Progress.Unit unit, float damage) {
			Hp -= (int)damage;
			return 0;
		}

		public Divan() {
			Hp = LevelEditor.Instance.DivanHealth;
		}
	}
}
