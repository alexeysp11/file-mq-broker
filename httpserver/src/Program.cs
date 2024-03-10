using FileMqBroker.HttpService.ResponseHandlers;
using FileMqBroker.MqLibrary.Adapters.ReadAdapters;
using FileMqBroker.MqLibrary.Adapters.WriteAdapters;
using FileMqBroker.MqLibrary.KeyCalculations;
using FileMqBroker.MqLibrary.KeyCalculations.FileNameGeneration;
using FileMqBroker.MqLibrary.KeyCalculations.RequestCollapsing;
using FileMqBroker.MqLibrary.Models;
using FileMqBroker.MqLibrary.RuntimeQueues;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// App init configs.
builder.Services.AddSingleton(_ =>
{
    return new AppInitConfigs
    {
        DbConnectionString = "",
        RequestDirectoryName = "",
        ResponseDirectoryName = "",
        OneTimeProcQueueElements = 20_000,
        DuplicateRequestCollapseType = DuplicateRequestCollapseType.Naive,
        BackendContinuationDelegate = HttpResponseHandler.ContinuationMethod
    };
});

// Key calculations.
builder.Services.AddSingleton<KeyCalculationMD5>();
builder.Services.AddSingleton<KeyCalculationSHA256>();
builder.Services.AddSingleton<IFileNameGeneration, FileNameGenerationMD5>();
builder.Services.AddSingleton<IRequestCollapser, RequestCollapserSHA256>();

// Queues.
builder.Services.AddSingleton<IReadMFQueue, MessageFileQueue>();
builder.Services.AddSingleton<IWriteMFQueue, MessageFileQueue>();

// Queue adapters.
builder.Services.AddSingleton<IReadAdapter, FileMqReadAdapter>();
builder.Services.AddSingleton<IWriteAdapter, FileMqWriteAdapter>();

// Backend worker service.
builder.Services.AddHostedService<HttpResponseHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
