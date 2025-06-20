# FileFlower

[[NuGet] - SOON]

![image](https://github.com/user-attachments/assets/5f8e42fc-532d-4285-99fe-f3b4aa767e1c)

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
var watcher = new FileFlowWatcher("C:\\incoming")
    .Filter("*.csv")
    .AddStep(ProcessCsv)
    .AddStep(MoveToArchive)
    .Start();

async Task ProcessCsv(FileInfo file)
{
    // Your custom processing logic here
}

async Task MoveToArchive(FileInfo file)
{
    // Move or archive file after processing
}
```
