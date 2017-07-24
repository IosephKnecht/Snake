using System;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp18.Interfaces;
using WindowsFormsApp18.Data;

namespace WindowsFormsApp18
{
    public partial class Form1 : Form
    {
        readonly IPole pole;
        public Form1()
        {
            InitializeComponent();
            pole = Pole.Instance(400,400,15);
            pole.UpdateForm += BRefresh;
            ((Pole)pole).StartSnake();
            
        }

        BufferedGraphicsContext currentContext;
        BufferedGraphics myBuffer;

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// Реализация отрисовки поля...
        /// </summary>
        void BRefresh()
        {
            currentContext = BufferedGraphicsManager.Current;
            myBuffer = currentContext.Allocate(this.CreateGraphics(),
               new Rectangle(new Point(0, 0), new Size(400, 400)));
            Bitmap BtmPole = pole.btm;
            myBuffer.Graphics.DrawImage(BtmPole, 0, 0);
            myBuffer.Render(this.CreateGraphics());
            myBuffer.Dispose();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            BRefresh();
        }

        /// <summary>
        /// Вызов управления змейкой...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            switch (e.KeyValue)
            {

                case (int)Keys.Space:
                    pole.Dispose();
                    break;
                //Left
                case 37:
                    Snake.Instance().Left();
                    break;
                //Right
                case 39:
                    Snake.Instance().Right();
                    break;
                //Down...
                case 40:
                    Snake.Instance().Down();
                    break;
                //Up...
                case 38:
                    Snake.Instance().Up();
                    break;
            }
            //BRefresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //pole.Dispose();
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
