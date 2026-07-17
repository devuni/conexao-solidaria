using ConexaoSolidaria.DonationWorker.Consumers;
using ConexaoSolidaria.Infrastructure;
using MassTransit;
using MongoDB.Driver;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddSingleton<IMongoClient>(_ =>
{
    var connectionString = builder.Configuration.GetConnectionString("MongoDb")
        ?? "mongodb://mongo:27017/conexao_solidaria";

    return new MongoClient(connectionString);
});

builder.Services.AddMassTransit(options =>
{
    options.SetKebabCaseEndpointNameFormatter();
    options.AddConsumer<DoacaoRecebidaConsumer>();

    options.UsingRabbitMq((context, cfg) =>
    {
        var host = builder.Configuration["RabbitMq:Host"] ?? "localhost";
        var port = ushort.Parse(builder.Configuration["RabbitMq:Port"] ?? "5672");
        var username = builder.Configuration["RabbitMq:Username"] ?? "guest";
        var password = builder.Configuration["RabbitMq:Password"] ?? "guest";

        cfg.Host(host, port, "/", rabbit =>
        {
            rabbit.Username(username);
            rabbit.Password(password);
        });

        cfg.ReceiveEndpoint("doacao-recebida", endpoint =>
        {
            endpoint.UseMessageRetry(retry => retry.Interval(3, TimeSpan.FromSeconds(5)));
            endpoint.ConfigureConsumer<DoacaoRecebidaConsumer>(context);
        });
    });
});

var host = builder.Build();
host.Run();
