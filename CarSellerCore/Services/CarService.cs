using System.Reflection;
using System.Text.Json;
using CarSellerCore.Model;
using CarSellerCore.Services.Abstraction;

namespace CarSellerCore.Services;

public class CarService : ICarService
{
    private List<Car> _cars { get; set; } = new List<Car>();
    
    public FilterModel FilterSelection { get; set; }

    public async Task<List<Car>> GetCarsAsync()
    {
        try
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(Car)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("CarSellerCore.Resource.dataset.json");
            string jsonString = "";
            using (var reader = new System.IO.StreamReader(stream))
            {
                jsonString = reader.ReadToEnd();
            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            _cars = JsonSerializer.Deserialize<List<Car>>(jsonString, options);
            for (var index = 0; index < _cars.Count; index++)
            {
                var car = _cars[index];
                car.Id = index + 1;
            }

            return _cars;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading JSON file: {ex.Message}");
            return new List<Car>();
        }
    }

    public void UpdateCar(Car car)
    {
        var carToUpdate = _cars.FirstOrDefault(c => c.Id == car.Id);
        carToUpdate = car;
        
        var index = _cars.FindIndex(c => c.Id == car.Id);
        if (index != -1)
        {
            _cars[index] = carToUpdate;
        }
    }

    public async Task<Car> GetCar(int id)
    {
        var car = _cars.FirstOrDefault(car => car.Id == id);
        return car ?? new Car();
    }
}