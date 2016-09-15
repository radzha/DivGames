using Progress;

public interface Damagable {
	int Health();
	int MaxHealth();
	void Die();
	bool IsDead();
	Unit.Profit TakeDamage(Unit unit, float damage);
	Unit.Profit TakeDamage(Unit unit, float damage, float slow, float attackSlow, float duration);
}
