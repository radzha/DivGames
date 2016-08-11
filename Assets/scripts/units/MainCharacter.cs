using System.Runtime.Remoting.Activation;

public class MainCharacter : Unit {
	public class Level {
		/// <summary>
		/// Уровень (Level) — уровень по порядку, начиная с первого (readonly поле, задается
		/// автоматически редактором главного персонажа)
		/// </summary>
		private readonly int levelNum;

		private int xp;

		/// <summary>
		/// Количество опыта, необходимое для достижения уровня
		/// </summary>
		public int Xp {
			get {
				return xp;
			}
			set {
				xp = value;
			}
		}

		// Статы, которые улучшаются от уровня к уровню
		private int hp;
		private float armor;
		private int attack;
		private float attackSpeed;
		private float speed;
		private float attackRange;
		private int gold;
	}

	private Level level;

	public MainCharacter(UnitType type) : base(type) {
		level = new Level();
	}

}
