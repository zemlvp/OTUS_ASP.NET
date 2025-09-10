using System;
namespace PromoCodeFactory.WebHost.Models;

public record AddEmployeeDto(
    string FirstName, 
    string LastName, 
    string Email, 
    Guid[] RolesId, 
    int AppliedPromocodesCount);