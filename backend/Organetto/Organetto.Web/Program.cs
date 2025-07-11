using Organetto.Infrastructure.Infrastructure.Extensions;
using Organetto.UseCases.Configuration.Extensions;
using Organetto.Web.Configuration.Extensions;
using Organetto.Infrastructure.Data.Extensions;
using Organetto.Infrastructure.Infrastructure.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddAppSettingsServerConfigurationFile();

// Add services to the container.

builder.Services.AddPersistenceServices(builder.Configuration.GetConnectionString("Organetto").ThrowIfNull());
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebLayerServices(builder.Configuration);

var app = builder.Build();

app.UseInfrastructurePipelines(builder.Environment);
app.UseWebLayerPipelines();



app.UseApplicationEndpoints();

app.Run();
