using System;

enum DeviceType { Heater, AirConditioner, Light, SmokeSensor, LeakSensor }

struct Device
{
    public string Name;
    public DeviceType Type;
    public bool IsOn;
    public int Power;    // Вт
    public int Value;    // показание датчика

    public Device(string name, DeviceType type, int power)
    {
        Name = name;
        Type = type;
        Power = power;
        IsOn = false;
        Value = 0;
    }
}

struct Room
{
    public string Name;
    public double Temperature;
    public int Humidity;
    public Device[] Devices;

    public Room(string name, double temperature, int humidity, Device[] devices)
    {
        Name = name;
        Temperature = temperature;
        Humidity = humidity;
        Devices = devices;
    }
}

struct Scenario
{
    public string Name;
    public int FromHour, ToHour;  // когда активен
    public bool NeedPeople;       // true - только когда жильцы дома
    public DeviceType Target;
    public bool TurnOn;
    public int Priority;

    public Scenario(string name, int from, int to, bool needPeople, DeviceType target, bool turnOn, int priority)
    {
        Name = name; FromHour = from; ToHour = to; NeedPeople = needPeople;
        Target = target; TurnOn = turnOn; Priority = priority;
    }
}

struct SmartHome
{
    public Room[] Rooms;
    public Scenario[] Scenarios;

    public SmartHome(Room[] rooms, Scenario[] scenarios)
    {
        Rooms = rooms;
        Scenarios = scenarios;
    }

    // Включить/выключить все устройства типа type во всех комнатах
    void SetAll(DeviceType type, bool on)
    {
        for (int r = 0; r < Rooms.Length; r++)
            for (int d = 0; d < Rooms[r].Devices.Length; d++)
                if (Rooms[r].Devices[d].Type == type) Rooms[r].Devices[d].IsOn = on;
    }

    // Климат: < 20 - обогрев, > 25 - кондиционер
    public void ControlClimate()
    {
        for (int r = 0; r < Rooms.Length; r++)
        {
            for (int d = 0; d < Rooms[r].Devices.Length; d++)
            {
                if (Rooms[r].Devices[d].Type == DeviceType.Heater)
                    Rooms[r].Devices[d].IsOn = Rooms[r].Temperature < 20;
                if (Rooms[r].Devices[d].Type == DeviceType.AirConditioner)
                    Rooms[r].Devices[d].IsOn = Rooms[r].Temperature > 25;
            }
            Console.WriteLine($"Климат: {Rooms[r].Name} {Rooms[r].Temperature} °C, влажность {Rooms[r].Humidity}%");
        }
    }

    // Сценарии: при конфликте побеждает больший приоритет
    public void RunScenarios(int hour, bool peopleHome)
    {
        foreach (DeviceType type in Enum.GetValues<DeviceType>())
        {
            int best = -1, count = 0;
            for (int i = 0; i < Scenarios.Length; i++)
            {
                Scenario s = Scenarios[i];
                bool active = s.Target == type && hour >= s.FromHour && hour < s.ToHour
                              && (!s.NeedPeople || peopleHome);
                if (active) count++;
                if (active && (best == -1 || s.Priority > Scenarios[best].Priority))
                    best = i;
            }
            if (best != -1)
            {
                SetAll(type, Scenarios[best].TurnOn);
                if (count > 1)
                    Console.WriteLine($"Конфликт {count} сценариев для {type} - выбран приоритет {Scenarios[best].Priority}");
                Console.WriteLine($"Сценарий \"{Scenarios[best].Name}\": {type} -> {(Scenarios[best].TurnOn ? "ON" : "OFF")}");
            }
        }
    }

    // Пожар: дым > 50; протечка: датчик > 0
    public void CheckAlarms()
    {
        foreach (Room room in Rooms)
            foreach (Device d in room.Devices)
            {
                if (d.Type == DeviceType.SmokeSensor && d.Value > 50)
                    Console.WriteLine($"ТРЕВОГА: пожар в комнате {room.Name}!");
                if (d.Type == DeviceType.LeakSensor && d.Value > 0)
                    Console.WriteLine($"ТРЕВОГА: протечка в комнате {room.Name}!");
            }
    }

    // Экономия: никого нет - выключаем свет
    public void SaveEnergy(bool peopleHome)
    {
        if (!peopleHome)
        {
            SetAll(DeviceType.Light, false);
            Console.WriteLine("Экономия: жильцов нет, свет выключен");
        }
    }

    public void PrintStatistics()
    {
        int total = 0;
        foreach (Room room in Rooms)
            foreach (Device d in room.Devices)
                if (d.IsOn)
                {
                    Console.WriteLine($"  включено: {room.Name} - {d.Name} ({d.Power} Вт)");
                    total += d.Power;
                }
        Console.WriteLine($"  Общее потребление: {total} Вт");
    }
}

class Program
{
    static void Main()
    {
        Room[] rooms =
        {
            new Room("Гостиная", 18, 40, new[]
            {
                new Device("Обогреватель", DeviceType.Heater, 1500),
                new Device("Люстра", DeviceType.Light, 100),
                new Device("Датчик дыма", DeviceType.SmokeSensor, 0)
            }),
            new Room("Кухня", 27, 55, new[]
            {
                new Device("Кондиционер", DeviceType.AirConditioner, 900),
                new Device("Свет", DeviceType.Light, 60),
                new Device("Датчик протечки", DeviceType.LeakSensor, 0)
            })
        };
        Scenario[] scenarios =
        {
            new Scenario("Вечерний свет", 18, 23, true, DeviceType.Light, true, 1),
            new Scenario("Кинорежим", 20, 22, true, DeviceType.Light, false, 2)
        };
        SmartHome home = new SmartHome(rooms, scenarios);

        Console.WriteLine("=== 19:00, жильцы дома ===");
        home.ControlClimate();
        home.RunScenarios(19, true);
        home.PrintStatistics();

        Console.WriteLine("\n=== 21:00, конфликт сценариев ===");
        home.RunScenarios(21, true);
        home.PrintStatistics();

        Console.WriteLine("\n=== 22:00, жильцы ушли ===");
        home.Rooms[1].Devices[1].IsOn = true;   // забыли свет на кухне
        home.SaveEnergy(false);
        home.Rooms[0].Devices[2].Value = 80;    // дым в гостиной
        home.Rooms[1].Devices[2].Value = 1;     // протечка на кухне
        home.CheckAlarms();
        home.PrintStatistics();
    }
}