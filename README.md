# Console example of ML.NET

**[Original manual](https://dotnet.microsoft.com/learn/machinelearning-ai/ml-dotnet-get-started-tutorial)**
----------------------------------------------------------------------------------------------------

### To start building .NET apps you just need to download and install the .NET SDK (Software Development Kit).

**[Download .NET SDK 64-bit](https://download.visualstudio.microsoft.com/download/pr/26f5f19d-3eba-4ee0-b4d4-3fa2a6dc0f4b/c4a0c3ff9c8df8e472b51532d7b3eb35/dotnet-sdk-2.2.105-win-gs-x64.exe)**

**[Download .NET SDK 32-bit](https://download.visualstudio.microsoft.com/download/pr/e2e6fc59-d6ed-4845-8769-872049fb50b4/d41c74a31b8a64545914dfe2479207ad/dotnet-sdk-2.2.105-win-gs-x86.exe)**

Check everything installed correctly.
`dotnet --list-sdks`

[Download .NET SDKs for Visual Studio](https://dotnet.microsoft.com/download/visual-studio-sdks)
.NET Core 2.1 or later
.NET Framework 4.6.1 or later

Open solution in Visual Studio and install [ML.NET package](https://www.nuget.org/packages/Microsoft.ML).
.NET CLI:
`Install-Package Microsoft.ML -Version 1.0.0-preview`
Package Manager:
`Install-Package Microsoft.ML -Version 0.11.0`

[Download data set.](https://archive.ics.uci.edu/ml/machine-learning-databases/iris/iris.data)
In Visual Studio, you'll need to configure iris-data.txt to be copied to the output directory.
Right-click on the file in Solution Explorer and select Properties.
Set Copy To Output Directory to Copy always.

**Write program code.**
