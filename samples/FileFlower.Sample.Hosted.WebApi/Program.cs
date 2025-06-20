using FileFlower.Core.Extensions;
using FileFlower.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFileWatcher(@"./incoming/", watcher =>
{
    watcher.WhenResourceCreated(rule => rule.Filter("*.txt").Filter("*.csv").WithOrLogic())
           .AddStep(file =>
           {
               Console.WriteLine($"CLIENT | Processed txt or csv: {file.FullName}");
               return Task.CompletedTask;
           });

    watcher.WhenResourceCreated(rule => rule.Filter("*test*").Filter("*.bat"))
           .AddStep(file =>
           {
               Console.WriteLine($"CLIENT | Processed bat which contains word 'test': {file.FullName}");
               return Task.CompletedTask;
           });

    watcher.WhenResourceCreated(rule => rule.Filter("*.log"))
           .AddStep(file =>
           {
               Console.WriteLine($"CLIENT | Processed log: {file.FullName}");
               return Task.CompletedTask;
           });
}).AddFileWatcherHostedService();

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();
