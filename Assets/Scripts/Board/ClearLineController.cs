using Assets.Scripts.Board;
using UnityEngine;

public class ClearLineController : MonoBehaviour
{
    private SpriteRenderer[] _spriteRenderers;

    public void SetLineSprites(Sprite[] sprites)
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < sprites.Length; i++)
            _spriteRenderers[i].sprite = sprites[i];
    }

    private void OnDestroy()
    {
        var board = FindObjectOfType<Board>();
        board.DestroyRedundantCells();
        //board.CreateMissingBlockedCells();
        //board.MoveBlockedCellsDown();
        // Тогда цвета то не те выставляются!!!
        board.RearrangeBlockedCells();
    }

    public void OnAnimationComplete()
    {
        Destroy(gameObject);
    }
}
