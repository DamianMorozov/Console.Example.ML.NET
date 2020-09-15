using Microsoft.ML.Data;

namespace NetCoreMLApp
{
    public class HouseData
    {
        public float Size { get; set; }
        public float Price { get; set; }
    }

    public class HousePrediction
    {
        [ColumnName("Score")] public float Price { get; set; }
    }
}
