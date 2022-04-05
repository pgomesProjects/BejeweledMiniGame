using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridPieceEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image gridImage;
    private Color colorDefault;
    [SerializeField] private Color colorOnValidMove = new Color(1, 1, 1, 1);
    [SerializeField] private Color colorOnHover = new Color(1, 1, 1, 1);

    // Start is called before the first frame update
    void Start()
    {
        gridImage = GetComponent<Image>();
        colorDefault = gridImage.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameMatrix.main.SetValidMovePieces(gameObject.name);
        gridImage.color = colorOnHover;
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
        GameMatrix.main.ResetValidMovePieces();
        gridImage.color = colorDefault;
    }

}
