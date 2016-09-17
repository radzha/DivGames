using Progress;

/// <summary>
/// Отображение специфической информации о главном персонаже.
/// </summary>
public class MainCharacterUI : UnitInfoUI {

	protected override void OnUnitSelected(Selectable unit) {
		if (unit is Unit && (unit as Unit).IsPlayer) {
			base.OnUnitSelected(unit);
		}
	}

	protected override void SetText() {
		var txt = string.Format("Уровень: {0}\nОпыт: {1}\nЗолото: {2}\nМетеор. дождь: {3}\nЛедяная стрела: {4}",
			((MainCharacter)thing).Level + 1,
			Player.Experience,
			Player.GoldAmount,
			((MainCharacter)thing).MeteoRainTimerString,
			((MainCharacter)thing).IceArrowTimerString);
		text.text = txt;
	}
}
