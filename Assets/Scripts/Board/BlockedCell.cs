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


}
