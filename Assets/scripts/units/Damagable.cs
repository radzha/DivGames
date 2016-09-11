public interface Damagable {
	int TakeDamage(Progress.Unit unit, float damage);
	int Health();
	int MaxHealth();
	void Die();
	bool IsDead();
}
