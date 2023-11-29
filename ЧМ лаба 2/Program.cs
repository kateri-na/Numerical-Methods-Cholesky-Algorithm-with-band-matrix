namespace Task2_NM
{
	internal class Program
	{
		static void Main(string[] args)
		{
			const int CountTests = 10;

			var testCases1 = new (int size, int L, double minValue, double maxValue)[]
			{
					(40, 4, -20, 20),
					(40, 10, -20, 20),
					(400, 38, -40, 40),
					(400, 90, -40, 40)
			};
			var testCases2 = new (int size, int L)[]
			{
					(25, 25),
					(50, 50),
					(100, 100),
					(500, 500)
			};
			var testCases3 = new (int size, int L, int k, double minValue, double maxValue)[]
			{
				(20, 20/10, 2, -35.5, 35.5),
				(20, 20/10, 4, -35.5, 35.5),
				(20, 20/10, 6, -35.5, 35.5),
				(40, 40/10, 2, -88.7, 88.7),
				(40, 40/10, 4, -88.7, 88.7),
				(40, 40/10, 6, -88.7, 88.7)
			};

			Console.WriteLine("1.Данные о решении систем уравнений с отношением L/N:");
			Console.WriteLine();
			foreach (var (size, L, minValue, maxValue) in testCases1)
			{
				double unitAccuracy = 0;
				for (int i = 0; i < CountTests; ++i)
				{
					CholeskyAlgorithm matrix = new CholeskyAlgorithm(size, L, minValue, maxValue);
					//Console.WriteLine("Random generated data:");
					//matrix.Print();
					matrix.Solution();
					unitAccuracy += matrix.CalculateAccuracy();
				}
				unitAccuracy /= CountTests;
				Console.WriteLine($"size: {size}; L: {L}; minValue: {minValue}; maxValue: {maxValue};");
				Console.WriteLine($"Среднее значение оценки точности: {unitAccuracy}");
			}
			Console.WriteLine();
			Console.WriteLine("2.Данные о решении систем уравнений с хорошо обусловленными матрицами:");
			Console.WriteLine();
			foreach (var (size, L) in testCases2)
			{
				double unitAccuracy = 0;
				for (int i = 0; i < CountTests; ++i)
				{
					CholeskyAlgorithm matrix = new CholeskyAlgorithm(size, L);
					//Console.WriteLine("Random generated data:");
					//matrix.Print();
					matrix.Solution();
					unitAccuracy += matrix.CalculateAccuracy();
				}
				unitAccuracy /= CountTests;
				Console.WriteLine($"size: {size}; L: {L};");
				Console.WriteLine($"Среднее значение оценки точности: {unitAccuracy}");
			}
			Console.WriteLine();
			Console.WriteLine("3.Данные о решении систем уравнений с плохо обусловленными матрицами:");
			Console.WriteLine();
			foreach (var (size, L, k, minValue, maxValue) in testCases3)
			{
				double unitAccuracy = 0;
				for (int i = 0; i < CountTests; ++i)
				{
					CholeskyAlgorithm matrix = new CholeskyAlgorithm(size, L, k, minValue, maxValue);
					matrix.Solution();
					unitAccuracy += matrix.CalculateAccuracy();
				}
				unitAccuracy /= CountTests;
				Console.WriteLine($"size: {size}; L: {L}; k: {k}; minValue: {minValue}; maxValue: {maxValue};");
				Console.WriteLine($"Среднее значение оценки точности: {unitAccuracy}");
			}
			Console.ReadLine();
		}
	}
}