using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeilofDeath.SpecialFX
{
    abstract class AParticleEnginge
    {
        public Random random;
        public Vector2 EmitterLocation { get; set; }
        public List<Particle> particles { get; set; }
        public List<Texture2D> textures { get; set; }
        public float total { get; set; }

        public float lifetime { get;  set;}



        public virtual void Update(GameTime gt)
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
                    Console.WriteLine(a + " particle(s) at " + z + " from: " + gt.TotalGameTime.Milliseconds);
            }
    

            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();
                if (particles[particle].TTL <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        public abstract Particle GenerateNewParticle();
        
        public void Draw(SpriteBatch spriteBatch)
        {
            
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }
            
        }

    }
}
