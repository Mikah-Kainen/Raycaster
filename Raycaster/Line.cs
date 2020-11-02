namespace Raycaster
{

        public struct Line
        {
            public int X1;
            public int X2;
            public int Y1;
            public int Y2;

            public Line(int x1, int y1, int x2, int y2)
            {
                this.X1 = x1;
                this.X2 = x2;
                this.Y1 = y1;
                this.Y2 = y2;
            }
        }
}
