using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class MadgeChase : GhostChase
{
    public LifePellet LifePellet;
    private bool inCage = true; // Flag to track if the ghost is in its initial caged state

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        // If life pellet hasn't been eaten yet
        if (LifePellet.gameObject.activeSelf)
        {
            // Madge's chase behavior
            // We only have to select the next direction to move to

            // Instantiating the intersection node object
            Node node = other.GetComponent<Node>();

            // First check if this behaviour is enabled and the ghost is not frightened
            if (node != null && !isFrightened())
            {
                // If ghost is in cage, move towards the life pellet
                if (inCage)
                {
                    // If ghost is in cage, move towards the life pellet
                    Vector2 moveToCenter = (new Vector2(0.02f, -3.5f) - new Vector2(currentPosition()[0], currentPosition()[1]));
                    setDirection(moveToCenter);
                    inCage = false; // Update the flag to indicate that ghost has left its initial state
                }
                // If ghost is not in cage, find shortest path to the pellet
                else
                {
                    // Directions and distances dictionary
                    Dictionary<Vector2, float> distances = new Dictionary<Vector2, float>();

                    // Get the available directions in this intersection
                    List<Vector2> dirs = getAvailableDirections(node);

                    // Add available directions and respective manhattan distances to dictionary
                    foreach (Vector2 dir in dirs)
                    {
                        Vector2 newPosition = new Vector2(currentPosition()[0], currentPosition()[1]) + dir;
                        float distanceToPellet = Mathf.Abs(newPosition.x - 0.02f) + Mathf.Abs(newPosition.y + 3.5f);
                        distances.Add(dir, distanceToPellet);
                    }

                    // Sort dictionary by distances
                    var sortedDistances = distances.OrderBy(pair => pair.Value);

                    // Avoid going back the same direction it came from
                    if (sortedDistances.First().Key != -currentDirection())
                    {
                        // Set direction to the closest direction to the pellet
                        setDirection(sortedDistances.First().Key);
                    }
                    // Choose the second closest direction if the first one is the opposite of the current direction
                    else
                        setDirection(sortedDistances.ElementAt(1).Key);
                }
            }
        }
        else
        {
            //Change color to red
            GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);

            // Pellet eaten, enable rage mode
            base.OnTriggerEnter2D(other);
        }
    }
}
