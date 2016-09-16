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
		var txt = string.Format("Уровень: {0}\nОпыт: {1}\nЗолото: {2}\nМетеор. дождь: {3}\nЛедяная стрела: {4}",
			((MainCharacter)unit).level + 1,
			MainCharacter.Experience,
			MainCharacter.GoldAmount,
			((MainCharacter)unit).MeteoRainTimerString,
			((MainCharacter)unit).IceArrowTimerString);
		text.text = txt;
	}
}
