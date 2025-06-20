using FileFlower.Core.FileWatchers;

string currentDirectory = Environment.CurrentDirectory;
Console.WriteLine("Current directory: " + currentDirectory);

var fileWatcher = new FileWatcherBuilder("~/files")
    .Filter("*.txt")
    .AddStep(Log)
    .Start();

static async Task Log(FileInfo file) => Console.WriteLine($"called FOR FILE WATCHER subscription!! {file.FullName}");
