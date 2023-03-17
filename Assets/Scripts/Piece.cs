using UnityEngine;

public class Piece : MonoBehaviour
{
    public Chess.Color color;
    public Chess.Type type;

    public bool hasMoved = false;

    [SerializeField]
    Sprite[] sprites;

    bool isStatic = true;
    Vector3 staticPosition;
    Chess chess;

    void Start()
    {
        chess = FindObjectOfType<Chess>();
        Reset();
    }

    void Reset()
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

    public void SetType(Chess.Type newType)
    {
        type = newType;
        Reset();
    }

    void Update()
    {
        if (!chess.isRunning || chess.turn != color) return;

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider == null || hit.collider.gameObject != gameObject) return;

            staticPosition = gameObject.transform.position;
            isStatic = false;
        }
        if (!isStatic && Input.GetMouseButtonUp(0))
        {
            isStatic = true;

            if (chess.CheckMove(Mathf.RoundToInt(staticPosition.x), Mathf.RoundToInt(staticPosition.y), Mathf.RoundToInt(gameObject.transform.position.x), Mathf.RoundToInt(gameObject.transform.position.y)))
            {
                gameObject.transform.position = new Vector3(Mathf.RoundToInt(gameObject.transform.position.x), Mathf.RoundToInt(gameObject.transform.position.y), staticPosition.z);
                hasMoved = true;
                chess.PromotePawn(X(), Y());
            }
            else
            {
                gameObject.transform.position = staticPosition;
            }
        }
        if (!isStatic && Input.GetMouseButton(0))
        {
            gameObject.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -0.1f);
        }
    }

    public int X()
    {
        return Mathf.RoundToInt(gameObject.transform.position.x);
    }

    public int Y()
    {
        return Mathf.RoundToInt(gameObject.transform.position.y);
    }
}
