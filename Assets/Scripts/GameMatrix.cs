using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMatrix : MonoBehaviour
{
    [SerializeField] private GameObject[] gridPieceRows;
    [SerializeField] private GameObject gemHolder;
    private GridPieceEvent[,] gridPieceEvents;
    private Gem[,] gemObjects;
    private GemSpawnerManager gemSpawnerManager;

    public static GameMatrix main;

    [HideInInspector]
    public bool pieceCurrentlySelected;
    private Gem currentGemSelected;
    private Vector2 currentSelectedPiece = new Vector2(-1, -1);
    private Vector2 originalSelectCoords = new Vector2(-1, -1);
    private Vector2 previousSelectedPiece = new Vector2(-1, -1);
    private Vector2 currentMousePos = new Vector2(-1, -1);

    [HideInInspector]
    public bool hasOneMatch = false;

    private void Awake()
    {
        main = this;
        gridPieceEvents = new GridPieceEvent[gridPieceRows.Length, gridPieceRows.Length];
        gemObjects = new Gem[gridPieceRows.Length, gridPieceRows.Length];
    }

    // Start is called before the first frame update
    void Start()
    {
        pieceCurrentlySelected = false;
        gemSpawnerManager = FindObjectOfType<GemSpawnerManager>();
        InitializeGridEventsArray();
    }

    private void InitializeGridEventsArray()
    {
        for(int row = 0; row < gridPieceRows.Length; row++)
        {
            GridPieceEvent[] currentRow = gridPieceRows[row].GetComponentsInChildren<GridPieceEvent>();
            for(int col = 0; col < currentRow.Length; col++)
            {
                gridPieceEvents[row, col] = currentRow[col];
            }
        }
    }

    public void InitializeGem(Gem currentGem, int col)
    {
        //Move down any other gems if needed
        ShiftDownColumn(col);

        //Place the gem in the top space and update the other gems
        PlaceGemInColumn(currentGem, col);
    }

    private void ShiftDownColumn(int col)
    {
        //If the row needs to be shifted down, shift down
        if(gemObjects[0, col] != null)
        {
            Gem[] gemColCopy = new Gem[gemObjects.GetLength(1)];

            int columnCounter = gemObjects.GetLength(1) - 1;
            for (int row = gemObjects.GetLength(1) - 1; row >= 0; row--)
            {
                if(gemObjects[row, col] != null)
                {
                    gemColCopy[columnCounter] = gemObjects[row, col];
                    columnCounter--;
                }
            }

            for (int row = 0; row < gemObjects.GetLength(1); row++)
            {
                gemObjects[row, col] = gemColCopy[row];
            }
        }
    }

    private void PlaceGemInColumn(Gem currentGem, int col)
    {
        bool gemPlaced = false;
        for (int row = 0; row < gemObjects.GetLength(1) - 1; row++)
        {
            if(gemObjects[(row + 1), col] == null)
            {
                continue;
            }
            else
            {
                gemObjects[row, col] = currentGem;
                gemPlaced = true;
                break;
            }
        }

        if (!gemPlaced)
        {
            gemObjects[gemObjects.GetLength(1) - 1, col] = currentGem;
        }
    }

    public void LockPositions()
    {
        for(int row = 0; row < gemObjects.GetLength(0); row++)
        {
            for(int col = 0; col < gemObjects.GetLength(1); col++)
            {
                gemObjects[row, col].SavePosition();
            }
        }
    }

    public void ResetValidMovePieces()
    {
        //If there's been a previous piece highlighted, reset the move pieces adjacent to it
        if(currentSelectedPiece != new Vector2(-1, -1))
        {
            for(int row = 0; row < gridPieceEvents.GetLength(0); row++)
            {
                if(row != (int)currentSelectedPiece.y)
                {
                    gridPieceEvents[(int)currentSelectedPiece.x, row].OnValidMoveExit();
                }

                gridPieceEvents[(int)currentSelectedPiece.x, row].SetValidPiece(false);
            }

            for (int col = 0; col < gridPieceEvents.GetLength(1); col++)
            {
                if (col != (int)currentSelectedPiece.x)
                {
                    gridPieceEvents[col, (int)currentSelectedPiece.y].OnValidMoveExit();
                }

                gridPieceEvents[col, (int)currentSelectedPiece.y].SetValidPiece(false);
            }
        }
    }

    public void SetValidMovePieces(string name)
    {
        string coords = name.Substring(name.Length - 5);
        //Debug.Log(coords);
        currentSelectedPiece = new Vector2(int.Parse(""+coords[1]), int.Parse("" + coords[3]));

        for (int row = 0; row < gridPieceEvents.GetLength(0); row++)
        {
            if (row != (int)currentSelectedPiece.y)
            {
                gridPieceEvents[(int)currentSelectedPiece.x, row].OnValidMoveEnter();
            }

            gridPieceEvents[(int)currentSelectedPiece.x, row].SetValidPiece(true);
        }

        for (int col = 0; col < gridPieceEvents.GetLength(1); col++)
        {
            if (col != (int)currentSelectedPiece.x)
            {
                gridPieceEvents[col, (int)currentSelectedPiece.y].OnValidMoveEnter();
            }

            gridPieceEvents[col, (int)currentSelectedPiece.y].SetValidPiece(true);
        }
    }

    public void ClearBoardHover()
    {
        foreach(var i in gridPieceEvents)
        {
            i.OnValidMoveExit();
        }
    }

    public Gem GetGemObject(Vector2 coords)
    {
        //Returns a gem object in the gem matrix
        return gemObjects[(int)coords.x, (int)coords.y];
    }

    public void SetGemObject(Vector2 coords, Gem gemValue)
    {
        gemObjects[(int)coords.x, (int)coords.y] = gemValue;
    }

    public void SwapPieces(Vector3 selectedPiece)
    {
        //Debug.Log("Swap Pieces!");
        //Debug.Log("First: " + gemObjects[(int)previousSelectedPiece.x, (int)previousSelectedPiece.y]);
        //Debug.Log("Second: " + gemObjects[(int)selectedPiece.x, (int)selectedPiece.y]);
        //Swap routine for gem pieces
        Gem temp = gemObjects[(int)previousSelectedPiece.x, (int)previousSelectedPiece.y];
        Vector3 tempPos = gemObjects[(int)previousSelectedPiece.x, (int)previousSelectedPiece.y].transform.position;

        gemObjects[(int)previousSelectedPiece.x, (int)previousSelectedPiece.y].transform.position = gemObjects[(int)selectedPiece.x, (int)selectedPiece.y].transform.position;
        gemObjects[(int)selectedPiece.x, (int)selectedPiece.y].transform.position = tempPos;
        gemObjects[(int)previousSelectedPiece.x, (int)previousSelectedPiece.y] = gemObjects[(int)selectedPiece.x, (int)selectedPiece.y];
        gemObjects[(int)selectedPiece.x, (int)selectedPiece.y] = temp;

        //Debug.Log("Swapped First: " + gemObjects[(int)previousSelectedPiece.x, (int)previousSelectedPiece.y]);
        //Debug.Log("Swapped Second: " + gemObjects[(int)selectedPiece.x, (int)selectedPiece.y]);
    }

    public void SwapPieces(Vector3 selectedPiece, Vector3 otherPiece)
    {
        Debug.Log("Swap Pieces!");
        Debug.Log("First: " + gemObjects[(int)otherPiece.x, (int)otherPiece.y]);
        Debug.Log("Second: " + gemObjects[(int)selectedPiece.x, (int)selectedPiece.y]);
        //Swap routine for gem pieces
        Gem temp = gemObjects[(int)otherPiece.x, (int)otherPiece.y];
        Vector3 tempPos = gemObjects[(int)otherPiece.x, (int)otherPiece.y].transform.position;

        gemObjects[(int)otherPiece.x, (int)otherPiece.y].transform.position = gemObjects[(int)selectedPiece.x, (int)selectedPiece.y].transform.position;
        gemObjects[(int)selectedPiece.x, (int)selectedPiece.y].transform.position = tempPos;
        gemObjects[(int)otherPiece.x, (int)otherPiece.y] = gemObjects[(int)selectedPiece.x, (int)selectedPiece.y];
        gemObjects[(int)selectedPiece.x, (int)selectedPiece.y] = temp;

        Debug.Log("Swapped First: " + gemObjects[(int)otherPiece.x, (int)otherPiece.y]);
        Debug.Log("Swapped Second: " + gemObjects[(int)selectedPiece.x, (int)selectedPiece.y]);
    }

    public Vector2 GetOriginalCoordsPos()
    {
        return originalSelectCoords;
    }

    public void SetOriginalCoordsPos(Vector2 coords)
    {
        originalSelectCoords = coords;
    }

    public void ResetSwap(Vector3 startingPosition, Vector3 endingPosition)
    {
        if(startingPosition != endingPosition)
        {
            Debug.Log("Starting Position: " + startingPosition);
            Debug.Log("Ending Position: " + endingPosition);

            //Reset pieces vertically
            if(startingPosition.x != endingPosition.x)
            {
                while (startingPosition.x != endingPosition.x)
                {
                    //Move up
                    if (startingPosition.x > endingPosition.x)
                    {
                        SwapPieces(startingPosition, new Vector2(startingPosition.x - 1, startingPosition.y));
                        startingPosition.x -= 1;
                    }
                    //Move down
                    else if (startingPosition.x < endingPosition.x)
                    {
                        SwapPieces(startingPosition, new Vector2(startingPosition.x + 1, startingPosition.y));
                        startingPosition.x += 1;
                    }
                }
            }

            //Reset pieces horizontally
            else if (startingPosition.y != endingPosition.y)
            {
                while (startingPosition.y != endingPosition.y)
                {
                    //Move left
                    if (startingPosition.y > endingPosition.y)
                    {
                        SwapPieces(startingPosition, new Vector2(startingPosition.x, startingPosition.y - 1));
                        startingPosition.y -= 1;
                    }
                    //Move right
                    else if(startingPosition.y < endingPosition.y)
                    {
                        SwapPieces(startingPosition, new Vector2(startingPosition.x, startingPosition.y + 1));
                        startingPosition.y += 1;
                    }
                    Debug.Log("New Starting Pos: " + startingPosition);
                    Debug.Log("Ending Pos: " + endingPosition);
                }
            }
        }
    }

    public void StartMatchCheck()
    {
        StartCoroutine(gemSpawnerManager.CheckForMatches());
    }

    public Vector2 GetCurrentMousePosition() { return currentMousePos; }
    public void SetCurrentMousePosition(Vector2 mousePos) { currentMousePos = mousePos; }
    public Vector3 GetPreviousSelectedPiece() { return previousSelectedPiece; }
    public void SetPreviousSelectedPiece(Vector3 selectedPiece) { previousSelectedPiece = selectedPiece; }
    public Gem[,] GetGemArray() { return gemObjects; }
    public Gem GetGemSelected() { return currentGemSelected; }
    public void SetGemSelected(Gem gemSelected) { currentGemSelected = gemSelected; }
}
