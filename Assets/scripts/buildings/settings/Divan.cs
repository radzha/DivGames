namespace Settings {
	/// <summary>
	/// Настройки дивана
	/// </summary>
	public class Divan {
		private int hp;

		/// <summary>
		/// Количество жизней
		/// </summary>
		public int Hp {
			get {
				return hp;
			}
			set {
				hp = value;
			}
		}

		public Divan(int hp) {
			Hp = hp;
		}
	}
}
