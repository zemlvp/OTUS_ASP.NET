using System;
namespace PromoCodeFactory.WebHost.Models;

public record UpdateEmployeeDto(
    string FirstName, 
    string LastName, 
    string Email,
    Guid[] RolesId,
    int AppliedPromocodesCount);