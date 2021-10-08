using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.NeuralNetwork
{
	class TrainingPoint
	{
		public float X { get; }
		public float Y { get; }
		public float Bias { get; } = 1;
		public int Label { get; }

		public TrainingPoint(float x, float y, int label)
		{
			X = x;
			Y = y;
			Label = label;
			Bias = 1;
		}
	}
}
