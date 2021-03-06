# DivGames

Игра в стиле The Last Stand с элементами стратегии.
Технология: Unity3D (версия 5.2.1p3), C#.
Игра практически без графики, для оценки кода.

Специфика игры:
- Наличие нескольких волн разных противников (по показателям, не обязательно визуально);
- Наличие нескольких типов строений, которые могут тренировать миньонов, а также улучшать
их;
- Возможность управлять героем с помощью мыши: осуществлять передвижение и атаку
противников;
- Герой должен получать опыт за убийство противников;
- Возможность управлять как отдельными миньонами, которые сражаются на стороне игрока,
так и группами миньонов;
- Наличие 3D-карты;
- Наличие двух режимов камеры:
- Камера держит главного героя в центре;
- Игрок может свободно перемещать камеру (например, как в игре StarCraft).

Описание геймплея.

Игра начинается с того, что игроку дается 30 секунд на выполнение каких-нибудь апгрейдов для миньонов, выбор умения для героя и подготовку к битве. 
По истечению этого времени противники начинают спауниться по заданному сценарию. 
Все противники спаунятся в специальной зоне вверху карты и начинают двигаться в сторону базы игрока. 
Их основной целью является разрушение дивана разработчиков.
Одновременно со спауном противников начинают работать казармы и производить миньонов игрока. 
Все миньоны автоматически начинают атаковать ближайших противников.
При наличии миньонов или главного игрока вне фонтана противники должны атаковать либо миньонов, либо главного персонажа (в зависимости от того, кто ближе).
Фонтан умеет восстанавливать жизни, а также является местом респауна главного персонажа. 
Если на карте нет врагов, но есть миньоны, то миньоны должны начать возвращение на базу, к фонтану, при этом жизни миньонов, находясь под фонтаном, тоже восстанавливаются.
Как только на карте появляется хотя бы один противник, все миньоны должны уйти из-под фонтана (вне зависимости от состояния здоровья) и начать атаку противников.
Игрок может выбирать цель для каждого миньона по отдельности. 
Когда цель миньона определена, при выделении этого миньона (левой кнопкой мышки) над головой выбранного врага загорается маркер, независимо от того, была ли цель выбрана автоматически или это сделал игрок.
Помимо одиночного выбора персонажа и/или миньона должна быть возможность выбрать и управлять группой юнитов, причем главный герой также может быть частью этой группы. 
Для того, чтобы выбрать группу юнитов необходимо зажать левую кнопку мыши и выделить прямоугольную область, в которой должен произойти выбор группы. 
После того, как левая кнопка отпущена, в выбранной области должны быть выбраны все юниты, которыми может управлять игрок, включая главного персонажа (если он тоже в этой области), и объединены в одну группу. 
При этом под каждым юнитом группы должен отображаться такой же круг, как и при одиночном выборе. 
Однако, круг под главным персонажем должен отличаться (каким именно способом – на ваш выбор).
Игра заканчивается тогда, когда все волны противников уничтожены либо противники смогли уничтожить (захватить) диван разработчиков.
Если главный персонаж умирает, то он появляется на фонтане через некоторое время (зависит от уровня персонажа) с полным здоровьем.
При этом время, оставшееся до возрождения, должно быть показано в правом верхнем углу с соответствующим сообщением. Когда главный персонаж воскресает, камера должна автоматически центровать главного персонажа.
