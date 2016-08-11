namespace Settings {
	/// <summary>
	/// Казармы
	/// </summary>
	public class Barraks {
		private Unit.UnitType type;
		private float trainingSpeed;
		private int level;

		/// <summary>
		/// Скорость тренировки - юнитов в секунду
		/// </summary>
		public float TrainingSpeed {
			get {
				return trainingSpeed;
			}
			set {
				trainingSpeed = value;
			}
		}

		/// <summary>
		/// Уровень казармы
		/// </summary>
		public int Level {
			get {
				return level;
			}
			set {
				level = value;
			}
		}

		public Unit.UnitType Type {
			get {
				return type;
			}
			set {
				type = value;
			}
		}
	}
}
