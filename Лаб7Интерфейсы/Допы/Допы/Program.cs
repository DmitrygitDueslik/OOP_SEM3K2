using System;
using System.Collections.Generic;
using System.Diagnostics;

// Данные, которые передаются по цепочке плагинов
class PluginContext
{
    public int[] Data { get; }
    public bool IsValid { get; set; } = true;
    public List<string> Journal { get; } = new List<string>(); // журнал действий

    public PluginContext(int[] data)
    {
        Data = data;
    }
}

// Контракт, который обязан выполнить любой плагин
interface IPlugin
{
    string Name { get; }
    int Order { get; }                  // порядок выполнения (меньше — раньше)
    void Initialize();
    void Execute(PluginContext context);
    void Shutdown();
}

// Засекает время работы всего конвейера: от Initialize до Shutdown
class TimerPlugin : IPlugin
{
    private Stopwatch stopwatch = new Stopwatch();
    public string Name => "TimerPlugin";
    public int Order => 0;

    public void Initialize()
    {
        stopwatch.Restart();
        Console.WriteLine($"[{Name}] таймер запущен");
    }

    public void Execute(PluginContext context)
    {
        context.Journal.Add($"{Name}: замер времени идёт");
    }

    public void Shutdown()
    {
        stopwatch.Stop();
        Console.WriteLine($"[{Name}] время работы: {stopwatch.Elapsed.TotalMilliseconds:F3} мс");
    }
}

// Проверяет входные данные: массив не пуст и нет отрицательных чисел
class ValidatorPlugin : IPlugin
{
    public string Name => "ValidatorPlugin";
    public int Order => 1;

    public void Initialize() => Console.WriteLine($"[{Name}] готов к проверке");

    public void Execute(PluginContext context)
    {
        if (context.Data.Length == 0)
        {
            context.IsValid = false;
            context.Journal.Add($"{Name}: массив пуст");
            return;
        }
        foreach (int x in context.Data)
        {
            if (x < 0)
            {
                context.IsValid = false;
                context.Journal.Add($"{Name}: найдено отрицательное число {x}");
                return;
            }
        }
        context.Journal.Add($"{Name}: данные корректны");
    }

    public void Shutdown() => Console.WriteLine($"[{Name}] завершён");
}

// Логирует все действия, накопленные в журнале
class LoggerPlugin : IPlugin
{
    public string Name => "LoggerPlugin";
    public int Order => 2;

    public void Initialize() => Console.WriteLine($"[{Name}] журнал открыт");

    public void Execute(PluginContext context)
    {
        context.Journal.Add($"{Name}: вывод журнала");
        foreach (string record in context.Journal)
        {
            Console.WriteLine($"   LOG> {record}");
        }
    }

    public void Shutdown() => Console.WriteLine($"[{Name}] журнал закрыт");
}

// Менеджер ничего не знает о конкретных плагинах — только об интерфейсе IPlugin
class PluginManager
{
    private List<IPlugin> plugins = new List<IPlugin>();

    public void Load(IPlugin plugin)
    {
        plugins.Add(plugin);
        plugins.Sort((a, b) => a.Order.CompareTo(b.Order)); // упорядочиваем по Order
    }

    public void Run(PluginContext context, Action<PluginContext> mainWork)
    {
        foreach (IPlugin p in plugins) p.Initialize();

        // основной код программы выполняется, только если данные валидны
        foreach (IPlugin p in plugins)
        {
            p.Execute(context);
            if (p is ValidatorPlugin && context.IsValid)
            {
                mainWork(context);
            }
        }

        // завершаем в обратном порядке
        for (int i = plugins.Count - 1; i >= 0; i--) plugins[i].Shutdown();
    }
}

class Program
{
    // «Основной код» — считает сумму; о плагинах он ничего не знает
    static void Sum(PluginContext ctx)
    {
        int sum = 0;
        foreach (int x in ctx.Data) sum += x;
        ctx.Journal.Add($"Основная программа: сумма = {sum}");
    }

    static void Main()
    {
        PluginManager manager = new PluginManager();
        // порядок загрузки не важен: менеджер сам упорядочит плагины по Order
        manager.Load(new LoggerPlugin());
        manager.Load(new ValidatorPlugin());
        manager.Load(new TimerPlugin());

        Console.WriteLine("=== Запуск 1: корректные данные ===");
        manager.Run(new PluginContext(new[] { 5, 10, 15 }), Sum);

        Console.WriteLine("\n=== Запуск 2: некорректные данные ===");
        manager.Run(new PluginContext(new[] { 5, -3, 15 }), Sum);
    }
}
 