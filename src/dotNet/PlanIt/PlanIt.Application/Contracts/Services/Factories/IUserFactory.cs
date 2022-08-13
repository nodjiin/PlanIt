using PlanIt.Application.Dtos.User;
using PlanIt.Domain.Entities;

namespace PlanIt.Application.Contracts.Services.Factories;
public interface IUserFactory : IEntityFactory<User, CreateUserDto>
{
}
