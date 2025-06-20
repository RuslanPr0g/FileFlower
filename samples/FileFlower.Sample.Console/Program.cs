using FileFlower.Core.Extensions;
using FileFlower.Core.FileWatchers;

string currentDirectory = Environment.CurrentDirectory;
Console.WriteLine("Current directory: " + currentDirectory);

var watcher = new FileWatcherBuilder("~/files");

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

Console.WriteLine("Please wait for file changes in: " + currentDirectory);
Console.ReadLine();