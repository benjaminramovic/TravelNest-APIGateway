using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace TravelNest_APIGateway.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly Urls _urls;

    public UserController(HttpClient httpClient, IOptions<Urls> urls)
    {
        _httpClient = httpClient;
        _urls = urls.Value;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        try
        {
            
            var response = await _httpClient.GetAsync(_urls.Users);
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