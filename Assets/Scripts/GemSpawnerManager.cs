using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSpawnerManager : MonoBehaviour
{
    [HideInInspector]
    public int duplicateGemCounter = 0;
    [HideInInspector]
    public int previousGemSpawn = -1;
    [HideInInspector]
    public int tempPreventSpawn = -1;
    [HideInInspector]
    public int successfulSpawnCounter = 0;

    [HideInInspector]
    public int maxPieces = 3;
    [HideInInspector]
    public int threeMatchStartLimit = 2; 

    private GemSpawner[] allGemSpawners;

    private void Start()
    {
        allGemSpawners = GetComponentsInChildren<GemSpawner>();
        StartCoroutine(SpawnOnStart());
    }

    IEnumerator SpawnOnStart()
    {
        for(int i = 0; i < 7; i++)
        {
            for(int j = 0; j < allGemSpawners.Length; j++)
            {
                allGemSpawners[j].SpawnGemStart();
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void CheckForReset()
    {
        //If all gem spawners have been used, reset the values
        if (successfulSpawnCounter == 7)
        {
            duplicateGemCounter = 0;
            previousGemSpawn = -1;
            tempPreventSpawn = -1;
            successfulSpawnCounter = 0;
            Debug.Log("===ROW FINISHED===");
        }
    }
}
