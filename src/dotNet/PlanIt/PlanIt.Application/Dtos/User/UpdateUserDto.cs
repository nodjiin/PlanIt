﻿using PlanIt.Application.Dtos.Availability;

namespace PlanIt.Application.Dtos.User;
public class UpdateUserDto
{
    public Guid Id { get; set; }
    public List<AvailabilityDto>? AvailabilitiesToAdd { get; set; }
    public List<AvailabilityDto>? AvailabilitiesToRemove { get; set; }
}
