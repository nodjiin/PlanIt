using Microsoft.AspNetCore.Mvc;
using PlanIt.Application.Contracts.Persistence;
using PlanIt.Application.Dtos.Plan;
using PlanIt.Domain.Entities;

namespace PlanIt.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlanController : Controller
{
    private readonly IPlanRepository _repository;

    public PlanController(IPlanRepository repository)
    {
        _repository = repository;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Create([FromBody] CreatePlanDto createPlanDto, CancellationToken token = default)
    {
        var newPlan = new Plan { FirstSchedulableDate = createPlanDto.FirstSchedulableDate, LastSchedulableDate = createPlanDto.LastSchedulableDate };
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
    public async Task<ActionResult> Read(Guid id, CancellationToken token = default)
    {
        var plan = await _repository.GetByIdAsync(id, token).ConfigureAwait(false);
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

        if (updatePlanDto.UpdateFirstSchedulableDate) planToUpdate.FirstSchedulableDate = updatePlanDto.FirstSchedulableDate;
        if (updatePlanDto.UpdateLastSchedulableDate) planToUpdate.LastSchedulableDate = updatePlanDto.LastSchedulableDate;

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
