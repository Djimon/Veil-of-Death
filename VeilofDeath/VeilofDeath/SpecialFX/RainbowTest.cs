using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeilofDeath.SpecialFX
{
    class RainbowTest : AParticleEnginge
    {
        public RainbowTest(List < Texture2D > textures, Vector2 location, float PartsPerSeconds, float time2life)
        {
            EmitterLocation = location;
            this.textures = textures;
            this.particles = new List<Particle>();
            random = new Random();
            total = PartsPerSeconds;
            lifetime = time2life;
        }


        public override Particle GenerateNewParticleLeft()
        {
            Texture2D texture = textures[random.Next(textures.Count)];
            Vector2 position = EmitterLocation;
            Vector2 velocity = new Vector2(
                                    1f * (float)(random.NextDouble() * 2 - 1), //links-rechts
                                    1f * (float)(random.NextDouble() * 2 - 1) //vor-zurück
                                    );
            float angle = 0;
            //may have to use quaternion? or yan-pitch-roll Vector
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
            Color color = new Color(
                        (float)random.NextDouble(),
                        (float)random.NextDouble(),
                        (float)random.NextDouble());
            float size = (float)random.NextDouble();
            int ttl =(int) lifetime*100 + random.Next(100);

            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, ttl);

        }
    }
}
