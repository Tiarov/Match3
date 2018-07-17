using UnityEngine;
using System.Collections.Generic;

public class ObjectPooling : ScriptableObject
{
    public List<PoolObject> Objects { get; private set; }
    public Transform ObjectsParent { get; private set; }

    void OnDestroy()
    {
        foreach (var o in Objects)
        {
            Destroy(o);
        }
    }

    public void Initialize(int count, GameObject go, Transform parent)
    {
        ObjectsParent = parent;

        Objects = new List<PoolObject>();
        for (int i = 0; i < count; i++)
        {
            AddObject(go, parent);
        }
    }

    public PoolObject GetPoolObject()
    {
        foreach (var obj in Objects)
        {
            if (obj.gameObject.activeInHierarchy == false)
            {
                return obj;
            }
        }

        AddObject(Objects[0].gameObject, ObjectsParent);
        return Objects[Objects.Count - 1];
    }

    private void AddObject(GameObject prefab, Transform parent)
    {
        var go = Instantiate(prefab.gameObject);
        var oP = go.GetComponent<PoolObject>();
        if (!oP)
            oP = go.AddComponent<PoolObject>();
        oP.ReturnToPool();

        go.name = prefab.name;
        go.transform.SetParent(parent);
        Objects.Add(go.GetComponent<PoolObject>());
    }
}