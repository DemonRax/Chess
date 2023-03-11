using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Chess.Color color;
    public Chess.Type type;

    [SerializeField]
    Sprite[] sprites;

    void Start()
    {
        int sprite = 0;

        if (color == Chess.Color.White)
        {
            if (type == Chess.Type.Queen) sprite = 1;
            if (type == Chess.Type.Bishop) sprite = 2;
            if (type == Chess.Type.Knight) sprite = 3;
            if (type == Chess.Type.Rook) sprite = 4;
            if (type == Chess.Type.Pawn) sprite = 5;
        }
        else 
        {
            if (type == Chess.Type.King) sprite = 6;
            if (type == Chess.Type.Queen) sprite = 7;
            if (type == Chess.Type.Bishop) sprite = 8;
            if (type == Chess.Type.Knight) sprite = 9;
            if (type == Chess.Type.Rook) sprite = 10;
            if (type == Chess.Type.Pawn) sprite = 11;
        }

        gameObject.GetComponent<SpriteRenderer>().sprite = sprites[sprite];
        gameObject.name = color.ToString() + type.ToString();
    }

    void Update()
    {
        
    }
}
