using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using RfidAttendance.Web.Data;
using RfidAttendance.Web.Models;
using RfidAttendance.Web.Workers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<AttendanceContext>();

builder.Services.AddSingleton<RfidTagWorker>();
builder.Services.AddSingleton<IRfidTagTaskQueue, RfidTagTaskQueue>(ctx =>
{
    if (!int.TryParse(builder.Configuration["QueueCapacity"], out var queueCapacity))
        queueCapacity = 100;
    return new RfidTagTaskQueue(queueCapacity);
});

builder.Services.AddHostedService<RfidTagHostedService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapPost("/tag", async ([FromBody]string body, [FromServices] IServiceScopeFactory serviceScopeFactory, [FromServices] RfidTagWorker worker) =>
{
    var req = JsonSerializer.Deserialize<ExternalTagRequest>(body);
    foreach(var t in req?.tags ?? Array.Empty<ExternalTag>())
    {
        var tag = new RfidTag()
        {
            Id = t.epc,
            LastSeen = DateTime.UtcNow,
            IsCurrenltyInAttendance = false,
            Student = null
        };
        await worker.RfidTagSeen(serviceScopeFactory, tag);
    }
});

app.Run();
