namespace Settings {
	/// <summary>
	/// Настройки дивана
	/// </summary>
	public class Divan {
		/// <summary>
		/// Количество жизней
		/// </summary>
		public int Hp {
			get;
			private set;
		}

		public void TakeDamage(float damage) {
			Hp -= (int)damage;
		}

		public Divan() {
			Hp = LevelEditor.Instance.DivanHealth;
		}
	}
}
