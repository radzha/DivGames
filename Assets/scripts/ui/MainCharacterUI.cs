using Progress;

/// <summary>
/// Отображение специфической информации о главном персонаже.
/// </summary>
public class MainCharacterUI : UnitInfoUI {

	protected override void OnUnitSelected(Unit unit, bool isSelected) {
		if (unit.IsPlayer) {
			base.OnUnitSelected(unit, isSelected);
		}
	}

	protected override void SetText() {
		var txt = string.Format("\nОпыт: {0}\nЗолото: {1}\nМетеор. дождь: {2}\nЛедяная стрела: {3}",
			MainCharacter.Experience,
			MainCharacter.GoldAmount,
			((MainCharacter)unit).MeteoRainTimerString,
			((MainCharacter)unit).IceArrowTimerString);
		text.text = txt;
	}
}
