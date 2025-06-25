using FileFlower.Core.Extensions;
using FileFlower.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// ***** Multiple file watchers per project.
builder.Services.AddFileWatchers(configuration =>
{
    configuration.ForDirectory("./incoming/", watcher =>
    {
        watcher.WhenResourceCreated(rule => rule.Filter("*.txt").Filter("*.csv").WithOrLogic())
               .AddStep(async context =>
               {
                   Console.WriteLine($"CLIENT | Processed txt or csv: {context.FileInfo.FullName}");
               });
    });

    configuration.ForDirectory("./incoming_v2/", watcher =>
    {
        watcher.WhenResourceCreated(rule => rule.Filter("*.txt").Filter("*.csv").WithOrLogic())
               .AddStep(async context =>
               {
                   Console.WriteLine($"CLIENT | Processed txt or csv: {context.FileInfo.FullName}");
               });

        watcher.WhenResourceDeleted(rule => rule.Filter("*.txt"))
               .AddStep(async context =>
               {
                   Console.WriteLine($"CLIENT | Processed txt or csv (deleted): {context.FileInfo.FullName}");
               });
    });
}).AddFileWatcherHostedService();

// ***** OR as a single context watcher per project.
//builder.Services.AddFileWatcher(@"./incoming/", watcher =>
//{
//    watcher.WhenResourceCreated(rule => rule.Filter("*.txt").Filter("*.csv").WithOrLogic())
//           .AddStep(async context =>
//           {
//               Console.WriteLine($"CLIENT | Processed txt or csv: {context.FileInfo.FullName}");
//           });

//    watcher.WhenResourceChanged(rule => rule.Filter("*.txt"))
//           .AddStep(async context =>
//           {
//               Console.WriteLine($"CLIENT | Processed txt or csv (changed): {context.FileInfo.FullName}");
//           });

//    watcher.WhenResourceCreated(rule => rule.Filter("*test*").Filter("*.bat"))
//           .AddStep(async context =>
//           {
//               Console.WriteLine($"CLIENT | Processed bat which contains word 'test': {context.FileInfo.FullName}");
//           });

//    watcher.WhenResourceCreated(rule => rule.Filter("*.log"))
//           .AddStep(async context =>
//           {
//               Console.WriteLine($"CLIENT | Processed log: {context.FileInfo.FullName}");
//           });
//}).AddFileWatcherHostedService();

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();
