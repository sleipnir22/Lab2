using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Lab2
{
    public partial class Form1 : Form
    {
        World world;
        FlyingObj flobj;
        public Form1()
        {
            InitializeComponent();
            HeightButton.Value = 0;
            SpeedButton.Value = 10;
            AngleButton.Value = 45;
            
            //chart1.Series[0].Points.AddXY(0, 0);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            world.Iterate(chart1);
            if (world.Obj.Y < 0)
            {
                timer1.Stop();
                button2.Enabled = false;
            }
            label6.Text = world.T.ToString() + " seconds"; 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!timer1.Enabled)
            {
                button2.Enabled = true;
                chart1.Series[0].Points.Clear();
                timer1.Start();
                var y0 = HeightButton.Value;
                var a = AngleButton.Value;
                var v0 = SpeedButton.Value;
                flobj = new FlyingObj(y0, a, v0);
                world = new World(flobj, 0.01M, chart1);
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisX.Maximum = trackBar1.Value;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisY.Maximum = trackBar2.Value;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "Unpause")
            {
                timer1.Start();
                button2.Text = "Pause";
            }
            else if (timer1.Enabled)
            {
                timer1.Stop();
                button2.Text = "Unpause";
            }
            else return;
        }
    }


    public class World
    {
        decimal _dt;
        public decimal T { get; set; }
        public FlyingObj Obj { get;  }
        const decimal g = 9.81M;
        Chart _chart;
        public World(FlyingObj obj, decimal dt, Chart chart)
        {
            T = 0;
            Obj = obj;
            _dt = dt;
            _chart = chart;
            _chart.Series[0].Points.Clear();
        }

        public void Iterate(Chart chart)
        {
            Obj.Fly(T, g);
            chart.Series[0].Points.AddXY(Obj.X, Obj.Y);
            T += _dt;
        }
    }

    public class FlyingObj
    {
        public readonly decimal v0;
        private readonly double _angle;
        private decimal _x0 = 0;
        private decimal _y0;
        private decimal cosa;
        private decimal sina;

        public decimal X { get; set; }
        public decimal Y { get; set; }



        public FlyingObj(decimal height, decimal angle, decimal velocity)
        {
            _y0 = height;
            _angle = (double)angle * Math.PI / 180;
            v0 = velocity;
            cosa = (decimal)Math.Cos(_angle);
            sina = (decimal)Math.Sin(_angle);

        }

        public void Fly(decimal time, decimal g)
        {
            X = _x0 + v0 * cosa * time;
            Y = _y0 + v0 * sina * time - g * time * time / 2;
        }
    }
}
