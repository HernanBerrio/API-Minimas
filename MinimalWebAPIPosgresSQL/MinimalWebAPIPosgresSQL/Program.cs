using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using MinimalWebAPIPosgresSQL.Data;
using MinimalWebAPIPosgresSQL.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connetionString = builder.Configuration.GetConnectionString("PostgreSQLConnetion");
builder.Services.AddDbContext<OfficeDb>(options =>
options.UseNpgsql(connetionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/Employees/", async (Employee e, OfficeDb db) =>
{
    db.Employees.Add(e); ;
    await db.SaveChangesAsync();
    return Results.Created($"/employee/{e.Id}", e);

});

app.MapGet("/Employees/{id:int}", async (int id, OfficeDb db) =>
{
    return await db.Employees.FindAsync(id)
    is Employee e
    ? Results.Ok(e)
    : Results.NotFound();
});


app.MapPut("/Employees/{id:int}", async (int id, Employee e, OfficeDb db) =>
{
    if (e.Id != id)
        return Results.BadRequest();
    var employee = await db.Employees.FindAsync(id);
    if (employee is null) return Results.NotFound();

    employee.FirstName = e.FirstName;
    employee.LastName = e.LastName;
    employee.Branch = e.Branch;
    employee.Age = e.Age;

    await db.SaveChangesAsync();
    return Results.Ok(employee);
});

app.MapDelete("/Employees/{id:int}", async (int id, OfficeDb db) =>
{
    var employee = await db.Employees.FindAsync(id);
    if (employee is null) return Results.NotFound();

    db.Employees.Remove(employee);
    await db.SaveChangesAsync();

    return Results.NoContent();
});


app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}