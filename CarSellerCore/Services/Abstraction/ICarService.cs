using CarSellerCore.Model;

namespace CarSellerCore.Services.Abstraction;

public interface ICarService
{
    FilterModel FilterSelection { get; set; }
    
    Task<List<Car>> GetCarsAsync();
    
    void UpdateCar(Car car);

    Task<Car> GetCar(int id);
}