using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Progress;

/// <summary>
/// Отображение жизни юнита.
/// </summary>
public class HealthControl : MonoBehaviour {
	public Color ColorMax = Color.green;
	public Color ColorMin = Color.red;
	public GameObject healthControlPrefab;

	private Slider slider;
	private Image fill;

	private Damagable thing;

	void Awake() {
		var unitComponent = GetComponent<Unit>();
		thing = unitComponent != null ? unitComponent as Damagable : GetComponent<Divan>() as Damagable;
		var healthControl = Instantiate(healthControlPrefab);
		if (unitComponent != null) {
			healthControl.transform.SetParent(transform);
			var mult = 8f / transform.localScale.y;
			healthControl.transform.localPosition = new Vector3(0f, mult, 0f);
		} else {
			healthControl.transform.SetParent(transform);
			healthControl.transform.position = transform.position + new Vector3(0f, 1.9f, 0f);
			healthControl.transform.localScale = new Vector3(0.15f, 0.5f, 1f);
			healthControl.transform.rotation = Quaternion.Euler(new Vector3(90f, 135f, 0f));
		}
		slider = healthControl.GetComponentInChildren<Slider>();
		fill = healthControl.transform.Find("Slider/Fill Area/Fill").GetComponent<Image>();
	}

	void Update() {
		var health = thing.Health();
		var maxHealth = thing.MaxHealth();
		var normalHealth = (float)health / maxHealth;
		slider.value = normalHealth * 100f;
		fill.color = Color.Lerp(ColorMin, ColorMax, normalHealth);
	}
}
