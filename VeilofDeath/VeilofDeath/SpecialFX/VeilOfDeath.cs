using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeilofDeath.SpecialFX
{
    class VeilOfDath : AParticleEnginge
    {
        private Vector2 attraction;
        private Vector2 em1, em2;

        public VeilOfDath(List<Texture2D> textures, Vector2 location1, Vector2 location2, float PartsPerSeconds, float time2life, Vector2 gravityCentre)
        {
            em1 = location1;
            em2 = location2;
            this.textures = textures;
            this.particles = new List<Particle>();
            random = new Random();
            total = PartsPerSeconds;
            lifetime = time2life;
            attraction = gravityCentre;
        }

        public override void Update(GameTime gt)
        {
            float x = 1000 / total;
            float z = gt.ElapsedGameTime.Milliseconds;
            if (gt.TotalGameTime.Milliseconds % (x) < z)
            {
                int a = 1;
                if (x < z)
                {
                    a = (int)(z / x);
                }
                for (int i = 0; i < a; i++)
                {
                    particles.Add(GenerateNewParticle());
                }
                if (GameConstants.isDebugMode)
                    Console.WriteLine(a + " partix at " + z + " from: " + gt.TotalGameTime.Milliseconds);
            }

            for (int particle = 0; particle < particles.Count; particle++)
            {
                Vector2 deltaGrav = Vector2.Multiply(attraction, (float)gt.ElapsedGameTime.Seconds);
                particles[particle].Affect(ref deltaGrav);
                particles[particle].TransformAlpha(1);
                particles[particle].TransformSize(1);
                particles[particle].Update();
                if (particles[particle].TTL <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }


        public override Particle GenerateNewParticle()
        {
            Texture2D texture = textures[random.Next(textures.Count)];
            Vector2 position = new Vector2( random.Next((int)em1.X, (int)em2.X),
                                            random.Next((int)em1.Y, (int)em2.Y));
            Vector2 velocity = new Vector2(
                                    1f * (float)(random.NextDouble() * 2 - 1), //links-rechts
                                    1f * (float)(random.NextDouble() * 2 - 1) //vor-zurück
                                    );
            velocity += attraction;


            float angle = 0;
            //may have to use quaternion? or yan-pitch-roll Vector
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
            float brghtness = (float)random.NextDouble();
            Color color = new Color(brghtness,brghtness,brghtness,brghtness);
            float size = Math.Max(0.1f, ((float)random.NextDouble()/3));
            int ttl = (int)lifetime * 50 + random.Next(50);

            return new Particle(texture, position, velocity, angle, angularVelocity, Color.White, size, ttl);
        }
    }
}
