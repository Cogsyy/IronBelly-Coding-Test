using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//***************
//Write a system making gameobjects with a specific script attached to them move randomly (3D space) within a zone that can be defined by three floats x,y,z exposed to the editor.
//This system should prioritize execution time (be as optimized as possible).
//
//***************

public class RandomMover : MonoBehaviour
{
    [Header("Pre defined movement zone")]
    [SerializeField] private float _x;
    [SerializeField] private float _y;
    [SerializeField] private float _z;
    [Header("Misc settings")]
    [SerializeField] private float _cubeMoveSpeed = 2;

    private Vector3 _destination;

    private void OnEnable()
    {
        //Spawn randomly in the zone
        transform.position = RandomPointInZone();
        StartCoroutine(MainBehaviourCo());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private Vector3 RandomPointInZone()
    {
        float randomX = Random.Range(0, _x);
        float randomY = Random.Range(0, _y);
        float randomZ = Random.Range(0, _z);
        return new Vector3(randomX, randomY, randomZ);
    }

    private IEnumerator MainBehaviourCo()
    {
        while (gameObject.activeSelf)
        {
            _destination = RandomPointInZone();
            yield return StartCoroutine(MoveToLocationCo());
        }
    }

    private IEnumerator MoveToLocationCo()
    {
        Vector3 startingPosition = transform.position;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * _cubeMoveSpeed;
            transform.position = Vector3.Lerp(startingPosition, _destination, t);
            yield return null;
        }
    }
}
