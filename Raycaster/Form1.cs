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
        static Bitmap canvas;

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

        public void DrawRays(int x, int y, double radianIncrimant, int radius)
        {
            float endX;
            float endY;
            Line currentLine;
            bool didntIntersect;
            Point intersectionPoint = new Point();
            Point[] intersectionPoints = new Point[LinePoints.Length];
            int leastDistance = int.MaxValue;
            int distance;
            for (int i = 0; i < (int)((double)(2*Math.PI) / radianIncrimant); i++)
            {
                didntIntersect = true;
                endX = radius * (float)Math.Cos(radianIncrimant*(double)i) + x;
                endY = radius * (float)Math.Sin(radianIncrimant*(double)i) + y;
                currentLine = new Line(x, y, (int)endX, (int)endY);
                
                for(int z = 0; z < LinePoints.Length; z ++)
                {
                    if(Intersect(new Line(LinePoints[z][0], LinePoints[z][1], LinePoints[z][2], LinePoints[z][3]), currentLine, out intersectionPoint))
                    {
                        intersectionPoints[z] = intersectionPoint;
                        didntIntersect = false;
                    }
                }
                if (didntIntersect)
                {
                    gfx.DrawLine(Pens.White, x, y, endX, endY);
                }
                else
                {
                    foreach(Point point in intersectionPoints)
                    {
                        if (point != default)
                        {
                            distance = (int)(Math.Sqrt(Math.Abs((point.X - x) * (point.X - x) + (point.Y - y) * (point.Y - y))));
                            if (distance < leastDistance)
                            {
                                leastDistance = distance;
                                intersectionPoint = point;
                            }
                        }
                    }
                    if(intersectionPoint != default)
                    {
                        gfx.DrawLine(Pens.White, x, y, intersectionPoint.X, intersectionPoint.Y);
                    }
                }
                //else
                //{
                //    gfx.DrawLine(Pens.White, x, y, intersectionPoint.X, intersectionPoint.Y);
                //}
            }
        }

        private bool LineIntersection(Line ray, Line barrier, out Point intersectionPoint)
        {
            intersectionPoint = default;
            int den = ((ray.X1 - ray.X2) * (barrier.Y1 - barrier.Y2)) - ((ray.Y1 - ray.Y2) * (barrier.X1 - barrier.X2));
            float t = ((ray.X1 - barrier.X1) * (barrier.Y1 - barrier.Y2)) - ((ray.Y1 - barrier.Y1) * (barrier.X1 - barrier.X2));
            float u = ((ray.X1 - ray.X2) * (ray.Y1 - barrier.Y1)) - ((ray.Y1 - ray.Y2) * (ray.X1 - barrier.X1));

            if (den == 0)
            {
                return false;
            }

            t /= den;
            u /= -den;

            if (t >= 0 && t <= 1 && u >= 0 && u <= 1)
            {
                intersectionPoint.X = (int)(ray.X1 + (t * (ray.X2 - ray.X1)));
                intersectionPoint.Y = (int)(ray.Y1 + (t * (ray.Y2 - ray.Y1)));
                return true;
            }
            return false;
        }

        public bool Intersect(Line line1, Line line2, out Point intersectionPoint)
        {
            intersectionPoint = default;
            double denom = (double)((line1.X1 - line1.X2)*(line2.Y1  - line2.Y2) - (line1.Y1 - line1.Y2)*(line2.X1 - line2.X2));
            
            
            if (denom == 0)
            {
                return false;
            }

            double t = (double)((line1.X1 - line2.X1)*(line2.Y1 - line2.Y2) - (line1.Y1 - line2.Y1)*(line2.X1 - line2.X2));
            t = t / denom;

            double u = (double)((line1.X1-line1.X2)*(line1.Y1 - line2.Y1) - (line1.Y1 - line1.Y2)*(line1.X1 - line2.X1));
            u = (double)(-1 * u / denom);

            if (t >= 0 && t <= 1 && u >= 0 && u <= 1)
            {
                // definite intersection
                // (Px,Py)= (x1 + t(x2-x1)), y1 + t(y2 - y1)

                intersectionPoint = new Point((int)(line1.X1 + t*(line1.X2 - line1.X1)), (int)(line1.Y1 + t*(line1.Y2 - line1.Y1)));
                return true;
            }

            return false;
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
            
            int[] temp = new int[2];
            for (int i = 0; i < 4; i++)
            {
                for (int x = 0; x < 2; x++)
                {
                    temp[x] = random.Next(0, 801);
                }
                if(temp[0] <= temp[1])
                {
                    returnArray[i][0] = temp[0];
                    returnArray[i][2] = temp[1]
;               }
                else
                {
                    returnArray[i][0] = temp[1];
                    returnArray[i][2] = temp[0];
                }
                for (int y = 0; y < 2; y++)
                {
                    temp[y] = random.Next(0, 451);
                }
                if(temp[0] <= temp[1])
                {
                    returnArray[i][1] = temp[0];
                    returnArray[i][3] = temp[1];
;               }
                else
                {
                    returnArray[i][1] = temp[1];
                    returnArray[i][3] = temp[0];
                }
            }
            return returnArray;
        }

        public void DrawBorder()
        {
            gfx.DrawLine(Pens.White, 0, 0, canvas.Width, 0);
            gfx.DrawLine(Pens.White, canvas.Width, 0, canvas.Width, canvas.Height);
            gfx.DrawLine(Pens.White, canvas.Width, canvas.Height, 0, canvas.Height);
            gfx.DrawLine(Pens.White, 0, canvas.Height, 0, 0);
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

            for (int i = 0; i < 4; i++)
            {
                gfx.DrawLine(Pens.White, LinePoints[i][0], LinePoints[i][1], LinePoints[i][2], LinePoints[i][3]);
            }

            DrawBorder();
            DrawRays(cursorLoc.X, cursorLoc.Y, .25, 1000);
            // this should be last line ideally to show the stuff we drew 
            pictureBox1.Image = canvas;
        }
    }
}
