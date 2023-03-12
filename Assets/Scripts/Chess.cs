using System.Collections.Generic;
using UnityEngine;

public class Chess : MonoBehaviour
{
    public enum Color { White, Black }
    public enum Type { King, Queen, Bishop, Knight, Rook, Pawn }

    public Color turn = Color.White;
    public GameObject parentOfPieces, piece;

    List<List<Piece>> pieces = new List<List<Piece>>(9);

    void Start()
    {
        for (int x = 0; x < pieces.Capacity; x++)
        {
            pieces.Add(new List<Piece>(pieces.Capacity));
            for (int y = 0; y < pieces.Capacity; y++)
            {
                pieces[x].Add(null);
            }
        }

        for (int x = 1; x < pieces.Capacity; x++)
        {
            SpawnPiece(x, 7, Color.Black, Type.Pawn);
            SpawnPiece(x, 2, Color.White, Type.Pawn);
        }

        List<Type> spawnOrder = new List<Type> { Type.Rook, Type.Knight, Type.Bishop, Type.Queen, Type.King, Type.Bishop, Type.Knight, Type.Rook };

        for (int x = 1; x < pieces.Capacity; x++)
        {
            SpawnPiece(x, 8, Color.Black, spawnOrder[x - 1]);
            SpawnPiece(x, 1, Color.White, spawnOrder[x - 1]);
        }
    }

    void Update()
    {

    }

    void SpawnPiece(int x, int y, Color color, Type type)
    {
        GameObject gamePiece = Instantiate(piece, new Vector3(x, y, 0), Quaternion.identity);
        gamePiece.transform.SetParent(parentOfPieces.transform);
        Piece p = gamePiece.GetComponent<Piece>();
        p.color = color;
        p.type = type;
        pieces[x][y] = p;
    }

    public bool CheckMove(int oX, int oY, int nX, int nY)
    {
        // Check outside borders
        if (nX < 1 || nX > pieces.Capacity - 1) return false;
        if (nY < 1 || nY > pieces.Capacity - 1) return false;

        Piece oP = pieces[oX][oY];
        if (oP == null) return false;
        Piece nP = pieces[nX][nY];

        // Check color
        if (oP.color != turn) return false;
        if (nP != null && nP.color == oP.color) return false;

        // Can move
        if (!CheckPieceMove(oX, oY, nX, nY)) return false;

        // Appove move, change color
        if (nP != null) DestroyImmediate(nP.gameObject);
        pieces[nX][nY] = oP;
        pieces[oX][oY] = null;
        turn = (turn == Color.White) ? Color.Black : Color.White;
        return true;
    }

    bool CheckPieceMove(int oX, int oY, int nX, int nY)
    {
        switch (pieces[oX][oY].type)
        {
            case Type.King:
                return IsOneAway(oX, oY, nX, nY);
            case Type.Rook:
                return IsStraight(oX, oY, nX, nY);
            case Type.Bishop:
                return IsDiagonal(oX, oY, nX, nY);
            case Type.Queen:
                return IsStraight(oX, oY, nX, nY) || IsDiagonal(oX, oY, nX, nY);
            case Type.Knight:
                return IsKnight(oX, oY, nX, nY);
            case Type.Pawn:
                return IsPawn(oX, oY, nX, nY, pieces[oX][oY].color, pieces[nX][nY]);
            default: return true;
        }
    }

    bool IsOneAway(int oX, int oY, int nX, int nY)
    {
        return Vector2.Distance(new Vector2(oX, oY), new Vector2(nX, nY)) < 2;
    }

    bool IsStraight(int oX, int oY, int nX, int nY)
    {
        if (oX != nX && oY != nY) return false;
        int sX = (oX > nX) ? nX : oX;
        int eX = (oX > nX) ? oX : nX;
        for (int x = sX + 1; x < eX; x++) if (pieces[x][oY] != null) return false;
        int sY = (oY > nY) ? nY : oY;
        int eY = (oY > nY) ? oY : nY;
        for (int y = sY + 1; y < eY; y++) if (pieces[oX][y] != null) return false;
        return true;
    }

    bool IsDiagonal(int oX, int oY, int nX, int nY)
    {
        if (((oX - nX) - (oY - nY) != 0) && ((oX - nX) + (oY - nY) != 0)) return false;
        int sX = (oX > nX) ? nX : oX;
        int eX = (oX > nX) ? oX : nX;
        int sY = (oX > nX) ? nY : oY;
        int dY = (((oX > nX) == (oY > nY)) ? 1 : -1);
        for (int i = 1; i < eX - sX; i++) if (pieces[sX + i][sY + i * dY] != null) return false;
        return true;
    }

    bool IsKnight(int oX, int oY, int nX, int nY)
    {
        return (Vector2.Distance(new Vector2(oX, oY), new Vector2(nX, nY)) > 2.23 && Vector2.Distance(new Vector2(oX, oY), new Vector2(nX, nY)) < 2.24);
    }

    bool IsPawn(int oX, int oY, int nX, int nY, Color color, Piece nP)
    {
        bool isWhite = color == Color.White;
        if (nP == null)
        {
            if (oX != nX) return false;
            if (oY == (isWhite ? 2 : 7) && oY - nY == (isWhite ? -2 : 2) && (pieces[oX][isWhite ? 3 : 6]) == null) return true;
            return (oY - nY == (isWhite ? -1 : 1));
        }
        else
        {
            return (Mathf.Abs(oX - nX) == 1 && oY - nY == (isWhite ? -1 : 1));
        }
    }
}