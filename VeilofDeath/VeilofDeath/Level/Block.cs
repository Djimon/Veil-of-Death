using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeilofDeath.Objects.Traps;

namespace VeilofDeath
{
    class Block
    {

        public bool isWalkable = true;
        /// <summary>
        /// Defines the grid positon
        /// </summary>
        Vector3 position;
        float scale = GameConstants.iBlockSize / 2;

        /// <summary>
        /// Look-up dictionary matching a string to the suitable model
        /// </summary>
        public Dictionary<string, Model> modelDictionary;

        /// <summary>
        /// Model of the block.
        /// </summary>
        public Model m_Block { get; private set; }

        /// <summary>
        /// <para>Gibt an, von welcher Art der Block ist.</para>
        /// <para>0 - Weg</para>
        /// <para>1 - Mauer</para>
        /// <para>2 - Loch</para>
        /// <para>3 - Startkachel</para>
        /// <para>4 - Zielkachel</para>
        /// <para>5 - Falle 1 </para>
        /// <para>6 - Falle 2 </para>
        /// <para>7 - Falle 3 </para>
        /// <para>8 - Falle 4 </para>
        /// </summary>
        public int blockType; //TODO: wieder private machen!!

        /// <summary>
        /// Set the model of this block.
        /// </summary>
        public void setModel(Model m)
        {
            this.m_Block = m;
        }

        /// <summary>
        /// return whether the Player can walk on this block.
        /// </summary>
        public bool getWalkable()
        {
            return this.isWalkable;
        }

        /// <summary>
        /// returns blocktype.
        /// </summary>
        public int type()
        {
            return this.blockType;
        }

        /// <summary>
        /// <para>Constructor</para>
        /// <para>The blocktype defines the model to be loaded</para>
        /// </summary>
        /// <param name="blockType">defines the Type of the block</param>
        /// <param name="pos">is the 2D psoition in the map (will be casted into 3D)</param>
        public Block(int blockType, Vector2 pos)
        {
            modelDictionary = GameConstants.levelDictionary;
            this.blockType = blockType;

            switch (blockType)
            {
                case 0: //weg (white)
                    {
                        m_Block = modelDictionary["weg"];
                        this.position = new Vector3(pos,GameConstants.fLevelHeight);
                        this.isWalkable = true;
                        break;
                    }

                case 1: //Mauer (grey)
                    {
                        this.m_Block = modelDictionary["mauer"];
                        this.position = new Vector3(pos, GameConstants.fLevelHeight); 
                        this.isWalkable = false;
                        break;
                    }
                case 2: //Loch (red)
                    {
                        this.m_Block = modelDictionary["stachelfalle"];
                        this.position = new Vector3(pos, GameConstants.fLevelHeight);
                        this.isWalkable = false;
                        GameManager.Instance.AddSpike(new SpikeTrap(new Vector3(pos, GameConstants.fLevelHeight),this));
                        //GameConstants.ListOfSpikeTraps.Add(new SpikeTrap(new Vector3(pos, GameConstants.fLevelHeight)));
                        //GameConstants.ListOfSpikeTraps.Add(new SpikeTrap(new Vector3(pos, GameConstants.fLevelHeight)));
                        break;
                    }
                case 3: //start (green)
                    {
                        this.m_Block = modelDictionary["start"];
                        this.position = new Vector3(pos, GameConstants.fLevelHeight);
                        GameManager.Instance.StartPos = this.position;
                        this.isWalkable = true;
                        break;
                    }
                case 4: //ziel (blue)
                    {
                        this.m_Block = modelDictionary["ende"];
                        this.position = new Vector3(pos, GameConstants.fLevelHeight);
                        this.isWalkable = true;
                        break;
                    }
                case 5: //Slowing Falle
                    {
                        this.m_Block= modelDictionary["slowtrap"];
                        this.position = new Vector3(pos, GameConstants.fLevelHeight);
                        this.isWalkable = false;
                        break;
                    }

                case 6: //Falle 2
                    {
                        this.m_Block = modelDictionary[""];
                        this.position = new Vector3(pos, GameConstants.fLevelHeight);
                        this.isWalkable = true;
                        break;
                    }
                case 7: //Falle 3
                    {
                        this.m_Block = modelDictionary[""];
                        this.position = new Vector3(pos, GameConstants.fLevelHeight);
                        this.isWalkable = true;
                        break;
                    }
                case 8: //Falle 4
                    {
                        this.m_Block = modelDictionary[""];
                        this.position = new Vector3(pos, GameConstants.fLevelHeight);
                        this.isWalkable = true;
                        break;
                    }
            }      

        }

        /// <summary>
        /// Draws each Levelblock
        /// </summary>
        public void Draw()
        {
            if (m_Block != null)
            {
                foreach (var mesh in m_Block.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.PreferPerPixelLighting = true;

                        //effect.World = GetWorldMatrix();
                        //effect.View = C.ViewMatrix;
                        //effect.Projection = C.SetProjectionsMatrix();
                        effect.World = GameConstants.MainCam.X_World * Matrix.CreateScale(scale) * Matrix.CreateTranslation(position);
                        effect.View = GameConstants.MainCam.X_View;
                        effect.Projection = GameConstants.MainCam.X_Projection;
                    }
                    mesh.Draw();
                }
            }
        }

        //Matrix GetWorldMatrix()
        //{
        //    const float circleRadius = 0;
        //    const float heightOffGround = 0;

        //    // this matrix moves the model "out" from the origin
        //    Matrix translationMatrix = Matrix.CreateTranslation(circleRadius, 0, heightOffGround);

        //    // this matrix rotates everything around the origin
        //    Matrix rotationMatrix = Matrix.Identity;

        //    // We combine the two to have the model move in a circle:
        //    Matrix combined = translationMatrix * rotationMatrix;

        //    return combined;
        //}

    }
}
