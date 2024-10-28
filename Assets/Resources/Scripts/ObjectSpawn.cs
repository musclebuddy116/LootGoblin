using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectSpawn : MonoBehaviour
{
    [SerializeField] List<GameObject> objectPrefabs;
    GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
        obj = Instantiate(objectPrefabs[Random.Range(0, objectPrefabs.Count)], transform.position, Quaternion.identity, DungeonManager.singleton.GetCurrRoom().transform);
        
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
