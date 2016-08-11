namespace Settings {
	/// <summary>
	/// Фонтан
	/// </summary>
	public class Fountain {
		private float heroHealSpeed = 10f;
		private float mignonHealSpeed = 10f;

		/// <summary>
		/// Скорость восстановления жизней героя
		/// </summary>
		public float HeroHealSpeed {
			get {
				return heroHealSpeed;
			}
			set {
				heroHealSpeed = value;
			}
		}

		/// <summary>
		/// Скорость восстановления жизней миньона
		/// </summary>
		public float MignonHealSpeed {
			get {
				return mignonHealSpeed;
			}
			set {
				mignonHealSpeed = value;
			}
		}
	}
}
