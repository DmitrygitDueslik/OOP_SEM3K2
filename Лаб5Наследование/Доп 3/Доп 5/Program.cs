using System;

// Базовый класс
class BankAccount
{
    public string AccountNumber { get; }
    public string OwnerName { get; set; }
    public decimal Balance { get; protected set; }

    public BankAccount(string accountNumber, string ownerName, decimal balance)
    {
        AccountNumber = accountNumber;
        OwnerName = ownerName;
        Balance = balance;
    }

    public virtual void Deposit(decimal amount)
    {
        Balance += amount;
        Console.WriteLine($"{OwnerName}: зачислено {amount} руб., баланс = {Balance} руб.");
    }

    public virtual void Withdraw(decimal amount)
    {
        if (amount > Balance)
        {
            Console.WriteLine($"{OwnerName}: недостаточно средств для снятия {amount} руб. (баланс {Balance} руб.)");
            return;
        }
        Balance -= amount;
        Console.WriteLine($"{OwnerName}: снято {amount} руб., баланс = {Balance} руб.");
    }

    // Статический метод перевода между счетами любых типов
    public static void Transfer(BankAccount from, BankAccount to, decimal amount)
    {
        Console.WriteLine($"Перевод {amount} руб.: {from.OwnerName} -> {to.OwnerName}");
        from.Withdraw(amount);
        to.Deposit(amount);
    }
}

// Сберегательный счёт - запрет ухода в минус
class SavingsAccount : BankAccount
{
    public SavingsAccount(string accountNumber, string ownerName, decimal balance)
        : base(accountNumber, ownerName, balance) { }

    public override void Withdraw(decimal amount)
    {
        if (amount > Balance)
        {
            Console.WriteLine($"{OwnerName}: операция отклонена - сберегательный счёт не может уйти в минус");
            return;
        }
        base.Withdraw(amount);
    }
}

// Расчётный счёт - овердрафт с комиссией
class CheckingAccount : BankAccount
{
    private const decimal OverdraftFee = 50m;

    public CheckingAccount(string accountNumber, string ownerName, decimal balance)
        : base(accountNumber, ownerName, balance) { }

    public override void Withdraw(decimal amount)
    {
        Balance -= amount;
        if (Balance < 0)
        {
            Balance -= OverdraftFee;
            Console.WriteLine($"{OwnerName}: снято {amount} руб., счёт ушёл в минус - списана комиссия {OverdraftFee} руб., баланс = {Balance} руб.");
        }
        else
        {
            Console.WriteLine($"{OwnerName}: снято {amount} руб., баланс = {Balance} руб.");
        }
    }
}

// Зарплатный счёт - пополнение только 5-го и 20-го числа
class SalaryAccount : BankAccount
{
    public int CurrentDay { get; set; } // текущий день месяца (для демонстрации проверки)

    public SalaryAccount(string accountNumber, string ownerName, decimal balance, int currentDay)
        : base(accountNumber, ownerName, balance)
    {
        CurrentDay = currentDay;
    }

    public override void Deposit(decimal amount)
    {
        if (CurrentDay != 5 && CurrentDay != 20)
        {
            Console.WriteLine($"{OwnerName}: пополнение зарплатного счёта возможно только 5-го и 20-го числа (сейчас {CurrentDay}-е)");
            return;
        }
        base.Deposit(amount);
    }
    // Withdraw - без ограничений, используется базовая реализация
}

class Program
{
    static void Main()
    {
        SavingsAccount savings = new SavingsAccount("SA-001", "Ирина", 5000m);
        CheckingAccount checking = new CheckingAccount("CA-002", "Олег", 2000m);
        SalaryAccount salary = new SalaryAccount("ZA-003", "Мария", 0m, 20);

        Console.WriteLine("Обычные операции:");
        savings.Withdraw(7000m);   // отклонено - нельзя в минус
        checking.Withdraw(2500m);  // уходит в минус, комиссия
        salary.Deposit(45000m);    // 20-е число - успешно

        Console.WriteLine("\nПопытка пополнить зарплатный счёт не в дату:");
        salary.CurrentDay = 12;
        salary.Deposit(1000m);     // отклонено

        Console.WriteLine("\nПеревод между счетами:");
        BankAccount.Transfer(salary, savings, 10000m);
    }
}
