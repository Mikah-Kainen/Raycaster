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

        Point cursorLoc => pictureBox1.PointToClient(MousePosition);
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            canvas = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            gfx = Graphics.FromImage(canvas);
        }

        public void DrawCircle(int x, int y, int radius)
        {
            for(int i = 0; i < 120; i ++)
            {
                gfx.DrawLine(Pens.White, x, y, radius*(float)Math.Cos(3*i) + x, radius*(float)Math.Sin(3*i) + y);

                //gfx.DrawLine(Pens.White, x, y, i + y, returnCircle(i, radius) + y);
                //gfx.DrawLine(Pens.White, x, y, i, -returnCircle(i, radius));
            }
            //return equation of circle translated up to cursor mark
        }

        public List<List<int>> DrawRandom()
        {
            Random random = new Random();
            //int[][] returnArray = new int[4][4];
            List<List<int>> returnArray = new List<List<int>>();
            for(int i = 0; i < 4; i ++)
            {
                returnArray.Add(new List<int>());
                for(int x = 0; x < 4; x ++)
                {
                    returnArray[i].Add(0);
                }
            }

            for(int i = 0; i < 4; i ++)
            {
                for(int x = 0; x < 2; x ++)
                {
                    returnArray[i][2*x] = random.Next(0, 801);
                }
                for(int y = 0; y < 2; y ++)
                {
                    returnArray[i][2*y + 1] = random.Next(0, 451);
                }
                gfx.DrawLine(Pens.White, returnArray[i][0], returnArray[i][1], returnArray[i][2], returnArray[i][3]);
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

            DrawCircle(cursorLoc.X, cursorLoc.Y, 20);

            // 0,0 is top left, x increases to the right, y increases towards the bottom
            this.Text = cursorLoc.ToString();   // set title of the form
            gfx.DrawLine(Pens.White, 0, 0, 100, 100);

            DrawRandom();
            // this should be last line ideally to show the stuff we drew 
            pictureBox1.Image = canvas;
        }
    }
}
