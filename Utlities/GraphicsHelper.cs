using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace NeuralNetworks.Utilities
{
	static class GraphicsHelper
	{
		//static SolidBrush transparentBrush = new SolidBrush(Color.FromArgb(0, 0, 0, 0));

		public static void PutPixel(Graphics drawing, SolidBrush brush, float x, float y)
		{
			drawing.FillRectangle(brush, x, y, 1, 1);
		}

		public static void DrawCircle(Graphics drawing, Pen pen, float x, float y, float size)
		{
			drawing.DrawEllipse(pen, x, y, size, size);
		}

		public static void DrawBigPoint(Graphics drawing, Pen pen, SolidBrush fill, float x, float y, float size)
		{
			drawing.FillEllipse(fill, x, y, size, size);
			//DrawCircle(drawing, pen, x, y, size);
		}
	}
}
