using Organetto.Infrastructure.Infrastructure.Extensions;
using Organetto.UseCases.Configuration.Extensions;
using Organetto.Web.Configuration.Extensions;
using Organetto.Infrastructure.Data.Extensions;
using Organetto.Infrastructure.Infrastructure.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddAppSettingsServerConfigurationFile();

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration.GetConnectionString("Organetto").ThrowIfNull());
builder.Services.AddApplicationServices();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGenNewtonsoftSupport();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseInfrastructureServices();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

if (!builder.Environment.IsDevelopment())
{
    app.MigrateApplicationDb();
}

app.Run();
