using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMatrix : MonoBehaviour
{
    [SerializeField] private GameObject[] gridPieceRows;
    [SerializeField] private GameObject gemHolder;
    private GridPieceEvent[,] gridPieceEvents;
    private Gem[,] gemObjects;

    public static GameMatrix main;

    private Vector2 currentSelectedPiece = new Vector2(-1, -1);

    private void Awake()
    {
        main = this;    
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeGridEventsArray();
    }

    private void InitializeGridEventsArray()
    {
        gridPieceEvents = new GridPieceEvent[gridPieceRows.Length, gridPieceRows.Length];
        for(int row = 0; row < gridPieceRows.Length; row++)
        {
            GridPieceEvent[] currentRow = gridPieceRows[row].GetComponentsInChildren<GridPieceEvent>();
            for(int col = 0; col < currentRow.Length; col++)
            {
                gridPieceEvents[row, col] = currentRow[col];
            }
        }
    }

    public void InitializeGemArray()
    {
        gemObjects = new Gem[gridPieceRows.Length, gridPieceRows.Length];
        Gem[] gemPieces = gemHolder.GetComponentsInChildren<Gem>();
        for (int row = 0; row < gridPieceRows.Length; row++)
        {
            for (int col = 0; col < gridPieceRows.Length; col++)
            {
                gemObjects[row, col] = gemPieces[(row * gridPieceRows.Length) + col];
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
            }

            for (int col = 0; col < gridPieceEvents.GetLength(1); col++)
            {
                if (col != (int)currentSelectedPiece.x)
                {
                    gridPieceEvents[col, (int)currentSelectedPiece.y].OnValidMoveExit();
                }
            }
        }
    }

    public void SetValidMovePieces(string name)
    {
        string coords = name.Substring(name.Length - 5);
        Debug.Log(coords);
        currentSelectedPiece = new Vector2(int.Parse(""+coords[1]), int.Parse("" + coords[3]));

        for (int row = 0; row < gridPieceEvents.GetLength(0); row++)
        {
            if (row != (int)currentSelectedPiece.y)
            {
                gridPieceEvents[(int)currentSelectedPiece.x, row].OnValidMoveEnter();
            }
        }

        for (int col = 0; col < gridPieceEvents.GetLength(1); col++)
        {
            if (col != (int)currentSelectedPiece.x)
            {
                gridPieceEvents[col, (int)currentSelectedPiece.y].OnValidMoveEnter();
            }
        }
    }
}
