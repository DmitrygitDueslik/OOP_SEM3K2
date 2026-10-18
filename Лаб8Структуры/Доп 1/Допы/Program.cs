using System;

enum ShipStatus { Operational, Damaged, Critical }
enum CrewRole { Captain, Pilot, Engineer, Medic }
enum HealthStatus { Healthy, Injured, Critical }

struct CrewMember
{
    public string Name;
    public CrewRole Role;
    public int Skill;            // 1..10
    public HealthStatus Health;

    public CrewMember(string name, CrewRole role, int skill)
    {
        Name = name;
        Role = role;
        Skill = skill;
        Health = HealthStatus.Healthy;
    }
}

struct Spaceship
{
    public string Name;
    public double Speed;       // км/с
    public double Fuel;        // тонн
    public CrewMember[] Crew;
    public ShipStatus Status;

    public Spaceship(string name, double speed, double fuel, CrewMember[] crew)
    {
        Name = name;
        Speed = speed;
        Fuel = fuel;
        Crew = crew;
        Status = ShipStatus.Operational;
    }

    // Дальность (млн км): 1 т топлива = 0,5 млн км, у повреждённого корабля вдвое меньше
    public double GetRange()
    {
        double range = Fuel * 0.5;
        return Status == ShipStatus.Operational ? range : range / 2;
    }

    // Есть ли в экипаже здоровый (не Critical) член с нужной ролью
    bool HasRole(CrewRole role)
    {
        foreach (CrewMember m in Crew)
            if (m.Role == role && m.Health != HealthStatus.Critical) return true;
        return false;
    }

    public bool CanFly(double distance)
    {
        foreach (CrewRole role in Enum.GetValues<CrewRole>())
        {
            if (!HasRole(role))
            {
                Console.WriteLine($"  Нет члена экипажа: {role}");
                return false;
            }
        }
        if (Status == ShipStatus.Critical)
        {
            Console.WriteLine("  Системы в критическом состоянии");
            return false;
        }
        if (GetRange() < distance)
        {
            Console.WriteLine($"  Мало топлива: дальность {GetRange()} < {distance}");
            return false;
        }
        return true;
    }

    // Метеорит: сила 1..10
    public void MeteorHit(int power)
    {
        Console.WriteLine($"Метеорит силой {power}!");
        Status = power >= 7 ? ShipStatus.Critical : ShipStatus.Damaged;
        Fuel -= power * 10;
        for (int i = 0; i < Crew.Length; i++)
            if (Crew[i].Skill < power) Crew[i].Health = HealthStatus.Injured;
    }

    public void Repair()
    {
        if (HasRole(CrewRole.Engineer)) Status = ShipStatus.Operational;
        Console.WriteLine($"Ремонт: системы {Status}");
    }

    public void HealCrew()
    {
        if (!HasRole(CrewRole.Medic)) return;
        for (int i = 0; i < Crew.Length; i++) Crew[i].Health = HealthStatus.Healthy;
        Console.WriteLine("Лечение: весь экипаж здоров");
    }

    public void Print()
    {
        Console.WriteLine($"{Name}: топливо {Fuel} т, дальность {GetRange()} млн км, системы {Status}");
        foreach (CrewMember m in Crew)
            Console.WriteLine($"  {m.Name} ({m.Role}), навык {m.Skill}, {m.Health}");
    }
}

class Program
{
    static void Main()
    {
        CrewMember[] crew =
        {
            new CrewMember("Орлов", CrewRole.Captain, 9),
            new CrewMember("Белова", CrewRole.Pilot, 8),
            new CrewMember("Кузнецов", CrewRole.Engineer, 7),
            new CrewMember("Лебедева", CrewRole.Medic, 5)
        };
        Spaceship ship = new Spaceship("Арес-1", 30, 600, crew);
        double mars = 225; // млн км

        ship.Print();
        Console.WriteLine($"Можно лететь к Марсу: {ship.CanFly(mars)}\n");

        ship.MeteorHit(6);
        ship.Print();
        Console.WriteLine($"Можно лететь к Марсу: {ship.CanFly(mars)}\n");

        ship.Repair();
        ship.HealCrew();
        ship.Print();
        Console.WriteLine($"Можно лететь к Марсу: {ship.CanFly(mars)}");
    }
}