using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class SpawnedObjectsLabel : MonoBehaviour
{
    [SerializeField] private GenericPooler _pooler;
    private TMP_Text _label;

    private void Start()
    {
        _label = GetComponent<TMP_Text>();
        _pooler.onObjectSpawned += OnObjectSpawned;
        _pooler.onObjectDespawned += OnObjectDespawned;
    }

    private void OnDestroy()
    {
        if (_pooler != null)
        {
            _pooler.onObjectSpawned -= OnObjectSpawned;
            _pooler.onObjectDespawned -= OnObjectDespawned;
        }
    }

    private void OnObjectSpawned(int objectCount)
    {
        _label.text = "Prefabs spawned: " + objectCount;
    }

    private void OnObjectDespawned(int objectCount)
    {
        _label.text = "Prefabs spawned: " + objectCount;
    }
}
