using System.Collections.Generic;
using RentApplication.Models;

public class AppartamentDTO : Appartament
{
    public string metro { get; set; }
    public string distanceToMetro { get; set; }
    public string address { get; set; }
    public string phoneNumber { get; set; }
    public int numberHouse { get; set; }
    public List<string> imageNames {get; set;}

    public List<string> ameneties {get;set;}
    public AppartamentDTO(Appartament appartament, House house, User user, List<string> imageNames)
    {
        Id = appartament.Id;      
        Description = appartament.Description;
        Area = appartament.Area;
        CountOfRooms = appartament.CountOfRooms;
        Floor = appartament.Floor;
        Price = appartament.Price;
        Type = appartament.Type;
        ReferenceTo3D = appartament.ReferenceTo3D;
        countOfBedrooms = appartament.countOfBedrooms;
        metro = house.Metro;
        distanceToMetro = house.DistanceToMetro;
        numberHouse = house.NumberHouse;
        address = house.Address;
        phoneNumber = user.PhoneNumber;
        this.imageNames = imageNames;
    }
}