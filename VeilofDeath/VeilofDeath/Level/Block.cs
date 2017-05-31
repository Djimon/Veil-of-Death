﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeilofDeath
{
    class Block
    {

        public bool isWalkable = true;
        Vector3 position;

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
        /// Methode, die Textur des Blocks auf mitgegebene Textur wechselt.
        /// </summary>
        public void setModel(Model m)
        {
            this.m_Block = m;
        }

        /// <summary>
        /// Methode, die zurückgibt, ob der Block walkable ist.
        /// </summary>
        public bool getWalkable()
        {
            return this.isWalkable;
        }

        /// <summary>
        /// Methode, die Typen des jeweiligen Blocks zurückgibt.
        /// </summary>
        public int type()
        {
            return this.blockType;
        }

        /// <summary>
        /// <para>Konstruktor</para>
        /// <para>Zunächst wird Typ des Blocks festgelegt und anhand dessen die restlichen Variablen und die Textur zugeordnet.</para>
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
                        this.position = new Vector3(pos,GameConstants.LevelHeight);
                        this.isWalkable = true;
                        break;
                        Console.WriteLine(modelDictionary["weg"].ToString());
                    }

                case 1: //Mauer (grey)
                    {
                        this.m_Block = modelDictionary["mauer"];
                        this.position = new Vector3(pos, GameConstants.LevelHeight); 
                        this.isWalkable = false;
                        break;
                    }
                case 2: //Loch (red)
                    {
                        //this.m_Block = modelDictionary["none"];
                        this.position = new Vector3(pos, GameConstants.LevelHeight);
                        this.isWalkable = false;

                        break;
                    }
                case 3: //start (green)
                    {
                        this.m_Block = modelDictionary["start"];
                        this.position = new Vector3(pos, GameConstants.LevelHeight);
                        this.isWalkable = true;
                        break;
                    }
                case 4: //ziel (blue)
                    {
                        this.m_Block = modelDictionary["ende"];
                        this.position = new Vector3(pos, GameConstants.LevelHeight);
                        this.isWalkable = true;
                        break;
                    }
                case 5: //Falle 1
                    {
                        this.m_Block= modelDictionary[""];
                        this.position = new Vector3(pos, GameConstants.LevelHeight);
                        this.isWalkable = false;
                        break;
                    }

                case 6: //Falle 2
                    {
                        this.m_Block = modelDictionary[""];
                        this.position = new Vector3(pos, GameConstants.LevelHeight);
                        this.isWalkable = true;
                        break;
                    }
                case 7: //Falle 3
                    {
                        this.m_Block = modelDictionary[""];
                        this.position = new Vector3(pos, GameConstants.LevelHeight);
                        this.isWalkable = true;
                        break;
                    }
                case 8: //Falle 4
                    {
                        this.m_Block = modelDictionary[""];
                        this.position = new Vector3(pos, GameConstants.LevelHeight);
                        this.isWalkable = true;
                        break;
                    }
            }      

        }

        /// <summary>
        /// Draws each Levelblocks
        /// </summary>
        /// <param name="C">main camera</param>
        public void Draw(Camera C)
        {
            if (m_Block != null)
            {
                foreach (var mesh in m_Block.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.PreferPerPixelLighting = true;

                        effect.World = GetWorldMatrix();
                        effect.View = C.ViewMatrix;
                        effect.Projection = C.SetProjectionsMatrix();
                    }
                    mesh.Draw();
                }
            }
        }

        Matrix GetWorldMatrix()
        {
            const float circleRadius = 0;
            const float heightOffGround = 0;

            // this matrix moves the model "out" from the origin
            Matrix translationMatrix = Matrix.CreateTranslation(circleRadius, 0, heightOffGround);

            // this matrix rotates everything around the origin
            Matrix rotationMatrix = Matrix.Identity;

            // We combine the two to have the model move in a circle:
            Matrix combined = translationMatrix * rotationMatrix;

            return combined;
        }

    }
}