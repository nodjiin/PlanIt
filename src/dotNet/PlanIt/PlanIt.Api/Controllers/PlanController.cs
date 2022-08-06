using Microsoft.AspNetCore.Mvc;
using PlanIt.Application.Contracts.Persistence;

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

    [HttpGet]
    public async Task<ActionResult> GetAllPlans()
    {
        return Ok(await _repository.ListAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetPlanById(Guid id)
    {
        var plan = await _repository.GetByIdAsync(id);
        if (plan is null) return BadRequest($"Couldn't find client with requested id: {id}");
        return Ok(plan);
    }
}
