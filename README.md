# FileFlower

## NuGet Packages

[![FileFlower.Core](https://img.shields.io/nuget/v/FileFlower.Core)](https://www.nuget.org/packages/FileFlower.Core)  
[![FileFlower.DependencyInjection](https://img.shields.io/nuget/v/FileFlower.DependencyInjection)](https://www.nuget.org/packages/FileFlower.DependencyInjection)

![image](https://github.com/user-attachments/assets/b85601f0-b82b-4bde-b737-e334e98f0a58)

## Overview

**FileFlower** is a lightweight, flexible .NET library that enables **event-driven, pipeline-based processing of file system changes**. Designed for building modular workflows around directory watching, FileFlower lets you:

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
```
