using System;
using System.Collections.Generic;
using System.Linq;

public abstract class Carriage
{
    public int Id { get; set; }
    public string Type { get; set; }
    public double Weight { get; set; }
    public double Length { get; set; }


    public string Manufacturer { get; set; }
    public DateTime ManufactureDate { get; set; }
    public string Model { get; set; }
    public bool IsOperational { get; set; }
    public string Color { get; set; }
    public int MaxSpeed { get; set; }
    public string Owner { get; set; }
    public DateTime LastMaintenanceDate { get; set; }
    public bool IsReserved { get; set; }
    public string Notes { get; set; }

    public Carriage(int id, string type, double weight, double length)
    {
        Id = id;
        Type = type;
        Weight = weight;
        Length = length;
    }

    public abstract void ShowInfo();
}

public class PassengerCarriage : Carriage
{
    public int SeatsCount { get; set; }
    public string ComfortLevel { get; set; }

    public PassengerCarriage(int id, double weight, double length, int seatsCount, string comfortLevel)
        : base(id, "Passenger", weight, length)
    {
        SeatsCount = seatsCount;
        ComfortLevel = comfortLevel;
    }

    public override void ShowInfo()
    {
        Console.WriteLine($"Passenger Carriage ID: {Id}, Weight: {Weight}, Length: {Length}, Seats: {SeatsCount}, Comfort Level: {ComfortLevel}");
    }
}

public class FreightCarriage : Carriage
{
    public double MaxLoadCapacity { get; set; }
    public string CargoType { get; set; }
    public bool IsHazardous { get; set; }

    public FreightCarriage(int id, double weight, double length, double maxLoadCapacity, string cargoType, bool isHazardous)
        : base(id, "Freight", weight, length)
    {
        MaxLoadCapacity = maxLoadCapacity;
        CargoType = cargoType;
        IsHazardous = isHazardous;
    }

    public override void ShowInfo()
    {
        Console.WriteLine($"Freight Carriage ID: {Id}, Weight: {Weight}, Length: {Length}, Max Load Capacity: {MaxLoadCapacity}, Cargo Type: {CargoType}, Is Hazardous: {(IsHazardous ? "Yes" : "No")}");
    }
}


public class DiningCarriage : Carriage
{
    public int TablesCount { get; set; }
    public bool HasKitchen { get; set; }

    public DiningCarriage(int id, double weight, double length, int tablesCount, bool hasKitchen)
        : base(id, "Dining", weight, length)
    {
        TablesCount = tablesCount;
        HasKitchen = hasKitchen;
    }

    public override void ShowInfo()
    {
        Console.WriteLine($"Dining Carriage ID: {Id}, Weight: {Weight}, Length: {Length}, Tables: {TablesCount}, Has Kitchen: {HasKitchen}");
    }
}

public class SleepingCarriage : Carriage
{
    public int CompartmentsCount { get; set; }
    public bool HasShowers { get; set; }

    public SleepingCarriage(int id, double weight, double length, int compartmentsCount, bool hasShowers)
        : base(id, "Sleeping", weight, length)
    {
        CompartmentsCount = compartmentsCount;
        HasShowers = hasShowers;
    }

    public override void ShowInfo()
    {
        Console.WriteLine($"Sleeping Carriage ID: {Id}, Weight: {Weight}, Length: {Length}, Compartments: {CompartmentsCount}, Has Showers: {HasShowers}");
    }
}


public class Train
{
    public LinkedList<Carriage> carriages = new LinkedList<Carriage>();
    public int ID { get; set; }
    public string Manufacturer { get; set; }
    public DateTime ManufactureDate { get; set; }
    public string Model { get; set; }
    public bool IsOperational { get; set; }
    public string Color { get; set; }
    public int MaxSpeed { get; set; }
    public string Owner { get; set; }
    public DateTime LastMaintenanceDate { get; set; }
    public bool IsReserved { get; set; }
    public string Notes { get; set; }

    public string Name { get; set; }
    public string RouteNumber { get; set; }

    public Train(string name, string routeNumber)
    {
        Name = name;
        RouteNumber = routeNumber;
    }

    private bool HasPassengerCarriages()
    {
        return carriages.Any(c => c is PassengerCarriage || c is SleepingCarriage || c is DiningCarriage);
    }

    private bool HasFreightCarriages()
    {
        return carriages.Any(c => c is FreightCarriage);
    }

