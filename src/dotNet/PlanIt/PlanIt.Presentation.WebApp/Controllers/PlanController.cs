using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PlanIt.Application.Dtos.Plan;
using PlanIt.Presentation.WebApp.Models;
using PlanIt.Presentation.WebApp.Options;
using System.Text;

namespace PlanIt.Presentation.WebApp.Controllers;
public class PlanController : Controller
{
    private readonly IHttpClientFactory _factory;
    private readonly string _createPlanUrl;
    private readonly ILogger<PlanController> _logger;

    public PlanController(IHttpClientFactory factory, IOptions<PlanItBackendApiUrls> options, ILogger<PlanController> logger)
    {
        _factory = factory;

        if (options is null || string.IsNullOrWhiteSpace(options.Value.ServerUrl) || string.IsNullOrWhiteSpace(options.Value.CreatePlanUrl))
            throw new ArgumentException($"{nameof(options)} doesn't contain enough information to call the create plan url.");

        _createPlanUrl = options.Value.ServerUrl + options.Value.CreatePlanUrl;
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
        if (!ModelState.IsValid)
        {
            // TODO error page
        }

        var dto = new CreatePlanDto
        {
            FirstSchedulableDate = model.DateFrom,
            LastSchedulableDate = model.DateTo
        };

        var client = _factory.CreateClient();
        var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8);

        Guid newPlanId;

        try
        {
            var response = await client.PostAsync(_createPlanUrl, content, token).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                // TODO log, error page
            }

            newPlanId = new Guid(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }
        catch (Exception ex)
        {
            // TODO error page
            _logger.LogError(ex, $"Exception raised while trying to create a new plan in interval {dto.FirstSchedulableDate} - {dto.LastSchedulableDate}");
            return RedirectToAction(nameof(Create));
        }

        return RedirectToAction(nameof(Calendar), new { id = newPlanId });
    }

    [HttpGet("{id}")]
    public IActionResult Calendar(Guid id)
    {
        return View();
    }
}
