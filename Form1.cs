using NeuralNetworks.NeuralNetwork;
using NeuralNetworks.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace NeuralNetworks
{
	public partial class Form1 : Form
	{
		Thread graphThread;         // Thread that uses Draw method and draws objects
		Graphics drawing;           // Graphics object for drawing
		Graphics background;        // Graphics object for background
		Bitmap backgroundBitmap;    // Bitmap used as clearing canvas
		bool isDrawing = true;
		int sameValue = 0;
		// Pens
		Pen myPen = new Pen(Color.Gray, 1f);
		Pen bluePen = new Pen(Color.Blue, 3f);
		Pen darkGrayPen = new Pen(Color.DarkGray, 1f);
		Pen whitePen = new Pen(Color.White, 2f);
		Pen outlinePen = new Pen(Color.SlateGray, 1f);

		// Brushes
		SolidBrush redBrush = new SolidBrush(Color.Red);
		SolidBrush greenBrush = new SolidBrush(Color.Green);
		SolidBrush whiteBrush = new SolidBrush(Color.White);
		SolidBrush grayBrush = new SolidBrush(Color.Gray);
		SolidBrush blackBrush = new SolidBrush(Color.Black);
		SolidBrush blueBrush = new SolidBrush(Color.Blue);
		SolidBrush fillBrush = new SolidBrush(Color.White);
		SolidBrush noFillBrush = new SolidBrush(Color.FromArgb(0, 0, 0, 0));
		//SolidBrush myBrush = new SolidBrush(Color.Blue);
		//SolidBrush myBrush = new SolidBrush(Color.Blue);

		// Neural Network
		const int pointsLength = 5000;
		Perceptron perceptron = new Perceptron(3, 0.001f);
		TrainingPoint[] points = new TrainingPoint[pointsLength];

		// Window size
		static int width = 1000;
		static int height = 1000;

		// Data
		static int cycleCounter = 0;
		static float[] oldWeights = new float[3];

		static float a = -1.2f;
		static float b = 230;

		static float minX = -width / 2;
		static float minY = -height / 2;
		static float maxX = width / 2;
		static float maxY = height / 2;

		static float startY = Utility.LinearFunction(a, minX, b);
		static float endY = Utility.LinearFunction(a, maxX, b);

		PointF startPoint = new PointF(minX, startY);
		PointF endPoint = new PointF(maxX, endY);

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			// Custom options
			Console.WriteLine("-=-=- Window's size -=-=-");
			AskForSize();
			Console.WriteLine("\n-=-=- Linear function -=-=-");
			AskForLine();
			Console.WriteLine("\n-=-=- Points -=-=-");
			AskForPoints();
			Console.WriteLine("\nPress any button to start learning...");
			Console.ReadKey();
			Console.Clear();


			// Disable resize window
			FormBorderStyle = FormBorderStyle.FixedSingle;

			// Setup size
			ClientSize = new Size(width, height);

			// Setup graphics
			backgroundBitmap = new Bitmap(width, height);
			drawing = Graphics.FromImage(backgroundBitmap);
			background = CreateGraphics();

			// Transform to cartisian plane
			//drawing.TranslateTransform(0, -ClientRectangle.Height);
			//drawing.ScaleTransform(Width, Height);
			drawing.TranslateTransform(width / 2, height / 2);
			drawing.ScaleTransform(1.0f, -1.0f);
			drawing.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			drawing.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;


			// Neural Network
			for (int i = 0; i < points.Length; i++)
			{
				float x = Utility.RandomFloat(minX, maxX);
				float y = Utility.RandomFloat(minY, maxY);
				int label = y < Utility.LinearFunction(a, x, b) ? -1 : 1;
				points[i] = new TrainingPoint(x, y, label);
			}

			for (int i = 0; i < oldWeights.Length; i++)
			{
				oldWeights[i] = perceptron.Weights[i];
			}

			// Run Draw
			graphThread = new Thread(Draw);
			graphThread.IsBackground = true;
			graphThread.Start();
		}
		public void AskForSize()
		{
			int minW = 200;
			int minH = 200;

			Console.Write($"Provide window's width (min = {minW}): ");
			string readWidth = Console.ReadLine();
			Console.Write($"Provide window's height (min = {minH}): ");
			string readHeight = Console.ReadLine();

			if (int.TryParse(readWidth, out int iWidth) && int.TryParse(readHeight, out int iHeight))
			{
				if (iWidth >= minW && iHeight >= minH)
				{
					width = iWidth;
					height = iHeight;
					minX = -width / 2;
					minY = -height / 2;
					maxX = width / 2;
					maxY = height / 2;
				}
			}
			Console.WriteLine($"Window's size: {width} x {height}");
		}
		public void AskForLine()
		{
			Console.Write("Provide 'a' for linear function: ");
			string readA = Console.ReadLine();
			Console.Write("Provide 'b' for linear function: ");
			string readB = Console.ReadLine();


			if (float.TryParse(readA, out float iA) && float.TryParse(readB, out float iB))
			{
				a = iA;
				b = iB;

				startY = Utility.LinearFunction(a, minX, b);
				endY = Utility.LinearFunction(a, maxX, b);

				startPoint = new PointF(minX, startY);
				endPoint = new PointF(maxX, endY);
			}

			char sign = '+';
			int iSign = 1;
			if (b < 0)
			{
				sign = '-';
				iSign = -1;
			}

			Console.WriteLine($"Function: y = {a} * x {sign} {iSign * b}");

		}
		public void AskForPoints()
		{
			Console.Write("Provide number of points: ");
			string readPoints = Console.ReadLine();


			if (int.TryParse(readPoints, out int iPoints))
			{
				points = new TrainingPoint[iPoints];
			}
			Console.WriteLine($"Points: {points.Length}");
		}

		public void Learning(int index)
		{
			float[] inputs = { points[index].X, points[index].Y, points[index].Bias };
			int target = points[index].Label;
			perceptron.Train(inputs, target);

			int guess = perceptron.FeedForward(inputs);
			if (guess > 0)
				fillBrush = greenBrush;
			else
			{
				fillBrush = redBrush;
			}
		}

		public void DrawPoints(float size)
		{
			for (int i = 0; i < points.Length; i++)
			{
				Learning(i);
				GraphicsHelper.DrawBigPoint(drawing, outlinePen, fillBrush, points[i].X, points[i].Y, size);
			}
		}

		public void ConsoleMessage()
		{
			Console.SetCursorPosition(7, 0);
			Console.WriteLine(cycleCounter);

			float calcA = -perceptron.Weights[0] / perceptron.Weights[1];
			float calcB = -perceptron.Weights[2] / perceptron.Weights[1];

			char sign = '+';
			int iSign = 1;
			if (calcB < 0)
			{
				sign = '-';
				iSign = -1;
			}

			string calculatedLine = string.Format("Calculated line function: y = {0,-6:0.0000} * x {1} {2,-6:0.0000}", calcA, sign, iSign * calcB);
			Console.Write(calculatedLine);
		}

		public void EndMessage()
		{
			float calcA = -perceptron.Weights[0] / perceptron.Weights[1];
			float calcB = -perceptron.Weights[2] / perceptron.Weights[1];

			char cSign = '+';
			int cISign = 1;
			if (b < 0)
			{
				cSign = '-';
				cISign = -1;
			}

			char fSign = '+';
			int fISign = 1;
			if (calcB < 0)
			{
				fSign = '-';
				fISign = -1;
			}

			Console.Clear();
			Console.WriteLine($"Finished learning in {cycleCounter} cycles.");

			string givenLine = string.Format("Program was looking for:  y = {0,-6:0.0000} * x {1} {2,-6:0.0000}", a, cSign, cISign * b);
			Console.WriteLine(givenLine);

			string calculatedLine = string.Format("Calculated line function: y = {0,-6:0.0000} * x {1} {2,-6:0.0000}", calcA, fSign, fISign * calcB);
			Console.WriteLine(calculatedLine);
		}

		public void Draw()
		{
			// Prevents graphics artifacts
			background.Clear(Color.Black);
			Console.Write("Cycle: ");

			while (isDrawing)
			{
				ConsoleMessage();

				sameValue = 0;
				drawing.Clear(Color.Black);
				DrawPoints(10);
				try
				{
					drawing.DrawLine(whitePen, startPoint, endPoint);
					drawing.DrawLine(bluePen, minX, perceptron.GuessY(minX), maxX, perceptron.GuessY(maxX));

				}
				catch (OverflowException OE)
				{
				}

				background.DrawImage(backgroundBitmap, new PointF(0, 0));

				cycleCounter++;

				for (int i = 0; i < oldWeights.Length; i++)
				{
					if (oldWeights[i] == perceptron.Weights[i])
						sameValue++;
				}

				if (sameValue == 3)
				{
					EndMessage();
					isDrawing = false;
				}

				for (int i = 0; i < oldWeights.Length; i++)
				{
					oldWeights[i] = perceptron.Weights[i];
				}

				//await Task.Delay(25);
			}
		}
	}
}
