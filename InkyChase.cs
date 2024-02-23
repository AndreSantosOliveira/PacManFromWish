using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkyChase : GhostChase
{
    protected override void OnTriggerEnter2D(Collider2D other){
        //Inky's chase behavior
        //We only have to select the next direction to move to

        //instantiating the intersection node object
        Node node = other.GetComponent<Node>(); 
 
        //First check if this behaviour is enabled
        //and the ghost is not frightened
        if (node != null && isChasing() && !isFrightened()){
            //Get the available directions in this intersection
            List<Vector2> dirs = getAvailableDirections(node);
            int count = dirs.Count;

            //Choose a random direction and avoid going back
            //the same direction it came from
            int i = Random.Range(0, count);
            if (count > 1 && dirs[i] == -currentDirection())
            {
                i = (i + 1) % count;
            }
            setDirection(dirs[i]);        
        }
    }
}
