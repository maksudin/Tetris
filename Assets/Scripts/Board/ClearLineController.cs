using Assets.Scripts.Board;
using UnityEngine;

public class ClearLineController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

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
