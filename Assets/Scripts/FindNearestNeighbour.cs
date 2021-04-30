using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//***************
//This script should prioritize execution time (be as optimized as possible)
//If a new GameObject is instantiated with this script attached it should be taken in account by the other gameobjects.
//Display the closest gameobject by drawing a line in between the two gameobjects.
//
//***************

public class FindNearestNeighbour : MonoBehaviour
{
    private static List<GameObject> _allNeighbours = new List<GameObject>();

    private void Start()
    {
        OnNewNeighbour(gameObject);//Call a static function so every other cube has the updated list
    }

    private static void OnNewNeighbour(GameObject newNeighbour)
    {
        _allNeighbours.Add(newNeighbour);
    }

    private Vector3 GetNearestNeighbour()
    {
        float nearestDistance = float.MaxValue;
        GameObject nearestNeighbour = null;

        for (int i = 0; i < _allNeighbours.Count; i++)
        {
            if (!_allNeighbours[i].activeSelf || _allNeighbours[i] == gameObject)
            {
                continue;
            }

            float distance = (transform.position - _allNeighbours[i].transform.position).sqrMagnitude;//slightly more performant than Vector3.Distance which uses sqrt
            if (distance < nearestDistance * nearestDistance)
            {
                nearestDistance = distance;
                nearestNeighbour = _allNeighbours[i];
            }
        }

        return nearestNeighbour != null ? nearestNeighbour.transform.position : Vector3.zero;
    }

    private void Update()
    {
        Debug.DrawLine(transform.position, GetNearestNeighbour(), Color.blue);
    }
}
