using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridPieceEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image gridImage;
    private Color colorDefault;
    [SerializeField] private Color colorOnHover = new Color(1, 1, 1, 1);

    // Start is called before the first frame update
    void Start()
    {
        gridImage = GetComponent<Image>();
        colorDefault = gridImage.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        gridImage.color = colorOnHover;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gridImage.color = colorDefault;
    }

}