    public void AddCarriageAtEnd(Carriage carriage)
    {
        if (CanAddCarriage(carriage))
        {
            if (!carriages.Any(c => c.Id == carriage.Id))
            {
                carriages.AddLast(carriage);
            }
            else
            {
                Console.WriteLine($"Carriage with ID {carriage.Id} already exists.");
            }
        }
        else
        {
            Console.WriteLine($"Cannot add {carriage.Type} carriage to this train.");
        }
    }
 
    public void AddCarriageAtPosition(Carriage carriage, int position)
    {
        if (CanAddCarriage(carriage))
        {
            var node = carriages.First;
            for (int i = 0; i < position && node != null; i++)
            {
                node = node.Next;
            }

            if (node != null)
            {
                carriages.AddBefore(node, carriage);
            }
            else
            {
                carriages.AddLast(carriage);
            }
        }
        else
        {
            Console.WriteLine($"Cannot add {carriage.Type} carriage to this train.");
        }
    }

    private bool CanAddCarriage(Carriage carriage)
    {
        if (carriage is FreightCarriage && HasPassengerCarriages())
        {
            return false;
        }

        if ((carriage is PassengerCarriage || carriage is SleepingCarriage || carriage is DiningCarriage) && HasFreightCarriages())
        {
            return false;
        }

        return true;
    }

    public void RemoveCarriageFromEnd()
    {
        if (carriages.Count == 0)
        {
            Console.WriteLine("No carriages to remove.");
            return;
        }

        carriages.RemoveLast();
    }

    public void RemoveCarriageFromPosition(int position)
    {
        if (position < 0 || position >= carriages.Count)
        {
            Console.WriteLine("Invalid position.");
            return;
        }

        var node = carriages.First;
        for (int i = 0; i < position; i++)
        {
            node = node.Next;
        }

        carriages.Remove(node);
    }

    public Carriage FindCarriageById(int id)
    {
        return carriages.FirstOrDefault(c => c.Id == id);
    }

    public IEnumerable<Carriage> FilterCarriagesByType(string type)
    {
        return carriages.Where(c => c.Type == type);
    }

    public IEnumerable<Carriage> FilterCarriagesByWeight(double weight)
    {
        return carriages.Where(c => c.Weight == weight);
    }

    public IEnumerable<Carriage> FilterCarriagesByCapacity(int capacity)
    {
        return carriages.OfType<PassengerCarriage>().Where(pc => pc.SeatsCount == capacity);
    }

    public double CalculateTotalWeight()
    {
        return carriages.Sum(c => c.Weight);
    }

    public void ShowAllCarriages()
    {
        foreach (var carriage in carriages)
        {
            carriage.ShowInfo();
        }
    }

    public int CountTotalPassengers()
    {
        return carriages.OfType<PassengerCarriage>().Sum(pc => pc.SeatsCount);
    }

    public FreightCarriage FindCarriageWithMaxLoadCapacity()
    {
        return carriages.OfType<FreightCarriage>().OrderByDescending(fc => fc.MaxLoadCapacity).FirstOrDefault();
    }

    public Dictionary<string, int> CountCarriagesByType()
    {
        return carriages.GroupBy(c => c.Type).ToDictionary(g => g.Key, g => g.Count());
    }
}


public class ConsoleCommands
{
    private List<Train> trains = new List<Train>();

    public void ShowCommandDescriptions()
    {
        Console.WriteLine("Доступні команди:");
        Console.WriteLine("addtrain <ID> <Manufacturer> <ManufactureDate> <Model> <IsOperational> <Color> <MaxSpeed> <Owner> <LastMaintenanceDate> <IsReserved> <Notes> - Додати новий потяг");
        Console.WriteLine("selecttrain <ID> - Вибрати потяг для роботи");
        Console.WriteLine("addcarriage_end <type> <weight> <length> <seats_count> <comfort_level> - Додати вагон в кінець потяга");
        Console.WriteLine("addcarriage_pos <id> <type> <weight> <length> <seats_count> <comfort_level> - Додати вагон на вказану позицію");
        Console.WriteLine("removecarriage -e - Видалити вагон з кінця потяга");
        Console.WriteLine("removecarriage -p <position> - Видалити вагон з вказаної позиції");
        Console.WriteLine("totalweight - Розрахувати загальну вагу потяга");
        Console.WriteLine("info - Показати інформацію про потяг та його вагони");
        Console.WriteLine("filtercarriage <criteria> <value> - Фільтрувати вагони за критеріями (тип, вага, місткість)");
        Console.WriteLine("aggregate - Показати агреговану інформацію (загальна кількість пасажирів, максимальна вантажопідйомність)");
    }

