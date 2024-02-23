using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


public class PinkyChase : GhostChase
{
    protected override void OnTriggerEnter2D(Collider2D other)
    {
                Node node = other.GetComponent<Node>(); 
 
        if (node != null && isChasing() && !isFrightened()) {
            Dictionary<Vector2, float> distancias = new();

            List<Vector2> dirs = getAvailableDirections(node);

            for (int i = 0; i < dirs.Count; ++i) {
                Vector2 posicaoAvanco = new Vector2(currentPosition()[0], currentPosition()[1]) + dirs[i] * 4; //pegar no vetor direção do pacman
                distancias.Add(dirs[i], distanciaManhattan(posicaoAvanco, getPacmanPosition()));
            }

            var duasDirecoesMaisCurtasPacman = distancias
                    .OrderBy(pair => pair.Value) //ordenar pelo valor (distancia de manhattan)
                    .Take(2) //retiramos as duas chaves com valor mais baixo
                    .Select(pair => pair.Key) //fazer lista das direções
                    .ToList(); //converter para lista

            if (duasDirecoesMaisCurtasPacman[0] == -currentDirection()) {
                setDirection(duasDirecoesMaisCurtasPacman[1]);
            } else {
                setDirection(duasDirecoesMaisCurtasPacman[0]);  
            }
        }
    }

    float distanciaManhattan(Vector2 pointA, Vector2 pointB) { return Mathf.Abs(pointA.x - pointB.x) + Mathf.Abs(pointA.y - pointB.y);}

}
