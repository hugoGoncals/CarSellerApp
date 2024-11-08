using System.Reflection;
using System.Text.Json;
using CarSellerCore.Model;
using CarSellerCore.Services.Abstraction;

namespace CarSellerCore.Services;

public class CarService : ICarService
{
    public async Task<List<Car>> GetCarsAsync()
    {
        try
        {
            // Read JSON data
            
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

            List<Car> cars = JsonSerializer.Deserialize<List<Car>>(jsonString, options);
            for (var index = 0; index < cars.Count; index++)
            {
                var car = cars[index];
                car.Id = index + 1;
            }

            return cars;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading JSON file: {ex.Message}");
            return new List<Car>();
        }
    }
}