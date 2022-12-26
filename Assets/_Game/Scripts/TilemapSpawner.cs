using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using UnityEngine.Tilemaps;

public class TilemapSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectsToSpawn;
    [SerializeField] private int amountToSpawnMin = 1, amountToSpawnMax = 10;
    [SerializeField] private Vector2 objectDetectRadius;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private bool useObjectPool;
    [SerializeField] private int objectPoolMinCapacity = 10;
    [SerializeField] private int objectPoolMaxCapacity = 100;
    [SerializeField] private List<Vector3> availableSpawnLocations = new List<Vector3>();

    private ObjectPool<GameObject> pool;
    private List<GameObject> activeObjects = new List<GameObject>();

    private void Start()
    {
        SetupPool();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Keyboard.current.gKey.wasPressedThisFrame)
        {
            AddSpawnLocations();
            SpawnObject();
        }
        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            DespawnObjects();
        }
    }

    private void SetupPool()
    {
        if (useObjectPool)
        {
            pool = new ObjectPool<GameObject>(OnPoolObjectCreate, OnPoolObjectGet, OnPoolObjectRelease, OnPoolObjectDestroy, false, objectPoolMinCapacity, objectPoolMaxCapacity);
        }
    }

    private GameObject OnPoolObjectCreate()
    {
        GameObject obj = null;
        Vector3 randomPos = Vector3.zero;

        if(availableSpawnLocations.Count > 0)
        {
            randomPos = availableSpawnLocations[Random.Range(0, availableSpawnLocations.Count)];
        }
        obj = Instantiate(objectsToSpawn[Random.Range(0, objectsToSpawn.Count)], randomPos, Quaternion.identity);
        obj.transform.parent = transform;
        availableSpawnLocations.Remove(randomPos);
        if (!activeObjects.Contains(obj))
        {
            activeObjects.Add(obj);
        }

        return obj;
    }
    private void OnPoolObjectGet(GameObject obj)
    {
        if(availableSpawnLocations.Count > 0)
        {
            obj.transform.position = availableSpawnLocations[Random.Range(0, availableSpawnLocations.Count)];
        }
        if (!activeObjects.Contains(obj))
        {
            activeObjects.Add(obj);
        }
        obj.SetActive(true);
    }
    private void OnPoolObjectRelease(GameObject obj)
    {
        if (activeObjects.Contains(obj))
        {
            activeObjects.Remove(obj);
        }
        obj.SetActive(false);
    }
    private void OnPoolObjectDestroy(GameObject obj)
    {
        if (activeObjects.Contains(obj))
        {
            activeObjects.Remove(obj);
        }
        Destroy(obj);
    }

    public void DespawnObjects()
    {
        while (activeObjects.Count > 0)
        {
            for (int i = 0; i < activeObjects.Count; i++)
            {
                pool.Release(activeObjects[i]);
            }
        }
    }

    public void SpawnObject()
    {
        var amountToSpawn = Random.Range(amountToSpawnMin, amountToSpawnMax + 1);
        for (int i = 0; i < amountToSpawn; i++)
        {
            if(availableSpawnLocations.Count > 0)
            {
                AddSpawnLocations();
                if (useObjectPool)
                {
                    pool.Get();
                }
                else
                {
                    var randomPos = availableSpawnLocations[Random.Range(0, availableSpawnLocations.Count)];
                    var obj = Instantiate(objectsToSpawn[Random.Range(0, objectsToSpawn.Count)], randomPos, Quaternion.identity);
                    obj.transform.parent = transform;
                    availableSpawnLocations.Remove(randomPos);
                }
            }
            else { break; }
        }

    }

    public void AddSpawnLocations()
    {
        availableSpawnLocations.Clear();
        foreach (var pos in groundTilemap.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            Vector3 place = groundTilemap.CellToWorld(localPlace);
            if (groundTilemap.HasTile(localPlace))
            {
                var objHere = GetObjectsAtPosition(place, objectDetectRadius, 0f);
                if (!objHere.objectFound)
                {
                    availableSpawnLocations.Add(place);
                }
                else
                {
                    for (int i = 0; i < objHere.colliders.Count; i++)
                    {
                        var obj = objHere.colliders[i];
                        if (obj != null)
                        {
                            Debug.Log($"Object: {obj.name} at X:{place.x} Y:{place.y} Z:{place.z}");
                        }
                    }
                }
            }
        }
    }

    public (bool objectFound, List<Collider2D> colliders) GetObjectsAtPosition(Vector2 position, Vector2 radius, float angle)
    {
        Collider2D[] intersecting = Physics2D.OverlapBoxAll(position, radius, angle);
        return (intersecting.Length != 0, intersecting.ToList());
    }
}
