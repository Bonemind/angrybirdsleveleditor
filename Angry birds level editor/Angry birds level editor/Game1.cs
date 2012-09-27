using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Angry_birds_level_editor
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        ArrayList baseObjectList = new ArrayList();
        ArrayList levelObjectList = new ArrayList();

        System.Windows.Forms.SaveFileDialog svDialog = new System.Windows.Forms.SaveFileDialog();

        KeyboardState aKeyboardState;
        KeyboardState previousKeyboardState;
  
        MouseState ms;
        MouseState previousMS;

        Texture2D cursorTexture;
        Vector2 mousePosition;

        bool objectSelected = false;
        LevelObject selectedObject;

        Texture2D originalCursorTexture;

        int prevScrollWheelValue = 0;

        Dictionary<String, Texture2D> textureList;

        public Game1()
        {
            
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
            //this.IsMouseVisible = true;
        }
        
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.ApplyChanges();
           
            textureList = Extensions.LoadContent<Texture2D>(Content, "");

            originalCursorTexture = Content.Load<Texture2D>("Exclude\\cursor");
            cursorTexture = originalCursorTexture;
            
            
            int currHeight = 0;
            LevelObject tmpObject;
            foreach (KeyValuePair<string, Texture2D> theTexture in textureList)
            {
                tmpObject = new LevelObject();
                tmpObject.Texture = theTexture.Value;
                tmpObject.Position = new Vector2(0, currHeight);
                tmpObject.Rectangle = new Rectangle((int) tmpObject.Origin.X - (tmpObject.Texture.Width / 2), (int) tmpObject.Origin.Y - tmpObject.Texture.Height / 2, tmpObject.Texture.Width, tmpObject.Texture.Height);
                tmpObject.Name = theTexture.Key;
                currHeight += theTexture.Value.Height + 10;
                baseObjectList.Add(tmpObject);
            }
            prevScrollWheelValue = ms.ScrollWheelValue;

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            mousePosition = new Vector2(ms.X, ms.Y);
            aKeyboardState = Keyboard.GetState();
            if (aKeyboardState.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
            ms = Mouse.GetState();
            if (ms.LeftButton == ButtonState.Pressed && ms.LeftButton != previousMS.LeftButton)
            {
                if (!objectSelected)
                {
                    foreach (LevelObject tmpObject in baseObjectList)
                    {
                        if (tmpObject.Rectangle.Contains(new Point(ms.X, ms.Y)))
                        {
                            cursorTexture = tmpObject.Texture;
                            selectedObject = new LevelObject();
                            selectedObject.Texture = tmpObject.Texture;
                            selectedObject.Position = tmpObject.Position;
                            selectedObject.Origin = tmpObject.Origin;
                            selectedObject.Bodytype = tmpObject.Bodytype;
                            selectedObject.Rectangle = tmpObject.Rectangle;
                            selectedObject.Rotation = tmpObject.Rotation;
                            selectedObject.Name = tmpObject.Name;
                            
                            objectSelected = true;
                        }
                    }
                    foreach (LevelObject tmpObject in levelObjectList)
                    {
                        if (tmpObject.Rectangle.Contains(new Point(ms.X, ms.Y)))
                        {
                            if (tmpObject.Bodytype == FarseerPhysics.Dynamics.BodyType.Dynamic)
                            {
                                tmpObject.Bodytype = FarseerPhysics.Dynamics.BodyType.Static;
                            }
                            else
                            {
                                tmpObject.Bodytype = FarseerPhysics.Dynamics.BodyType.Dynamic;
                            }
                        }
                    }
                    
                }
                else
                {
                    levelObjectList.Add(selectedObject);
                    objectSelected = false;
                    cursorTexture = originalCursorTexture;
                    
                    
                }
            }
            else if (ms.RightButton == ButtonState.Pressed && ms.RightButton != previousMS.RightButton)
            {
                foreach (LevelObject tmpObject in levelObjectList)
                {
                    if (tmpObject.Rectangle.Contains(new Point(ms.X, ms.Y)))
                    {
                        levelObjectList.Remove(tmpObject);
                        break;
                    }
                }
            }
            else if (ms.ScrollWheelValue != prevScrollWheelValue)
            {
                if (objectSelected && aKeyboardState.IsKeyUp(Keys.LeftControl))
                {
                    selectedObject.Rotation += ms.ScrollWheelValue - prevScrollWheelValue;
                }
                else if (objectSelected && aKeyboardState.IsKeyDown(Keys.LeftControl))
                {
                    selectedObject.Rotation += ((float) (ms.ScrollWheelValue - prevScrollWheelValue) / 700);
                    Console.WriteLine( selectedObject.Rotation.ToString());
                }
                prevScrollWheelValue = ms.ScrollWheelValue;
            }
            else if (aKeyboardState.IsKeyDown(Keys.Q) && aKeyboardState != previousKeyboardState)
            {
                selectedObject.Rotation -= (float)((5.0f/180.0f) * float.Parse(Math.PI.ToString()));
            }
            else if (aKeyboardState.IsKeyDown(Keys.E) && aKeyboardState != previousKeyboardState)
            {
                selectedObject.Rotation += (float) ((5.0f/180.0f) * float.Parse(Math.PI.ToString()));
            }
            else if (aKeyboardState.IsKeyDown(Keys.LeftControl) && aKeyboardState.IsKeyDown(Keys.S))
            {
                svDialog.ShowDialog();
                if (svDialog.FileName != "")
                {
                    TextWriter tw = new StreamWriter(svDialog.FileName);
                    foreach (LevelObject theObject in levelObjectList)
                    {
                        string currLine = "";
                        currLine = theObject.getShapeFromTextureName() + ",";
                        currLine += theObject.Name + ",";
                       
                        if (theObject.Bodytype == FarseerPhysics.Dynamics.BodyType.Dynamic)
                        {
                            currLine += "dynamic,";
                        }
                        else
                        {
                            currLine += "static,";
                        }
                        currLine += ConvertUnits.ToSimUnits(theObject.Position.X).ToString().Replace(',','.') + "," + ConvertUnits.ToSimUnits(theObject.Position.Y).ToString().Replace(',','.') + "," + theObject.Rotation.ToString().Replace(',','.');
                        tw.WriteLine(currLine);
                    }
                    tw.Close();
                }
            }

            if (objectSelected)
            {
                selectedObject.Position = new Vector2(ms.X, ms.Y);
                selectedObject.Rectangle = new Rectangle(ms.X, ms.Y, selectedObject.Texture.Width, selectedObject.Texture.Height);
            }
            prevScrollWheelValue = ms.ScrollWheelValue;
            previousMS = ms;
            previousKeyboardState = aKeyboardState;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            Color textureColor;
            foreach (LevelObject theTexture in baseObjectList)
            {
                spriteBatch.Draw(theTexture.Texture, theTexture.Rectangle, null,Color.White,theTexture.Rotation,new Vector2(0, 0),SpriteEffects.None,0);
            }
            foreach (LevelObject anObject in levelObjectList)
            {
                if (anObject.Bodytype == FarseerPhysics.Dynamics.BodyType.Dynamic)
                {
                    textureColor = Color.White;
                }

                else
                {
                    textureColor = Color.Black;
                }
                spriteBatch.Draw(anObject.Texture, anObject.Rectangle, null, textureColor, anObject.Rotation, new Vector2(anObject.Texture.Width / 2.0f, anObject.Texture.Height / 2.0f), SpriteEffects.None, 0);
            }
            if (objectSelected)
            {
                spriteBatch.Draw(selectedObject.Texture, selectedObject.Rectangle, null, Color.White, selectedObject.Rotation, new Vector2(selectedObject.Texture.Width / 2.0f, selectedObject.Texture.Height / 2.0f), SpriteEffects.None, 0);
            }
            else
            {
                spriteBatch.Draw(cursorTexture, mousePosition, Color.White);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
