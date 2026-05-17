using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace TravelNest_APIGateway.Controllers;

[ApiController]
[Route("[controller]")]
public class ReservationController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly Urls _urls;

    public ReservationController(HttpClient client, IOptions<Urls> urls)
    {
        _httpClient = client;
        _urls = urls.Value;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            
            var response = await _httpClient.GetAsync(_urls.Reservations);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return Content(content, response.Content.Headers.ContentType?.ToString() ?? "application/json");
            }

            return StatusCode((int)response.StatusCode);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}