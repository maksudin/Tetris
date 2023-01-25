using Assets.Scripts.Board;
using UnityEngine;

public class ClearLineController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private SpriteRenderer[] _spriteRenderers;

    //private void Start()
    //{
    //    _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    //}

    public void SetLineSprites(Sprite[] sprites)
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < sprites.Length; i++)
            _spriteRenderers[i].sprite = sprites[i];
    }


    public void ClearAnimation()
    {
        _animator.SetTrigger("Clear");
    }

    private void OnDestroy()
    {
        var board = FindObjectOfType<Board>();
        board.DestroyRedundantCells();
        board.CreateMissingBlockedCells();
    }

    public void OnAnimationComplete()
    {
        Destroy(gameObject);
    }


}
