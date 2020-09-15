using Microsoft.ML;
using System;
using NetCoreMLAppML.Model;

// CS0649 compiler warning is disabled because some fields are only
// assigned to dynamically by ML.NET at runtime
#pragma warning disable CS0649

namespace NetCoreMLApp
{
    internal class Program
    {
        private static void Main()
        {
            var numberMenu = -1;
            while (numberMenu != 0)
            {
                PrintCaption();
                try
                {
                    numberMenu = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error: " + exception.Message);
                    numberMenu = -1;
                }
                Console.WriteLine();
                PrintSwitch(numberMenu);
            }
        }

        private static void PrintCaption()
        {
            Console.Clear();
            Console.WriteLine(@"----------------------------------------------------------------------");
            Console.WriteLine(@"---                        Examples ML.NET                         ---");
            Console.WriteLine(@"----------------------------------------------------------------------");
            Console.WriteLine("0. Exit from console.");
            Console.WriteLine("1. Iris data from file.");
            Console.WriteLine("2. Iris data from ModelBuilder.");
            Console.WriteLine("3. House data from array.");
            Console.WriteLine(@"----------------------------------------------------------------------");
            Console.Write("Type switch: ");
        }

        private static void PrintSwitch(int numberMenu)
        {
            Console.Clear();
            var isPrintMenu = false;
            switch (numberMenu)
            {
                case 1:
                    isPrintMenu = true;
                    PrintIrisDataFromFile();
                    break;
                case 2:
                    isPrintMenu = true;
                    PrintIrisDataFromModelBuilder();
                    break;
                case 3:
                    isPrintMenu = true;
                    PrintHouseDataFromArray();
                    break;
            }
            if (isPrintMenu)
            {
                Console.WriteLine(@"----------------------------------------------------------------------");
                Console.Write("Type any key to return in main menu.");
                Console.ReadKey();
            }
        }

        static void PrintIrisDataFromFile()
        {
            Console.WriteLine(@"----------------------------------------------------------------------");
            Console.WriteLine(@"---                    1. Iris data from file.                     ---");
            Console.WriteLine(@"----------------------------------------------------------------------");
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
                .Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features"))
                .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            // STEP 4: Train your model based on the data set
            var model = pipeline.Fit(trainingDataView);

            // STEP 5: Use your model to make a prediction
            // You can change these numbers to test different predictions
            var irisData = new IrisData()
            {
                SepalLength = 3.3f,
                SepalWidth = 1.6f,
                PetalLength = 0.2f,
                PetalWidth = 5.1f,
            };
            var prediction = mlContext.Model.CreatePredictionEngine<IrisData, IrisPrediction>(model).Predict(irisData);

            Console.WriteLine($"Flower data: {irisData.SepalLength}, {irisData.SepalWidth}, {irisData.PetalLength}, {irisData.PetalWidth}.");
            Console.WriteLine($"Predicted flower type is: {prediction.PredictedLabels}");
        }

        private static void PrintIrisDataFromModelBuilder()
        {
            Console.WriteLine(@"----------------------------------------------------------------------");
            Console.WriteLine(@"---                2. Iris data from ModelBuilder.                 ---");
            Console.WriteLine(@"----------------------------------------------------------------------");
            // Create single instance of sample data from first line of dataset for model input
            ModelInput sampleData = new ModelInput()
            {
                Col0 = 5.1F,
                Col1 = 3.5F,
                Col2 = 1.4F,
                Col3 = 0.2F,
            };

            // Make a single prediction on the sample data and print results
            var predictionResult = ConsumeModel.Predict(sampleData);

            Console.WriteLine("Using model to make single prediction -- Comparing actual Col4 with predicted Col4 from sample data...");
            Console.WriteLine($"Col0: {sampleData.Col0}");
            Console.WriteLine($"Col1: {sampleData.Col1}");
            Console.WriteLine($"Col2: {sampleData.Col2}");
            Console.WriteLine($"Col3: {sampleData.Col3}");
            Console.WriteLine($"Predicted Col4 value {predictionResult.Prediction}");
            Console.WriteLine($"Predicted Col4 scores: [{string.Join(",", predictionResult.Score)}]");
        }

        private static void PrintHouseDataFromArray()
        {
            Console.WriteLine(@"----------------------------------------------------------------------");
            Console.WriteLine(@"---                   3. House data from array.                    ---");
            Console.WriteLine(@"----------------------------------------------------------------------");
            MLContext mlContext = new MLContext();

            // 1. Import or create training data.
            HouseData[] houseData = {
                new HouseData() { Size = 1.1F, Price = 1.2F },
                new HouseData() { Size = 1.9F, Price = 2.3F },
                new HouseData() { Size = 2.8F, Price = 3.0F },
                new HouseData() { Size = 3.4F, Price = 3.7F } };
            IDataView trainingData = mlContext.Data.LoadFromEnumerable(houseData);

            // 2. Specify data preparation and model training pipeline.
            var pipeline = mlContext.Transforms.Concatenate("Features", new[] { "Size" })
                .Append(mlContext.Regression.Trainers.Sdca(labelColumnName: "Price", maximumNumberOfIterations: 100));

            // 3. Train model.
            var model = pipeline.Fit(trainingData);

            // 4. Make a prediction.
            var size = new HouseData() { Size = 2.5F };
            var price = mlContext.Model.CreatePredictionEngine<HouseData, HousePrediction>(model).Predict(size);

            Console.WriteLine($"Predicted price for size: {size.Size * 1000} sq ft= {price.Price * 100:C}k");
        }
    }
}
