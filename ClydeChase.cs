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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClydeChase : GhostChase
{
    protected override void OnTriggerEnter2D(Collider2D other)
    {

        //Clyde's chase behavior
        //We only have to select the next direction to move to

        //instantiating the intersection node object
        Node node = other.GetComponent<Node>();

        //First check if this behaviour is enabled
        //and the ghost is not frightened
        if (node != null && isChasing() && !isFrightened())
        {
            Vector3 nearestGhostPosition = getClosestGhostPosition();
            Vector2 direction = Vector2.zero;
            Dictionary<Vector2, float> distancias = new();
            foreach (Vector2 availableDirection in getAvailableDirections(node))
            {
                // Calculate new position and distance
                Vector3 newPosition = currentPosition() + new Vector3(availableDirection.x, availableDirection.y);
                float distance = Math.Abs(nearestGhostPosition.x - newPosition.x) + Math.Abs(nearestGhostPosition.y - newPosition.y);

                // Check if the direction already exists in the dictionary
                if (distancias.ContainsKey(availableDirection))
                {
                    // If the new distance is smaller than the existing one, update it
                    if (distance < distancias[availableDirection])
                    {
                        distancias[availableDirection] = distance; // Update the distance
                    }
                }
                else
                {
                    // If the direction is not in the dictionary, add it
                    distancias.Add(availableDirection, distance);
                }
            }


            //Sort dictionary from greater to lesser and extract 2 smallest distances
            var duasDirecoes = distancias
                    .OrderByDescending(pair => pair.Value) //sort
                    .Take(2) //take first two (biggest ones)
                    .Select(pair => pair.Key) //make dir list
                    .ToList(); //convert list

            //Avoid going back the same direction it came from
            if (duasDirecoes[0] == -currentDirection())
            {
                //If the direction with the biggest distance is equal to the opposite current 
                //direction of the ghost, choose the second biggest instead
                setDirection(duasDirecoes[1]);
            }
            else
                setDirection(duasDirecoes[0]);
        }
    }
}
