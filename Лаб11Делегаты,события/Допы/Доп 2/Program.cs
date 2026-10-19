using System;

// Игра: игрок загадал число, компьютер угадывает делением отрезка пополам
internal class GuessGame
{
    private int secret;
    private int min, max;

    public event Action<int, int>? GameStarted;          // границы диапазона

    public event Action<int, int, string>? AttemptMade;  // номер попытки, вариант, ответ

    public event Action<int, int>? NumberGuessed;        // число, количество попыток

    public event Action? GameOver;

    public GuessGame(int secret, int min, int max)
    {
        this.secret = secret;
        this.min = min;
        this.max = max;
    }

    public void Play()
    {
        GameStarted?.Invoke(min, max);
        int low = min, high = max, attempts = 0;

        while (true)
        {
            int guess = (low + high) / 2;
            attempts++;
            if (guess == secret)
            {
                AttemptMade?.Invoke(attempts, guess, "угадал");
                NumberGuessed?.Invoke(guess, attempts);
                break;
            }
            if (guess < secret)
            {
                AttemptMade?.Invoke(attempts, guess, "больше");
                low = guess + 1;
            }
            else
            {
                AttemptMade?.Invoke(attempts, guess, "меньше");
                high = guess - 1;
            }
        }
        GameOver?.Invoke();
    }
}

// Подписчик 1: лог хода игры
internal class GameLogger
{
    public void OnStarted(int min, int max) => Console.WriteLine($"[Лог] Игра началась, число от {min} до {max}");

    public void OnAttempt(int n, int guess, string answer) => Console.WriteLine($"[Лог] Попытка {n}: {guess}? Игрок: {answer}");

    public void OnOver() => Console.WriteLine("[Лог] Игра окончена");
}

// Подписчик 2: подсказки
internal class Hinter
{
    public void OnAttempt(int n, int guess, string answer)
    {
        if (answer == "больше") Console.WriteLine($"    Подсказка: ищем в числах больше {guess}");
        if (answer == "меньше") Console.WriteLine($"    Подсказка: ищем в числах меньше {guess}");
    }
}

// Подписчик 3: статистика по всем играм
internal class Statistics
{
    private int games, totalAttempts;

    public void OnGuessed(int number, int attempts)
    {
        games++;
        totalAttempts += attempts;
        Console.WriteLine($"[Статистика] Число {number} угадано, попыток: {attempts}");
    }

    public void Print()
    {
        Console.WriteLine($"[Статистика] Игр: {games}, всего попыток: {totalAttempts}, в среднем: {(double)totalAttempts / games:F1}");
    }
}

internal class Program
{
    private static void Main()
    {
        GameLogger logger = new GameLogger();
        Hinter hinter = new Hinter();
        Statistics stats = new Statistics();

        int[] secrets = { 37, 100 };
        for (int i = 0; i < secrets.Length; i++)
        {
            GuessGame game = new GuessGame(secrets[i], 1, 100);
            game.GameStarted += logger.OnStarted;
            game.AttemptMade += logger.OnAttempt;
            game.GameOver += logger.OnOver;
            game.NumberGuessed += stats.OnGuessed;

            if (i == 0) game.AttemptMade += hinter.OnAttempt; // подсказки только в первой игре

            Console.WriteLine($"=== Игра {i + 1} ===");
            game.Play();
            Console.WriteLine();
        }
        stats.Print();
    }
}