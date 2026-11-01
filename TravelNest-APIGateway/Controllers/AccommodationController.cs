using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TravelNest_APIGateway.Models;

namespace TravelNest_APIGateway.Controllers;

[ApiController]
[Route("[controller]")]
public class AccommodationController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly Urls _urls;

    public AccommodationController(HttpClient client, IOptions<Urls> urls)
    {
        _httpClient = client;
        _urls = urls.Value;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            
            var response = await _httpClient.GetAsync(_urls.Accommodations);
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
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync(_urls.Accommodations + "/" + id);
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
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAccommodation([FromRoute] int id)
    {
        try
        {
            var response = await _httpClient.GetAsync(_urls.Accommodations + "/" + id);
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
    public async Task<IActionResult> CreateAccommodation(
        [FromForm] Accommodation request)
    {
        using var content = new MultipartFormDataContent();

        content.Add(
            new StringContent(request.Name),
            "name"
        );

        content.Add(
            new StringContent(request.Description),
            "description"
        );

        content.Add(
            new StringContent(request.MinPrice.ToString(
                System.Globalization.CultureInfo.InvariantCulture)),
            "minPrice"
        );

        content.Add(
            new StringContent(request.Address),
            "address"
        );

        content.Add(
            new StringContent(request.City),
            "city"
        );

        content.Add(
            new StringContent(request.Rate.ToString(
                System.Globalization.CultureInfo.InvariantCulture)),
            "rate"
        );

        content.Add(
            new StringContent(request.MaxGuests.ToString()),
            "maxGuests"
        );

        content.Add(
            new StringContent(request.Latitude.ToString(
                System.Globalization.CultureInfo.InvariantCulture)),
            "latitude"
        );

        content.Add(
            new StringContent(request.Longitude.ToString(
                System.Globalization.CultureInfo.InvariantCulture)),
            "longitude"
        );

        content.Add(
            new StringContent(request.CategoryId.ToString()),
            "categoryId"
        );
        content.Add(
            new StringContent(request.Cleanliness.ToString()),
            "cleanliness"
        );

        content.Add(
            new StringContent(request.Location.ToString()),
            "location"
        );
        content.Add(
            new StringContent(request.Luxury.ToString()),
            "luxury"
        );
        content.Add(
            new StringContent(request.Family_friendly.ToString()),
            "family_friendly"
        );
        var featureIds = request.FeatureIds;

        if (string.IsNullOrWhiteSpace(featureIds))
        {
            featureIds = "[]";
        }
        else if (!featureIds.TrimStart().StartsWith("["))
        {
            featureIds = $"[{featureIds}]";
        }

        content.Add(
            new StringContent(featureIds),
            "featureIds"
        );
  

        // Slike
        foreach (var image in request.Images)
        {
            var stream = image.OpenReadStream();

            var fileContent = new StreamContent(stream);

            fileContent.Headers.ContentType =
                
                new MediaTypeHeaderValue(image.ContentType);

            content.Add(
                fileContent,
                "images",
                image.FileName
            );
        }

        var response = await _httpClient.PostAsync(
            _urls.Accommodations,
            content
        );

        var result = await response.Content.ReadAsStringAsync();

        return StatusCode(
            (int)response.StatusCode,
            result
        );
    }
    
    //[HttpPost]
    /* public async Task<IActionResult> Post([FromBody] Guest guest)
    {
        try
        {
            var content = new StringContent(JsonConvert.SerializeObject(guest),Encoding.UTF8, @"application/json");
            var response = await _httpClient.PostAsync(_urls.Accommodations + "api/accommodations", content);
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
    } */
}