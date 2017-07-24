using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApp18.Interfaces
{
    public enum Vectors
    {
        Left,
        Right,
        Up,
        Down,
    }

    interface ISnake:IDisposable
    {
        event Action RePaint;
        void TimeStep();
        void Add();
        void Add(float X, float Y);
        void Add(ICube cube);
        void Left(bool timestep = false);
        void Right(bool timestep = false);
        void Up(bool timestep = false);
        void Down(bool timestep = false);
        List<ICube> cubes { get; set; }
        Vectors direction { get; set; }
        void StepByStep();
        bool EndSnake { get; set; }
    }
}
