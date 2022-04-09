using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private List<Vector2> gemSpawnQueue;
    private float spawnTimer = 1;
    private float currentTimer = 0;

    private void Start()
    {
        allGemSpawners = GetComponentsInChildren<GemSpawner>();
        gemSpawnQueue = new List<Vector2>();
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

        //GameMatrix.main.LockPositions();

        StartCoroutine(StartingMatches());
    }

    IEnumerator StartingMatches()
    {
        bool allMatchesMade = false;
        while (!allMatchesMade)
        {
            allMatchesMade = !CheckForStartingMatches();
            foreach (var i in GameMatrix.main.GetGemArray())
            {
                Debug.Log("Gem Array Object: " + i);
            }
            currentTimer = spawnTimer;
            while(currentTimer > 0)
            {
                currentTimer -= Time.deltaTime;
                yield return null;
            }
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
                if (GameMatrix.main.GetGemArray()[row, col - 1].GetComponent<SpriteRenderer>().sprite ==
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

        if(gemSpawnQueue.Count != 0)
        {
            //Check for duplicate gem names in the queue
            CheckForDuplicates();
            //Spawn the gems in the queue
            StartCoroutine(SpawnQueue());
        }

        return matchMade;
    }

    private void CheckForDuplicates()
    {
        //Sort Vector2 list
        List<Vector2> orderedQueue = gemSpawnQueue.OrderBy(x => x.x).ThenBy(x => x.y).ToList();

        //Check for duplicates. If a duplicate coordinate is found, move it to the next row
        for (int i = 1; i < orderedQueue.Count; i++)
        {
            if(orderedQueue[i] == orderedQueue[i - 1])
            {
                orderedQueue.Add(new Vector2(orderedQueue[i].x + 1, orderedQueue[i].y));
                orderedQueue.Remove(orderedQueue[i]);
            }
        }

        //Sort list again once finished and save to list
        orderedQueue = orderedQueue.OrderBy(x => x.x).ThenBy(x => x.y).Reverse().ToList();
        gemSpawnQueue = orderedQueue;
    }

    IEnumerator SpawnQueue()
    {
        int row = (int)gemSpawnQueue[0].x;

        for(int i = 0; i < gemSpawnQueue.Count; i++)
        {
            if(gemSpawnQueue[i].x < row)
            {
                row -= 1;
                yield return new WaitForSeconds(0.5f);
            }
            allGemSpawners[(int)gemSpawnQueue[i].y].SpawnGem(gemSpawnQueue[i]);
            yield return null;
        }

        foreach (var i in GameMatrix.main.GetGemArray())
        {
            Debug.Log("Gem Array Object: " + i);
        }
    }

    private void DestroyHorizontal(int row, int col, int duplicateCounter)
    {
        for (int i = duplicateCounter; i >= 0; i--)
        {
            PlayerController.main.UpdateScore((GameMatrix.main.GetGemArray()[row, col - i - 1].GetScoreValue()));
            Destroy(GameMatrix.main.GetGemArray()[row, col - i - 1].gameObject);
            GameMatrix.main.GetGemArray()[row, col - i - 1] = null;
            gemSpawnQueue.Add(new Vector2(0, (col - i - 1)));
            //Adjust the names of the gems in their columns
            AdjustColumn(col - i - 1);
        }
    }

    private void AdjustColumn(int col)
    {
        int row = GameMatrix.main.GetGemArray().GetLength(0) - 1;

        for (int i = GameMatrix.main.GetGemArray().GetLength(1) - 1; i >= 0; i--)
        {
            if (GameMatrix.main.GetGemArray()[row, col] != null)
            {
                GameMatrix.main.GetGemArray()[row, col].gameObject.name = "Gem (" + i + "," + col + ")";
                GameMatrix.main.GetGemArray()[row, col] = GameMatrix.main.GetGemArray()[i, col];
                row -= 1;
            }
            else
            {
                continue;
            }
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
            //Debug.Log("===ROW FINISHED===");
        }
    }
}
