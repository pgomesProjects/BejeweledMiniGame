using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject gemPrefab;
    [SerializeField] private GameObject parent;

    private void Start()
    {
        StartCoroutine(SpawnAtStart());
    }

    IEnumerator SpawnAtStart()
    {
        for (int i = 0; i < 7; i++)
        {
            GameObject newObject = Instantiate(gemPrefab, transform.position, gemPrefab.transform.rotation);
            newObject.transform.SetParent(parent.transform);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
