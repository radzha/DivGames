/// <summary>
/// Базовый класс юнита
/// </summary>
public class Unit {
	public enum UnitType {
		// Воин ближнего боя
		Warrior,
		// Воин дальнего боя
		Archer,
		// Босс
		Boss
	}

	private int hp;
	private float armor;
	private int attack;
	private float attackSpeed;
	private float speed;
	private float attackRange;
	private int gold;
	private int xp;

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

	public Unit(UnitType type) {
		switch (type) {
		case UnitType.Warrior:
			Hp = 10;
			Armor = 0.1f;
			Attack = 10;
			AttackSpeed = 10f;
			Speed = 10f;
			AttackRange = 3f;
			break;
		case UnitType.Archer:
			Hp = 3;
			Armor = 0f;
			Attack = 5;
			AttackSpeed = 17f;
			Speed = 10f;
			AttackRange = 30f;
			break;
		case UnitType.Boss:
			Hp = 50;
			Armor = 0.5f;
			Attack = 50;
			AttackSpeed = 2f;
			Speed = 2f;
			AttackRange = 30f;
			break;
		}
	}
}
