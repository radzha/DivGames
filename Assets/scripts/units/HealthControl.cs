using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Отображение жизни юнита.
/// </summary>
public class HealthControl : MonoBehaviour {
	public Color ColorMax = Color.green;
	public Color ColorMin = Color.red;
	public GameObject healthControlPrefab;

	private Slider slider;
	private Image fill;

	private Enemy unit;

	void Start() {
		unit = GetComponent<Enemy>();
		var healthControl = Instantiate(healthControlPrefab);
		healthControl.transform.SetParent(transform);
		healthControl.transform.localPosition = new Vector3(0f, 2.3f, 0f);
		slider = healthControl.GetComponentInChildren<Slider>();
		fill = healthControl.transform.Find("Slider/Fill Area/Fill").GetComponent<Image>();
	}

	void Update() {
		var health = unit.GetHealth();
		var maxHealth = unit.Settings.Hp;
		var normalHealth = (float)health / maxHealth;
		slider.value = normalHealth * 100f;
		fill.color = Color.Lerp(ColorMin, ColorMax, normalHealth);
	}
}
