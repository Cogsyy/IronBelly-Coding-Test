using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//***************
//Goals:
// Should be as generic as possible
// Allows you to choose the amount of objects to put in the pool by default per object in the pool.
// Should allow expansion of the pool.
//
//***************

public class GenericPooler : MonoBehaviour
{
    [SerializeField] private List<PooledPrefabDefinition> _definitionsToPool;
    [SerializeField] private TMP_InputField _inputField;

    private List<GameObject> _currentPool = new List<GameObject>();

    public event Action<int> onObjectSpawned;//Anyone who wants to know when an object is spawned can subscribe to this event (The UI label)
    public event Action<int> onObjectDespawned;

    private int _objectsActive = 0;

    private void Start()
    {
        InitializeDefaultItems();
    }

    private void InitializeDefaultItems()
    {
        for (int i = 0; i < _definitionsToPool.Count; i++)
        {
            for (int j = 0; j < _definitionsToPool[i].initialAmount; j++)
            {
                CreateNewItemAndAddToPool(_definitionsToPool[i], startActive:true);
            }
        }
    }

    private GameObject CreateNewItemAndAddToPool(PooledPrefabDefinition definition, bool startActive)
    {
        //Not sure where in space they should go, will keep it at 0,0,0 for now.
        GameObject instance = Instantiate(definition.prefabToBePooled, transform);
        instance.SetActive(startActive);
        return AddToPool(instance);
    }

    private GameObject AddToPool(GameObject itemToAdd)
    {
        if (_currentPool.Contains(itemToAdd))
        {
            Debug.LogWarning("Adding an item to the pool that already exists");
        }
        else
        {
            _currentPool.Add(itemToAdd);
            _objectsActive++;
            onObjectSpawned?.Invoke(_objectsActive);
        }

        return itemToAdd;
    }

    /// <summary>
    /// Labelled as "Button_name" So the team is aware this function is called from the editor, since no usage can be found otherwise
    /// with a "Find all references". Called by the object named "Spawn button" in the editor
    /// </summary>
    public void Button_SpawnAmount()
    {
        if (_definitionsToPool.Count <= 0)
        {
            Debug.LogError("Error, nothing to spawn", this);
            return;
        }

        for (int i = 0; i < GetInputAmount(); i++)
        {
            //Spawn a random one
            PooledPrefabDefinition def = _definitionsToPool[UnityEngine.Random.Range(0, _definitionsToPool.Count)];
            SpawnItem(def);
        }
    }

    public void SpawnItem(PooledPrefabDefinition definition)
    {
        //First try find an existing item
        for (int i = 0; i < _currentPool.Count; i++)
        {
            if (!_currentPool[i].activeSelf)
            {
                _currentPool[i].SetActive(true);
                _objectsActive++;
                onObjectSpawned?.Invoke(_objectsActive);
                return;
            }
        }

        //Expand
        CreateNewItemAndAddToPool(definition, startActive: true);
    }

    /// <summary>
    /// Labelled as "Button_name" So the team is aware this function is called from the editor, since no usage can be found otherwise
    /// with a "Find all references". Called by the object named "Despawn button" in the editor
    /// </summary>
    public void Button_DespawnAmount()
    {
        DespawnRandomItem(GetInputAmount());
    }

    public void DespawnRandomItem(int amount)//Not exactly random, but despawns the first one it finds.
    {
        int despawned = 0;
        for (int i = 0; i < _currentPool.Count; i++)
        {
            if (_currentPool[i].activeSelf)
            {
                _currentPool[i].SetActive(false);
                despawned++;
                _objectsActive--;
                onObjectDespawned?.Invoke(_objectsActive);

                if (despawned >= amount)
                {
                    break;
                }
            }
        }
    }

    private int GetInputAmount()
    {
        if (_inputField.text.Length <= 0)
        {
            Debug.LogWarning("Did not enter a valid amount!", this);
            return 0;
        }
        else
        {
            int result;
            if (!int.TryParse(_inputField.text, out result))
            {
                Debug.LogWarning("Did not enter a valid amount!");
            }

            return result;
        }
    }

    [Serializable]
    public class PooledPrefabDefinition
    {
        [Header("Required Input")]
        public GameObject prefabToBePooled;

        [Header("Pool settings")]
        public int initialAmount = 5;//Comfortable default amount (In theory would be a number used in 90% of cases)
    }
}
