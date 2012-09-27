using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FarseerPhysics.Factories;
using FarseerPhysics;
using FarseerPhysics.Dynamics;

namespace Angry_birds_level_editor
{
    class LevelObject
    {
        Vector2 objectPosition;
        BodyType objectType = BodyType.Dynamic;
        Texture2D objectTexture;
        Rectangle objectRectangle;
        Vector2 objectOrigin;
        string textureName = "";
        float objectRotation;

        public String Name
        {
            get
            {
                return textureName;
            }
            set
            {
                textureName = value;
            }
        }
        public Rectangle Rectangle
        {
            get
            {
                return objectRectangle;
            }
            set
            {
                objectRectangle = value;
            }
        }

        public Vector2 Position
        {
            get
            {
                return objectPosition;
            }
            set
            {
                objectPosition = value;
            }
        }

        public Vector2 Origin
        {
            get
            {
                return objectOrigin;
            }
            set
            {
                objectOrigin = value;
                objectOrigin.X += objectTexture.Width / 2;
                objectOrigin.Y += objectTexture.Height / 2;
            }
        }
        public float Rotation
        {
            get
            {
                return objectRotation;
            }
            set
            {
                objectRotation = value;
            }

        }

        public BodyType Bodytype
        {
            get
            {
                return objectType;
            }
            set
            {
                objectType = value;
            }
        }
        public Texture2D Texture
        {
            get
            {
                return objectTexture;
            }
            set
            {
                objectTexture = value;
            }
        }

        public string getShapeFromTextureName()
        {
            if (textureName.Equals("plank"))
            {
                return "woodblock";
            }
            else if (textureName.Equals("crate"))
            {
                return "woodblock";
            }
            else if (textureName.Equals("circle"))
            {
                return "birdblock";
            }
            else
            {
                Console.WriteLine("Could not find blocktype for name:" + textureName);
                return "woodblock";
            }

        }
    }
}
