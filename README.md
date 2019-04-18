# Console example of ML.NET.

**[Original manual.](https://dotnet.microsoft.com/learn/machinelearning-ai/ml-dotnet-get-started-tutorial)**
**[ML.NET Guide.](https://docs.microsoft.com/ru-ru/dotnet/machine-learning/)**
----------------------------------------------------------------------------------------------------

## Windows.

----------------------------------------------------------------------------------------------------

### Download and install.

[Download .NET SDKs for Visual Studio.](https://dotnet.microsoft.com/download/visual-studio-sdks)
[Download .NET SDK 64-bit.](https://download.visualstudio.microsoft.com/download/pr/26f5f19d-3eba-4ee0-b4d4-3fa2a6dc0f4b/c4a0c3ff9c8df8e472b51532d7b3eb35/dotnet-sdk-2.2.105-win-gs-x64.exe)
[Download .NET SDK 32-bit.](https://download.visualstudio.microsoft.com/download/pr/e2e6fc59-d6ed-4845-8769-872049fb50b4/d41c74a31b8a64545914dfe2479207ad/dotnet-sdk-2.2.105-win-gs-x86.exe)

Check everything installed correctly.
'> dotnet --info'
----------------------------------------------------------------------------------------------------

### Create your app

Command prompt.
'> dotnet new console -o myMLApp
> cd myMLApp'

Visual Studio.
'.NET Core 2.1 or later'
----------------------------------------------------------------------------------------------------

### Install ML.NET package.

Command prompt.
'> dotnet add package Microsoft.ML --version 1.0.0-preview'
.NET CLI.
`> Install-Package Microsoft.ML -Version 1.0.0-preview`
Package Manager.
`> Install-Package Microsoft.ML -Version 0.11.0`
----------------------------------------------------------------------------------------------------

### Download data set.

[Download iris.data.](https://archive.ics.uci.edu/ml/machine-learning-databases/iris/iris.data)

Visual Studio.
Configure iris-data.txt to be copied to the output directory.
Set Copy To Output Directory to Copy always.
----------------------------------------------------------------------------------------------------

### Write program code.

Program.cs.
'using Microsoft.ML;
using Microsoft.ML.Data;
using System;

// CS0649 compiler warning is disabled because some fields are only
// assigned to dynamically by ML.NET at runtime
#pragma warning disable CS0649

namespace myMLApp
{
    class Program
    {
        // STEP 1: Define your data structures
        // IrisData is used to provide training data, and as
        // input for prediction operations
        // - First 4 properties are inputs/features used to predict the label
        // - Label is what you are predicting, and is only set when training
        public class IrisData
        {
            [LoadColumn(0)]
            public float SepalLength;

            [LoadColumn(1)]
            public float SepalWidth;

            [LoadColumn(2)]
            public float PetalLength;

            [LoadColumn(3)]
            public float PetalWidth;

            [LoadColumn(4)]
            public string Label;
        }

        // IrisPrediction is the result returned from prediction operations
        public class IrisPrediction
        {
            [ColumnName("PredictedLabel")]
            public string PredictedLabels;
        }

        static void Main(string[] args)
        {
            // STEP 2: Create a ML.NET environment
            MLContext mlContext = new MLContext();

            // If working in Visual Studio, make sure the 'Copy to Output Directory'
            // property of iris-data.txt is set to 'Copy always'
            IDataView trainingDataView = mlContext.Data.LoadFromTextFile<IrisData>(path: "iris-data.txt", hasHeader: false, separatorChar: ',');

            // STEP 3: Transform your data and add a learner
            // Assign numeric values to text in the "Label" column, because only
            // numbers can be processed during model training.
            // Add a learning algorithm to the pipeline. e.g.(What type of iris is this?)
            // Convert the Label back into original text (after converting to number in step 3)
            var pipeline = mlContext.Transforms.Conversion.MapValueToKey("Label")
                .Append(mlContext.Transforms.Concatenate("Features", "SepalLength", "SepalWidth", "PetalLength", "PetalWidth"))
                .AppendCacheCheckpoint(mlContext)
                .Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy(labelColumnName: "Label", featureColumnName: "Features"))
                .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            // STEP 4: Train your model based on the data set
            var model = pipeline.Fit(trainingDataView);

            // STEP 5: Use your model to make a prediction
            // You can change these numbers to test different predictions
            var prediction = mlContext.Model.CreatePredictionEngine<IrisData, IrisPrediction>(model).Predict(
                new IrisData()
                {
                    SepalLength = 3.3f,
                    SepalWidth = 1.6f,
                    PetalLength = 0.2f,
                    PetalWidth = 5.1f,
                });

            Console.WriteLine($"Predicted flower type is: {prediction.PredictedLabels}");

            Console.WriteLine("Press any key to exit....");
            Console.ReadLine();
        }
    }
}'
----------------------------------------------------------------------------------------------------

### Run your app.

Command prompt.
'> dotnet run'
----------------------------------------------------------------------------------------------------

## Linux.

### Download and install.

**Ubuntu.**

Register Microsoft key and feed.
'$ wget -q https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb
$ sudo dpkg -i packages-microsoft-prod.deb'

Install the .NET SDK.
'$ sudo add-apt-repository universe
$ sudo apt-get install apt-transport-https
$ sudo apt-get update
$ sudo apt-get install dotnet-sdk-2.2'

Terminal.
'$ dotnet --info'
----------------------------------------------------------------------------------------------------

### Create your app

Terminal.
'$ dotnet new console -o myMLApp
$ cd myMLApp'
----------------------------------------------------------------------------------------------------

### Install ML.NET package.

Terminal.
'$ dotnet add package Microsoft.ML --version 1.0.0-preview'
----------------------------------------------------------------------------------------------------

### Download data set.

[Download iris.data.](https://archive.ics.uci.edu/ml/machine-learning-databases/iris/iris.data)
----------------------------------------------------------------------------------------------------

### Write program code.

Program.cs.
'using Microsoft.ML;
using Microsoft.ML.Data;
using System;

// CS0649 compiler warning is disabled because some fields are only
// assigned to dynamically by ML.NET at runtime
#pragma warning disable CS0649

namespace myMLApp
{
    class Program
    {
        // STEP 1: Define your data structures
        // IrisData is used to provide training data, and as
        // input for prediction operations
        // - First 4 properties are inputs/features used to predict the label
        // - Label is what you are predicting, and is only set when training
        public class IrisData
        {
            [LoadColumn(0)]
            public float SepalLength;

            [LoadColumn(1)]
            public float SepalWidth;

            [LoadColumn(2)]
            public float PetalLength;

            [LoadColumn(3)]
            public float PetalWidth;

            [LoadColumn(4)]
            public string Label;
        }

        // IrisPrediction is the result returned from prediction operations
        public class IrisPrediction
        {
            [ColumnName("PredictedLabel")]
            public string PredictedLabels;
        }

        static void Main(string[] args)
        {
            // STEP 2: Create a ML.NET environment
            MLContext mlContext = new MLContext();

            // If working in Visual Studio, make sure the 'Copy to Output Directory'
            // property of iris-data.txt is set to 'Copy always'
            IDataView trainingDataView = mlContext.Data.LoadFromTextFile<IrisData>(path: "iris-data.txt", hasHeader: false, separatorChar: ',');

            // STEP 3: Transform your data and add a learner
            // Assign numeric values to text in the "Label" column, because only
            // numbers can be processed during model training.
            // Add a learning algorithm to the pipeline. e.g.(What type of iris is this?)
            // Convert the Label back into original text (after converting to number in step 3)
            var pipeline = mlContext.Transforms.Conversion.MapValueToKey("Label")
                .Append(mlContext.Transforms.Concatenate("Features", "SepalLength", "SepalWidth", "PetalLength", "PetalWidth"))
                .AppendCacheCheckpoint(mlContext)
                .Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy(labelColumnName: "Label", featureColumnName: "Features"))
                .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            // STEP 4: Train your model based on the data set
            var model = pipeline.Fit(trainingDataView);

            // STEP 5: Use your model to make a prediction
            // You can change these numbers to test different predictions
            var prediction = mlContext.Model.CreatePredictionEngine<IrisData, IrisPrediction>(model).Predict(
                new IrisData()
                {
                    SepalLength = 3.3f,
                    SepalWidth = 1.6f,
                    PetalLength = 0.2f,
                    PetalWidth = 5.1f,
                });

            Console.WriteLine($"Predicted flower type is: {prediction.PredictedLabels}");

            Console.WriteLine("Press any key to exit....");
            Console.ReadLine();
        }
    }
}'
----------------------------------------------------------------------------------------------------

### Run your app.

Terminal.
'$ dotnet run'
----------------------------------------------------------------------------------------------------
