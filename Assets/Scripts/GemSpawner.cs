using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] gemPrefabs;
    [SerializeField] private GameObject parent;

    [SerializeField] private int[] usedGems;

    private GemSpawnerManager gemManager;

    private void Awake()
    {
        usedGems = new int[gemPrefabs.Length];
        gemManager = GetComponentInParent<GemSpawnerManager>();
    }

    public void SpawnGemStart()
    {
        //If 3 of the same gem have spawned, prevent it from spawning a 4th
        if (gemManager.duplicateGemCounter == gemManager.maxPieces - 1)
        {
            Debug.Log("Max Pieces Have Been Spawned!");
            gemManager.tempPreventSpawn = gemManager.previousGemSpawn;
            usedGems[gemManager.tempPreventSpawn] = 1;
            
            //If the max amount of match three pieces have been spawned, limit the spawn to only 2 pieces
            gemManager.threeMatchStartLimit--;
            if(gemManager.threeMatchStartLimit == 0)
            {
                Debug.LogWarning("ONLY 2 ROW PIECES TOGETHER FOR NOW.");
                gemManager.maxPieces = 2;
            }
        }

        //If all gems have been spawned, clear the array
        if (HaveAllGemsSpawned())
            usedGems = new int[gemPrefabs.Length];

        int index = 0;
        //Variables that check whether a duplicate item has spawned
        bool validGem = false;
        int counter = 0;

        //Keep generating a gem a max of 3 times until it uses one that hasn't been spawned yet
        while (!validGem && counter < 100)
        {
            index = Random.Range(0, gemPrefabs.Length);
            validGem = !HasGemSpawned(index);

            //If the gem manager does not want the current gem to spawn, invalidate the pick and try again
            if (index == gemManager.tempPreventSpawn)
                validGem = false;

            counter++;

            if (counter == 100)
                Debug.Log("Failsafe Reached!");
        }

        //Tell the array that the item has been used before
        usedGems[index] = 1;

        //Tell the manager a gem spawned successfully
        gemManager.successfulSpawnCounter++;

        //If this gem is the same as the previous gem, tell the manager there's a duplicate
        if (gemManager.previousGemSpawn == index)
            gemManager.duplicateGemCounter++;
        else
            gemManager.duplicateGemCounter = 0;

        //Tell the gem manager what the last gem spawned was
        gemManager.previousGemSpawn = index;

        Debug.Log("Previous Gem Spawn: " + gemManager.previousGemSpawn);

        //Check to see if the gem manager needs to be reset
        gemManager.CheckForReset();

        Gem newObject = Instantiate(gemPrefabs[index].GetComponent<Gem>(), transform.position, gemPrefabs[index].transform.rotation);
        newObject.gameObject.name = "Gem (" + gemManager.currentRow + "," + gemManager.currentCol + ")";
        newObject.gameObject.transform.SetParent(parent.transform);
        GameMatrix.main.InitializeGem(newObject, gemManager.currentRow, gemManager.currentCol);

        gemManager.currentCol += 1;

        //If the last gem in a row has been spawned, decrement the row counter and reset the col counter
        if (gemManager.currentCol == 7)
        {
            gemManager.currentRow -= 1;
            gemManager.currentCol = 0;
        }
    }

    private bool HasGemSpawned(int index)
    {
        return usedGems[index] == 1;
    }

    private bool HaveAllGemsSpawned()
    {
        for (int i = 0; i < usedGems.Length; i++)
            if (usedGems[i] == 0)
                return false;
        return true;
    }
}
