using System;
using System.Collections.Generic;
using System.Linq;
using WindowsFormsApp18.Interfaces;

namespace WindowsFormsApp18.Data
{

    class Snake : ISnake
    {
        public List<ICube> cubes { get; set; }//Кубики змейки...
        public Vectors direction { get; set; }//Вектор змейки...
        public bool EndSnake { get; set; }//Состояние змейки(движется/остановилась)...

        static ISnake snake;

        /// <summary>
        /// Событие на перезаполнение матрицы Pole...
        /// </summary>
        public event Action RePaint;

        /// <summary>
        /// Констрктор...
        /// </summary>
        protected Snake()
        {
            cubes = new List<ICube>();
            direction = Vectors.Right;
            EndSnake = false;
        }

        /// <summary>
        /// Метод инстанцирующий экземпляр класса...
        /// </summary>
        /// <returns>Экземпляр класса...</returns>
        public static ISnake Instance()
        {
            if (snake == null) return snake = new Snake();
            else
                return snake;
        }

        /// <summary>
        /// Вспомогательный метод для получения "будущих" координат змейки...
        /// </summary>
        /// <returns>KeyValuePair c координатами...</returns>
        private KeyValuePair<float,float> Transition()
        {
            switch (direction)
            {
                case Vectors.Left:
                    return new KeyValuePair<float, float>(cubes.Last()._X, cubes.Last()._Y + Cube.size);
                    break;

                case Vectors.Right:
                    return new KeyValuePair<float, float>(cubes.Last()._X, cubes.Last()._Y - Cube.size);
                    break;

                case Vectors.Down:
                    return new KeyValuePair<float, float>(cubes.Last()._X - Cube.size, cubes.Last()._Y);
                    break;

                case Vectors.Up:
                    return new KeyValuePair<float, float>(cubes.Last()._X + Cube.size, cubes.Last()._Y);
                    break;
                default:
                    return new KeyValuePair<float, float>();
                    break;
            }
        }
        /// <summary>
        /// Вспомогательный метод для добавления случайного кубика в "хвост" змейки...
        /// </summary>
        public void Add()
        {
            var kvp = Transition();
            Add(kvp.Key, kvp.Value);
        }

        /// <summary>
        /// Метод реализующий логику добавления кубика к змейке по заданным координатам...
        /// </summary>
        /// <param name="X">Расположение по оси абцисс...</param>
        /// <param name="Y">Расположение по оси ординат...</param>
        public void Add(float X, float Y)
        {
            Cube new_cube = new Cube(X, Y);
            cubes.Add(new_cube);
            RePaint();
        }

        /// <summary>
        /// Метод реализующий логику добавления BuffCube в конец змейки...
        /// </summary>
        /// <param name="cube"></param>
        public void Add(ICube cube)
        {
            var kvp = Transition();
            cube._X = kvp.Key;
            cube._Y = kvp.Value;
            cubes.Add(cube);
            RePaint();
        }

        public void Down(bool timestep=false)
        {
            if (timestep || (direction == Vectors.Left || direction == Vectors.Right))
            {
                StepByStep();
                if (PositionOK(snake.cubes[0]._X + Cube.size, snake.cubes[0]._Y)) snake.cubes[0]._X += Cube.size;
                else EndSnake = true;
                direction = Vectors.Down;
                RePaint();
            }
        }

        public void Left(bool timestep=false)
        {
            if (timestep || (direction == Vectors.Up || direction == Vectors.Down))
            {
                StepByStep();
                if (PositionOK(snake.cubes[0]._X, snake.cubes[0]._Y - Cube.size)) snake.cubes[0]._Y -= Cube.size;
                else EndSnake = true;
                direction = Vectors.Left;
                RePaint();
            }
        }

        public void Right(bool timestep=false)
        {
            if (timestep || (direction == Vectors.Up || direction == Vectors.Down))
            {
                StepByStep();
                if (PositionOK(snake.cubes[0]._X, snake.cubes[0]._Y + Cube.size)) snake.cubes[0]._Y += Cube.size;
                else EndSnake = true;
                direction = Vectors.Right;
                RePaint();
            }
        }

        public void Up(bool timestep=false)
        {
            if (timestep || (direction == Vectors.Left || direction == Vectors.Right))
            {
                StepByStep();
                if (PositionOK(snake.cubes[0]._X - Cube.size, snake.cubes[0]._Y)) snake.cubes[0]._X -= Cube.size;
                else EndSnake = true;
                direction = Vectors.Up;
                RePaint();
            }
        }

        /// <summary>
        /// Метод перестановки всех кубиков...
        /// </summary>
        public void StepByStep()
        {
            for (int i = cubes.Count - 1; i > 0; i--)
            {
                cubes[i] = Cube.Step(cubes[i], cubes[i - 1]);
                cubes[i].UpdateIJ();
            }
        }

        /// <summary>
        /// Метод реализующий логику передвижения 
        /// змейки раз в такт таймера...
        /// </summary>
        public void TimeStep()
        {
            switch (direction)
            {
                case Vectors.Left:
                    Left(true);
                    break;
                case Vectors.Right:
                    Right(true);
                    break;
                case Vectors.Up:
                    Up(true);
                    break;
                case Vectors.Down:
                    Down(true);
                    break;
            }

        }

        /// <summary>
        /// Метод проверки на возможность передвижения...
        /// </summary>
        /// <param name="X">Будущие координаты по абциссе...</param>
        /// <param name="Y">Будущие координаты по ординате...</param>
        /// <returns></returns>
        private bool PositionOK(float X, float Y)
        {
            for (int i = 1; i < cubes.Count; i++)
            {
                if (cubes[i]._X == X && cubes[i]._Y == Y) return false;
            }
            return true;
        }

        /// <summary>
        /// Реализация метода Dispose...
        /// </summary>
        public void Dispose()
        {
            snake = new Snake();
        }
    }
}
