using FormSubmission.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllerConfig();
builder.Services.AddCorsPolicy();
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

// Добавляем глобальную обработку исключений
app.UseGlobalExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();
