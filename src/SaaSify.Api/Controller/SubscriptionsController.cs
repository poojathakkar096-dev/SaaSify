using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaaSify.Application.Interface.Services;
using SaaSify.Domain.Entity;

namespace SaaSify.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionsController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;

    public SubscriptionsController(ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    [HttpGet("plans")]
    [AllowAnonymous]
    public IActionResult GetPlans()
    {
        var plans = _subscriptionService.GetPlans();
        return Ok(plans);
    }

    [HttpGet("current")]
    [Authorize]
    public async Task<IActionResult> GetCurrent()
    {
        var subscription = await _subscriptionService.GetCurrentAsync();
        if (subscription == null)
            return NotFound(new { message = "No active subscription found." });

        return Ok(subscription);
    }

    [HttpPost("save")]
    [Authorize]
    public async Task<IActionResult> Save([FromBody] SubscriptionRequest request)
    {
        try
        {
            var result = await _subscriptionService.SaveAsync(request);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("history")]
    [Authorize]
    public async Task<IActionResult> GetHistory()
    {
        var history = await _subscriptionService.GetHistoryAsync();
        return Ok(history);
    }

    [HttpPost("cancel")]
    [Authorize]
    public async Task<IActionResult> Cancel()
    {
        var success = await _subscriptionService.CancelAsync();
        if (!success)
            return NotFound(new { message = "No active subscription to cancel." });

        return Ok(new { message = "Subscription cancelled successfully." });
    }
}