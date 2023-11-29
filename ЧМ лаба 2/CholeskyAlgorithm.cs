using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Task2_NM
{
	public class CholeskyAlgorithm
	{
		protected int N;

		protected int L;

		protected double[,]? A;

		protected double[,]? B;

		protected double[]? f;

		protected double[]? x;

		protected double[]? x_rand;

		protected double[]? y;

		public CholeskyAlgorithm(String filename)
		{
			StreamReader sr = new StreamReader(filename);
			string? readN = sr.ReadLine();
			N = Convert.ToInt32(readN);
			string? readL = sr.ReadLine();
			L = Convert.ToInt32(readL);
			A = new double[N, L];
			B = new double[N, L];
			f = new double[N];
			y = new double[N];
			x = new double[N];
			ReadFromFile(sr);
		}

		public CholeskyAlgorithm(int size, int L, double minValue, double maxValue) 
		{
			N = size;
			this.L = L;
			A = new double[N, L];
			B = new double[N, L];
			f = new double[N];
			y = new double[N];
			x = new double[N];
			x_rand = new double[N];
			CalculateRandomA(minValue, maxValue);
			CalculateRandomX(minValue, maxValue);
			Calculate_f();
		}
		public CholeskyAlgorithm(int size, int L)
		{
			N = size;
			this.L = L;
			A = new double[N, L];
			B = new double[N, L];
			f = new double[N];
			y = new double[N];
			x = new double[N];
			x_rand = new double[N];
			CalculateRandomA();
			CalculateRandomX();
			Calculate_f();
		}

		public CholeskyAlgorithm(int size, int L, int k, double minValue, double maxValue)
		{
			N = size;
			this.L = L;
			A = new double[N, L];
			B = new double[N, L];
			f = new double[N];
			y = new double[N];
			x = new double[N];
			x_rand = new double[N];
			CalculateRandomA(minValue, maxValue);
			//Print();
			multipleAonK(k);
			CalculateRandomX(minValue, maxValue);
			Calculate_f();
		}
		public void ReadBandedMatrixFromFile(StreamReader sr, double[,] matrix)
		{
			for (int i = 0; i < N; i++)
			{
				string line = sr.ReadLine();
				string[] text = line.Split(' ');
				for (int j = 0; j < L; j++)
				{
					matrix[i, j] = Convert.ToDouble(text[j]);
				}
			}
		}

		public void ReadArrayFromFile(StreamReader sr, double[] arr)
		{
			string line;
			if ((line = sr.ReadLine()) != null)
			{
				string[] text = line.Split(' ');
				for (int ind = 0; ind < text.Length; ++ind)
					arr[ind] = Convert.ToDouble(text[ind]);   
			}
			else
				Console.WriteLine("Can't read array from file");
		}

		public void ReadFromFile(StreamReader sr)
		{
			ReadBandedMatrixFromFile(sr, A);
			ReadArrayFromFile(sr, f);
		}

		//Функция k0(i)
		protected int GetColumnStartsIndex(int i)
		{
			int result;

			if (i <= L - 1)
				result = 0;
			else
				result = i - L + 1;

			return result;
		}

		//Функция kN(i)
		protected int GetColumnEndsIndex(int i)
		{
			int result;

			if (i <= N - L - 1)
				result = i + L - 1;
			else
				result = N - 1;

			return result;
		}
		protected double CalculateSumForElem_b(int rowNum, int colNum)
		{
			int start = GetColumnStartsIndex(rowNum);
			int end = colNum - 1;
			double sum = 0;
			if (end >= start)
			{
				for (var k = start; k <= end; ++k)
				{
					sum += (B[rowNum, k - rowNum + L - 1] * B[colNum, k - colNum + L - 1] / B[k, L - 1]);
				}
			}
			return sum;
		}

		protected double CalculateSumForElem_y(int rowNum)
		{
			int start = GetColumnStartsIndex(rowNum);
			int end = rowNum - 1;
			double sum = 0;
			if (end >= start)
			{
				for (var k = start; k <= end; ++k)
				{
					sum += B[rowNum, k - rowNum + L - 1] * y[k];
				}
			}
			return sum;
		}

		protected double CalculateSumForElem_x(int rowNum)
		{
			int start = rowNum + 1;
			int end = GetColumnEndsIndex(rowNum);
			double sum = 0;
			if (end >= start)
			{
				for (var k = start; k <= end; ++k)
				{
					sum += B[k, rowNum - k + L - 1] * x[k];
				}
			}
			return sum;
		}
		protected void CalculateMatrixB()
		{
			for (var colNum = 0; colNum < N; ++colNum)
			{
				int ind = GetColumnEndsIndex(colNum);
				for (var rowNum = colNum; rowNum <= ind; ++rowNum)
				{
					int mappedColNum = colNum - rowNum + L - 1;
					double sum = CalculateSumForElem_b(rowNum, colNum);
					B[rowNum, mappedColNum] = A[rowNum, mappedColNum] - sum;
				}
			}
		}
		protected void Calculate_y()
		{
			for (var rowNum = 0; rowNum < N; ++rowNum)
			{
				double sum = CalculateSumForElem_y(rowNum);
				y[rowNum] = (f[rowNum] - sum) / B[rowNum, L - 1];
			}
		}

		protected void Calculate_x()
		{
			for (var rowNum = N - 1; rowNum >= 0; --rowNum)
			{
				double sum = CalculateSumForElem_x(rowNum);  
				x[rowNum] = y[rowNum] - sum / B[rowNum, L - 1];
			}
		}

		public void Solution()
		{
			//Console.WriteLine("A: ");
			//PrintMatrix(A);
			//Console.WriteLine("f: ");
			//PrintArray(f);
			//Console.WriteLine("B: ");
			CalculateMatrixB();
			//PrintMatrix(B);
			Calculate_y();
			//Console.WriteLine("y: ");
			//PrintArray(y);
			Calculate_x();
			//Console.WriteLine("x: ");
			//PrintArray(x);
			//Console.WriteLine("x rand: ");
			//PrintArray(x_rand);
			//Console.WriteLine("Accuracy: " + CalculateAccuracy());
		}

		protected void PrintArray(double[] arr)
		{
			foreach (var item in arr) Console.Write("{0,7} ", string.Format("{0:F5}", item));
			Console.WriteLine();
		}
		protected void PrintMatrix(double[,] matrix)
		{
			int countRows = matrix.GetLength(0);
			int countCols = matrix.GetLength(1);

			for (int i = 0; i < countRows; i++)
			{
				for (int j = 0; j < countCols; j++)
					Console.Write("{0,7} ", string.Format("{0:F5}", matrix[i, j]));
				Console.WriteLine();
			}
		}

		public void Print()
		{
			Console.WriteLine("Matrix A: ");
			PrintMatrix(A);
			Console.WriteLine("----------------");
		}

		protected void CalculateRandomA(double minValue, double maxValue)
		{
			Random rnd = new Random();
			double d = maxValue - minValue;
			for (var colNum = 0; colNum < N; ++colNum)
			{
				int ind = GetColumnEndsIndex(colNum);
				for (var rowNum = colNum; rowNum <= ind; ++rowNum)
				{
					int mappedColNum = colNum - rowNum + L - 1;
					A[rowNum, mappedColNum] = minValue + rnd.NextDouble() * d;
				}
			}
		}

		protected void CalculateRandomA()
		{
			Random rnd = new Random();
			for (var colNum = 0; colNum < N; ++colNum)
			{
				int ind = GetColumnEndsIndex(colNum);
				for (var rowNum = colNum; rowNum <= ind; ++rowNum)
				{
					int mappedColNum = colNum - rowNum + L - 1;
					A[rowNum, mappedColNum] = rnd.NextDouble();
				}
			}
		}

		protected void CalculateRandomX(double minValue, double maxValue)
		{
			Random rnd = new Random();
			double d = maxValue - minValue;
			for (var index = 0; index < N; ++index)
			{
				x_rand[index] = minValue + rnd.NextDouble() * d;
			}
		}

		protected void CalculateRandomX()
		{
			Random rnd = new Random();
			for (var index = 0; index < N; ++index)
			{
				x_rand[index] = rnd.NextDouble();
			}
		}

		public void Calculate_f()
		{
			for (int i = 0; i < N; ++i)
			{
				f[i] = 0;
				for (int j = 0; j < N; ++j)
				{
					int column = Math.Abs(j - i);
					if (column <= L-1)
						f[i] += A[(j - i) >= 0 ? j : i, L - 1 - column] * x_rand[j];
				}
			}
		}

		public double CalculateAccuracy()
		{
			double max = 0.0;
			double current;
			double q = 1e-5;
			for (int index = 0; index < N; ++index)
			{
				if (Math.Abs(x_rand[index]) > q)
					current = Math.Abs((x[index] - x_rand[index]) / x_rand[index]);
				else
					current = Math.Abs(x[index] - x_rand[index]);
				if (current > max)
					max = current;
			}
			return max;
		}

		public void multipleAonK(int k)
		{
			for(int rowInd = 0; rowInd < N; ++rowInd)
			{
				A[rowInd, L-1] *= Math.Pow(10, -k);
			}
		}
	}
}
