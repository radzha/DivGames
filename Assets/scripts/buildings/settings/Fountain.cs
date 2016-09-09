namespace Settings {
	/// <summary>
	/// Фонтан
	/// </summary>
	public class Fountain {
		/// <summary>
		/// Скорость восстановления жизней героя
		/// </summary>
		public float PlayerCureSpeed {
			get;
			private set;		
		}

		/// <summary>
		/// Скорость восстановления жизней миньона
		/// </summary>
		public float MignonCureSpeed {
			get;
			private set;
		}

		public Fountain() {
			PlayerCureSpeed = LevelEditor.Instance.fountain.playerCureSpeed;
			MignonCureSpeed = LevelEditor.Instance.fountain.mignonCureSpeed;
		}
	}
}
