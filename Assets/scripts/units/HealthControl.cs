using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Отображение жизни юнита.
/// </summary>
public class HealthControl : MonoBehaviour {
	public Color ColorMax = Color.green;
	public Color ColorMin = Color.red;
	public Slider slider;
	public Image fill;

	private Enemy unit;

	void Start() {
		unit = GetComponent<Enemy>();
	}

	void Update() {
		var health = unit.GetHealth();
		var maxHealth = unit.Settings.Hp;
		var normalHealth = (float)health / maxHealth;
		slider.value = normalHealth * 100f;
		fill.color = Color.Lerp(ColorMin, ColorMax, normalHealth);
	}
}
