using FileFlower.Core.Extensions;
using FileFlower.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// ***** Multiple file watchers per project.
builder.Services.AddFileWatchers(configuration =>
{
    configuration.ForDirectory("./incoming/", watcher =>
    {
        watcher.WhenResourceCreated(rule => rule.Filter("*.txt").Filter("*.csv").WithOrLogic())
               .AddStep(file =>
               {
                   Console.WriteLine($"CLIENT | Processed txt or csv: {file.FullName}");
                   return Task.CompletedTask;
               });
    });

    configuration.ForDirectory("./incoming_v2/", watcher =>
    {
        watcher.WhenResourceCreated(rule => rule.Filter("*.txt").Filter("*.csv").WithOrLogic())
               .AddStep(file =>
               {
                   Console.WriteLine($"CLIENT | Processed txt or csv: {file.FullName}");
                   return Task.CompletedTask;
               });

        watcher.WhenResourceDeleted(rule => rule.Filter("*.txt"))
               .AddStep(file =>
               {
                   Console.WriteLine($"CLIENT | Processed txt or csv (deleted): {file.FullName}");
                   return Task.CompletedTask;
               });
    });
}).AddFileWatcherHostedService();

// ***** OR as a single file watcher per project.
//builder.Services.AddFileWatcher(@"./incoming/", watcher =>
//{
//    watcher.WhenResourceCreated(rule => rule.Filter("*.txt").Filter("*.csv").WithOrLogic())
//           .AddStep(file =>
//           {
//               Console.WriteLine($"CLIENT | Processed txt or csv: {file.FullName}");
//               return Task.CompletedTask;
//           });

//    watcher.WhenResourceChanged(rule => rule.Filter("*.txt"))
//           .AddStep(file =>
//           {
//               Console.WriteLine($"CLIENT | Processed txt or csv (changed): {file.FullName}");
//               return Task.CompletedTask;
//           });

//    watcher.WhenResourceCreated(rule => rule.Filter("*test*").Filter("*.bat"))
//           .AddStep(file =>
//           {
//               Console.WriteLine($"CLIENT | Processed bat which contains word 'test': {file.FullName}");
//               return Task.CompletedTask;
//           });

//    watcher.WhenResourceCreated(rule => rule.Filter("*.log"))
//           .AddStep(file =>
//           {
//               Console.WriteLine($"CLIENT | Processed log: {file.FullName}");
//               return Task.CompletedTask;
//           });
//}).AddFileWatcherHostedService();

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();
