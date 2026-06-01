# Energy System - Тестовое задание

## Как запустить
1. Открыть проект в Unity 6000.4.9f1
2. Установить пакеты: UniTask, VContainer (см. инструкцию ниже)
3. Открыть сцену `Assets/EnergySystem/Scenes/MainScene.unity`
4. Нажать Play

## Установка пакетов
Window → Package Manager → Install package from git URL:
- `com.cysharp.unitask` : https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask
- `jp.hadashikick.vcontainer` : https://github.com/hadashiA/VContainer.git?path=VContainer/Assets/VContainer#1.18.0
- NuGetForUnity : https://github.com/GlitchEnzo/NuGetForUnity.git?path=/src/NuGetForUnity
- `com.cysharp.r3` : устанавливаем из NuGet -> Manage NuGet Packages -> ищем R3 -> накатываем поледнюю версию
- `com.demigiant.dotween` : взят из Asset Store

## Управление
- Кнопка "Spend 10" тратит 10 энергии
- Энергия регенерируется автоматически (5 сек на 1 единицу)
- Прогресс-бар показывает долю до следующей единицы

## Что бы я доделал за 2 часа
1. Сохранение прогресса (PlayerPrefs/JSON) при закрытии игры
2. Анимацию прогресс-бара через DOTween (плавные изменения)
3. Лимит на траты (защита от спама кнопки)
4. Звуки при трате энергии и при полном восстановлении
5. Unit-тесты на EnergyService (таймеры замокать)
6. Отображение времени до следующей единицы в формате "00:00"
7. Поддержка разных разрешений экрана для Canvas
