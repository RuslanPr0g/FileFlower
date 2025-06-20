using FileFlower.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFileWatcher(@"./incoming/", watcher =>
{
    watcher.Filter("*.txt")
           .AddStep(file =>
           {
               Console.WriteLine($"Processed: {file.FullName}");
               return Task.CompletedTask;
           });
}).AddFileWatcherHostedService();

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();
