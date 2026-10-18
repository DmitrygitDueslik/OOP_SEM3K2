using System;
using System.Collections.Generic;

//  Собственные исключения
internal class AccountNotFoundException : Exception
{
    public string AccountNumber { get; }

    public AccountNotFoundException(string accountNumber)
        : base($"Счёт {accountNumber} не найден")
    {
        AccountNumber = accountNumber;
    }
}

internal class InsufficientFundsException : Exception
{
    public decimal Balance { get; }
    public decimal Requested { get; }

    public InsufficientFundsException(decimal balance, decimal requested)
        : base($"Недостаточно средств: баланс {balance}, запрошено {requested}")
    {
        Balance = balance;
        Requested = requested;
    }
}

internal class InvalidAmountException : Exception
{
    public decimal Amount { get; }

    public InvalidAmountException(decimal amount)
        : base($"Некорректная сумма перевода: {amount}")
    {
        Amount = amount;
    }
}

//  Счёт
internal class Account
{
    public string Number { get; }
    public string Owner { get; }
    public decimal Balance { get; private set; }
    public bool IsBlocked { get; set; } // заблокированный счёт не принимает зачисления

    public Account(string number, string owner, decimal balance)
    {
        Number = number;
        Owner = owner;
        Balance = balance;
    }

    public void Withdraw(decimal amount)
    {
        if (amount > Balance) throw new InsufficientFundsException(Balance, amount);
        Balance -= amount;
    }

    public void Deposit(decimal amount)
    {
        if (IsBlocked) throw new InvalidOperationException($"Счёт {Number} заблокирован для зачислений");
        Balance += amount;
    }

    // Используется только при откате: возврат средств без проверок
    public void Refund(decimal amount) => Balance += amount;
}

//  Банк
internal class Bank
{
    private Dictionary<string, Account> accounts = new Dictionary<string, Account>();

    public void Open(Account account) => accounts[account.Number] = account;

    public Account Find(string number)
    {
        if (!accounts.TryGetValue(number, out Account? account))
        {
            throw new AccountNotFoundException(number);
        }
        return account;
    }

    // Перевод как транзакция: либо выполнен полностью, либо откатан
    public void Transfer(string fromNumber, string toNumber, decimal amount)
    {
        if (amount <= 0) throw new InvalidAmountException(amount);

        Account from = Find(fromNumber);
        from.Withdraw(amount);              // шаг 1: списание
        Console.WriteLine($"  списано {amount} со счёта {fromNumber}");

        try
        {
            Account to = Find(toNumber);    // шаг 2: зачисление
            to.Deposit(amount);
            Console.WriteLine($"  зачислено {amount} на счёт {toNumber}");
        }
        catch (Exception)
        {
            from.Refund(amount);            // откат шага 1
            Console.WriteLine($"  ОТКАТ: {amount} возвращено на счёт {fromNumber}");
            throw;                          // пробрасываем то же исключение дальше, сохраняя стек
        }
    }

    public void PrintBalances()
    {
        foreach (Account a in accounts.Values)
        {
            Console.WriteLine($"  {a.Number} {a.Owner,-8} {a.Balance,8} руб.{(a.IsBlocked ? " (заблокирован)" : "")}");
        }
    }
}

internal class Program
{
    private static void TryTransfer(Bank bank, string from, string to, decimal amount)
    {
        Console.WriteLine($"\nПеревод {amount} руб.: {from} -> {to}");
        try
        {
            bank.Transfer(from, to, amount);
            Console.WriteLine("  Результат: успешно");
        }
        catch (InvalidAmountException ex)
        {
            Console.WriteLine($"  Результат: ошибка суммы - {ex.Message}");
        }
        catch (AccountNotFoundException ex)
        {
            Console.WriteLine($"  Результат: ошибка счёта - {ex.Message}");
        }
        catch (InsufficientFundsException ex)
        {
            Console.WriteLine($"  Результат: {ex.Message} (не хватает {ex.Requested - ex.Balance})");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"  Результат: операция невозможна - {ex.Message}");
        }
    }

    private static void Main()
    {
        Bank bank = new Bank();
        bank.Open(new Account("A-1", "Иванов", 5000));
        bank.Open(new Account("A-2", "Петрова", 1200));
        bank.Open(new Account("A-3", "Сидоров", 300) { IsBlocked = true });

        Console.WriteLine("Начальные балансы:");
        bank.PrintBalances();

        TryTransfer(bank, "A-1", "A-2", 1500);   // успешно
        TryTransfer(bank, "A-2", "A-1", -200);   // некорректная сумма
        TryTransfer(bank, "A-9", "A-1", 100);    // нет счёта отправителя
        TryTransfer(bank, "A-2", "A-1", 9000);   // недостаточно средств
        TryTransfer(bank, "A-1", "A-7", 700);    // нет счёта получателя -> откат
        TryTransfer(bank, "A-2", "A-3", 400);    // получатель заблокирован -> откат

        Console.WriteLine("\nИтоговые балансы:");
        bank.PrintBalances();
    }
}