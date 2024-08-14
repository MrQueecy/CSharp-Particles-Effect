using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ParticlesEffect.Particles1;

namespace ParticlesEffect
{
    public partial class Particles2 : Form
    {
        private List<Particle> particles = new List<Particle>();
        private Random random = new Random();
        private Color backgroundColor = Color.FromArgb(17, 17, 17);

        public Particles2()
        {
            InitializeComponent();

            particleseffect();
            timer1.Interval = 1;
            timer1.Start();
            DoubleBuffered = true;
        }

        private void Particles2_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            particleseffectdev();
            Invalidate();
        }

        private const float SpeedFactor = 0.5f;

        private void particleseffect()
        {
            int numParticles = 50;
            for (int i = 0; i < numParticles; i++)
            {
                double angle = random.NextDouble() * 2 * Math.PI;
                double speed = random.Next(1, 5) * SpeedFactor;
                particles.Add(new Particle()
                {
                    Position = new PointF(random.Next(0, ClientSize.Width), random.Next(0, ClientSize.Height)),
                    Velocity = new PointF((float)(Math.Cos(angle) * speed), (float)(Math.Sin(angle) * speed)),
                    Radius = random.Next(2, 5),
                    Color = Color.Red
                });
            }
        }

        private void particleseffectdev()
        {
            foreach (var particle in particles)
            {
                particle.Position = new PointF(particle.Position.X + particle.Velocity.X, particle.Position.Y + particle.Velocity.Y);
                if (particle.Position.X < 0) particle.Position = new PointF(ClientSize.Width, particle.Position.Y);
                if (particle.Position.X > ClientSize.Width) particle.Position = new PointF(0, particle.Position.Y);
                if (particle.Position.Y < 0) particle.Position = new PointF(particle.Position.X, ClientSize.Height);
                if (particle.Position.Y > ClientSize.Height) particle.Position = new PointF(particle.Position.X, 0);
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(backgroundColor);
            foreach (var particle in particles)
            {
                e.Graphics.FillEllipse(new SolidBrush(particle.Color),
                    particle.Position.X - particle.Radius,
                    particle.Position.Y - particle.Radius,
                    particle.Radius * 2, particle.Radius * 2);
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
                            e.Graphics.DrawLine(new Pen(Color.FromArgb(alpha, Color.Red), 1),
                                particle.Position, otherParticle.Position);
                        }
                    }
                }
            }
        }
    }
}
