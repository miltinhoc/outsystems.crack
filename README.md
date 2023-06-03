# outsystems password cracker

![image](https://github.com/miltinhoc/outsystems.crack/assets/26238419/2a4f5661-d567-46d6-bdf2-c68ae868f0ee)

## Building and Running

### Step 1: Download and Install .NET Core 6.0 SDK

Download and install the .NET Core 6.0 SDK from the official website:
[.NET Core 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)

Follow the installation instructions for your operating system.

### Step 2: Build the Project
```bash
dotnet build
```

### Step 3: Run the Project
```bash
dotnet run --project outsystems.crack/outsystems.crack.csproj
```

Make sure to pass the arguments needed.

### Step 5: Publish the Project (Optional)
```bash
dotnet publish --configuration Release --runtime linux-x64
```

If you want to build for other platform, check the official list of available options from microsoft:
[.NET RID Catalog](https://learn.microsoft.com/en-us/dotnet/core/rid-catalog)