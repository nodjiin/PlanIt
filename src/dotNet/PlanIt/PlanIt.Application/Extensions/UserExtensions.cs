using PlanIt.Application.Dtos.User;
using PlanIt.Domain.Entities;

namespace PlanIt.Application.Extensions;
public static class UserExtensions
{
    public static void Update(this User user, UpdateUserDto dto)
    {
        if (user.Availabilities == null)
        {
            user.Availabilities = new List<Availability>();
        }

        if (dto.Availabilities == null || dto.Availabilities.Count == 0)
        {
            // TODO delete policy
            user.Availabilities = new List<Availability>();
            return;
        }

        // user availabilities are sorted by default
        var sortedDates = dto.Availabilities.OrderBy(a => a.Date).ToList();

        // overwrite availabilities
        int i;
        for (i = 0; i < sortedDates.Count && i < user.Availabilities.Count; i++)
        {
            user.Availabilities[i].Date = sortedDates[i].Date;
        }

        // add missing elements
        if (sortedDates.Count > user.Availabilities.Count)
        {
            while (i < sortedDates.Count)
            {
                user.Availabilities.Add(new Availability { UserId = user.UserId, Date = sortedDates[i++].Date });
            }
        }
        else if (user.Availabilities.Count > sortedDates.Count) // or remove the additional ones
        {
            while (i++ < user.Availabilities.Count)
            {
                user.Availabilities.RemoveAt(user.Availabilities.Count - 1);
            }
        }
    }

    public static ReadUserDto ConvertToDto(this User user)
    {
        ReadUserDto dto = new ReadUserDto();
        dto.UserId = user.UserId;
        dto.Name = user.Name;
        if (user.Availabilities != null)
        {
            dto.Availabilities = user.Availabilities.Select(av => av.ConvertToDto()).ToList();
        }

        return dto;
    }
}
