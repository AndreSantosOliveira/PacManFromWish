/*
    ______  _____           
   |  ____||_   _|    /\    
   | |__     | |     /  \   
   |  __|    | |    / /\ \  
   | |      _| |_  / ____ \ 
   |_|     |_____|/_/    \_\    

Trabalho realizado por:
André Santos Oliveira  2021226714
José António Rodrigues 2021235353
Saulo José Mendes      2021235944
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class MadgeChase : GhostChase
{
    public LifePellet LifePellet;
    public SpriteRenderer spriteRenderer;
    protected override void OnTriggerEnter2D(Collider2D other)
    {

        //If life pellet hasn't been eaten yet
        if (LifePellet.gameObject.activeSelf)
        {
            //Madge's chase behavior
            //We only have to select the next direction to move to

            //instantiating the intersection node object
            Node node = other.GetComponent<Node>();


            //First check if this behaviour is enabled
            //and the ghost is not frightened
            if (node != null && isChasing() && !isFrightened())
            {
                //Directions and distances dictionary
                Dictionary<Vector2, float> distancias = new();

                //Get the available directions in this intersection
                List<Vector2> dirs = getAvailableDirections(node);




                //Add available directions and respective manhattan distances to dictionary
                for (int i = 0; i < dirs.Count; ++i)
                {
                    Vector2 posicaoAvanco = new Vector2(currentPosition()[0], currentPosition()[1]) + dirs[i];
                    float[] posicaoLifePellet = { 0.02f, -3.5f };
                    distancias.Add(dirs[i], distanciaManhattan(posicaoAvanco, new Vector2(posicaoLifePellet[0], posicaoLifePellet[1])));
                }

                //Sort dictionary from lesser to greater and extract 2 smallest distances
                var duasDirecoesMaisCurtasPacman = distancias
                        .OrderBy(pair => pair.Value) //sort
                        .Take(2) //take first two (smallest ones)
                        .Select(pair => pair.Key) //make dir list
                        .ToList(); //convert list

                //Avoid going back the same direction it came from
                if (duasDirecoesMaisCurtasPacman[0] == -currentDirection())
                {
                    //If the direction with the smallest manhattan value is equal to the opposite current 
                    //direction of the ghost, choose the second smallest instead
                    setDirection(duasDirecoesMaisCurtasPacman[1]);
                }

                else
                {
                    setDirection(duasDirecoesMaisCurtasPacman[0]);
                }
            }
        }

        else
        {
            // Change color to red
            spriteRenderer.color = new Color(1, 0, 0, 1);
            //Pellet eaten, enable rage mode
            base.OnTriggerEnter2D(other);
        }
    }

    float distanciaManhattan(Vector2 pointA, Vector2 pointB) { return Mathf.Abs(pointA.x - pointB.x) + Mathf.Abs(pointA.y - pointB.y); }

}