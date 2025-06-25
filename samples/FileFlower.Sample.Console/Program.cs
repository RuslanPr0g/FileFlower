using FileFlower.Core.Extensions;
using FileFlower.Core.FileWatchers;

string currentDirectory = Environment.CurrentDirectory;
Console.WriteLine("Current directory: " + currentDirectory);

var watcher = new FileWatcherBuilder("~/files");

watcher.WhenResourceCreated(rule => rule.Filter("*.txt").Filter("*.csv").WithOrLogic())
       .AddStep(async context =>
       {
           Console.WriteLine($"CLIENT | Processed txt or csv: {context.FileInfo.FullName}");
       });

watcher.WhenResourceCreated(rule => rule.Filter("*test*").Filter("*.bat"))
       .AddStep(async context =>
       {
           Console.WriteLine($"CLIENT | Processed bat which contains word 'test': {context.FileInfo.FullName}");
       });

watcher.WhenResourceCreated(rule => rule.Filter("*.log"))
       .AddStep(async context =>
       {
           Console.WriteLine($"CLIENT | Processed log: {context.FileInfo.FullName}");
       });

Console.WriteLine("Please wait for file changes in: " + currentDirectory);
Console.ReadLine();