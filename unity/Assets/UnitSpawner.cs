using System.Collections.Generic;

using UnityEngine;

public class UnitSpawner : BaseSingleton<UnitSpawner>
{
    [SerializeField]
    public List<GameObject> prefabs = new List<GameObject>();


    public void SpawnEntity(Vector3 pos)
    {
        GameObject newUnit = Instantiate(prefabs[0]) as GameObject;
        newUnit.transform.position = pos;
    }


}
