using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParticlesEffect
{
    public partial class Particles1 : Form
    {
        private List<Particle> particles = new List<Particle>();
        private Random random = new Random();
        private System.Windows.Forms.Timer timerParticles = new System.Windows.Forms.Timer();
        private Point previousFormLocation;
        private bool isFirstUpdate = true;

        public Particles1()
        {
            InitializeComponent();

            DoubleBuffered = true;
            InitializeParticles();
            timerParticles.Interval = 1;
            timerParticles.Tick += timerparticleseffect_Tick;
            timerParticles.Start();
            DoubleBuffered = true;
            this.ShowIcon = false;
        }

        private void Particles1_Load(object sender, EventArgs e)
        {

        }

        private void timerparticleseffect_Tick(object sender, EventArgs e)
        {
            UpdateParticles();
            Invalidate();
        }

        public class Particle
        {
            public PointF Position { get; set; }
            public PointF Velocity { get; set; }
            public int Radius { get; set; }
            public Color Color { get; set; }

        }

        private void InitializeParticles()
        {
            int numParticles = 50;
            for (int i = 0; i < numParticles; i++)
            {
                double angle = random.NextDouble() * 2 * Math.PI;
                double speed = random.Next(1, 3);
                particles.Add(new Particle()
                {
                    Position = new PointF(random.Next(0, ClientSize.Width), random.Next(0, ClientSize.Height)),
                    Velocity = new PointF((float)(Math.Cos(angle) * speed), (float)(Math.Sin(angle) * speed)),
                    Radius = random.Next(2, 5),
                    Color = Color.Red
                });
            }
        }

        private void UpdateParticles()
        {
            foreach (var particle in particles)
            {
                particle.Position = new PointF(particle.Position.X + particle.Velocity.X * 0.5f, particle.Position.Y + particle.Velocity.Y * 0.5f);
                if (particle.Position.X < 0 || particle.Position.X > ClientSize.Width)
                {
                    particle.Velocity = new PointF(-particle.Velocity.X, particle.Velocity.Y);
                    particle.Position = new PointF(particle.Position.X + particle.Velocity.X * 0.5f, particle.Position.Y);
                }
                if (particle.Position.Y < 0 || particle.Position.Y > ClientSize.Height)
                {
                    particle.Velocity = new PointF(particle.Velocity.X, -particle.Velocity.Y);
                    particle.Position = new PointF(particle.Position.X, particle.Position.Y + particle.Velocity.Y * 0.5f);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            foreach (var particle in particles)
            {
                int transparency = (int)((1.0f - (particle.Position.Y / (float)ClientSize.Height)) * 255);
                if (transparency > 255) transparency = 255;
                if (transparency < 0) transparency = 0;

                Color particleColor = Color.FromArgb(transparency, ColorTranslator.FromHtml("#FF0000")); 

                int reducedRadius = particle.Radius / 2;
                e.Graphics.FillEllipse(new SolidBrush(particleColor),
                    particle.Position.X - reducedRadius,
                    particle.Position.Y - reducedRadius,
                    reducedRadius * 2, reducedRadius * 2);
            }

            foreach (var particle in particles)
            {
                foreach (var otherParticle in particles)
                {
                    if (particle != otherParticle)
                    {
                        float dx = particle.Position.X - otherParticle.Position.X;
                        float dy = particle.Position.Y - otherParticle.Position.Y;
                        float distance = (float)Math.Sqrt(dx * dx + dy * dy);

                        if (distance < 50)
                        {
                            int alpha = (int)((1.0f - (distance / 50.0f)) * 255.0f);
                            Color lineColor = Color.FromArgb(alpha, 255, 0, 0); 
                            e.Graphics.DrawLine(new Pen(lineColor, 1),
                                particle.Position, otherParticle.Position);
                        }
                    }
                }
            }
        }
    }
}
