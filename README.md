# Bulletin
[![Build Status](https://dev.azure.com/bpicolo/Bulletin/_apis/build/status/bpicolo.Bulletin?branchName=master)](https://dev.azure.com/bpicolo/Bulletin/_build/latest?definitionId=1&branchName=master) ![Nuget](https://img.shields.io/nuget/v/Bulletin?color=2ca399)

Bulletin is an in-progress library that simplifies file storage and processing for Dotnet Core projects using ASP.NET and Entity Framework Core.


## Getting Started
First, you'll need to update your EF Database context to support Bulletin's Attachment model.

```c#
using Bulletin.EFCore;
using Bulletin.Models;

class MyDatabaseContext : IBulletinDbContext {
    
    public DbSet<Attachment> Attachments { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new AttachmentEntityTypeConfiguration());
    }
}
```

Next, configure ASP.NET application startup with Bulletin.
```c#
using Bulletin;
using Bulletin.AspNetCore;
using Bulletin.Storage.File;
...
public class Startup {

    public void ConfigureServices(IServiceCollection services)
        services.AddBulletin<MyDatabaseContext>(b => {
            b.AddBoard(
                "images",
                new FileStorage(
                    new FileStorageOptions("./tmp/storage")
                )
            )
        })
    }
}
```

Bulletin will setup a service provider for easy access to storage boards.
```c#
class MyController {
    private readonly IBulletinBoard _imagesBoard;
    
    public MyController(IBulletin bulletin) {
        _imagesBoard = bulletin.GetBoard("images");
    }

    [HttpPost()]
    public Task<Attachment> UploadImage(IFormFile file) {
        return await _imagesBoard.AttachAsync(file.FileName, file.OpenReadStream());
    }
}
```