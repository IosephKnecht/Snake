using System;
using System.Collections.Generic;
using System.Drawing;
using WindowsFormsApp18.Interfaces;

namespace WindowsFormsApp18.Data
{

    class Pole : IPole
    {
        byte[,] grid;

        float width;
        float height;
        float sideofsquare;


        readonly ISnake snake;

        static IPole _pole;

        System.Windows.Forms.Timer timer;

        /// <summary>
        /// Массив под генерируемые кубики...
        /// </summary>
        public List<ICube> BuffCubes { get; set; }
        /// <summary>
        /// Bitmap для отрисовки поля...
        /// </summary>
        public Bitmap btm { get; set; }

        /// <summary>
        /// Событие на перерисовку формы(адресуется к View)...
        /// </summary>
        public event Action UpdateForm;

        /// <summary>
        /// Вспомогательный метод для генерации случайного кубика...
        /// </summary>
        private void GenerateBuff()
        {
            Random ran = new Random();
            int I = 0;
            int J = 0;
            do
            {
                I = ran.Next(grid.GetLength(0));
                J = ran.Next(grid.GetLength(1));
            }
            while (!snake.EndSnake && (grid[I, J]) != 0);

            BuffCubes.Add(new Cube(I*sideofsquare,J*sideofsquare));
        }

        /// <summary>
        /// Конструктор одиночки...
        /// </summary>
        /// <param name="width">Ширина поля...</param>
        /// <param name="height">Высота поля...</param>
        /// <param name="sideofsquare">Размер стороны кубика...</param>
        protected Pole(float width, float height, float sideofsquare)
        {
            this.width = width;
            this.height = height;
            this.sideofsquare = sideofsquare;

            Cube.size = sideofsquare;

            this.btm = new Bitmap(Convert.ToInt32(width), Convert.ToInt32(height));

            grid = new byte[Convert.ToInt32(width / sideofsquare), Convert.ToInt32(height / sideofsquare)];

            snake = Snake.Instance();

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += Tick;
        }

        /// <summary>
        /// Вспомогательный метод для старта Змейки...
        /// </summary>
        public void StartSnake()
        {
            BuffCubes = new List<ICube>();
            GenerateBuff();
            //BuffCubes.Add(new Cube(15, 30));

            int head_X = (grid.GetLength(0) / 2);
            int head_Y = (grid.GetLength(1) / 2);

            snake.RePaint += RePaint;
            snake.Add(head_X * sideofsquare, head_Y * sideofsquare);

            grid[head_X, head_Y] = (byte)snake.cubes[0].colorNum;

            timer.Enabled = true;
        }

        /// <summary>
        /// Перерисовка поля раз в интервал...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tick(object sender, EventArgs e)
        {
            if (!snake.EndSnake)
            {
                IncBuff();
                snake.TimeStep();
                UpdateForm();
            }
            else
                timer.Stop();
        }

        /// <summary>
        /// Увеличение длины змейки при "поедании" buffCube...
        /// </summary>
        private void IncBuff()
        {
            foreach (ICube cube in BuffCubes)
            {
                if (snake.cubes[0]._X == cube._X && snake.cubes[0]._Y == cube._Y)
                {
                    snake.Add(cube);
                    BuffCubes.Remove(cube);
                    GenerateBuff();
                    break;
                }
            }
        }

        /// <summary>
        /// Реализация события Snake.RePaint(),обновляет матрицу поля....
        /// </summary>
        private void RePaint()
        {
            grid = new byte[grid.GetLength(0), grid.GetLength(1)];

            foreach (Cube cube in snake.cubes)
            {
                try
                {
                    grid[cube._I, cube._J] = (byte)(cube.colorNum + 1);
                }
                catch
                {
                    snake.EndSnake = true;
                }
            }

            Paint();

            UpdateForm();

        }

        /// <summary>
        /// Огромная процедура рисовки на Bitmap'e...
        /// </summary>
        /// <returns>отрисованный Bitmap...</returns>
        public Bitmap Paint()
        {
            Graphics g = Graphics.FromImage(btm);
            Brush newBrush;
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] != 0)
                    {
                        newBrush = Cube.Colors[grid[i, j]-1];
                    }
                    else
                    {
                        newBrush = Brushes.White;
                    }
                    g.FillRectangle(newBrush, j * sideofsquare, i * sideofsquare, sideofsquare, sideofsquare);
                }
            }

            foreach (ICube cube in BuffCubes)
            {
                g.FillRectangle(cube.color, cube._Y, cube._X, sideofsquare, sideofsquare);
            }

            for (int i = 1; i <= (grid.GetLength(0)); i++)
            {
                g.DrawLine(Pens.Black, new Point(1, (int)sideofsquare * i), new Point(Convert.ToInt32(width), (int)sideofsquare * i));//горизонталь
            }

            for (int j = 1; j <= (grid.GetLength(1)); j++)
            {
                g.DrawLine(Pens.Black, new Point((int)sideofsquare * j, Convert.ToInt32(height)), new Point((int)sideofsquare * j, 1));//вертикаль
            }

            g.Dispose();

            return btm;

        }

        /// <summary>
        /// Метод инстанцирующий экземпляр класса...
        /// </summary>
        /// <param name="width">Ширина поля...</param>
        /// <param name="height">Высота поля...</param>
        /// <param name="sideofsquare">Размер стороны кубика...</param>
        /// <returns></returns>
        public static IPole Instance(float width, float height, float sideofsquare)
        {
            if (_pole == null) return _pole = new Pole(width, height, sideofsquare);
            else
                return _pole;
        }

        /// <summary>
        /// Реализация Dispose...
        /// </summary>
        public void Dispose()
        {
            grid = new byte[grid.GetLength(0), grid.GetLength(1)];
            btm.Dispose();
            snake.Dispose();
        }
    }
}
