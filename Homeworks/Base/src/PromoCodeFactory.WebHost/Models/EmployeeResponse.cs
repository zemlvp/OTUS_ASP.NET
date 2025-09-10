using System;
using System.Collections.Generic;

namespace PromoCodeFactory.WebHost.Models;

public record EmployeeResponse(Guid Id, string FullName, string Email, List<RoleItemResponse> Roles, int AppliedPromocodesCount);
