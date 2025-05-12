using System.Windows.Forms.VisualStyles;

namespace ttt
{
    public partial class Form1 : Form
    {
        private int[,] board = new int[3, 3]; // 0 = пусто, 1 = X (крестик), 2 = O (нолик)
        private bool xTurn = true; // чей ход: true - крестик ходит, false - нолик
        private int scoreX = 0; // победа X
        private int scoreO = 0;
        private bool gameOver = false; //конец игры
        private Point winStart, winEnd; //координаты нач и кон линии победы (зелена€)

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true; //чтобы не мерцало при рисовании
            this.Paint += Form1_Paint; //когда нужно перерисовать форму, вызываетс€ метод
            this.MouseClick += Form1_MouseClick; //при щелчке мыши вызываетс€ метод (клик мыши по полю)

            button1.Text = "Ќова€ игра!";
            button2.Text = "—брос счета";
        }

        /*private void pictureBox1_Click(object sender, EventArgs e)
        {

        }*/

        private void Form1_Paint(object sender, PaintEventArgs e) //рисование(заново,если надо)
        {
            Graphics g = e.Graphics;
            int cellSize = 100; //размер 1 клетки по ширине и высоте

            // —етка:
            Pen gridPen = new Pen(Color.Black, 2); //черный цвет - толщина 2 пиксел€
            for (int i = 1; i < 3; i++)
            {
                g.DrawLine(gridPen, i * cellSize, 0, i * cellSize, 300); //при 1 => от (100;0) до (100; 300) - верт линии
                g.DrawLine(gridPen, 0, i * cellSize, 300, i * cellSize); //гориз линии
            }
            g.DrawRectangle(new Pen(Color.Black, 4), 0, 0, 300, 300); //рамка

            // –исуем фигуры
            for (int i = 0; i < 3; i++) //проходим по всем клеткам пол€
            {
                for (int j = 0; j < 3; j++)
                {
                    int x = j * cellSize;
                    int y = i * cellSize;
                    if (board[i, j] == 1) // крестик X
                    {
                        g.DrawLine(Pens.Red, x + 10, y + 10, x + cellSize - 10, y + cellSize - 10); //с отступом в 10 пикселей
                        g.DrawLine(Pens.Red, x + cellSize - 10, y + 10, x + 10, y + cellSize - 10);
                    }
                    else if (board[i, j] == 2) // нолик O
                    {
                        g.DrawEllipse(Pens.Blue, x + 10, y + 10, cellSize - 20, cellSize - 20);
                    }
                }
            }

            //Ћини€, показывающа€, какой игрок выиграл
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
            if (gameOver) return; // игра уже окончена
            int cellSize = 100;
            int row = e.Y / cellSize; //опр строку (координата клика)
            int col = e.X / cellSize; //опр столбец
            if (row < 3 && col < 3 && board[row, col] == 0) //если клетка пуста
            {
                board[row, col] = xTurn ? 1 : 2; //заполн€ю или ’, или ќ
                xTurn = !xTurn; //ход изменила
                CheckWin(); //есть ли победа
                this.Invalidate(); // ѕерерисовать
            }
        }
        private void CheckWin()
        {
            int[,] lines = new int[,] //массив всех линий дл€ победы
            {
               {0,0, 0,1, 0,2},
               {1,0, 1,1, 1,2},
               {2,0, 2,1, 2,2},
               {0,0, 1,0, 2,0},
               {0,1, 1,1, 2,1},
               {0,2, 1,2, 2,2},
               {0,0, 1,1, 2,2}, //гл диагональ
               {0,2, 1,1, 2,0}  //поб диагональ
            };
            for (int i = 0; i < lines.GetLength(0); i++) //проверка линий на победу
            {
                int r1 = lines[i, 0], c1 = lines[i, 1];
                int r2 = lines[i, 2], c2 = lines[i, 3];
                int r3 = lines[i, 4], c3 = lines[i, 5];
                if (board[r1, c1] != 0 &&
                    board[r1, c1] == board[r2, c2] &&
                    board[r2, c2] == board[r3, c3])
                {
                    gameOver = true; //игра завершена

                    // координаты дл€ линии
                    winStart = new Point(c1 * 100 + 50, r1 * 100 + 50); //*100-размер клетки, +50-смещение к центру клетки
                    winEnd = new Point(c3 * 100 + 50, r3 * 100 + 50);
                    this.Invalidate();
                    
                    if (board[r1, c1] == 1)
                    {
                        scoreX++;
                        MessageBox.Show("»грок X победил!:)");
                    }
                    else
                    {
                        scoreO++;
                        MessageBox.Show("»грок O победил!;)");
                    }
                    return;
                }
            }

            // ѕроверка на ничью
            bool draw = true; //есть ничь€
            foreach (int val in board)
                if (val == 0) draw = false; //если пуста€ клетка - ничьи нет
            if (draw)
            {
                gameOver = true; //игра закончилась
                MessageBox.Show("Ќичь€!:(");
            }
        }

        private void button1_Click(object sender, EventArgs e) //нова€ игра
        {
            board = new int[3, 3]; //двум массив - очистка пол€
            gameOver = false;
            winStart = Point.Empty; //очищаю координаты линии победы
            winEnd = Point.Empty;
            this.Invalidate(); //перерисовка формы
        }

        private void button2_Click(object sender, EventArgs e) //сброс счета
        {
            scoreX = 0;
            scoreO = 0;
            button1_Click(sender, e); //новое поле - сброс gameOver, нет линии поб + запуск новой игры
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            e.Graphics.DrawString($"X: {scoreX} <-—чет-> O: {scoreO}", new Font("Segoe UI", 12), Brushes.Black, 65, 310);
        }
    }
}