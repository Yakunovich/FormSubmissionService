using FormSubmissionService.Data;
using FormSubmissionService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5000", "https://localhost:5001");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<FormDbContext>(options =>
    options.UseInMemoryDatabase("FormsDb"));

builder.Services.AddScoped<IFormService, FormService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run(); 