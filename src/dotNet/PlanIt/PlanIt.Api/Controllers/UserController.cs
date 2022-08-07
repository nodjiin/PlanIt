using Microsoft.AspNetCore.Mvc;
using PlanIt.Application.Contracts.Persistence;
using PlanIt.Application.Dtos.User;
using PlanIt.Domain.Entities;

namespace PlanIt.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserRepository _repository;

    public UserController(IUserRepository repository)
    {
        _repository = repository;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Create([FromBody] CreateUserDto createUserDto, CancellationToken token = default)
    {
        var availabilities = createUserDto.Availabilities?.Select(av => new Availability { Date = av.Date }).ToList();
        var newUser = new User { PlanId = createUserDto.PlanId, Availabilities = availabilities };
        var user = await _repository.AddAsync(newUser, token).ConfigureAwait(false);
        return Ok(user.Id);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Read(CancellationToken token = default)
    {
        return Ok(await _repository.ListAllAsync(token).ConfigureAwait(false));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Read(Guid id, CancellationToken token = default)
    {
        var user = await _repository.GetByIdAsync(id, token).ConfigureAwait(false);
        if (user is null) return StatusCode(StatusCodes.Status404NotFound);
        return Ok(user);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update([FromBody] UpdateUserDto updateUserDto, CancellationToken token = default)
    {
        // TODO update availability

        return Ok();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(Guid id, CancellationToken token = default)
    {
        var userToDelete = await _repository.GetByIdAsync(id, token).ConfigureAwait(false);
        if (userToDelete is null) return StatusCode(StatusCodes.Status404NotFound);
        await _repository.DeleteAsync(userToDelete, token).ConfigureAwait(false);
        return Ok();
    }
}
