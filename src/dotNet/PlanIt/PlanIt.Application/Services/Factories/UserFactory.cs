using PlanIt.Application.Contracts.Services.Factories;
using PlanIt.Application.Dtos.User;
using PlanIt.Domain.Entities;

namespace PlanIt.Application.Services.Factories;

public class UserFactory : IUserFactory
{
    public User Create(CreateUserDto dto)
    {
        var availabilities = dto.Availabilities?.Select(av => new Availability { Date = av.Date }).ToList();
        return new User { Name = dto.Name, PlanId = dto.PlanId, Availabilities = availabilities };
    }
}

