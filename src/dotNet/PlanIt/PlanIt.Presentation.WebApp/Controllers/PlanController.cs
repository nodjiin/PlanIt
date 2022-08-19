using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PlanIt.Application.Dtos.Plan;
using PlanIt.Domain.Entities;
using PlanIt.Presentation.WebApp.Models;
using PlanIt.Presentation.WebApp.Options;
using System.Text;

namespace PlanIt.Presentation.WebApp.Controllers;
public class PlanController : Controller
{
    private readonly IHttpClientFactory _factory;
    private readonly string _planApiUrl;
    private readonly ILogger<PlanController> _logger;

    public PlanController(IHttpClientFactory factory, IOptions<PlanItBackendApiUrls> options, ILogger<PlanController> logger)
    {
        _factory = factory;

        if (options is null || string.IsNullOrWhiteSpace(options.Value.ServerUrl) || string.IsNullOrWhiteSpace(options.Value.PlanApiUrl))
            throw new ArgumentException($"{nameof(options)} doesn't contain enough information to call the create plan url.");

        _planApiUrl = options.Value.ServerUrl + options.Value.PlanApiUrl;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Create()
    {
        var model = new PlanModel { DateFrom = DateTime.Today, DateTo = DateTime.Today.AddDays(1) };
        return View(model);
    }

    [HttpPost]
    public async Task<ActionResult> Create(PlanModel model, CancellationToken token = default)
    {
        if (!ModelState.IsValid) throw new ArgumentException(nameof(model));

        var dto = new CreatePlanDto
        {
            FirstSchedulableDate = model.DateFrom,
            LastSchedulableDate = model.DateTo
        };

        var client = _factory.CreateClient();
        var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

        Guid newPlanId;

        try
        {
            var response = await client.PostAsync(_planApiUrl, content, token).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"Failed to create a new plan. Server responded with status code '{response.StatusCode}' and reason '{response.ReasonPhrase}'"); // TODO custom exception

            var responseContent = await response.Content.ReadAsStringAsync(token).ConfigureAwait(false);
            newPlanId = new Guid(responseContent.Substring(1, responseContent.Length - 2));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Exception raised while trying to create a new plan in interval {dto.FirstSchedulableDate} - {dto.LastSchedulableDate}");
            throw;
        }

        return RedirectToAction(nameof(Calendar), new { id = newPlanId });
    }

    [HttpGet]
    [Route("[controller]/[action]/{id}")]
    public async Task<IActionResult> Calendar(Guid id, CancellationToken token = default)
    {
        var client = _factory.CreateClient();
        Plan? plan;

        try
        {
            var response = await client.GetAsync($"{_planApiUrl}/{id}", token);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"Failed to create a new plan. Server responded with status code '{response.StatusCode}' and reason '{response.ReasonPhrase}'"); // TODO custom exception

            plan = await response.Content.ReadFromJsonAsync<Plan>(cancellationToken: token).ConfigureAwait(false);
            if (plan is null) throw new InvalidOperationException("Value retrieved from server response is null.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Exception raised while trying to retrieve plan {id}");
            throw;
        }

        return View(plan);
    }
}
