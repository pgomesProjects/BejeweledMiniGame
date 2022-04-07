using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gem: MonoBehaviour
{
    [SerializeField] private int scoreValue;
    private bool isSelected;
    [SerializeField] private Vector3 previousPosition;
    [SerializeField] private Vector3 originalPosition;
    private float previousGravity;

    private void Awake()
    {
        isSelected = false;
        previousGravity = GetComponent<Rigidbody2D>().gravityScale;
    }

    private void Update()
    {
/*        if (isSelected)
        {
            Vector3 mousePos = Mouse.current.position.ReadValue();
            mousePos.z = Camera.main.nearClipPlane;
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePos);
            transform.position = mouseWorldPosition;
        }*/
    }

    public void SetSelectedProperties()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0;
        previousPosition = transform.position;
    }

    public void ResetPiece()
    {
        GetComponent<Rigidbody2D>().gravityScale = previousGravity;
        transform.position = previousPosition;
    }

    public void SavePosition()
    {
        originalPosition = transform.position;
        previousPosition = transform.position;
    }

    public Vector3 GetPreviousPosition() { return previousPosition; }
    public void SetPreviousPosition(Vector3 position) { previousPosition = position; }
    public Vector3 GetOriginalPosition() { return originalPosition; }
    public bool GetIsSelected() { return isSelected; }
    public void SetIsSelected(bool selected) { isSelected = selected; }
    public int GetScoreValue() { return scoreValue; }
    public void SetScoreValue(int value) { scoreValue = value; }
}
