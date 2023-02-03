using Assets.Scripts.Board;
using UnityEngine;

public class BlockedCell : MonoBehaviour
{
    public Piece Piece;
    public bool IsVisible => GetComponent<SpriteRenderer>().enabled;

    public void SetVisibility(bool visibility)
    {
        GetComponent<SpriteRenderer>().enabled = visibility;
    }

    public void SetPosition()
    {
        transform.position = new Vector3(Piece.XPos + 0.5f, Piece.YPos + 0.5f, transform.position.z);
    }


}
