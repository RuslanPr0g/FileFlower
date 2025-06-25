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
- Minimal dependencies, cross-platform (.NET 8+)

---

## Getting Started

```csharp
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
```

---

## How to Create and Deploy a NuGet Package

Follow these steps to create, version, and deploy your NuGet package for FileFlower projects:

1. **Add your code changes**  
   Make all necessary code updates or feature additions in your local branch.

2. **Increase the NuGet version**  
   Update the version number inside the `Directory.Build.props` file to reflect your new release.

3. **Update the changelog**  
   Add a version description to the `CHANGELOG.md` file under each NuGet project, documenting your changes.

4. **(Optional) Update the README**  
   Modify the `README.md` under each NuGet project if you want to update usage instructions or examples.

5. **Create a pull request**  
   Push your changes and create a pull request targeting the `master` branch.

6. **Merge and tag**  
   After your PR is merged to `master`, create a Git tag on GitHub corresponding to the new NuGet package version (e.g., `v1.2.3`).

7. **Wait for deployment**  
   The CI/CD pipeline will automatically build and deploy the package to the NuGet feed. Wait until the package is available on [nuget.org](https://www.nuget.org/).

8. **Update the NuGet API key (every 3 months)**  
   For security, update the NuGet API key periodically (once every three months) in your CI/CD environment to maintain deployment access.
