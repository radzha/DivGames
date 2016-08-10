/// <summary>
/// Базовый класс юнита
/// </summary>
public class Unit {
	private int hp = 10;
	private float armor = 10f;
	private int attack = 10;
	private float attackSpeed = 10f;
	private float speed = 10f;
	private float attackRange = 10f;
	private int gold = 10;
	private int xp = 10;

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

	/// <summary>
	/// Броня (Armor) — сколько урона в процентном соотношении может быть поглощено
	/// </summary>
	public float Armor {
		get {
			return armor;
		}
		set {
			armor = value;
		}
	}

	/// <summary>
	/// Показатель атаки (Attack) — сколько урона наносится за один удар
	/// </summary>
	public int Attack {
		get {
			return attack;
		}
		set {
			attack = value;
		}
	}

	/// <summary>
	/// Скорость атаки (Attack Speed) — ударов в секунду
	/// </summary>
	public float AttackSpeed {
		get {
			return attackSpeed;
		}
		set {
			attackSpeed = value;
		}
	}

	/// <summary>
	/// Скорость передвижения (Speed) — метров в секунду
	/// </summary>
	public float Speed {
		get {
			return speed;
		}
		set {
			speed = value;
		}
	}

	/// <summary>
	/// Дальность атаки (Attack Range) — расстояние, на которое необходимо подойти, чтобы нанести урон
	/// </summary>
	public float AttackRange {
		get {
			return attackRange;
		}
		set {
			attackRange = value;
		}
	}

	/// <summary>
	/// Количество золота (Gold) — количество золота, которое получает игрок за убийство юнита
	/// </summary>
	public int Gold {
		get {
			return gold;
		}
		set {
			gold = value;
		}
	}

	/// <summary>
	/// Количество опыта (XP) — количество опыта, которое получает главный персонаж игрока за убийство юнита
	/// </summary>
	public int Xp {
		get {
			return xp;
		}
		set {
			xp = value;
		}
	}
}
