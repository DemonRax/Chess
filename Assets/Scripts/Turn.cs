using TMPro;
using UnityEngine;

public class Turn : MonoBehaviour
{
    Chess chess;
    TMP_Text text;

    void Start()
    {
        chess = GameObject.FindGameObjectWithTag("GameController").GetComponent<Chess>();
        text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        text.text = "Turn " + chess.turn;
        text.color = (chess.turn == Chess.Color.White ? Color.white : Color.black);
    }
}