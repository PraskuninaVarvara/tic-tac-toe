using System.Windows.Forms.VisualStyles;

namespace ttt
{
    public partial class Form1 : Form
    {
        private int[,] board = new int[3, 3]; // 0 = �����, 1 = X (�������), 2 = O (�����)
        private bool xTurn = true; // ��� ���: true - ������� �����, false - �����
        private int scoreX = 0; // ������ X
        private int scoreO = 0;
        private bool gameOver = false; //����� ����
        private Point winStart, winEnd; //���������� ��� � ��� ����� ������ (�������)

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true; //����� �� ������� ��� ���������
            this.Paint += Form1_Paint; //����� ����� ������������ �����, ���������� �����
            this.MouseClick += Form1_MouseClick; //��� ������ ���� ���������� ����� (���� ���� �� ����)

            button1.Text = "����� ����!";
            button2.Text = "����� �����";
        }

        /*private void pictureBox1_Click(object sender, EventArgs e)
        {

        }*/

        private void Form1_Paint(object sender, PaintEventArgs e) //���������(������,���� ����)
        {
            Graphics g = e.Graphics;
            int cellSize = 100; //������ 1 ������ �� ������ � ������

            // �����:
            Pen gridPen = new Pen(Color.Black, 2); //������ ���� - ������� 2 �������
            for (int i = 1; i < 3; i++)
            {
                g.DrawLine(gridPen, i * cellSize, 0, i * cellSize, 300); //��� 1 => �� (100;0) �� (100; 300) - ���� �����
                g.DrawLine(gridPen, 0, i * cellSize, 300, i * cellSize); //����� �����
            }
            g.DrawRectangle(new Pen(Color.Black, 4), 0, 0, 300, 300); //�����

            // ������ ������
            for (int i = 0; i < 3; i++) //�������� �� ���� ������� ����
            {
                for (int j = 0; j < 3; j++)
                {
                    int x = j * cellSize;
                    int y = i * cellSize;
                    if (board[i, j] == 1) // ������� X
                    {
                        g.DrawLine(Pens.Red, x + 10, y + 10, x + cellSize - 10, y + cellSize - 10); //� �������� � 10 ��������
                        g.DrawLine(Pens.Red, x + cellSize - 10, y + 10, x + 10, y + cellSize - 10);
                    }
                    else if (board[i, j] == 2) // ����� O
                    {
                        g.DrawEllipse(Pens.Blue, x + 10, y + 10, cellSize - 20, cellSize - 20);
                    }
                }
            }

            //�����, ������������, ����� ����� �������
            if (gameOver)
            {
                Pen winPen = new Pen(Color.Green, 4);
                winPen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                g.DrawLine(winPen, winStart, winEnd);
            }
        }

        /*private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {

        }*/

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (gameOver) return; // ���� ��� ��������
            int cellSize = 100;
            int row = e.Y / cellSize; //��� ������ (���������� �����)
            int col = e.X / cellSize; //��� �������
            if (row < 3 && col < 3 && board[row, col] == 0) //���� ������ �����
            {
                board[row, col] = xTurn ? 1 : 2; //�������� ��� �, ��� �
                xTurn = !xTurn; //��� ��������
                CheckWin(); //���� �� ������
                this.Invalidate(); // ������������
            }
        }
        private void CheckWin()
        {
            int[,] lines = new int[,] //������ ���� ����� ��� ������
            {
               {0,0, 0,1, 0,2},
               {1,0, 1,1, 1,2},
               {2,0, 2,1, 2,2},
               {0,0, 1,0, 2,0},
               {0,1, 1,1, 2,1},
               {0,2, 1,2, 2,2},
               {0,0, 1,1, 2,2}, //�� ���������
               {0,2, 1,1, 2,0}  //��� ���������
            };
            for (int i = 0; i < lines.GetLength(0); i++) //�������� ����� �� ������
            {
                int r1 = lines[i, 0], c1 = lines[i, 1];
                int r2 = lines[i, 2], c2 = lines[i, 3];
                int r3 = lines[i, 4], c3 = lines[i, 5];
                if (board[r1, c1] != 0 &&
                    board[r1, c1] == board[r2, c2] &&
                    board[r2, c2] == board[r3, c3])
                {
                    gameOver = true; //���� ���������

                    // ���������� ��� �����
                    winStart = new Point(c1 * 100 + 50, r1 * 100 + 50); //*100-������ ������, +50-�������� � ������ ������
                    winEnd = new Point(c3 * 100 + 50, r3 * 100 + 50);
                    this.Invalidate();
                    
                    if (board[r1, c1] == 1)
                    {
                        scoreX++;
                        MessageBox.Show("����� X �������!:)");
                    }
                    else
                    {
                        scoreO++;
                        MessageBox.Show("����� O �������!;)");
                    }
                    return;
                }
            }

            // �������� �� �����
            bool draw = true; //���� �����
            foreach (int val in board)
                if (val == 0) draw = false; //���� ������ ������ - ����� ���
            if (draw)
            {
                gameOver = true; //���� �����������
                MessageBox.Show("�����!:(");
            }
        }

        private void button1_Click(object sender, EventArgs e) //����� ����
        {
            board = new int[3, 3]; //���� ������ - ������� ����
            gameOver = false;
            winStart = Point.Empty; //������ ���������� ����� ������
            winEnd = Point.Empty;
            this.Invalidate(); //����������� �����
        }

        private void button2_Click(object sender, EventArgs e) //����� �����
        {
            scoreX = 0;
            scoreO = 0;
            button1_Click(sender, e); //����� ���� - ����� gameOver, ��� ����� ��� + ������ ����� ����
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            e.Graphics.DrawString($"X: {scoreX} <-����-> O: {scoreO}", new Font("Segoe UI", 12), Brushes.Black, 65, 310);
        }
    }
}