using PlanIt.Application.Dtos.Availability;
using PlanIt.Application.Dtos.User;
using PlanIt.Domain.Entities;

namespace PlanIt.Application.Extensions;
public static class UserExtensions
{
    public static void Update(this User user, UpdateUserDto dto)
    {
        if (user.Availabilities != null)
            foreach (var element in dto.AvailabilitiesToRemove ?? Enumerable.Empty<AvailabilityDto>())
            {
                var toRemove = user.Availabilities.FirstOrDefault(a => a.Date == element.Date);
                if (toRemove is not null) user.Availabilities.Remove(toRemove);
            }
        else
            user.Availabilities = new List<Availability>();

        foreach (var element in dto.AvailabilitiesToAdd ?? Enumerable.Empty<AvailabilityDto>())
            user.Availabilities.Add(new Availability { Date = element.Date });
    }
}
