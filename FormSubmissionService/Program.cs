using FormSubmissionService.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllerConfig();
builder.Services.AddCorsPolicy();
builder.Services.AddApplicationDbContext(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run(); 