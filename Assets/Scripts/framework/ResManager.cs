using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResManager : MonoBehaviour
{
    public static Dictionary<string, GameObject> path2prefabs_ = new();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static GameObject LoadPrefab(string path)
    {
        if (path2prefabs_.ContainsKey(path))
        {
            return path2prefabs_[path];
        }

        GameObject prefab = Resources.Load<GameObject>(path);
        path2prefabs_[path] = prefab;
        return prefab;
    }
}
