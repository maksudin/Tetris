using Assets.Scripts.Board;
using UnityEngine;

public class BlockedCell : MonoBehaviour
{
    public Piece Piece;
    public bool IsVisible => GetComponent<SpriteRenderer>().enabled;
    [SerializeField] private Animator _animator;

    public void SetVisibility(bool visibility)
    {
        GetComponent<SpriteRenderer>().enabled = visibility;
        PlayAnimation();
    }

    public void SetSprite(Sprite sprite)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void PlayAnimation()
    {
        _animator.SetTrigger("show");
    }

    public void SetPosition()
    {
        transform.position = new Vector3(Piece.XPos + 0.5f, Piece.YPos + 0.5f, transform.position.z);
    }
}
