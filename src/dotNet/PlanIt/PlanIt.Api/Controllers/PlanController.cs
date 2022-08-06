using Microsoft.AspNetCore.Mvc;
using PlanIt.Application.Contracts.Persistence;
using PlanIt.Application.Contracts.Services;
using PlanIt.Application.Dtos.Plan;

namespace PlanIt.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlanController : Controller
{
    private readonly IPlanRepository _repository;
    private readonly IPlanGenerator _generator;

    public PlanController(IPlanRepository repository, IPlanGenerator generator)
    {
        _repository = repository;
        _generator = generator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> Create([FromBody] CreatePlanDto createPlanDto, CancellationToken token = default)
    {
        var newPlan = _generator.CreatePlan(createPlanDto.FirstSchedulableDate, createPlanDto.LastSchedulableDate);
        var plan = await _repository.AddAsync(newPlan, token).ConfigureAwait(false);
        return Ok(plan?.Id);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Read(CancellationToken token = default)
    {
        return Ok(await _repository.ListAllAsync(token).ConfigureAwait(false));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Read(Guid id, CancellationToken token = default)
    {
        var plan = await _repository.GetByIdAsync(id, token).ConfigureAwait(false);
        if (plan is null) return StatusCode(StatusCodes.Status404NotFound);
        return Ok(plan);
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
