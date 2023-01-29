var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddHttpClient//>http://localhost:5063/swagger/v2/swagger.json
builder.Services.AddHttpClient<IClinicClient, ClinicClient>("ClinicClient", client =>
{
    return null;
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