    public void ExecuteCommand(string command)
    {
        var parts = command.Split(' ');
        var commandName = parts[0];

        switch (commandName)
        {
            case "addtrain":
                AddTrain(parts.Skip(1).ToArray());
                break;
            case "selecttrain":
                SelectTrain(int.Parse(parts[1]));
                break;
            case "addcarriage_end":
                AddCarriageToEnd(parts.Skip(1).ToArray());
                break;
            case "addcarriage_pos":
                AddCarriageWithID(parts.Skip(1).ToArray());
                break;
            case "removecarriage":
                RemoveCarriage(parts.Skip(1).ToArray());
                break;
            case "totalweight":
                CalculateTotalWeight();
                break;
            case "info":
                ShowTrainInfo();
                break;
            case "filtercarriage":
                FilterCarriage(parts.Skip(1).ToArray());
                break;
            case "aggregate":
                ShowAggregateInfo();
                break;
            default:
                Console.WriteLine("Невідома команда.");
                break;
        }
    }

    private Train selectedTrain;

    private void AddTrain(string[] parameters)
    {
        int id = int.Parse(parameters[0]);
        string manufacturer = parameters[1];
        DateTime manufactureDate = DateTime.Parse(parameters[2]);
        string model = parameters[3];
        bool isOperational = bool.Parse(parameters[4]);
        string color = parameters[5];
        int maxSpeed = int.Parse(parameters[6]);
        string owner = parameters[7];
        DateTime lastMaintenanceDate = DateTime.Parse(parameters[8]);
        bool isReserved = bool.Parse(parameters[9]);
        string notes = parameters[10];

        var newTrain = new Train("Train " + id, "Route " + id)
        {
            ID = id,
            Manufacturer = manufacturer,
            ManufactureDate = manufactureDate,
            Model = model,
            IsOperational = isOperational,
            Color = color,
            MaxSpeed = maxSpeed,
            Owner = owner,
            LastMaintenanceDate = lastMaintenanceDate,
            IsReserved = isReserved,
            Notes = notes
        };

        trains.Add(newTrain);
        Console.WriteLine($"Train {id} додано.");
    }

    private void SelectTrain(int id)
    {
        selectedTrain = trains.FirstOrDefault(t => t.ID == id);
        if (selectedTrain == null)
        {
            Console.WriteLine($"Train з ID {id} не знайдено.");
        }
        else
        {
            Console.WriteLine($"Вибрано train з ID {id}.");
        }
    }

    private void AddCarriageToEnd(string[] parameters)
    {
        if (selectedTrain == null)
        {
            Console.WriteLine("Немає вибраного потяга.");
            return;
        }

        var type = parameters[0];
        var id = selectedTrain.carriages.Count + 1;
        var weight = double.Parse(parameters[1]);
        var length = double.Parse(parameters[2]);

        Carriage carriage = null;

        switch (type)
        {
            case "Passenger":
                var seatsCount = int.Parse(parameters[3]);
                var comfortLevel = parameters[4];
                carriage = new PassengerCarriage(id, weight, length, seatsCount, comfortLevel);
                break;
            case "Freight":
                var maxLoadCapacity = double.Parse(parameters[3]);
                var cargoType = parameters[4];
                var isHazardous = bool.Parse(parameters[5]);
                carriage = new FreightCarriage(id, weight, length, maxLoadCapacity, cargoType, isHazardous);
                break;
            case "Dining":
                var tablesCount = int.Parse(parameters[3]);
                var hasKitchen = bool.Parse(parameters[4]);
                carriage = new DiningCarriage(id, weight, length, tablesCount, hasKitchen);
                break;
            case "Sleeping":
                var compartmentsCount = int.Parse(parameters[3]);
                var hasShowers = bool.Parse(parameters[4]);
                carriage = new SleepingCarriage(id, weight, length, compartmentsCount, hasShowers);
                break;
            default:
                Console.WriteLine("Невідомий тип вагону.");
                return;
        }

        selectedTrain.AddCarriageAtEnd(carriage);

        Console.WriteLine($"Вагон {type} з ID {id} додано.");
    }

