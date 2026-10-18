using System;

// Собственное исключение для банковских операций
internal class InsufficientFundsException : Exception
{
    // Конструктор по умолчанию
    public InsufficientFundsException() : base() { }

    // Конструктор с сообщением
    public InsufficientFundsException(string message) : base(message) { }

    // Конструктор с сообщением и внутренним исключением
    public InsufficientFundsException(string message, Exception inner) : base(message, inner) { }

    // Дополнительные свойства для хранения информации об ошибке
    public decimal Balance { get; set; }

    public decimal RequestedAmount { get; set; }
}

// Собственное исключение для некорректной суммы
internal class InvalidAmountException : Exception
{
    public InvalidAmountException() : base()
    {
    }

    public InvalidAmountException(string message) : base(message)
    {
    }

    public InvalidAmountException(string message, Exception inner) : base(message, inner)
    {
    }

    public decimal Amount { get; set; }
}

// Класс банковского счета
internal class BankAccount
{
    public string AccountNumber { get; private set; }
    public string OwnerName { get; set; }
    private decimal balance;

    public BankAccount(string accountNumber, string ownerName, decimal initialBalance)
    {
        AccountNumber = accountNumber;
        OwnerName = ownerName;
        balance = initialBalance;
    }

    // Свойство только для чтения
    public decimal Balance
    {
        get { return balance; }
    }

    // Метод пополнения счета
    public void Deposit(decimal amount)
    {
        // Проверяем корректность суммы
        if (amount <= 0)
        {
            // Создаем исключение и заполняем дополнительные данные
            InvalidAmountException ex = new InvalidAmountException(
                $"Сумма пополнения должна быть положительной. Получено: {amount}");
            ex.Amount = amount;
            throw ex; // Генерируем исключение
        }

        balance += amount;
        Console.WriteLine($"Счет {AccountNumber}: пополнение на {amount} руб. Новый баланс: {balance} руб.");
    }

    // Метод снятия со счета
    public void Withdraw(decimal amount)
    {
        // Проверяем корректность суммы
        if (amount <= 0)
        {
            InvalidAmountException ex = new InvalidAmountException(
                $"Сумма снятия должна быть положительной. Получено: {amount}");
            ex.Amount = amount;
            throw ex;
        }

        // Проверяем достаточно ли средств
        if (amount > balance)
        {
            InsufficientFundsException ex = new InsufficientFundsException(
                $"Недостаточно средств. Баланс: {balance} руб., запрошено: {amount} руб.");
            ex.Balance = balance;
            ex.RequestedAmount = amount;
            throw ex;
        }

        balance -= amount;
        Console.WriteLine($"Счет {AccountNumber}: снятие {amount} руб. Новый баланс: {balance} руб.");
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Счет {AccountNumber}: владелец {OwnerName}, баланс: {balance} руб.");
    }
}

internal class Program
{
    private static void Main()
    {
        Console.WriteLine("Собственные исключения\n");

        // Создаем счет
        BankAccount account = new BankAccount("ACC123", "Иван Петров", 1000);
        account.DisplayInfo();

        Console.WriteLine();

        // Демонстрация работы с исключениями
        try
        {
            Console.WriteLine("Попытка 1: Снятие 500 руб.");
            account.Withdraw(500); // Успешное снятие

            Console.WriteLine("\nПопытка 2: Снятие -100 руб. (отрицательная сумма)");
            account.Withdraw(-100); // Должно вызвать исключение
        }
        catch (InvalidAmountException ex)
        {
            Console.WriteLine($"Перехвачено исключение: {ex.Message}");
            Console.WriteLine($"Некорректная сумма: {ex.Amount}");
        }
        catch (InsufficientFundsException ex)
        {
            Console.WriteLine($"Перехвачено исключение: {ex.Message}");
            Console.WriteLine($"Баланс: {ex.Balance}, Запрошено: {ex.RequestedAmount}");
        }

        Console.WriteLine();

        try
        {
            Console.WriteLine("Попытка 3: Снятие 800 руб.");
            account.Withdraw(800); // Успешное снятие

            Console.WriteLine("\nПопытка 4: Снятие 2000 руб. (превышение баланса)");
            account.Withdraw(2000); // Должно вызвать исключение
        }
        catch (InvalidAmountException ex)
        {
            Console.WriteLine($"Перехвачено исключение: {ex.Message}");
        }
        catch (InsufficientFundsException ex)
        {
            Console.WriteLine($"Перехвачено исключение: {ex.Message}");
            Console.WriteLine($"Баланс: {ex.Balance}, Запрошено: {ex.RequestedAmount}");
        }

        Console.WriteLine();
        account.DisplayInfo(); // Показываем итоговый баланс
    }
}