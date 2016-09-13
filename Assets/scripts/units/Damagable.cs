public interface Damagable {
	int Health();
	int MaxHealth();
	void Die();
	bool IsDead();
	int TakeDamage(Progress.Unit unit, float damage);
	int TakeDamage(Progress.Unit unit, float damage, float slow, float attackSlow);
}
