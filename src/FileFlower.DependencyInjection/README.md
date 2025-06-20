# FileFlower

## Overview

**This package provides an ability to register file watcher in the DI container and host it as a HostedService**.
It works on top of the **FileFlower.Core** package, which is a lightweight, flexible .NET library that enables **event-driven, pipeline-based processing of file system changes**. Designed for building modular workflows around directory watching, FileFlower lets you:

- React to file creation, modification, deletion events
- Filter and route files by patterns and extensions
- Define processing pipelines with pre- and post-steps
- Easily chain multiple processors in a clean, reusable way

FileFlower is ideal for building file ingestion services, automated workflows, and data pipelines that work with file systems in a reactive and maintainable manner.

---

## Features

- Monitor directories and subdirectories for file changes
- Filter files by pattern, extension, or custom rules
- Define composable pipeline steps for processing files
- Support for error handling, retries, and archiving
- Minimal dependencies, cross-platform (.NET 8+)

---

## Getting Started

```csharp
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
```
