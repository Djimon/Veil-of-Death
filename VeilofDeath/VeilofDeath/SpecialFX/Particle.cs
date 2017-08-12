using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeilofDeath.SpecialFX
{
    class Particle
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Angle { get; set; }
        public float AngularVelocity { get; set; }
        public Color Color { get; set; }
        public float Size { get; set; }
        public int TTL { get; set; }
        private int maxLifetime;
        private float maxSize;

        public Particle(Texture2D texture, Vector2 position, Vector2 velocity,
            float angle, float angularVelocity, Color color, float size, int ttl)
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
            Angle = angle;
            AngularVelocity = angularVelocity;
            Color = color;
            maxSize = Size = size;
            TTL = ttl;
            maxLifetime = ttl;
        }

        public void Update()
        {
            TTL--;            
            Position += Velocity;
            Angle += AngularVelocity;
        }

        public void Affect(ref Vector2 attraction)
        {
            Velocity += attraction;
        }

        public void TransformAlpha(float sign)
        {
            if (TTL > 0)
            {
                float alpha = sign > 0 ? ((float)TTL / (float)maxLifetime ) : sign < 0 ? 1-((float)TTL/(float)maxLifetime) : 0;
                Color = new Color(Color.R, Color.G, Color.B, alpha);
            }             
        }

        public void TransformSize(float sign)
        {
            if (TTL > 0)
            {
                float x = sign < 0 ? ((float)TTL / (float)maxLifetime) : sign > 0 ? 1 - ((float)TTL / (float)maxLifetime) : 0;
                Size = x * maxSize;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            spriteBatch.Draw(Texture, new Vector2(Position.X, Position.Y), sourceRectangle, Color, Angle, origin, Size, SpriteEffects.None, 0f);
        }

    }
}
