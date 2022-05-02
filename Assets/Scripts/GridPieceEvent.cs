using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridPieceEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Image gridImage;
    private Color colorDefault;
    [SerializeField] private Color colorOnValidMove = new Color(1, 1, 1, 1);
    [SerializeField] private Color colorOnHover = new Color(1, 1, 1, 1);
    private bool isValidPiece;
    private bool isHovered;

    // Start is called before the first frame update
    void Start()
    {
        gridImage = GetComponent<Image>();
        colorDefault = gridImage.color;
        isValidPiece = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;

        if (PlayerController.main.GetCanMove() && !PlayerController.main.IsMenuActive())
        {
            GameMatrix.main.SetCurrentMousePosition(GetCoords());

            if(GameMatrix.main.pieceCurrentlySelected)
            {
                //If the grid piece is a valid move and is not the gem selected piece, swap places with them
                if (isValidPiece)
                {
                    Vector3 currentPreviousSelected = GameMatrix.main.GetPreviousSelectedPiece();
                    Action swap = () => GameMatrix.main.SwapPieces(GetCoords(), currentPreviousSelected, true);
                    Debug.Log("First: (" + (int)currentPreviousSelected.x + "," + (int)currentPreviousSelected.y + ") | Second: (" + (int)GetCoords().x + "," + (int)GetCoords().y + ")");
                    GameMatrix.main.AddSwapToQueue(swap);
                }
                else
                {
                    Debug.Log("Outside Of Bounds!");
                    CheckForMoves(GameMatrix.main.GetPreviousMousePosition(), true);
                    GameMatrix.isValidMousePointer = false;
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (PlayerController.main.GetCanMove() && !PlayerController.main.IsMenuActive())
        {
            GameMatrix.isValidMousePointer = true;
            GameMatrix.main.SetGemSelected(GameMatrix.main.GetGemObject(GetCoords()));
            GameMatrix.main.SetOriginalCoordsPos(GetCoords());
            GameMatrix.main.GetGemSelected().SetIsSelected(true);
            GameMatrix.main.pieceCurrentlySelected = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (PlayerController.main.GetCanMove() && !PlayerController.main.IsMenuActive())
        {
            CheckForMoves(GameMatrix.main.GetCurrentMousePosition(), GameMatrix.isValidMousePointer);

            GameMatrix.main.SetOriginalCoordsPos(new Vector2(-1, -1));
            GameMatrix.isValidMousePointer = false;
        }
    }

    public void CheckForMoves(Vector2 mousePos, bool canSwap)
    {
        if(GameMatrix.main.GetGemSelected() != null)
            GameMatrix.main.GetGemSelected().SetIsSelected(false);

        //Check for any matches
        GameMatrix.main.StartMatchCheck();
        GameMatrix.main.ClearBoardHover();

        //If no matches have made made, swap the piece back to it's original position
        if (!GameMatrix.main.hasOneMatch && canSwap)
        {
            GameMatrix.main.ResetSwap(mousePos, GameMatrix.main.GetOriginalCoordsPos());
        }

        GameMatrix.main.SetGemSelected(null);
        GameMatrix.main.pieceCurrentlySelected = false;
    }

    public void OnValidMoveEnter()
    {
        gridImage.color = colorOnValidMove;
    }

    public void OnValidMoveExit()
    {
        gridImage.color = colorDefault;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameMatrix.main.SetPreviousMousePosition(GetCoords());
        isHovered = false;

        if (!GameMatrix.main.pieceCurrentlySelected && !PlayerController.main.IsMenuActive())
        {
            GameMatrix.main.ResetValidMovePieces();
            gridImage.color = colorDefault;
        }

        if (PlayerController.main.GetCanMove() && !PlayerController.main.IsMenuActive())
        {
            if(GameMatrix.main.pieceCurrentlySelected)
            {
                if (isValidPiece)
                    GameMatrix.main.SetPreviousSelectedPiece(GetCoords());
            }
        }
    }

    private Vector2 GetCoords()
    {
        string coords = name.Substring(name.Length - 5);
        return new Vector2(int.Parse("" + coords[1]), int.Parse("" + coords[3]));
    }

    private void Update()
    {
        if (PlayerController.main.GetCanMove() && !PlayerController.main.IsMenuActive())
        {
            if (!GameMatrix.main.pieceCurrentlySelected && isHovered)
            {
                GameMatrix.main.SetValidMovePieces(gameObject.name);
                gridImage.color = colorOnHover;
            }
        }
    }

    public bool IsValidPiece() { return isValidPiece; }
    public void SetValidPiece(bool validPiece) { isValidPiece = validPiece; }

}
