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

        public int returnCircle(int x)
        {
            return 

                //return equation of circle translated up to cursor mark
        }

        //https://en.wikipedia.org/wiki/Line%E2%80%93line_intersection
        private void timer1_Tick(object sender, EventArgs e)
        {
            gfx.Clear(Color.Black);

            // drawing and update happens here

            for(int i = 0; i < 10; i ++)
            {
                gfx.DrawLine(Pens.White, cursorLoc.X - 10, cursorLoc.Y - 10 + i, cursorLoc.X + 10, cursorLoc.Y + 10 + i);
            }

            // 0,0 is top left, x increases to the right, y increases towards the bottom
            this.Text = cursorLoc.ToString();   // set title of the form
            gfx.DrawLine(Pens.White, 0, 0, 100, 100);


            // this should be last line ideally to show the stuff we drew 
            pictureBox1.Image = canvas;

        }
    }
}
