using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeilofDeath.Core;

namespace VeilofDeath.SpecialFX
{
    class GUIFlash : AParticleEnginge
    {
        private Vector2 attraction;
        private bool hasSpawned = false;
        private bool moveUp = true;

        /// <summary>
        /// Definey which Effect is shown
        /// <para>"speed" - Textures[0]: powerUp_speed </para>
        /// <para>"slow" - Textures[1]: slowdown </para>
        /// <para>"sword" - Textures[2]: powerUp_Sword </para>
        /// </summary>
        public string sParticleType;

        /// <summary>
        /// Constructor for GUI effects. gets List of Textures
        /// <para>"textures[0]" PowerUp_Speed </para>
        /// <para> "textures[1]" SlowDown </para>
        /// <para> "textures[2]" PowerUp_Sword </para>
        /// </summary>
        /// <param name="textures"> List of Textures </param>
        /// <param name="goUP">defines if the texture gos up or down</param>
        /// <param name="type">Definey which Effect is shown
        /// <para>"speed" - Textures[0]: powerUp_speed </para>
        /// <para>"slow" - Textures[1]: slowdown </para>
        /// <para>"sword" - Textures[2]: powerUp_Sword </para></param>
        public GUIFlash(List<Texture2D> textures)
        {
            this.textures = textures;
            this.particles = new List<Particle>();
            random = new Random();
            lifetime = 1f;            
        }

        public override void Update(GameTime gt)
        {
            if (hasSpawned)
            {
                for (int particle = 0; particle < particles.Count; particle++)
                {
                    Vector2 deltaGrav = Vector2.Multiply(attraction, 1f);
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

                if (particles.Count <= 0)
                {
                    hasSpawned = false;
                    //Console.WriteLine(" all particle depsawned..................");
                }                
            }            
        }

        public void SpawnSpeed()
        {
            sParticleType = "speed";
            particles.Add(GenerateNewParticleLeft());
            hasSpawned = true;
        }
        public void SpawnSlow()
        {
            sParticleType = "slow";
            particles.Add(GenerateNewParticleLeft());
            hasSpawned = true;
        }
        public void SpawnSword()
        {
            sParticleType = "sword";
            particles.Add(GenerateNewParticleLeft());
            hasSpawned = true;
        }

        public override Particle GenerateNewParticleLeft()
        {
            Texture2D texture = null;
            switch (sParticleType)
            {
                case "speed":
                    texture = textures[0];
                    break;
                case "slow":
                    texture = textures[1];
                    break;
                case "sword":
                    texture = textures[2];
                    break;
                default:
                    break;
            }
            
            Vector2 position = EmitterLocation;
            attraction = position + new Vector2(0, (moveUp ? 1f : -1f));

            return Spawnparticle(position, texture);
        }

        private Particle Spawnparticle(Vector2 position, Texture2D tex, bool moveUp = true)
        {
            Vector2 velocity = new Vector2(1f , (moveUp ? 1f : -1f));
            velocity += attraction;
            if (tex == null)
                GameConstants.Content.Load<Texture2D>("Panels/void");

            float angle = 0;
            float angularVelocity = 1f;
            float brghtness = (float)random.NextDouble();
            Color color = new Color(brghtness, brghtness, brghtness, brghtness);
            float size = 1f;
            int ttl = (int)lifetime * 100;            
            //Console.WriteLine("particle spawned.. at: " + position.X + ":" + position.Y);
            //Console.WriteLine("with: " + tex.Name.ToString());
            return new Particle(tex, position, velocity, angle, angularVelocity, Color.White, size, ttl);
        }
    }
}
