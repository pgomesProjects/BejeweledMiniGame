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

    // Start is called before the first frame update
    void Start()
    {
        gridImage = GetComponent<Image>();
        colorDefault = gridImage.color;
        isValidPiece = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!GameMatrix.main.pieceCurrentlySelected)
        {
            GameMatrix.main.SetValidMovePieces(gameObject.name);
            gridImage.color = colorOnHover;
        }
        else
        {
            //If the grid piece is a valid move and is not the gem selected piece, swap places with them
            if(isValidPiece)
            {
                GameMatrix.main.SwapPieces(GetCoords());
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameMatrix.main.SetGemSelected(GameMatrix.main.GetGemObject(GetCoords()));
        GameMatrix.main.GetGemSelected().SetIsSelected(true);
        GameMatrix.main.pieceCurrentlySelected = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameMatrix.main.GetGemSelected().SetIsSelected(false);
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
        if (!GameMatrix.main.pieceCurrentlySelected)
        {
            GameMatrix.main.ResetValidMovePieces();
            gridImage.color = colorDefault;
        }
        else
        {
            if(isValidPiece)
                GameMatrix.main.SetPreviousSelectedPiece(GetCoords());
        }
    }

    private Vector2 GetCoords()
    {
        string coords = name.Substring(name.Length - 5);
        return new Vector2(int.Parse("" + coords[1]), int.Parse("" + coords[3]));
    }

    public bool IsValidPiece() { return isValidPiece; }
    public void SetValidPiece(bool validPiece) { isValidPiece = validPiece; }

}
