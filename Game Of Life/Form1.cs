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
using System.IO;
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

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamReader reader = new StreamReader(dlg.FileName);

                // Create a couple variables to calculate the width and height
                // of the data in the file.
                int maxWidth = 0;
                int maxHeight = 0;

                // Iterate through the file once to get its size.
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();

                    // If the row begins with '!' then it is a comment
                    // and should be ignored.
                    if(row.Contains("!"))
                    {

                    }

                    // If the row is not a comment then it is a row of cells.
                    // Increment the maxHeight variable for each row read.
                    else
                    {
                        maxHeight++;
                    }
                    // Get the length of the current row string
                    // and adjust the maxWidth variable if necessary.
                    maxWidth = row.Length;
                }

                // Resize the current universe and scratchPad
                // to the width and height of the file calculated above.
                sizeX = maxWidth;
                sizeY = maxHeight;
                universe = new bool[sizeX, sizeY];
                scratchpad = new bool[sizeX, sizeY];
                // Reset the file pointer back to the beginning of the file.
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                int ypos = 0;
                // Iterate through the file again, this time reading in the cells.
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();
                    
                    // If the row begins with '!' then
                    // it is a comment and should be ignored.
                    if (row.Contains("!"))
                    {

                    }
                    // If the row is not a comment then 
                    // it is a row of cells and needs to be iterated through.
                    else
                    {
                        for (int xPos = 0; xPos < row.Length; xPos++)
                        {
                            // If row[xPos] is a 'O' (capital O) then
                            // set the corresponding cell in the universe to alive.
                            if (row[xPos].Equals('O'))
                            {
                                universe[xPos, ypos] = true;
                            }
                            // If row[xPos] is a '.' (period) then
                            // set the corresponding cell in the universe to dead.
                            else if (row[xPos].Equals('.'))
                            {
                                universe[xPos, ypos] = false;
                            }
                        }
                        ypos++;
                    }
                    
                }

                // Close the file.
                reader.Close();
                graphicsPanel1.Invalidate();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2; dlg.DefaultExt = "cells";


            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamWriter writer = new StreamWriter(dlg.FileName);

                // Write any comments you want to include first.
                // Prefix all comment strings with an exclamation point.
                // Use WriteLine to write the strings to the file. 
                // It appends a CRLF for you.
                writer.WriteLine("!This is my comment.");

                // Iterate through the universe one row at a time.
                for (int y = 0; y < sizeY; y++)
     {
                    // Create a string to represent the current row.
                    String currentRow = string.Empty;

                    // Iterate through the current row one cell at a time.
                    for (int x = 0; x < sizeX; x++)
          {
                        // If the universe[x,y] is alive then append 'O' (capital O)
                        // to the row string.
                        if (universe[x, y] == true)
                        {
                            currentRow = currentRow + "O";
                        }
                        // Else if the universe[x,y] is dead then append '.' (period)
                        // to the row string.
                        else
                        {
                            if (universe[x, y] == false)
                            {
                                currentRow = currentRow + ".";
                            }
                        }
                    }

                    // Once the current row has been read through and the 
                    // string constructed then write it to the file using WriteLine.
                    writer.WriteLine(currentRow);
                }

                // After all rows and columns have been written then close the file.
                writer.Close();
            }
        }
    }
}
