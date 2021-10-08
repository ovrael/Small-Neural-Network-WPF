using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Utilities
{
	static class Utility
	{
		private static Random random = new Random();

		// The perceptron activation function
		public static int Sign(float number)
		{
			if (number >= 0)
				return 1;
			else
				return -1;
		}

		// Helper method for randoming float between min and max
		public static float RandomFloat(float min, float max)
		{
			return (float)random.NextDouble() * (max - min) + min;
		}
		public static int SpecialRandomInt()
		{
			if ((random.Next(101) + 1) < 50)
				return 0;
			else
				return 1;
		}

		public static float LinearFunction(float a, float x, float b)
		{
			return a * x + b;
		}
	}
}
