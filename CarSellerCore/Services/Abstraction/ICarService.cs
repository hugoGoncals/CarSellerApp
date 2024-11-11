using CarSellerCore.Model;

namespace CarSellerCore.Services.Abstraction;

public interface ICarService
{
    FilterModel FilterSelection { get; set; }
    
    Task<List<Car>> GetCarsAsync();
    
    Task UpdateCar(Car car);

    Car GetCar(int id);
}