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

    private List<Vector2> gemDestroyQueue;
    private int[] gemSpawnQueue;
    //private float spawnTimer = 0.75f;
    private float currentTimer = 0;

    [SerializeField] private float gemSpawnDelayOnStart;
    [SerializeField] private float gemSpawnDelay;

    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private Color[] gemColors;

    private int chainMultiplier;

    private void Start()
    {
        allGemSpawners = GetComponentsInChildren<GemSpawner>();
        gemDestroyQueue = new List<Vector2>();
        gemSpawnQueue = new int[allGemSpawners.Length];
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
            yield return new WaitForSeconds(gemSpawnDelayOnStart);
        }
        yield return new WaitForSeconds(gemSpawnDelayOnStart);

        StartCoroutine(CheckForMatches());
    }

    public IEnumerator CheckForMatches()
    {
        GameMatrix.main.hasOneMatch = false;
        bool allMatchesMade = false;
        PlayerController.main.SetCanMove(false);

        bool swapQueueNotDone = false;

        chainMultiplier = 1;

        //Wait for the swap queue to be empty before making matches
        while (!GameMatrix.swapQueueEmpty)
        {
            swapQueueNotDone = true;
            yield return null;
        }

        //If the swap queue was not done, wait the swap time before making matches
        if (swapQueueNotDone)
        {
            yield return new WaitForSeconds(GameMatrix.swapTime);
            swapQueueNotDone = false;
        }

        //While matches can be made, detect matches
        while (!allMatchesMade)
        {
            allMatchesMade = !CheckIfMatchMade();
            //Check to see if there's been at least one match, stop checking if it turns true
            if (!GameMatrix.main.hasOneMatch)
            {
                GameMatrix.main.hasOneMatch = !allMatchesMade;
                if (GameMatrix.main.hasOneMatch)
                    Debug.Log("One Match Made");
                //If no matches have been made, immediately leave the match check
                else
                    break;
            }

            //If the chain multiplier is greater than 1, display it
            if(chainMultiplier > 1 && !allMatchesMade)
            {
                PlayerController.main.DisplayChain(chainMultiplier);
            }

/*            foreach (var i in GameMatrix.main.GetGemArray())
            {
                Debug.Log("Gem Array Object: " + i);
            }*/
            currentTimer = gemSpawnDelay + 0.25f;
            while(currentTimer > 0)
            {
                //Debug.Log("Current Timer: " + currentTimer);
                currentTimer -= Time.deltaTime;
                yield return null;
            }

            chainMultiplier++;
        }
        PlayerController.main.SetCanMove(true);
    }

    public bool CheckIfMatchMade()
    {
        bool isMatchMadeHorizontal = CheckHorizontal();
        bool isMatchMadeVertical = CheckVertical();
        //Destroy all matched gems
        DestroyMatches();
        //Spawn the gems in the queue
        StartCoroutine(SpawnQueue());
        return isMatchMadeHorizontal || isMatchMadeVertical;
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
                        AddToDestroyHorizontal(row, col, duplicateCounter);
                        matchMade = true;
                    }
                    duplicateCounter = 0;
                }
            }
            //If there are at least 2 previous duplicates, there is a match
            if (duplicateCounter > 1)
            {
                AddToDestroyHorizontal(row, GameMatrix.main.GetGemArray().GetLength(1), duplicateCounter);
                matchMade = true;
            }
            duplicateCounter = 0;
        }

        return matchMade;
    }

    private bool CheckVertical()
    {
        bool matchMade = false;
        int duplicateCounter = 0;
        for (int col = 0; col < GameMatrix.main.GetGemArray().GetLength(1); col++)
        {
            for (int row = 1; row < GameMatrix.main.GetGemArray().GetLength(0); row++)
            {
                //If the previous piece is equal to the first piece, there is a duplicate
                if (GameMatrix.main.GetGemArray()[row - 1, col].GetComponent<SpriteRenderer>().sprite ==
                    GameMatrix.main.GetGemArray()[row, col].GetComponent<SpriteRenderer>().sprite)
                {
                    duplicateCounter += 1;
                }
                else
                {
                    //If there are at least 2 previous duplicates, there is a match
                    if (duplicateCounter > 1)
                    {
                        AddToDestroyVertical(row, col, duplicateCounter);
                        matchMade = true;
                    }
                    duplicateCounter = 0;
                }
            }
            //If there are at least 2 previous duplicates, there is a match
            if (duplicateCounter > 1)
            {
                AddToDestroyVertical(GameMatrix.main.GetGemArray().GetLength(0), col, duplicateCounter);
                matchMade = true;
            }
            duplicateCounter = 0;
        }

        return matchMade;
    }

    private void CheckForDestroyDuplicates()
    {
        //Remove duplicates and sort array
        gemDestroyQueue = gemDestroyQueue.Distinct().OrderBy(x => x.x).ThenBy(x => x.y).ToList();
    }

    private void DestroyMatches()
    {
        CheckForDestroyDuplicates();

        int popSound = Random.Range(1, 4);
        AudioManager audioManager = FindObjectOfType<AudioManager>();

        if (gemDestroyQueue.Count != 0)
        {
            for (int i = 0; i < gemDestroyQueue.Count; i++)
            {
                //Get the score of the gem and destory the spawn
                PlayerController.main.UpdateScore(GameMatrix.main.GetGemObject(gemDestroyQueue[i]).GetScoreValue() * chainMultiplier);
                //Add gem to column spawn queue
                gemSpawnQueue[(int)gemDestroyQueue[i].y] += 1;
                //Spawn a particle when the gem is being destroyed and color it
                SpawnDestroyParticle(GameMatrix.main.GetGemObject(gemDestroyQueue[i]));
                Destroy(GameMatrix.main.GetGemObject(gemDestroyQueue[i]).gameObject);
                if(audioManager != null && PlayerController.main.IsGameActive())
                {
                    audioManager.PlayOneShot("Pop" + popSound, PlayerPrefs.GetFloat("SFXVolume", 0.5f));
                }
                GameMatrix.main.SetGemObject(gemDestroyQueue[i], null);
            }

            gemDestroyQueue.Clear();
        }
    }

    private void SpawnDestroyParticle(Gem currentGem)
    {
        ParticleSystem currentParticle = Instantiate(explosionParticle, currentGem.gameObject.transform.position, Quaternion.identity);
        ParticleSystem.MainModule particleSettings = currentParticle.main;
        particleSettings.startColor = gemColors[currentGem.GetID()];
    }

    IEnumerator SpawnQueue()
    {
        while(!IsSpawnQueueClear())
        {
            for(int col = 0; col < gemSpawnQueue.Length; col++)
            {
                if(gemSpawnQueue[col] > 0)
                {
                    gemSpawnQueue[col] -= 1;
                    allGemSpawners[col].SpawnGem(new Vector2(gemSpawnQueue[col], col));
                }
            }

            yield return new WaitForSeconds(gemSpawnDelay);
            currentTimer = gemSpawnDelay + 0.1f;
        }

        yield return null;

    }

    private void AddToDestroyHorizontal(int row, int col, int duplicateCounter)
    {
        for (int i = duplicateCounter; i >= 0; i--)
        {
            gemDestroyQueue.Add(new Vector2(row, col - i - 1));
            //gemSpawnQueue[col - i - 1] += 1;
        }
    }

    private void AddToDestroyVertical(int row, int col, int duplicateCounter)
    {
        for (int i = duplicateCounter; i >= 0; i--)
        {
            gemDestroyQueue.Add(new Vector2(row - i - 1, col));
            //gemSpawnQueue[col] += 1;
        }
    }

    private bool IsSpawnQueueClear()
    {
        foreach(var i in gemSpawnQueue)
        {
            if (i > 0)
                return false;
        }
        return true;
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
