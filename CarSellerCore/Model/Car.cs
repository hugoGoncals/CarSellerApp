namespace CarSellerCore.Model;
public class Car
{
    public int Id { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public string EngineSize { get; set; }
    public string Fuel { get; set; }
    public int Year { get; set; }
    public int Mileage { get; set; }
    public string AuctionDateTime { get; set; }
    public int StartingBid { get; set; }
    public bool Favourite { get; set; }
    public Details Details { get; set; }
    public byte[] SelectedPhoto { get; set; }
}

public class Details
{
    public Specification Specification { get; set; }
    public Ownership Ownership { get; set; }
    public List<string> Equipment { get; set; }
}

public class Specification
{
    public string VehicleType { get; set; }
    public string Colour { get; set; }
    public string Fuel { get; set; }
    public string Transmission { get; set; }
    public int NumberOfDoors { get; set; }
    public string Co2Emissions { get; set; }
    public int NoxEmissions { get; set; }
    public int NumberOfKeys { get; set; }
}

public class Ownership
{
    public string LogBook { get; set; }
    public int NumberOfOwners { get; set; }
    public string DateOfRegistration { get; set; }
}