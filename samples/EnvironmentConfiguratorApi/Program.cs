using EnvironmentConfigurator;

var builder = WebApplication.CreateBuilder(args);

// Environment'a göre appsettings.{Environment}.json + environment variables yüklenir.
// Loads appsettings.{Environment}.json + environment variables based on the active environment.
builder.AddEnvironmentConfiguration();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
