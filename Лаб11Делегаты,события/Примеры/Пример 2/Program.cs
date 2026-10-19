using System;

// Определяем свой класс для передачи данных в событии
// Обычно классы для событий наследуют от EventArgs
internal class ClickEventArgs : EventArgs
{
    // Дополнительная информация о клике
    public int ClickX { get; set; } // Координата X клика

    public int ClickY { get; set; } // Координата Y клика
    public DateTime ClickTime { get; set; } // Время клика

    public ClickEventArgs(int x, int y)
    {
        ClickX = x;
        ClickY = y;
        ClickTime = DateTime.Now;
    }
}

// Объявляем делегат для события
// По соглашению, делегаты для событий имеют два параметра:
// object sender - объект, который сгенерировал событие
// EventArgs e - данные события
internal delegate void ClickHandler(object sender, ClickEventArgs e);

// Класс-издатель события (тот, кто генерирует событие)
internal class Button
{
    // Приватное поле для хранения имени кнопки
    private string buttonName;

    // Конструктор
    public Button(string name)
    {
        buttonName = name;
    }

    // Объявление события с использованием ключевого слова event
    // Событие - это специальный тип делегата, который можно только добавлять (+=)
    // или удалять (-=) подписчиков, но нельзя перезаписать или вызвать извне
    public event ClickHandler Click;

    // Метод для имитации нажатия на кнопку
    // Обычно такой метод называется OnИмяСобытия
    public void OnClick(int x, int y)
    {
        Console.WriteLine($"\n[Кнопка '{buttonName}']: Нажата в координатах ({x}, {y})");

        // Проверяем, есть ли подписчики на событие
        // Если есть (Click не равен null), то вызываем событие
        // Передаем ссылку на себя (this) и данные события
        Click?.Invoke(this, new ClickEventArgs(x, y));
        // Оператор ?. означает: если Click не null, то вызвать Invoke, иначе ничего не делать
    }

    // Метод для имитации нажатия без координат
    public void OnClick()
    {
        OnClick(0, 0); // Вызываем основной метод с координатами по умолчанию
    }
}

// Класс-подписчик 1 (реагирует на событие)
internal class Window
{
    private string windowName;

    public Window(string name)
    {
        windowName = name;
    }

    // Метод-обработчик события, подходящий по сигнатуре делегата
    public void Button_Click(object sender, ClickEventArgs e)
    {
        // Проверяем тип отправителя, чтобы получить доступ к его свойствам
        if (sender is Button)
        {
            Button btn = (Button)sender; // Приводим к типу Button
            Console.WriteLine($"  [Окно '{windowName}']: Получено событие от кнопки");
            Console.WriteLine($"    Координаты: ({e.ClickX}, {e.ClickY})");
            Console.WriteLine($"    Время: {e.ClickTime:T}");
        }
    }
}

// Класс-подписчик 2 (логирует все нажатия)
internal class Logger
{
    public void LogClick(object sender, ClickEventArgs e)
    {
        Console.WriteLine($"  [Логгер]: Запись в лог - клик в ({e.ClickX}, {e.ClickY}) в {e.ClickTime:T}");
    }
}

internal class Program
{
    private static void Main()
    {
        Console.WriteLine("События\n");

        // Создаем объекты
        Button button = new Button("ОК");
        Window mainWindow = new Window("Главное окно");
        Window settingsWindow = new Window("Окно настроек");
        Logger logger = new Logger();

        // Подписываемся на событие
        // Используем оператор += для добавления обработчиков
        Console.WriteLine("Подписка на события...");
        button.Click += mainWindow.Button_Click;
        button.Click += logger.LogClick;

        // Генерируем событие - нажимаем кнопку
        Console.WriteLine("\nНажатие кнопки №1 (должны сработать 2 обработчика):");
        button.OnClick(150, 75);

        // Добавляем еще одного подписчика
        Console.WriteLine("\nПодписка второго окна...");
        button.Click += settingsWindow.Button_Click;

        // Снова генерируем событие
        Console.WriteLine("\nНажатие кнопки №2 (должны сработать 3 обработчика):");
        button.OnClick(320, 180);

        // Отписываемся от события
        Console.WriteLine("\nОтписка логгера...");
        button.Click -= logger.LogClick;

        // Генерируем событие после отписки
        Console.WriteLine("\nНажатие кнопки №3 (должны сработать 2 обработчика):");
        button.OnClick(50, 25);

        Console.WriteLine("\nВажные особенности событий:");
        Console.WriteLine("1. События можно только += и -=, нельзя перезаписать (=)");
        Console.WriteLine("2. События нельзя вызвать за пределами класса, где они объявлены");
        Console.WriteLine("3. Подписчики могут быть динамически добавлены и удалены");
        Console.WriteLine("4. Если нет подписчиков, событие равно null");
    }
}