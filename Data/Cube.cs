using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using WindowsFormsApp18.Interfaces;

namespace WindowsFormsApp18.Data
{
    class Cube : ICube
    {
        private float X, Y; //Координаты кубика...
        int I, J; //Строка,столбец...

        public static float size;//Размер стороны кубика...

        public Brush color { get; set; }//Цвет кубика...

        /// <summary>
        /// Набор цветов...
        /// </summary>
        public readonly static Brush[] Colors=new Brush[] { Brushes.CadetBlue, Brushes.Orange, Brushes.Blue, Brushes.Red, Brushes.Green,
            Brushes.DarkOrchid, Brushes.Violet, Brushes.Tomato, Brushes.SteelBlue };

        /// <summary>
        /// Конструктор...
        /// </summary>
        /// <param name="X">Координата по абциссе...</param>
        /// <param name="Y">Координата по ординате...</param>
        public Cube(float X, float Y)
        {
            this.X = X;
            this.Y = Y;

            Random ran = new Random();
            int colorNum = ran.Next(0, Colors.Length);
            color = Colors[colorNum];

            UpdateIJ();
        }

        /// <summary>
        /// Вспомогательный метод для перестановки одного кубика на место другого...
        /// </summary>
        /// <param name="ChangedCube">Кубик координаты которого мы меняем...</param>
        /// <param name="FutureCube">Кубик на место которого будет поставлен ChangedCube...</param>
        /// <returns>Новый ICube с новыми координатами...</returns>
        public static ICube Step(ICube ChangedCube, ICube FutureCube)
        {
            ChangedCube._X = FutureCube._X;
            ChangedCube._Y = FutureCube._Y;

            return ChangedCube;
        }

        /// <summary>
        /// Вспомогательный метод для представления координат в виде номеров строк...
        /// </summary>
        public void UpdateIJ()
        {
            I = Convert.ToInt32(X / size);
            J = Convert.ToInt32(Y / size);
        }

        public int _I
        {
            get { return I; }
        }

        public int _J
        {
            get { return J; }
        }

        public float _X
        {
            get
            {
                return X;
            }

            set
            {
                X = value;
                UpdateIJ();
            }
        }

        public float _Y
        {
            get
            {
                return Y;
            }

            set
            {
                Y = value;
                UpdateIJ();
            }
        }

        public int colorNum
        {
            get
            {
                for (int i = 0; i < Colors.Length; i++)
                {
                    if (Colors[i].Equals(color)) return i;
                }

                return 0;
            }
        }
    }
}
