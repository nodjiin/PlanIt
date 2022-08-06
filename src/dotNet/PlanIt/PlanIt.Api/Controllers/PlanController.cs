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

    [HttpGet]
    public async Task<ActionResult> GetAllPlans(CancellationToken token = default)
    {
        return Ok(await _repository.ListAllAsync(token).ConfigureAwait(false));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetPlanById(Guid id, CancellationToken token = default)
    {
        var plan = await _repository.GetByIdAsync(id, token).ConfigureAwait(false);
        if (plan is null) return BadRequest($"Couldn't find client with requested id: {id}");
        return Ok(plan);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreatePlanDto createPlanDto, CancellationToken token = default)
    {
        var newPlan = _generator.CreatePlan(createPlanDto.FirstSchedulableDate, createPlanDto.LastSchedulableDate);
        var plan = await _repository.AddAsync(newPlan, token).ConfigureAwait(false);
        return Ok(plan?.Id);
    }
}
