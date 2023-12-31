using Domain.Repositories;
using Infra.Repositories;
using Publisher.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IExampleRepository, ExampleRepository>();
builder.Services.AddSingleton<IPublisherServices, PublisherServices>();

builder.Services.AddHostedService<RabbitMqConsumerAdd>();
builder.Services.AddHostedService<RabbitMqConsumerDelete>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();