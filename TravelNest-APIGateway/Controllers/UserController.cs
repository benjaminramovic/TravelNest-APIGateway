using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TravelNest_APIGateway.Models;

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
    public async Task<IActionResult> Get()
    {
        try
        {
            var authorization = Request.Headers.Authorization.ToString();

            if (string.IsNullOrWhiteSpace(authorization) ||
                !authorization.StartsWith(
                    "Bearer ",
                    StringComparison.OrdinalIgnoreCase))
            {
                return Unauthorized();
            }

            using var request = new HttpRequestMessage(
                HttpMethod.Get,
                _urls.Users
            );

            request.Headers.Authorization =
                AuthenticationHeaderValue.Parse(authorization);

            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            return new ContentResult
            {
                StatusCode = (int)response.StatusCode,
                Content = content,
                ContentType = response.Content.Headers.ContentType?.ToString()
                              ?? "application/json"
            };
        }
        catch (HttpRequestException)
        {
            return StatusCode(502, "Users service is unavailable.");
        }
    }
    
    [HttpGet("length")]
    public async Task<IActionResult> GetLength()
    {
        try
        {
            var authorization = Request.Headers.Authorization.ToString();

            if (string.IsNullOrWhiteSpace(authorization) ||
                !authorization.StartsWith(
                    "Bearer ",
                    StringComparison.OrdinalIgnoreCase))
            {
                return Unauthorized();
            }

            using var request = new HttpRequestMessage(
                HttpMethod.Get,
                _urls.Users + "/length"
            );

            request.Headers.Authorization =
                AuthenticationHeaderValue.Parse(authorization);

            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            return new ContentResult
            {
                StatusCode = (int)response.StatusCode,
                Content = content,
                ContentType = response.Content.Headers.ContentType?.ToString()
                              ?? "application/json"
            };
        }
        catch (HttpRequestException)
        {
            return StatusCode(502, "Users service is unavailable.");
        }
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] User user)
    {
        try
        {
            var content = new StringContent(JsonConvert.SerializeObject(user),Encoding.UTF8, @"application/json");
            Console.WriteLine(JsonConvert.SerializeObject(user));
            var response = await _httpClient.PostAsync(_urls.Users + "login", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return Content(responseContent, response.Content.Headers.ContentType?.ToString() ?? "application/json");
            }

            return StatusCode((int)response.StatusCode);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] NewUser user)
    {
        try
        {
            var content = new StringContent(JsonConvert.SerializeObject(user),Encoding.UTF8, @"application/json");
            var response = await _httpClient.PostAsync(_urls.Users + "register", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return Content(responseContent, response.Content.Headers.ContentType?.ToString() ?? "application/json");
            }

            return StatusCode((int)response.StatusCode);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> Register([FromBody] RefreshToken token)
    {
        try
        {
            var content = new StringContent(JsonConvert.SerializeObject(token),Encoding.UTF8, @"application/json");
            var response = await _httpClient.PostAsync(_urls.Users + "refresh", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return Content(responseContent, response.Content.Headers.ContentType?.ToString() ?? "application/json");
            }

            return StatusCode((int)response.StatusCode);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    
}