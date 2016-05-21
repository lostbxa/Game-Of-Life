using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Game_Of_Life.Properties;
namespace Game_Of_Life
{
    public partial class Form1 : Form
    {
        //Create the necessary basics for the universe
        bool[,] universe = new bool[Settings.Default.X, Settings.Default.Y];
        bool[,] scratchpad = new bool[Settings.Default.X, Settings.Default.Y];
        int sizeX = Settings.Default.X;
        int sizeY = Settings.Default.Y;
        int liveCount = 0;
        Pen p3n = new Pen(Color.Black);
        SolidBrush b = new SolidBrush(Color.Gray);
        int gen = 0;
        bool on = true;
        public Form1()
        {
            InitializeComponent();
            timer1.Interval = 20;
            timer1.Enabled = true;
            timer1.Stop();
        }
        private void graphicsPanel1_Paint(object sender, PaintEventArgs e)
        {

            liveCount = 0;
            float width = graphicsPanel1.Width / universe.GetLength(0);
            float height = graphicsPanel1.Height / universe.GetLength(1);
            RectangleF[] rekts = new RectangleF[1];
            //iterate through the list and draw rectangles according to their current status
            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    if (universe[x,y] == true)
                    {
                        RectangleF rekt = new RectangleF(new PointF(x * width, y * height), new SizeF(width, height));
                        e.Graphics.FillRectangle(b, rekt);
                        liveCount++;
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

            int width = graphicsPanel1.Width / sizeX;
            int height = graphicsPanel1.Height /sizeY;
            try
            {
                //set the 
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
            liveCells.Text = "Live Cells: " + liveCount;
            GenerationChange();
            graphicsPanel1.Invalidate();
        }
        private static int chekNeigburs(bool[,] universe, int size, int x, int y, int offsetx, int offsety)
        {
            int result = 0;

            int OffsetX = x + offsetx;
            int OffsetY = y + offsety;
            bool outOfBounds = OffsetX < 0 || OffsetX >= universe.GetLength(0) | OffsetY < 0 || OffsetY >= universe.GetLength(1);
            if (!outOfBounds)
            {
                result = universe[x + offsetx, y + offsety] ? 1 : 0;
            }
            return result;
        }
        private void GenerationChange()
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
                    int neighburs = chekNeigburs(universe, sizeX , x, y, -1, 0) + chekNeigburs(universe, sizeX , x, y, -1, 1) + chekNeigburs(universe, sizeX , x, y, 0, 1)  + chekNeigburs(universe, sizeX , x, y, 1, 1) + chekNeigburs(universe, sizeX, x, y, 1, 0)  + chekNeigburs(universe, sizeX , x, y, 1, -1)  + chekNeigburs(universe, sizeX , x, y, 0, -1) + chekNeigburs(universe, sizeX, x, y, -1, -1);
                    bool shouldLive = false;
                    bool isAlive = universe[x, y];

                    if (isAlive && (neighburs == 2 || neighburs == 3))
                    {
                        shouldLive = true;
                    }
                    else if (!isAlive && neighburs == 3) 
                    {
                        shouldLive = true;
                    }

                    scratchpad[x, y] = shouldLive;
                }
            }
            for (int a = 0; a < sizeX; a++)
            {
                for (int i = 0; i < sizeY; i++)
                {
                     universe[a, i] =scratchpad[a, i];
                }
            }
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

        private void changeSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Size_Change chng = new Size_Change();
            if (chng.ShowDialog() == DialogResult.OK)
            {

                sizeX = chng.X;
                sizeY = chng.Y;
                universe = new bool[sizeX, sizeY];
                scratchpad = new bool[sizeX, sizeY];
                graphicsPanel1.Invalidate();

            }
        }

        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
            gen++;
            generation.Text = "Generations: " + gen.ToString();
            GenerationChange();
            graphicsPanel1.Invalidate();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            graphicsPanel1.Refresh();
        }


        private void Form1_FormClosed_1(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.X = sizeX;
            Properties.Settings.Default.Y = sizeY;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.bgc = graphicsPanel1.BackColor;
        }

        private void changeColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();

            dlg.Color = graphicsPanel1.BackColor;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                graphicsPanel1.BackColor = dlg.Color;
            }

        }

        private void changeLiveColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();

            dlg.Color = graphicsPanel1.BackColor;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                b.Color = dlg.Color;
            }

        }

        private void changeLineColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();

            dlg.Color = graphicsPanel1.BackColor;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                p3n.Color = dlg.Color;
            }
        }

        private void customizeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
