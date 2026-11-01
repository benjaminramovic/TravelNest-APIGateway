using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TravelNest_APIGateway.Models;

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
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetReservation([FromRoute] int id)
    {
        try
        {
            var response = await _httpClient.GetAsync(_urls.Reservations + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return Content(content, response.Content.Headers.ContentType?.ToString() ?? "application/json");
            }

            return StatusCode((int)response.StatusCode);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] Reservation reservation)
    {
        try
        {
            var content = new StringContent(JsonConvert.SerializeObject(reservation), Encoding.UTF8,
                "application/json");
            var response = await _httpClient.PostAsync(_urls.Reservations, content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return Content(responseContent,
                    response.Content.Headers.ContentType?.ToString() ?? "application/json");
            }

            return StatusCode((int)response.StatusCode);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync(_urls.Reservations + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return Content(responseContent, response.Content.Headers.ContentType?.ToString() ?? "application/json");
            }

            return StatusCode((int)response.StatusCode);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}