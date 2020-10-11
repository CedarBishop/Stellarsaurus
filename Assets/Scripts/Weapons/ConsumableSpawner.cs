using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ConsumableSpawner : MonoBehaviour
{
    public List<Consumable> consumables = new List<Consumable>();

    [StringInList(typeof(StringInListHelper), "AllConsumablePrefabs")] public string[] consumablesPath;
    public float respawnTime = 5;


    private void Start()
    {
#if UNITY_EDITOR
        LoadConsumableFromPath();
#endif
        InitConsumable();
    }

    void InitConsumable()
    {
        if (consumables == null)
        {
            return;
        }
        if (consumables.Count == 0)
        {
            return;
        }

        Consumable consumable = Instantiate(consumables[Random.Range(0, consumables.Count)], transform.position, Quaternion.identity);
        consumable.OnPickUp += SpawnedConsumableIsGrabbed;
    }

    public void SpawnedConsumableIsGrabbed()
    {
        StartCoroutine("DelayConsumableRespawn");
    }

    IEnumerator DelayConsumableRespawn()
    {
        yield return new WaitForSeconds(respawnTime);
        InitConsumable();
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        LoadConsumableFromPath();
#endif
    }

    void LoadConsumableFromPath()
    {
        if (consumablesPath != null)
        {
            consumables.Clear();
            foreach (var item in consumablesPath)
            {
                consumables.Add(AssetDatabase.LoadAssetAtPath<Consumable>("Assets/Prefabs/Consumables/" + item));
            }
        }
    }
}
