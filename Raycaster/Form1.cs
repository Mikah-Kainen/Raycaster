using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Raycaster
{
    public partial class Form1 : Form
    {
        Graphics gfx;
        Bitmap canvas;

        Random random = new Random();

        public int cursorRadius = 20;
        Point cursorLoc => pictureBox1.PointToClient(MousePosition);

        public int[][] LinePoints = new int[4][];
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            canvas = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            LinePoints = DrawRandom();
            gfx = Graphics.FromImage(canvas);

        }

        public void DrawCircle(int x, int y, double degreeIncrimant, int radius)
        {
            for (int i = 0; i < (360/degreeIncrimant); i++)
            {
                gfx.DrawLine(Pens.White, x, y, radius * (float)Math.Cos(degreeIncrimant * i * Math.PI/180) + x, radius * (float)Math.Sin(degreeIncrimant * i * Math.PI/180) + y);

                //gfx.DrawLine(Pens.White, x, y, i + y, returnCircle(i, radius) + y);
                //gfx.DrawLine(Pens.White, x, y, i, -returnCircle(i, radius));
            }
            //return equation of circle translated up to cursor mark
        }

        public void DrawRays(int x, int y, int degreeIncrimant, int radius)
        {
            int endX;
            int endY;
            int[] currentLine;
            for (int i = 0; i < (360 / degreeIncrimant); i++)
            {
                endX = radius * (int)Math.Cos(degreeIncrimant * i * Math.PI / 180) + x;
                endY = radius * (int)Math.Sin(degreeIncrimant * i * Math.PI / 180) + y;
                currentLine = new int[4] {x, y, endX, endY};
                for(int z = 0; z < LinePoints.Length + 1; z ++)
                {
                    if(z == LinePoints.Length)
                    {
                        gfx.DrawLine(Pens.White, x, y, endX, endY);
                    }
                    else if(isT(LinePoints[z], currentLine) && isU(LinePoints[z], currentLine))
                    {
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        public bool isT(int[] lineSegment, int[] rayLine)
        {
            double top;
            double bottom;
            top = (double)((lineSegment[0] - rayLine[0])*(rayLine[1] - rayLine[3]) - (lineSegment[1] - rayLine[1])*(rayLine[0] - rayLine[2]));
            bottom = (double)((lineSegment[0] - lineSegment[2])*(rayLine[1] - rayLine[3]) - (lineSegment[1] - lineSegment[3])*(rayLine[0] - rayLine[2]));
            if(bottom == 0)
            {
                return false;
            }
            double T = top / bottom;
            if(T >= 0 && T <= 1)
            {
                return true;
            }
            return false;
        }

        public bool isU(int[] lineSegment, int[] rayLine)
        {
            double top;
            double bottom;
            top = (double)((lineSegment[0] - lineSegment[2])*(lineSegment[1] - rayLine[1]) - (lineSegment[1] - lineSegment[3])*(lineSegment[0] - lineSegment[2]));
            bottom = (double)((lineSegment[0] - lineSegment[2])*(rayLine[1] - rayLine[3]) - (lineSegment[1] - lineSegment[3])*(rayLine[0] - rayLine[2]));
            if(bottom == 0)
            {
                return false;
            }
            double U = (-1)*(top / bottom);
            if(U >= 0 && U <= 1)
            {
                return true;
            }
            return false;
        }

        public int[][] DrawRandom()
        {
            int[][] returnArray = new int[4][];
            for (int i = 0; i < 4; i++)
            {
                for (int x = 0; x < 4; x++)
                {
                    returnArray[i] = new int[4];
                }
            }

            for (int i = 0; i < 4; i++)
            {
                for (int x = 0; x < 2; x++)
                {
                    returnArray[i][2 * x] = random.Next(0, 801);
                }
                for (int y = 0; y < 2; y++)
                {
                    returnArray[i][2 * y + 1] = random.Next(0, 451);
                }
            }
            return returnArray;
        }

        public int returnCircle(int x, int radius)
        {
            return (int)Math.Sqrt(radius * radius - x * x);
        }

        //https://en.wikipedia.org/wiki/Line%E2%80%93line_intersection
        private void timer1_Tick(object sender, EventArgs e)
        {
            gfx.Clear(Color.Black);
            // drawing and update happens here

            DrawCircle(cursorLoc.X, cursorLoc.Y, 3, cursorRadius);

            // 0,0 is top left, x increases to the right, y increases towards the bottom
            this.Text = cursorLoc.ToString();   // set title of the form

            for(int i = 0; i < 4; i ++)
            {
                gfx.DrawLine(Pens.White, LinePoints[i][0], LinePoints[i][1], LinePoints[i][2], LinePoints[i][3]);
            }

            DrawRays(cursorLoc.X, cursorLoc.Y, 10, 50);
            // this should be last line ideally to show the stuff we drew 
            pictureBox1.Image = canvas;
        }
    }
}