    private void AddCarriageWithID(string[] parameters)
    {
        if (selectedTrain == null)
        {
            Console.WriteLine("Немає вибраного потяга.");
            return;
        }

        var id = int.Parse(parameters[0]);
        var type = parameters[1];
        var weight = double.Parse(parameters[2]);
        var length = double.Parse(parameters[3]);

        Carriage carriage = null;

        switch (type)
        {
            case "Passenger":
                var seatsCount = int.Parse(parameters[4]);
                var comfortLevel = parameters[5];
                carriage = new PassengerCarriage(id, weight, length, seatsCount, comfortLevel);
                break;
            case "Freight":
                var maxLoadCapacity = double.Parse(parameters[4]);
                var cargoType = parameters[5];
                var isHazardous = bool.Parse(parameters[6]);
                carriage = new FreightCarriage(id, weight, length, maxLoadCapacity, cargoType, isHazardous);
                break;
            case "Dining":
                var tablesCount = int.Parse(parameters[4]);
                var hasKitchen = bool.Parse(parameters[5]);
                carriage = new DiningCarriage(id, weight, length, tablesCount, hasKitchen);
                break;
            case "Sleeping":
                var compartmentsCount = int.Parse(parameters[4]);
                var hasShowers = bool.Parse(parameters[5]);
                carriage = new SleepingCarriage(id, weight, length, compartmentsCount, hasShowers);
                break;
            default:
                Console.WriteLine("Невідомий тип вагону.");
                return;
        }

        selectedTrain.AddCarriageAtEnd(carriage);

        Console.WriteLine($"Вагон {type} з ID {id} додано.");
    }




    private void RemoveCarriage(string[] parameters)
    {
        if (selectedTrain == null)
        {
            Console.WriteLine("Немає вибраного потяга.");
            return;
        }

        var positionType = parameters[0];

        if (positionType == "-e")
        {
            selectedTrain.RemoveCarriageFromEnd();
        }
        else if (positionType == "-p")
        {
            var position = int.Parse(parameters[1]);
            selectedTrain.RemoveCarriageFromPosition(position);
        }

        Console.WriteLine("Вагон видалено.");
    }

    private void CalculateTotalWeight()
    {
        if (selectedTrain == null)
        {
            Console.WriteLine("Немає вибраного потяга.");
            return;
        }

        var totalWeight = selectedTrain.CalculateTotalWeight();
        Console.WriteLine($"Загальна вага потяга: {totalWeight}.");
    }

    private void ShowTrainInfo()
    {
        if (selectedTrain == null)
        {
            Console.WriteLine("Немає вибраного потяга.");
            return;
        }

        Console.WriteLine($"Train ID: {selectedTrain.ID}, Name: {selectedTrain.Name}, Route Number: {selectedTrain.RouteNumber}, Manufacturer: {selectedTrain.Manufacturer}, Manufacture Date: {selectedTrain.ManufactureDate}, Model: {selectedTrain.Model}, Operational: {selectedTrain.IsOperational}, Color: {selectedTrain.Color}, Max Speed: {selectedTrain.MaxSpeed}, Owner: {selectedTrain.Owner}, Last Date: {selectedTrain.LastMaintenanceDate}, Reserved: {selectedTrain.IsReserved}, Notes: {selectedTrain.Notes}");
        Console.WriteLine("Вагони:");
        selectedTrain.ShowAllCarriages();
    }

    private void FilterCarriage(string[] parameters)
    {
        if (selectedTrain == null)
        {
            Console.WriteLine("Немає вибраного потяга.");
            return;
        }

        var criteria = parameters[0];
        var value = parameters[1];

        switch (criteria)
        {
            case "type":
                var carriagesByType = selectedTrain.FilterCarriagesByType(value);
                foreach (var carriage in carriagesByType)
                {
                    carriage.ShowInfo();
                }
                break;
            case "weight":
                var weight = double.Parse(value);
                var carriagesByWeight = selectedTrain.FilterCarriagesByWeight(weight);
                foreach (var carriage in carriagesByWeight)
                {
                    carriage.ShowInfo();
                }
                break;
            case "capacity":
                var capacity = int.Parse(value);
                var carriagesByCapacity = selectedTrain.FilterCarriagesByCapacity(capacity);
                foreach (var carriage in carriagesByCapacity)
                {
                    carriage.ShowInfo();
                }
                break;
            default:
                Console.WriteLine("Невідомий критерій.");
                break;
        }
    }

    private void ShowAggregateInfo()
    {
        if (selectedTrain == null)
        {
            Console.WriteLine("Немає вибраного потяга.");
            return;
        }

        var totalPassengers = selectedTrain.CountTotalPassengers();
        var maxLoadCapacity = selectedTrain.FindCarriageWithMaxLoadCapacity()?.MaxLoadCapacity ?? 0;

        Console.WriteLine($"Загальна кількість пасажирів: {totalPassengers}");
        Console.WriteLine($"Максимальна вантажопідйомність: {maxLoadCapacity}");
    }
}


public class Program
{
    public static void Main(string[] args)
    {
        var consoleCommands = new ConsoleCommands();
        consoleCommands.ShowCommandDescriptions();

        string command;
        while ((command = Console.ReadLine()) != "exit")
        {
            consoleCommands.ExecuteCommand(command);
        }
    }
}
