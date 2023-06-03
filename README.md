# outsystems password cracker

![image](https://github.com/miltinhoc/outsystems.crack/assets/26238419/23ced8df-c05d-4590-a790-7af06dcf41af)

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

## Usage
```bash
Usage: outsystems.crack [-options]

options:
	-u <username>		outsystems account username
	-p <wordlist>		path to your wordlist
	-H <hash>	        outsystems account password hash 
	-h			show this help message and exit
```

### Example
```bash
outsystems.crack -u admin -p "C:\rockyou.txt" -H $1$To+OhVILLgzMwacVpwLeRiONkUZkFqze0nL7GLl+vsw=9C86626C48B477885290CC9F24F64BFB838D10907CF23F1A0EA9F7F15C46C04B0369577EE14AD5448A78E3491F0EA92EC0FB58FB8E5848F0E7EBC95B7B3AB438
```
