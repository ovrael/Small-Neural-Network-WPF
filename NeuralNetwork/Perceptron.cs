using NeuralNetworks.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.NeuralNetwork
{

	class Perceptron
	{
		public float[] Weights { get; }
		float learningRate;

		public void ShowWeights()
		{
			if (Weights != null && Weights.Length > 1)
			{

				Console.WriteLine("0: " + Weights[0] + "\t 1: " + Weights[1] + "\t 2: " + Weights[2]);
			}
		}

		// Constructor
		public Perceptron(int weightsNumber, float learningRate)
		{
			Weights = new float[weightsNumber];
			for (int i = 0; i < Weights.Length; i++)
			{
				Weights[i] = Utility.SpecialRandomInt();
			}

			this.learningRate = learningRate;
		}

		public int FeedForward(float[] inputs)
		{
			if (inputs.Length != Weights.Length)
				throw new Exception("Inputs length is different than weights length.");

			float sum = 0;

			for (int i = 0; i < Weights.Length; i++)
			{
				sum += inputs[i] * Weights[i];
			}

			// The activation
			int result = Utility.Sign(sum);

			return result;
		}

		public void Train(float[] inputs, int target)
		{
			int guess = FeedForward(inputs);
			int error = target - guess;

			// Tune all the weights
			for (int i = 0; i < Weights.Length; i++)
			{
				Weights[i] += error * inputs[i] * learningRate;
			}
		}

		public float GuessY(float x)
		{
			return -(Weights[2] / Weights[1]) - (Weights[0] / Weights[1]) * x;
		}

	}
}
