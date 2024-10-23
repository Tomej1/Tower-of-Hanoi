using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;

namespace Uppgift_4_test
{
    public class Game1 : Game          // Parprogrammering med Oleg
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Pelare pelare1;                     // Skapar 3 variabler av klassen Pelare
        private Pelare pelare2;
        private Pelare pelare3;

        SpriteFont RegularMove;                     // Olika SpriteFonts
        SpriteFont SpeedRun;

        Texture2D discTexture;
        Vector2 discPosition;                       // Disk information som ska skickas till de olika klasserna
        Vector2 discOrigin;

        Texture2D plattformTexture;                 // Information angående botten plattformen
        Vector2 plattFormPosition;

        Texture2D pelareTexture;                    // Texturen för pelarna
        Vector2 pelare1Position;                    // Anger de olika pelarnas positioner
        Vector2 pelare2Position;
        Vector2 pelare3Position;

        public bool gameIteration = false;          // Genom att trycka på space kan spelaren köra ett steg i taget
        private bool oneMove = false;              // Genom att trycka på enter kan spelaren speedruna programmet, rekommenderar att köra 8+ diskar!!

        int maxNumberOfDiscs = 5;                   // Användaren får ange valfritt antal diskar
        int discMaxHeight = 120;                    // Högsta höjden som disken ska förhålla sig till
        int discMaxWidth = 130;                     // diskarna får inte vara överstiga värdet i bredd
        int margin = 40;                            // En int som hjälper integern floor att ge ett värde från golvet
        int floor;                                  // Avstånd som pelarna och diskarna ska förhålla sig till från marken

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        protected override void Initialize()
        {
            floor = _graphics.PreferredBackBufferHeight - margin;       // floor är en variabel som beskriver avstånden från botten av skärmen upp till pelarna
            base.Initialize();
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            plattformTexture = Content.Load<Texture2D>("botten");       // Bottenplatta
            pelareTexture = Content.Load<Texture2D>("pelare1");         // Texturen för pelarna
            discTexture = Content.Load<Texture2D>("disk1");             // Texturen för disken

            RegularMove = Content.Load<SpriteFont>("Enter");            // Laddar in SpriteFonts
            SpeedRun = Content.Load<SpriteFont>("SpeedRun");

            plattFormPosition = new Vector2(_graphics.PreferredBackBufferWidth / 6 - 32, floor);                      // Plattformens position
            pelare1Position = new Vector2(_graphics.PreferredBackBufferWidth / 4, floor);                                  // Pelarnas position när de skrivs ut
            pelare2Position = new Vector2(_graphics.PreferredBackBufferWidth / 2, floor);
            pelare3Position = new Vector2(_graphics.PreferredBackBufferWidth / 2 + 200, floor);

            discPosition = new Vector2(_graphics.PreferredBackBufferWidth / 4 + 20 - pelareTexture.Width, floor);     // Information för diskens position och Origo
            discOrigin = new Vector2(discTexture.Width / 2, discTexture.Height);

            pelare1 = new Pelare(pelareTexture, maxNumberOfDiscs, pelare1Position);                                        // Pelarna matar in information från pelarens konstruktor
            pelare2 = new Pelare(pelareTexture, maxNumberOfDiscs, pelare2Position);
            pelare3 = new Pelare(pelareTexture, maxNumberOfDiscs, pelare3Position);

            for (int i = 0; i < maxNumberOfDiscs; i++)    // En for iteration som kallar på Push metoden i Pelare klassen och trycker in värdena från disk klassens konstruktor
            {
                pelare1.tower.Push(new Discs(discTexture,i , maxNumberOfDiscs, discPosition, discOrigin,
                    discMaxHeight, discMaxWidth, margin, _graphics.PreferredBackBufferHeight / 2));
            }
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
                                                                                        // Just den här biten hade jag väldigt svårt med, fick hjälp av en klasskamrat
            pelare1.isAnythingMoving(pelare1.tower, pelare2.tower, pelare3.tower);      // Den här metoden frågar programmet ifall någonting är i rörelse
            
            foreach (Discs disc in pelare1.tower)         // Bestämmer rörelsen olika moment för de olika tornen
            {
                disc.discDirectionAndSpeed();
            }
            foreach (Discs disc in pelare2.tower)
            {
                disc.discDirectionAndSpeed();
            }
            foreach (Discs disc in pelare3.tower)
            {
                disc.discDirectionAndSpeed();
            }


            if (Keyboard.GetState().IsKeyDown(Keys.Space))                              // När användaren är redo så kan den starta igång iterationen
            {
                gameIteration = true;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Space) && gameIteration == true)       // Iterationen kör långsamt tills den är klar
            {
                pelare1.GameIteration(pelare1, pelare2, pelare3);
            }


            if (Keyboard.GetState().IsKeyDown(Keys.Enter))                              // Ifall användaren har lust att flytta en disk i taget
            {
                 oneMove = true;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Space) && oneMove == true)             // En disk flyttas i taget
            {
                pelare1.GameIteration(pelare1, pelare2, pelare3);
                oneMove = false;
            }

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            pelare1.DrawPelare(_spriteBatch);       // Ritar ut texturerna för pelarna
            pelare2.DrawPelare(_spriteBatch);
            pelare3.DrawPelare(_spriteBatch);

           _spriteBatch.DrawString(SpeedRun,"Press Enter to make one move:", new Vector2(50,50), Color.Black);    // De olika SpriteFontsen som skrivs ut på skärmen
           _spriteBatch.DrawString(RegularMove, "Press Space to Start the Iteration(Enjoy):", new Vector2(50,80), Color.Black);

                                                    // Det här steget av att hålla reda på index för stacksen fick jag stor hjälp med från föreläsning 12 samt en klasskamrat

            for (int i = 0; i < pelare1.tower.Count(); i++)            // Kallar på metoden för att hålla reda på indexet i Pelare1
            {
                pelare1.tower.ElementAt(i).Draw(_spriteBatch);         // Ritar tornet och diskarna genom disk klassens Draw
            }
            for (int i = 0; i < pelare2.tower.Count(); i++)            // Hålla reda på indexet för Pelare2
            {
                pelare2.tower.ElementAt(i).Draw(_spriteBatch);         // Ritar ut genom disk klassens Draw
            }
            for (int i = 0; i < pelare3.tower.Count(); i++)            // Hålla reda på indexet för Pelare3
            {
                pelare3.tower.ElementAt(i).Draw(_spriteBatch);         // Ritar ut genom disk klassens Draw
            }

            _spriteBatch.Draw(plattformTexture, plattFormPosition, Color.White);    // Ritar ut plattformen

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}