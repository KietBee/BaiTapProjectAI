using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace mecung
{
    public partial class Form1 : Form
    {
        private const int Rows = 9;
        private const int Columns = 32;
        private int cellSize;
        private int BorderSize = 20; // Kích thước của hộp bao quanh mê cung

        private int[,] maze;
        private Panel[,] mazeCells;

        private Point startCell;
        private Point endCell;
        private Point playerPosition;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Kích thước ô và mê cung được thiết lập cố định trong các hằng số
            cellSize = Math.Min((pbMaze.Width - BorderSize) / Columns, (pbMaze.Height - BorderSize) / Rows);

            // Tạo mê cung cố định
            // 0: ô đường đi, 1: ô tường
            int[,] fixedMaze = new int[Rows, Columns] {
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1},
            {1, 0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1},
            {1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1},
            {1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 0, 1, 0, 1},
            {1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1},
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
        };

            maze = fixedMaze;

            // Vẽ mê cung lên Panel
            DrawMaze();

            // Đánh dấu điểm bắt đầu và kết thúc
            DrawStartAndEnd();

            // Tìm đường đi từ điểm bắt đầu đến điểm kết thúc bằng thuật toán A*
            List<Point> path = AStarSearch(startCell, endCell);

            // Di chuyển người chơi trên đường đi
            MovePlayer(path);
        }

        private void GenerateMaze()
        {
            // Khởi tạo mê cung với các ô tường cản và đường đi ban đầu
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    maze[i, j] = 1; // Tạo tường ban đầu
                }
            }

            // Sinh mê cung bằng thuật toán Recursive Backtracking
            RecursiveBacktracking(1, 1);

            // Thêm tường bao quanh
            for (int i = 0; i < Rows; i++)
            {
                maze[i, 0] = 1; // Tường bên trái
                maze[i, Columns - 1] = 1; // Tường bên phải
            }

            for (int j = 0; j < Columns; j++)
            {
                maze[0, j] = 1; // Tường trên cùng
                maze[Rows - 1, j] = 1; // Tường dưới cùng
            }
        }

        private void RecursiveBacktracking(int x, int y)
        {
            maze[x, y] = 0; // Đánh dấu ô hiện tại là ô đường đi
            List<int> directions = new List<int> { 1, 2, 3, 4 };
            Shuffle(directions); // Xáo trộn thứ tự các hướng

            foreach (int direction in directions)
            {
                int nextX = x;
                int nextY = y;

                switch (direction)
                {
                    case 1: // Hướng bên trái
                        nextX -= 2;
                        break;
                    case 2: // Hướng bên phải
                        nextX += 2;
                        break;
                    case 3: // Hướng lên trên
                        nextY -= 2;
                        break;
                    case 4: // Hướng xuống dưới
                        nextY += 2;
                        break;
                }

                if (nextX > 0 && nextX < Rows - 1 && nextY > 0 && nextY < Columns - 1 && maze[nextX, nextY] == 1)
                {
                    int wallX = x + (nextX - x) / 2;
                    int wallY = y + (nextY - y) / 2;
                    maze[wallX, wallY] = 0; // Đánh dấu ô trung gian là ô đường đi
                    RecursiveBacktracking(nextX, nextY);
                }
            }
        }

        private void Shuffle<T>(IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        private void DrawMaze()
        {
            // Vẽ mê cung lên PictureBox
            pbMaze.Controls.Clear();
            mazeCells = new Panel[Rows, Columns];

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Panel cell = new Panel
                    {
                        Size = new Size(cellSize, cellSize),
                        Location = new Point(j * cellSize + BorderSize / 2, i * cellSize + BorderSize / 2),
                        BackColor = maze[i, j] == 1 ? Color.DarkSlateBlue : Color.White
                    };

                    pbMaze.Controls.Add(cell);
                    mazeCells[i, j] = cell;
                }
            }
        }

        private void DrawStartAndEnd()
        {
            // Đánh dấu điểm bắt đầu và kết thúc
            startCell = new Point(1, 1);
            endCell = new Point(Rows - 2, Columns - 2);

            // Điểm bắt đầu (ô màu đỏ)
            mazeCells[startCell.X, startCell.Y].BackColor = Color.Red;

            // Điểm kết thúc (ô màu xanh lá cây)
            mazeCells[endCell.X, endCell.Y].BackColor = Color.LimeGreen;
        }

        private List<Point> AStarSearch(Point start, Point end)
        {
            // Triển khai thuật toán A* để tìm đường đi từ điểm start đến điểm end trong mê cung

            // Hàm heuristic (manhattan distance)
            int Heuristic(Point p)
            {
                return Math.Abs(end.X - p.X) + Math.Abs(end.Y - p.Y);
            }

            // Hàm tìm các ô lân cận có thể di chuyển
            List<Point> GetNeighbors(Point p)
            {
                List<Point> neighbors = new List<Point>
                {
                    new Point(p.X - 1, p.Y),
                    new Point(p.X + 1, p.Y),
                    new Point(p.X, p.Y - 1),
                    new Point(p.X, p.Y + 1)
                };

                // Loại bỏ các ô không hợp lệ (nằm ngoài mê cung hoặc là tường)
                neighbors.RemoveAll(n => n.X < 0 || n.X >= Rows || n.Y < 0 || n.Y >= Columns || maze[n.X, n.Y] == 1);

                return neighbors;
            }

            // Khởi tạo danh sách open và close
            List<Point> open = new List<Point>();
            Dictionary<Point, Point> cameFrom = new Dictionary<Point, Point>();
            Dictionary<Point, int> gScore = new Dictionary<Point, int>();
            gScore[start] = 0;

            // Bắt đầu thuật toán A*
            open.Add(start);

            while (open.Count > 0)
            {
                // Tìm ô với gScore nhỏ nhất trong open
                Point current = open[0];
                foreach (Point point in open)
                {
                    if (gScore.ContainsKey(point) && gScore[point] < gScore[current])
                    {
                        current = point;
                    }
                }

                // Nếu đã đến ô đích, trả về đường đi
                if (current.Equals(end))
                {
                    List<Point> path = new List<Point>();
                    while (cameFrom.ContainsKey(current))
                    {
                        path.Insert(0, current);
                        current = cameFrom[current];
                    }
                    path.Insert(0, start);
                    return path;
                }

                open.Remove(current);

                // Kiểm tra các ô lân cận
                foreach (Point neighbor in GetNeighbors(current))
                {
                    int tentativeG = gScore[current] + 1;

                    if (!gScore.ContainsKey(neighbor) || tentativeG < gScore[neighbor])
                    {
                        // Nếu chưa xét qua ô này hoặc chi phí giảm, cập nhật thông tin của ô và đưa vào danh sách open
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeG;

                        if (!open.Contains(neighbor))
                        {
                            open.Add(neighbor);
                        }
                    }
                }
            }

            return null; // Nếu không tìm thấy đường đi
        }

        private void MovePlayer(List<Point> path)
        {
            if (path == null)
            {
                MessageBox.Show("Không tìm thấy đường đi từ điểm bắt đầu đến điểm kết thúc!");
                return;
            }

            // Di chuyển người chơi trên đường đi
            playerPosition = startCell;
            mazeCells[playerPosition.X, playerPosition.Y].BackColor = Color.Yellow;

            Timer timer = new Timer { Interval = 200 };
            int currentIndex = 1;

            timer.Tick += (sender, e) =>
            {
                if (currentIndex < path.Count - 1)
                {
                    playerPosition = path[currentIndex];
                    mazeCells[playerPosition.X, playerPosition.Y].BackColor = Color.Yellow;
                    currentIndex++;
                }
                else
                {
                    timer.Stop();
                    // Hiển thị thông báo đã đến đích
                    MessageBox.Show("Đã đến đích!");
                }
            };

            timer.Start();
        }

    }
}
