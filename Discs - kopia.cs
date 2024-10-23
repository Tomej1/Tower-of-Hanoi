using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Uppgift_4_test
{
    public class Discs
    {
        public Texture2D discTexture;           // Information angående disken
        public Vector2 discPosition;
        public Vector2 discOrigin;
        public Vector2 destination;

        public bool discIsMoving;       // En bool som gör att en disk kan röra på sig

        float discRotation;                     // Mer information angående disken som gör att jag kan justera höjd och bredd
        float speedX;
        float speedY;
        float maxY;

        int discNumber;
        int discHeight;
        int discWidth;

        // Maffig konstruktor med massor av godsaker
        public Discs(Texture2D discTexture, int discNumber, int maxNumberOfDiscs, Vector2 discPosition,
            Vector2 discOrigin, int discMaxHeight, int discMaxWidth, int margin, float maxY)
        {   
            this.discTexture = discTexture;                                                         // texturen för disken som skrivs ut
            this.discNumber = discNumber;                                                           // Antal diskar som använder anger i Game 1

            discWidth = discMaxWidth + 15 - (discMaxWidth * this.discNumber / maxNumberOfDiscs);    // Räknar ut hur mycket diskarna ska skilja sig bredd mässigt ju högre upp dem är på tornet
            this.discHeight = discMaxHeight / maxNumberOfDiscs;                                     // Blev lite tokigt här, diskarna ska förhålla sig till en viss höjd på pelaren
            this.maxY = maxY;                                                                       // maxY är en bestämd höjd ovanför staplarna

            this.discPosition.Y = discPosition.Y - (discNumber * this.discHeight);
            this.discPosition.X = discPosition.X;                                                   // Diskens Positioner i X och Y
            this.discOrigin = discOrigin;                                                           // Diskens origin, satt till mitten av disken
            discRotation = 0.0f;                                                                    // Diskens rotation, 
        }
        public int GetSize()                                // Returnerar diskens storlek till sändaren, storleken hämtas i konstruktorn i början
        {
            return discWidth;
        }
        public void DiscBeingMoved(Pelare pelare)           // En metod som hanterar diskens rörelse mellan pelarna
        {
            if (discIsMoving == true)                       // Information om diskens destination matas in här
            {
                speedY = -5f;
                destination.X = pelare.pelarePosition.X;
                destination.Y = pelare.pelarePosition.Y - (discHeight * pelare.tower.Count());
            }
        }
        public void discDirectionAndSpeed()         // En metod som säger till disken hur den ska röra sig
        {                                           // En klasskamrat hjälpte mig mycket med det här steget
            discPosition.X += speedX;
            discPosition.Y += speedY;

            if(discPosition.Y <= maxY)
            {
                speedY = 0f;
                if(discPosition.X <= destination.X)
                {
                    speedX = 5f;
                }
                if(discPosition.X >= destination.X)
                {
                    speedX = -5f;
                }
            }
            if(discPosition.X == destination.X)
            {
                speedX = 0f;
                speedY = 5f;
                if(discPosition.Y >= destination.Y + discHeight)        // Skivan måste förhålla sig till destination.Y + discHeight
                {
                    speedY = 0f;
                    speedX = 0f;
                    discPosition.Y = destination.Y + discHeight;        // Iterationen blir alltid rätt då den förhåller sig till diskhöjden

                    discIsMoving = false;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)           // En Draw metod som hanterar diskens alla värden vid utritning
        {
            spriteBatch.Draw(discTexture, new Rectangle((int)discPosition.X, (int)discPosition.Y, discWidth, discHeight),   // Information för Draw i Game1
                null, Color.White, discRotation, discOrigin, SpriteEffects.None, 0.0f);
        }
    }
}
