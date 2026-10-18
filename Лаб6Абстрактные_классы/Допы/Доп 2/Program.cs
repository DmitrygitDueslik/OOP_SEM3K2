using System;

// Абстрактный класс персонажа
public abstract class Character
{
    public string Name { get; }
    public int Level { get; private set; }
    public int MaxHealth { get; private set; }
    public int Strength { get; protected set; }
    public int Agility { get; protected set; }
    public int Intelligence { get; protected set; }

    private int _health;
    private int _experience;
    private const int ExperienceToLevelUp = 100;

    // Здоровье инкапсулировано: не может быть меньше 0 и больше MaxHealth
    public int Health
    {
        get { return _health; }
        private set { _health = Math.Max(0, Math.Min(value, MaxHealth)); }
    }

    public bool IsAlive => Health > 0;

    protected Character(string name, int maxHealth, int strength, int agility, int intelligence)
    {
        Name = name;
        Level = 1;
        MaxHealth = maxHealth;
        Health = maxHealth;
        Strength = strength;
        Agility = agility;
        Intelligence = intelligence;
    }

    public abstract void Attack(Character target);

    public void TakeDamage(int amount)
    {
        if (!IsAlive) return;
        Health -= amount;
        Console.WriteLine($"{Name} получает {amount} урона (осталось {Health}/{MaxHealth} HP){(IsAlive ? "" : " - повержен!")}");
    }

    // Общая система опыта - при достижении порога происходит повышение уровня
    public void GainExperience(int amount)
    {
        _experience += amount;
        while (_experience >= ExperienceToLevelUp)
        {
            _experience -= ExperienceToLevelUp;
            LevelUp();
        }
    }

    protected virtual void LevelUp()
    {
        Level++;
        MaxHealth += 10;
        Health = MaxHealth; // полное восстановление при повышении уровня
        Console.WriteLine($"{Name} повышает уровень до {Level}! (HP теперь {MaxHealth})");
    }
}

public class Warrior : Character
{
    public int Rage { get; private set; } // ярость, копится при атаках

    public Warrior(string name) : base(name, maxHealth: 120, strength: 15, agility: 8, intelligence: 4) { }

    public override void Attack(Character target)
    {
        int damage = Strength + Rage / 2;
        Console.WriteLine($"{Name} рубит мечом (ярость: {Rage})");
        target.TakeDamage(damage);
        Rage = Math.Min(Rage + 10, 50);
    }

    protected override void LevelUp()
    {
        base.LevelUp();
        Strength += 3; // воин растёт в основном по силе
    }
}

public class Mage : Character
{
    public int Mana { get; private set; }
    public int MaxMana { get; private set; }

    public Mage(string name) : base(name, maxHealth: 80, strength: 4, agility: 6, intelligence: 18)
    {
        MaxMana = 60;
        Mana = MaxMana;
    }

    public override void Attack(Character target)
    {
        const int manaCost = 20;
        if (Mana < manaCost)
        {
            Console.WriteLine($"{Name}: недостаточно маны для заклинания, бьёт посохом");
            target.TakeDamage(Strength);
            return;
        }
        Mana -= manaCost;
        int damage = Intelligence * 2;
        Console.WriteLine($"{Name} произносит заклинание (мана: {Mana}/{MaxMana})");
        target.TakeDamage(damage);
    }

    protected override void LevelUp()
    {
        base.LevelUp();
        Intelligence += 4;
        MaxMana += 15;
        Mana = MaxMana;
    }
}

public class Archer : Character
{
    private int _attackCount;

    public Archer(string name) : base(name, maxHealth: 90, strength: 8, agility: 16, intelligence: 6) { }

    public override void Attack(Character target)
    {
        _attackCount++;
        // Каждый третий выстрел - критический
        bool isCritical = _attackCount % 3 == 0;
        int damage = isCritical ? Strength * 3 : Strength;
        Console.WriteLine($"{Name} стреляет из лука{(isCritical ? " - КРИТИЧЕСКИЙ УДАР!" : "")}");
        target.TakeDamage(damage);
    }

    protected override void LevelUp()
    {
        base.LevelUp();
        Agility += 3;
    }
}

class Program
{
    static void Main()
    {
        Warrior warrior = new Warrior("Грог");
        Mage mage = new Mage("Элара");
        Archer archer = new Archer("Лиара");

        warrior.Attack(mage);
        mage.Attack(warrior);
        archer.Attack(warrior);

        Console.WriteLine();
        warrior.GainExperience(250); // должен подняться на 2 уровня

        Console.WriteLine();
        // Наносим много урона, чтобы проверить, что здоровье не уходит в минус
        mage.TakeDamage(500);
        Console.WriteLine($"{mage.Name} жив: {mage.IsAlive}");
    }
}
