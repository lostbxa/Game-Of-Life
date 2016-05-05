using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game_Of_Life
{
    public partial class Form1 : Form
    {
        bool[,] universe = new bool[20,20];
        bool[,] scratchpad = new bool[20,20];
        int[,] neighbors = new int[20, 20];
        int sizeX = 20;
        int sizeY = 20;
        int size = 20;
        int gen = 0;
        bool on = true;
        public Form1()
        {
            InitializeComponent();
            timer1.Interval = 100;
            timer1.Enabled = true;
            timer1.Start();
        }
        private void graphicsPanel1_Paint(object sender, PaintEventArgs e)
        {
            Pen p3n = new Pen(Color.Black);
            float width = graphicsPanel1.Width / universe.GetLength(0);
            float height = graphicsPanel1.Height / universe.GetLength(1);
            RectangleF[] rekts = new RectangleF[1];
            for (int x = 0; x < universe.GetLength(0); x++)
            {
                for (int y = 0; y < universe.GetLength(1); y++)
                {
                    if (universe[x,y] == true)
                    {
                        RectangleF rekt = new RectangleF(new PointF(x * width, y * height), new SizeF(width, height));
                        e.Graphics.FillRectangle(Brushes.Gray, rekt);
                    }
                    else
                    {
                        RectangleF rekt = new RectangleF(new PointF(x * width, y * height), new SizeF(width, height));
                        rekts[0] = rekt;
                        e.Graphics.DrawRectangles(p3n, rekts);
                    }
                }
            }

        }

        private void graphicsPanel1_MouseClick(object sender, MouseEventArgs e)
        {

            int width = graphicsPanel1.Width / universe.GetLength(0);
            int height = graphicsPanel1.Height / universe.GetLength(1);
            try
            {
                if (universe[e.X / width, e.Y / height] == true)
                {
                    universe[e.X / width, e.Y / height] = false;
                }
                else
                {
                    universe[e.X / width, e.Y / height] = true;
                }
                graphicsPanel1.Invalidate();
            }
            catch (IndexOutOfRangeException)
            {

                
            }
            
        }

        private void newToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            for (int x = 0; x < universe.GetLength(0); x++)
            {
                for (int y = 0; y < universe.GetLength(1); y++)
                {

                        universe[x, y] = false;

                }
            }
            graphicsPanel1.Invalidate();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            gen++;
            generation.Text = "Generations: " + gen.ToString();
            ProcessGeneration();
        }
        void GenerationChange()
        {
            scratchpad = universe;

            for (int x = 0; x < universe.GetLength(0); x++)
            {
                for (int y = 0; y < universe.GetLength(1); y++)
                {
                    if (scratchpad[x, y] == true)
                    {

                    }
                }
            }
            graphicsPanel1.Invalidate();          
        }
        private static int chekNeigburs(bool[,] universe, int size, int x, int y, int offsetx, int offsety)
        {
            int result = 0;

            int OffsetX = x + offsetx;
            int OffsetY = y + offsety;
            bool outOfBounds = OffsetX < 0 || OffsetX >= size | OffsetY < 0 || OffsetY >= size;
            if (!outOfBounds)
            {
                result = universe[x + offsetx, y + offsety] ? 1 : 0;
            }
            return result;
        }
        private void ProcessGeneration()
        {

            for (int a = 0; a < sizeX; a++)
            {
                for (int i = 0; i < sizeY; i++)
                {
                    scratchpad[a, i] = universe[a, i];
                }
            }
            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    int neighburs = chekNeigburs(universe, 20, x, y, -1, 0 + chekNeigburs(universe, 20, x, y, -1, 1) + chekNeigburs(universe, 20, x, y, 0, 1)  + chekNeigburs(universe, 20, x, y, 1, 1) + chekNeigburs(universe, 20, x, y, 1, 0)  + chekNeigburs(universe, 20, x, y, 1, -1)  + chekNeigburs(universe, 20, x, y, 0, -1) + chekNeigburs(universe, 20, x, y, -1, -1));
                    bool shouldLive = false;
                    bool isAlive = universe[x, y];

                    if (isAlive && (neighburs == 2 || neighburs == 3))
                    {
                        shouldLive = true;
                    }
                    else if (!isAlive && neighburs == 3) // zombification
                    {
                        shouldLive = true;
                    }

                    scratchpad[x, y] = shouldLive;
                }
            }
            for (int a = 0; a < 20; a++)
            {
                for (int i = 0; i < 20; i++)
                {
                     universe[a, i] =scratchpad[a, i];
                }
            }

            graphicsPanel1.Invalidate();
        }
        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void generations_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void cutToolStripButton_Click(object sender, EventArgs e)
        {
            if (on)
            {
                timer1.Stop();
                on = false;
            }

            else if (!on)
            {
                timer1.Start();
                on = true;
            }
        }
    }
}
