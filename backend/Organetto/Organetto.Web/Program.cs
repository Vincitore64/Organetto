using Organetto.Infrastructure.Data.Extensions;
using Organetto.Infrastructure.Infrastructure.Extensions;
using Organetto.UseCases.Configuration.Extensions;
using Organetto.UseCases.Shared.Extensions;
using Organetto.Web.Configuration.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddWebLayer();

// Add services to the container.

builder.Services.AddPersistenceServices(builder.Configuration.GetConnectionString("Organetto").ThrowIfNull());
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebLayerServices(builder.Configuration);

var app = builder.Build();

app.UseWebLayer();
app.UseApplication();
app.UseInfrastructure(builder.Environment);



app.UseApplicationEndpoints();

app.Run();
