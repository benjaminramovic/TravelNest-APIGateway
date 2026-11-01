using TravelNest_APIGateway;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<Urls>(builder.Configuration.GetSection("Urls"));
builder.Services.AddHttpClient();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:5173") 
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Console.WriteLine("URL: " + builder.Configuration["Urls:Users"]);
Console.WriteLine("ALL URLS:");
Console.WriteLine(builder.Configuration.GetSection("Urls").GetChildren().Count());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigins");
app.UseHttpsRedirection();
app.MapControllers();



app.Run();
