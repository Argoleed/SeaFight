using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFight
{
    public class Sector
    {
        private char L { get; set; }
        private char N { get; set; }
        public int status { get; set; }
        public Sector(char L, char N) { this.L = L; this.N = N; status = 0; }

        public override string ToString()
        {
            if (N < (int)'1' + 9)
                return L.ToString() + N.ToString();
            else
                return L.ToString() + "10";
        }

    }

    public class Sea
    {
        Random R = new Random();
        private Sector[,] grid;
        public Sector this[int x, int y]
        {
            get
            {
                return grid[x, y];
            }
            set
            {
                grid[x, y] = value;
            }
        }

        public Sea()
        {
            grid = new Sector[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    grid[i, j] = new Sector((char)((int)'A' + i),
                                            (char)((int)'1' + j));
                }
            }
        }

        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (this[i, j].status == 0)
                        result += "~";
                    else result += (this[i, j].status - 1).ToString();
                    result += " ";
                }
                result += "\n";
            }
            return result;
        }

        public bool Fire(int x, int y)
        {
            this[x, y].status += 100;
            if (this[x, y].status > 100) return true;
            return false;
        }

        public bool isDeath(int x, int y)
        {
            for (int i = 0; i < 10; i++) for (int j = 0; j < 10; j++) {
                if (this[i, j].status == this[x, y].status - 100)
                {
                    return false;
                }
            }
            return true;
        }

        public bool isWin()
        {
            for (int i = 0; i < 10; i++) for (int j = 0; j < 10; j++)
                {
                    if (this[i, j].status > 0 && this[i, j].status < 100)
                    {
                        return false;
                    }
                }
            return true;
        }

        public bool oneShip()
        {
            int nship = 0;
            for (int i = 0; i < 10; i++) for (int j = 0; j < 10; j++)
            {
                if (this[i,j].status > 0 && this[i, j].status < 100 && this[i, j].status != nship)
                    {
                        if (nship == 0) nship = this[i, j].status;
                        else return false;
                    }
            }
            if (nship == 0) return false;
            return true;
        }

        private bool aroundIsEmpty(int x, int y)
        {
            if ((x > 0 && this[x - 1, y].status != 0) ||
                (x < 9 && this[x + 1, y].status != 0) ||
                (y > 0 && this[x, y - 1].status != 0) ||
                (y < 9 && this[x, y + 1].status != 0) ||
                (x > 0 && y > 0 && this[x - 1, y - 1].status != 0) ||
                (x < 9 && y < 9 && this[x + 1, y + 1].status != 0) ||
                (x > 0 && y < 9 && this[x - 1, y + 1].status != 0) ||
                (x < 9 && y > 0 && this[x + 1, y - 1].status != 0))
                return false;
            return true;
        }

        private bool isPartOfDouble(int x, int y)
        {
            if ((x > 0 && y > 0 && this[x - 1, y - 1].status != 0) ||
                (x < 9 && y < 9 && this[x + 1, y + 1].status != 0) ||
                (x > 0 && y < 9 && this[x - 1, y + 1].status != 0) ||
                (x < 9 && y > 0 && this[x + 1, y - 1].status != 0))
                return false;

            if ((x > 0 && this[x - 1, y].status == this[x, y].status) &&
                (x == 9 || this[x + 1, y].status == 0) &&
                (y == 0 || this[x, y - 1].status == 0) &&
                (y == 9 || this[x, y + 1].status == 0))
                return true;

            if ((x == 0 || this[x - 1, y].status == 0) &&
                (x < 9 && this[x + 1, y].status == this[x, y].status) &&
                (y == 0 || this[x, y - 1].status == 0) &&
                (y == 9 || this[x, y + 1].status == 0))
                return true;

            if ((x == 0 || this[x - 1, y].status == 0) &&
                (x == 9 || this[x + 1, y].status == 0) &&
                (y > 0 && this[x, y - 1].status == this[x, y].status) &&
                (y == 9 || this[x, y + 1].status == 0))
                return true;

            if ((x == 0 || this[x - 1, y].status == 0) &&
                (x == 9 || this[x + 1, y].status == 0) &&
                (y == 0 || this[x, y - 1].status == 0) &&
                (y < 9 && this[x, y + 1].status == this[x, y].status))
                return true;

            return false;
        }

        private bool isPartOfTriple(int x, int y)
        {
            if ((x > 0 && y > 0 && this[x - 1, y - 1].status != 0) ||
                (x < 9 && y < 9 && this[x + 1, y + 1].status != 0) ||
                (x > 0 && y < 9 && this[x - 1, y + 1].status != 0) ||
                (x < 9 && y > 0 && this[x + 1, y - 1].status != 0))
                return false;

            if ((x == 0 || this[x - 1, y].status == 0) &&
                (x == 9 || this[x + 1, y].status == 0) &&
                (y > 0 && this[x, y - 1].status == this[x, y].status) &&
                (y < 9 && this[x, y + 1].status == this[x, y].status))
                return true;

            if ((x > 0 && this[x - 1, y].status == this[x, y].status) &&
                (x < 9 && this[x + 1, y].status == this[x, y].status) &&
                (y == 0 || this[x, y - 1].status == 0) &&
                (y == 9 || this[x, y + 1].status == 0))
                return true;

            return false;
        }

        public bool checkPositions()
        {
            for (int ship_n = 1; ship_n <= 4; ship_n++)
            {
                bool shipIs = false;
                for (int i = 0; i < 10; i++) for (int j = 0; j < 10; j++)
                        if (this[i, j].status == ship_n)
                        {
                            if (!shipIs) shipIs = true;
                            else return false;
                            if (!aroundIsEmpty(i, j)) return false;
                        }
                if (!shipIs) return false;
            }

            for (int ship_n = 5; ship_n <= 7; ship_n++)
            {
                int parts_counter = 0;
                for (int i = 0; i < 10; i++) for (int j = 0; j < 10; j++)
                        if (this[i, j].status == ship_n)
                        {
                            parts_counter++;
                            if (!isPartOfDouble(i, j)) return false;
                        }
                if (parts_counter != 2) return false;
            }

            for (int ship_n = 8; ship_n <= 9; ship_n++)
            {
                int double_parts = 0, triple_parts = 0;
                for (int i = 0; i < 10; i++) for (int j = 0; j < 10; j++)
                        if (this[i, j].status == ship_n)
                        {
                            if (isPartOfTriple(i, j)) triple_parts++;
                            else if (isPartOfDouble(i, j)) double_parts++;
                            else return false;
                        }
                if (!(triple_parts == 1 && double_parts == 2)) return false;
            }

            {
                int double_parts = 0, triple_parts = 0;
                for (int i = 0; i < 10; i++) for (int j = 0; j < 10; j++)
                        if (this[i, j].status == 10)
                        {
                            if (isPartOfTriple(i, j)) triple_parts++;
                            else if (isPartOfDouble(i, j)) double_parts++;
                            else return false;
                        }
                if (!(triple_parts == 2 && double_parts == 2)) return false;
            }

            return true;
        }

        public void Generate(Random R)
        {
            for (int x = 0; x < 10; x++) for (int y = 0; y < 10; y++) this[x, y].status = 0;
            while (!this.checkPositions())
            {
                for (int x = 0; x < 10; x++) for (int y = 0; y < 10; y++) this[x, y].status = 0;

                for (int ship_n = 1; ship_n <= 4; ship_n++)
                {
                    for (int k = 0; k < 200; k++)
                    {
                        int x = R.Next(10), y = R.Next(10);
                        if (aroundIsEmpty(x, y))
                        {
                            this[x, y].status = ship_n;
                            break;
                        }
                    }
                }

                for (int ship_n = 5; ship_n <= 7; ship_n++)
                {
                    for (int k = 0; k < 200; k++)
                    {
                        int x = R.Next(10), y = R.Next(10), queue = R.Next(2);
                        if (aroundIsEmpty(x, y))
                        {
                            if (queue == 0)
                            {
                                if (x < 9 && aroundIsEmpty(x + 1, y))
                                {
                                    this[x, y].status = ship_n;
                                    this[x + 1, y].status = ship_n;
                                    break;
                                }
                                if (x > 0 && aroundIsEmpty(x - 1, y))
                                {
                                    this[x, y].status = ship_n;
                                    this[x - 1, y].status = ship_n;
                                    break;
                                }
                                if (y < 9 && aroundIsEmpty(x, y + 1))
                                {
                                    this[x, y].status = ship_n;
                                    this[x, y + 1].status = ship_n;
                                    break;
                                }
                                if (y > 0 && aroundIsEmpty(x, y - 1))
                                {
                                    this[x, y].status = ship_n;
                                    this[x, y - 1].status = ship_n;
                                    break;
                                }
                            }
                            else
                            {
                                if (y < 9 && aroundIsEmpty(x, y + 1))
                                {
                                    this[x, y].status = ship_n;
                                    this[x, y + 1].status = ship_n;
                                    break;
                                }
                                if (y > 0 && aroundIsEmpty(x, y - 1))
                                {
                                    this[x, y].status = ship_n;
                                    this[x, y - 1].status = ship_n;
                                    break;
                                }
                                if (x < 9 && aroundIsEmpty(x + 1, y))
                                {
                                    this[x, y].status = ship_n;
                                    this[x + 1, y].status = ship_n;
                                    break;
                                }
                                if (x > 0 && aroundIsEmpty(x - 1, y))
                                {
                                    this[x, y].status = ship_n;
                                    this[x - 1, y].status = ship_n;
                                    break;
                                }
                            }
                        }
                    }
                }


                for (int ship_n = 8; ship_n <= 9; ship_n++)
                {
                    for (int k = 0; k < 200; k++)
                    {
                        int x = R.Next(10), y = R.Next(10), queue = R.Next(2); ;
                        if (aroundIsEmpty(x, y))
                        {
                            if (queue == 0)
                            {
                                if (x < 8 && aroundIsEmpty(x + 1, y) && aroundIsEmpty(x + 2, y))
                                {
                                    this[x, y].status = ship_n;
                                    this[x + 1, y].status = ship_n;
                                    this[x + 2, y].status = ship_n;
                                    break;
                                }
                                if (x > 1 && aroundIsEmpty(x - 1, y) && aroundIsEmpty(x - 2, y))
                                {
                                    this[x, y].status = ship_n;
                                    this[x - 1, y].status = ship_n;
                                    this[x - 2, y].status = ship_n;
                                    break;
                                }
                                if (y < 8 && aroundIsEmpty(x, y + 1) && aroundIsEmpty(x, y + 2))
                                {
                                    this[x, y].status = ship_n;
                                    this[x, y + 1].status = ship_n;
                                    this[x, y + 2].status = ship_n;
                                    break;
                                }
                                if (y > 1 && aroundIsEmpty(x, y - 1) && aroundIsEmpty(x, y - 2))
                                {
                                    this[x, y].status = ship_n;
                                    this[x, y - 1].status = ship_n;
                                    this[x, y - 2].status = ship_n;
                                    break;
                                }
                            }
                            else
                            {
                                if (y < 8 && aroundIsEmpty(x, y + 1) && aroundIsEmpty(x, y + 2))
                                {
                                    this[x, y].status = ship_n;
                                    this[x, y + 1].status = ship_n;
                                    this[x, y + 2].status = ship_n;
                                    break;
                                }
                                if (y > 1 && aroundIsEmpty(x, y - 1) && aroundIsEmpty(x, y - 2))
                                {
                                    this[x, y].status = ship_n;
                                    this[x, y - 1].status = ship_n;
                                    this[x, y - 2].status = ship_n;
                                    break;
                                }
                                if (x < 8 && aroundIsEmpty(x + 1, y) && aroundIsEmpty(x + 2, y))
                                {
                                    this[x, y].status = ship_n;
                                    this[x + 1, y].status = ship_n;
                                    this[x + 2, y].status = ship_n;
                                    break;
                                }
                                if (x > 1 && aroundIsEmpty(x - 1, y) && aroundIsEmpty(x - 2, y))
                                {
                                    this[x, y].status = ship_n;
                                    this[x - 1, y].status = ship_n;
                                    this[x - 2, y].status = ship_n;
                                    break;
                                }
                            }
                        }
                    }
                }


                {
                    int ship_n = 10;
                    for (int k = 0; k < 200; k++)
                    {
                        int x = R.Next(10), y = R.Next(10), queue = R.Next(2);
                        if (aroundIsEmpty(x, y))
                        {
                            if (queue == 0)
                            {
                                if (x < 7 && aroundIsEmpty(x + 1, y) && aroundIsEmpty(x + 2, y) && aroundIsEmpty(x + 3, y))
                                {
                                    this[x, y].status = ship_n;
                                    this[x + 1, y].status = ship_n;
                                    this[x + 2, y].status = ship_n;
                                    this[x + 3, y].status = ship_n;
                                    break;
                                }
                                if (x > 2 && aroundIsEmpty(x - 1, y) && aroundIsEmpty(x - 2, y) && aroundIsEmpty(x - 3, y))
                                {
                                    this[x, y].status = ship_n;
                                    this[x - 1, y].status = ship_n;
                                    this[x - 2, y].status = ship_n;
                                    this[x - 3, y].status = ship_n;
                                    break;
                                }
                                if (y < 7 && aroundIsEmpty(x, y + 1) && aroundIsEmpty(x, y + 2) && aroundIsEmpty(x, y + 3))
                                {
                                    this[x, y].status = ship_n;
                                    this[x, y + 1].status = ship_n;
                                    this[x, y + 2].status = ship_n;
                                    this[x, y + 3].status = ship_n;
                                    break;
                                }
                                if (y > 2 && aroundIsEmpty(x, y - 1) && aroundIsEmpty(x, y - 2) && aroundIsEmpty(x, y - 3))
                                {
                                    this[x, y].status = ship_n;
                                    this[x, y - 1].status = ship_n;
                                    this[x, y - 2].status = ship_n;
                                    this[x, y - 3].status = ship_n;
                                    break;
                                }
                            }
                            else
                            {
                                if (y < 7 && aroundIsEmpty(x, y + 1) && aroundIsEmpty(x, y + 2) && aroundIsEmpty(x, y + 3))
                                {
                                    this[x, y].status = ship_n;
                                    this[x, y + 1].status = ship_n;
                                    this[x, y + 2].status = ship_n;
                                    this[x, y + 3].status = ship_n;
                                    break;
                                }
                                if (y > 2 && aroundIsEmpty(x, y - 1) && aroundIsEmpty(x, y - 2) && aroundIsEmpty(x, y - 3))
                                {
                                    this[x, y].status = ship_n;
                                    this[x, y - 1].status = ship_n;
                                    this[x, y - 2].status = ship_n;
                                    this[x, y - 3].status = ship_n;
                                    break;
                                }
                                if (x < 7 && aroundIsEmpty(x + 1, y) && aroundIsEmpty(x + 2, y) && aroundIsEmpty(x + 3, y))
                                {
                                    this[x, y].status = ship_n;
                                    this[x + 1, y].status = ship_n;
                                    this[x + 2, y].status = ship_n;
                                    this[x + 3, y].status = ship_n;
                                    break;
                                }
                                if (x > 2 && aroundIsEmpty(x - 1, y) && aroundIsEmpty(x - 2, y) && aroundIsEmpty(x - 3, y))
                                {
                                    this[x, y].status = ship_n;
                                    this[x - 1, y].status = ship_n;
                                    this[x - 2, y].status = ship_n;
                                    this[x - 3, y].status = ship_n;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public int statusCount(int status)
        {
            int count = 0;
            for (int i = 0; i < 10; i++) 
                for (int j = 0; j < 10; j++)
                {
                    if (this[i, j].status == status) count++;
                }
            return count;
        }

        public int aroundCount(int x, int y)
        {
            int count = 0;
            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (this[i, j].status < 100 && this[i, j].status > 0) count++;
                }
            }
            return count;
        }


        public Point lowAIstep()
        {
            while (true)
            {
                int x = R.Next(0, 10), y = R.Next(0, 10);
                if (this[x, y].status < 100) return new Point(x, y);
            }
        }

        public Point mediumAIstep()
        {
            foreach (int i in createRandomOrder(0, 10)) foreach (int j in createRandomOrder(0, 10))
            {
                if (this[i,j].status > 100)
                {
                    if (!((i == 0 || this[i - 1, j].status >= 100) &&
                       (i == 9 || this[i + 1, j].status >= 100) &&
                       (j == 0 || this[i, j - 1].status >= 100) &&
                       (j == 9 || this[i, j + 1].status >= 100)))
                    {
                            if (i != 0 && i != 9 && this[i - 1, j].status > 100 && this[i + 1, j].status < 100)
                                return new Point(i + 1, j);
                            if (i != 0 && i != 9 && this[i + 1, j].status > 100 && this[i - 1, j].status < 100)
                                return new Point(i - 1, j);
                            if (j != 0 && j != 9 && this[i, j - 1].status > 100 && this[i, j + 1].status < 100)
                                return new Point(i, j + 1);
                            if (j != 0 && j != 9 && this[i, j + 1].status > 100 && this[i, j - 1].status < 100)
                                return new Point(i, j - 1);

                            if (statusCount(this[i, j].status) > 1) continue;

                            while (true)
                            {
                            int rand = R.Next(0, 4);
                            switch (rand)
                            {
                                case 0:
                                    if (i != 0 && this[i - 1, j].status < 100) return new Point(i - 1, j);
                                    break;
                                 case 1:
                                    if (i != 9 && this[i + 1, j].status < 100) return new Point(i + 1, j);
                                    break;
                                 case 2:
                                    if (j != 0 && this[i, j - 1].status < 100) return new Point(i, j - 1);
                                    break;
                                 case 3:
                                    if (j != 9 && this[i, j + 1].status < 100) return new Point(i, j + 1);
                                    break;
                            }
                        }
                    }
                }
            }
            while (true)
            {
                int x = R.Next(0, 10), y = R.Next(0, 10);
                if (this[x, y].status < 100) return new Point(x, y);
            }
        }

        
        public Point highAIstep()
        {
            foreach (int i in createRandomOrder(0, 10)) foreach (int j in createRandomOrder(0, 10))
            {
                if (this[i, j].status > 100)
                {
                    if (!((i == 0 || this[i - 1, j].status >= 100) &&
                        (i == 9 || this[i + 1, j].status >= 100) &&
                        (j == 0 || this[i, j - 1].status >= 100) &&
                        (j == 9 || this[i, j + 1].status >= 100)))
                    {
                        if (i != 0 && i != 9 && this[i - 1, j].status > 100 && this[i + 1, j].status < 100)
                            return new Point(i + 1, j);
                        if (i != 0 && i != 9 && this[i + 1, j].status > 100 && this[i - 1, j].status < 100)
                            return new Point(i - 1, j);
                        if (j != 0 && j != 9 && this[i, j - 1].status > 100 && this[i, j + 1].status < 100)
                            return new Point(i, j + 1);
                        if (j != 0 && j != 9 && this[i, j + 1].status > 100 && this[i, j - 1].status < 100)
                            return new Point(i, j - 1);

                        if (statusCount(this[i, j].status) > 1) continue;

                        while (true)
                        {
                            int rand = R.Next(0, 4);
                            switch (rand)
                            {
                                case 0:
                                    if (i != 0 && this[i - 1, j].status < 100) return new Point(i - 1, j);
                                    break;
                                case 1:
                                    if (i != 9 && this[i + 1, j].status < 100) return new Point(i + 1, j);
                                    break;
                                case 2:
                                    if (j != 0 && this[i, j - 1].status < 100) return new Point(i, j - 1);
                                    break;
                                case 3:
                                    if (j != 9 && this[i, j + 1].status < 100) return new Point(i, j + 1);
                                    break;
                            }
                        }
                    }
                }
            }

            int max_count = 0;
            Point around = new Point(0, 0);
            foreach (int i in createRandomOrder(1, 9)) foreach (int j in createRandomOrder(1, 9))
            {
                int this_count = aroundCount(i, j);
                if (this_count > max_count)
                {
                    max_count = this_count;
                    around = new Point(i, j);
                }
            }

            while (true)
            {
                Point result = new Point(around.X + R.Next(-1, 2), around.Y + R.Next(-1, 2));
                if (this[result.X, result.Y].status < 100) return result;
            }
        }
        
        public int[] createRandomOrder(int min, int max)
        {
            int[] order = new int[max - min];

            for (int i = 0; i < max - min; i++)
            {
                bool repeat = false;
                do
                {
                    repeat = false;
                    order[i] = R.Next(min, max);
                    for (int j = 0; j < i; j++) if (order[i] == order[j])
                    {
                        repeat = true;
                        break;
                    }
                } while (repeat == true);
            }
            return order;
        }

    }
}
