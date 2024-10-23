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
    public class Pelare
    {
        public Stack<Discs> tower;           // Deklarerar en stack här som kommer till användning i Game 1
        Texture2D pelareTexture;             // Texturen för pelaren

        public Vector2 pelarePosition;       // Positioner för vart de olika pelarna befinner sig
        Vector2 pelare1Origin;               // Origo för pelarna, Kanske onödigt?

        public bool discIsMoving = false;

        public int totalNumOfMoves;
        public int movesDone;                // Skapar 2 variabler som ska hantera runda samt totala antalet rundor
        int maxNumberOfDiscs;
        float pelareRotation;                // Rotationen är i det här fallet bara med för spriteBatch, satt till 0 i konstruktorn

        public Pelare(Texture2D pelareTexture, int maxNumberOfDiscs, Vector2 pelarePosition)       // Konstruktor med information som är bra att ha
        {
            tower = new Stack<Discs>();                              // Skapar en hänvisning till stacken tower här för att det ska kunna användas i Game1

            this.pelarePosition = pelarePosition;                    // Pelarnas position när de skrivs ut
            this.pelareTexture = pelareTexture;                      // Texturer hämtas från Game1

            pelare1Origin = new Vector2(pelareTexture.Width / 2, pelareTexture.Height);            // Pelarnas Origo, kanske onödig?

            this.maxNumberOfDiscs = maxNumberOfDiscs;                           // Beräknar totala numret förflyttningar i metoden GameIteration, baseras på hur många diskar i spelet
            totalNumOfMoves = (int)(Math.Pow(2, maxNumberOfDiscs) - 1);

            pelareRotation = 0.0f;                                                                 // Pelarnas rotation, kanske onödig?
        }
        public void DrawPelare(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pelareTexture, pelarePosition, null, Color.White, pelareRotation, pelare1Origin, 1.0f, SpriteEffects.None, 0.0f);   // Jag ritar ut pelarna med Draw i Game1
        }
        public void MoveBetweenTowers(Pelare pelare1, Pelare pelare2)           // Den här metoden fick jag hjälp med av en kamrat i klassen
        {
            if (pelare1.tower.Count() == 0)                                     // Metoden ser över de 2 stacksen som angetts och jämför dem i metoden
            {                                                             // Ifall pelare1 har noll diskar så tar den en disk från pelare2
                pelare1.tower.Push(pelare2.tower.Pop());                  // Push och Pop metod
                pelare1.tower.Peek().discIsMoving = true;                 // Pelaren hanteras som en Stack och tittar på översta disken samt sätter boolen till true
                pelare1.tower.Peek().DiscBeingMoved(pelare1);             // Metoden för förflyttning kallas och genomförs nu när boolen är true
            }
            else if (pelare2.tower.Count() == 0)                                // Ifall pelare2 har noll diskar tar den en disk från pelare1
            {
                pelare2.tower.Push(pelare1.tower.Pop());                  // Likadant som ovanför
                pelare2.tower.Peek().discIsMoving = true;
                pelare2.tower.Peek().DiscBeingMoved(pelare2);
            }
            else if (pelare1.tower.Count() > 0 && pelare2.tower.Count() > 0)    // Ifall båda tornen har skivor så jämför metoden storleken på den översta skivan i båda stacksen
            {
                Discs disc1 = pelare1.tower.Peek();                       // Det skapas en variabel som hänvisar till den översta disken i Stacken
                Discs disc2 = pelare2.tower.Peek();
                if (disc1.GetSize() < disc2.GetSize())                    // Jämför vilken skiva som är störst av de 2
                {
                    pelare2.tower.Push(pelare1.tower.Pop());              // Sedan bestämmer metoden vilken stack som ska ge en skiva till den andra stacken
                    pelare2.tower.Peek().discIsMoving = true;
                    pelare2.tower.Peek().DiscBeingMoved(pelare2);
                }
                else if (disc1.GetSize() > disc2.GetSize())               // Jämför vilken skiva som är störst av de 2
                {
                    pelare1.tower.Push(pelare2.tower.Pop());
                    pelare1.tower.Peek().discIsMoving = true;
                    pelare1.tower.Peek().DiscBeingMoved(pelare1);
                }
            }
        }
        public void GameIteration(Pelare tower1, Pelare tower2, Pelare tower3)        // En metod som ska hantera hela iteration
        {                                                                             // Fick mycket hjälp med den här metoden från klasskamrater samt google dokumentation                                               
            if (maxNumberOfDiscs % 2 == 0)                                            // Ifall totala antalet diskar % 2 == 0 körs den här loopen
            {
                if (movesDone < totalNumOfMoves && discIsMoving == false)                          // Så länge M.D är mindre än T.N.O.M samt boolen är false så fortsätter iterationen
                {
                    movesDone++;                                    // Höjer M.D med 1
                    discIsMoving = true;
                    if (movesDone % 3 == 1)
                    {
                        MoveBetweenTowers(tower1, tower2);          // tower1 och tower2 körs igenom metoden för att se vilken stack som ska skicka en disk
                        Console.WriteLine($"There has been: {movesDone} moves so far out of: {totalNumOfMoves} total moves\n\n");
                    }
                    else if (movesDone % 3 == 2)
                    {
                        MoveBetweenTowers(tower1, tower3);          // tower1 och tower3 körs igenom metoden för att se vilken stack som ska skicka en disk
                        Console.WriteLine($"There has been: {movesDone} moves so far out of: {totalNumOfMoves} total moves\n\n");
                    }
                    else if (movesDone % 3 == 0)
                    {
                        MoveBetweenTowers(tower2, tower3);          // tower2 och tower3 körs igenom metoden för att se vilken stack som ska skicka en disk
                        Console.WriteLine($"There has been: {movesDone} moves so far out of: {totalNumOfMoves} total moves\n\n");
                    }
                }
            }
            else if (maxNumberOfDiscs % 2 != 0)                           // Ifall totala antalet diskar % 2 != 0 så körs den här loopen istället
            {
                if (movesDone < totalNumOfMoves && discIsMoving == false)                          // Så länge M.D är mindre än T.N.O.M samt boolen är false så fortsätter iterationen
                {
                    movesDone++;                                    // Höjer M.D med 1
                    discIsMoving = true;
                    if (movesDone % 3 == 1)
                    {
                        MoveBetweenTowers(tower1, tower3);          // Den här loopen är i princip likadan som ovan med några få justeringar för vilka torn som körs
                        Console.WriteLine($"There has been: {movesDone} moves so far out of: {totalNumOfMoves} total moves\n\n");
                    }
                    else if (movesDone % 3 == 2)
                    {
                        MoveBetweenTowers(tower1, tower2);
                        Console.WriteLine($"There has been: {movesDone} moves so far out of: {totalNumOfMoves} total moves\n\n");
                    }
                    else if (movesDone % 3 == 0)
                    {
                        MoveBetweenTowers(tower2, tower3);
                        Console.WriteLine($"There has been: {movesDone} moves so far out of: {totalNumOfMoves} total moves\n\n");
                    }
                }
            }
        }
        public void isAnythingMoving(Stack<Discs> tower1, Stack<Discs> tower2, Stack<Discs> tower3)   // Den här metoden tar en titt ifall en disk är i rörelse
        {
            discIsMoving = false;
            foreach (Discs disc in tower1)
            {
                if (disc.discIsMoving == true)
                {
                    discIsMoving = true;
                }
            }
            foreach (Discs disc in tower2)
            {
                if (disc.discIsMoving == true)
                {
                    discIsMoving = true;
                }
            }
            foreach (Discs disc in tower3)
            {
                if (disc.discIsMoving == true)
                {
                    discIsMoving = true;
                }
            }
        }
    }
}



