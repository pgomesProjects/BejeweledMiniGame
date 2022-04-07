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
    public int currentRow = 6;
    [HideInInspector]
    public int currentCol = 0;

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
        yield return new WaitForSeconds(1);
        GameMatrix.main.LockPositions();
        //StartCoroutine(StartingMatches());
    }

    IEnumerator StartingMatches()
    {
        bool allMatchesMade = false;
        while (!allMatchesMade)
        {
            allMatchesMade = !CheckForStartingMatches();
            yield return new WaitForSeconds(1);
        }
    }

    public bool CheckForStartingMatches()
    {
        return CheckHorizontal();
    }

    private bool CheckHorizontal()
    {
        bool matchMade = false;
        int duplicateCounter = 0;
        for(int row = 0; row < GameMatrix.main.GetGemArray().GetLength(0); row++)
        {
            for(int col = 1; col < GameMatrix.main.GetGemArray().GetLength(1); col++)
            {
                //If the previous piece is equal to the first piece, there is a duplicate
                if(GameMatrix.main.GetGemArray()[row, col - 1].GetComponent<SpriteRenderer>().sprite == 
                    GameMatrix.main.GetGemArray()[row, col].GetComponent<SpriteRenderer>().sprite)
                {
                    duplicateCounter += 1;
                }
                else
                {
                    //If there are at least 2 previous duplicates, there is a match
                    if (duplicateCounter > 1)
                    {
                        DestroyHorizontal(row, col, duplicateCounter);
                        matchMade = true;
                    }
                    duplicateCounter = 0;
                }
            }
            //If there are at least 2 previous duplicates, there is a match
            if (duplicateCounter > 1)
            {
                DestroyHorizontal(row, 7, duplicateCounter);
                matchMade = true;
            }
            duplicateCounter = 0;
        }

        return matchMade;
    }

    private void DestroyHorizontal(int row, int col, int duplicateCounter)
    {
        for (int i = duplicateCounter; i >= 0; i--)
        {
            PlayerController.main.UpdateScore((GameMatrix.main.GetGemArray()[row, col - i - 1].GetScoreValue()));
            Destroy(GameMatrix.main.GetGemArray()[row, col - i - 1].gameObject);
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
