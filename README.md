# GetOut
Игра про лабиринты 🐍

## Техническая информация
**`C#` `MonoGame`**\
- Игра сделана в рамках курса по C#.
- Чтобы включить God Mode, во время прохождения уровня нажмите GOD одновременно
- Решения лабиринтов тут https://github.com/yakovlev05/GetOut-MonoGame/tree/main/Solutions

# Как открыть проект у себя
При работе с проектом могут возникнуть трудности на этапе компиляции. Снизу представлены шаги, что необходимо сделать, чтобы проект корректно запустился

## Проблема с библиотеками
![изображение](https://github.com/yakovlev05/GetOut-MonoGame/assets/144012782/4b11b8d1-8e1e-463c-b7ec-b4238e3ca4e0) \
На фото показан набор используемых библиотек. Все проблемы связаны с monogame extended.

Чтобы все эти библиотеки корректно загрузились (как видно monogame extended загружен не с nuget и вообще это альфа версия), то необходимо проверить конфиг nuget. В нём должен присутствовать источник \
`<add key="lithiumtoast" value="https://www.myget.org/F/lithiumtoast/api/v3/index.json" />` \
Об этом же написано здесь https://github.com/craftworkgames/MonoGame.Extended

В проекте уже лежит конфиг файл, поэтому всё должно работать без этих манипуляций

## Настройка mgcb editor
**Не забудьте установить плагин в вашей ide для редактирования файла mgcb**\
Необходимо в mgcb editor указать путь до скачанного **MonoGame.Extended.Content.Pipeline**

- **Выбираем Content**\
![изображение](https://github.com/yakovlev05/GetOut-MonoGame/assets/144012782/8924ccdb-09e8-4823-9668-3ae9efe89f85) \

- **Выбираем References** \
![изображение](https://github.com/yakovlev05/GetOut-MonoGame/assets/144012782/f00291e4-392e-49b5-8b36-2e54588abb67) \

- **Указываем путь** \
![изображение](https://github.com/yakovlev05/GetOut-MonoGame/assets/144012782/91bcfd6a-32fa-4f2c-8f91-bbcbdc401bd5)\
