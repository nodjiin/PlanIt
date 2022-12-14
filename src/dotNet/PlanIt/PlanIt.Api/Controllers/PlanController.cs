using Microsoft.AspNetCore.Mvc;
using PlanIt.Application.Contracts.Persistence;
using PlanIt.Application.Contracts.Services.Factories;
using PlanIt.Application.Dtos.Plan;
using PlanIt.Application.Extensions;
using PlanIt.Domain.Entities;

namespace PlanIt.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlanController : Controller
{
    private readonly IPlanRepository _repository;
    private readonly IPlanFactory _factory;

    public PlanController(IPlanRepository repository, IPlanFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Create([FromBody] CreatePlanDto createPlanDto, CancellationToken token = default)
    {
        var newPlan = _factory.Create(createPlanDto);
        var plan = await _repository.AddAsync(newPlan, token).ConfigureAwait(false);
        return Ok(plan.Id);
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
    public async Task<ActionResult> Read(Guid id, [FromQuery] bool full = true, CancellationToken token = default)
    {
        Plan? plan;

        if (full) plan = await _repository.GetFullPlanByIdAsync(id).ConfigureAwait(false);
        else plan = await _repository.GetByIdAsync(id, token).ConfigureAwait(false);

        if (plan is null) return StatusCode(StatusCodes.Status404NotFound);
        return Ok(plan);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Update([FromBody] UpdatePlanDto updatePlanDto)
    {
        var planToUpdate = await _repository.GetByIdAsync(updatePlanDto.Id).ConfigureAwait(false);
        if (planToUpdate is null) return StatusCode(StatusCodes.Status404NotFound);

        planToUpdate.Update(updatePlanDto);
        await _repository.UpdateAsync(planToUpdate).ConfigureAwait(false);
        return Ok();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Delete(Guid id, CancellationToken token = default)
    {
        var planToDelete = await _repository.GetByIdAsync(id, token).ConfigureAwait(false);
        if (planToDelete is null) return StatusCode(StatusCodes.Status404NotFound);
        await _repository.DeleteAsync(planToDelete, token).ConfigureAwait(false);
        return Ok();
    }
}
