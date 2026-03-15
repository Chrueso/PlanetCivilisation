using DG.Tweening;
using UnityEngine;

public class EntityView : MonoBehaviour
{
    public void Move(Vector3 position)
    {
        transform.DOMove(position, 1f).SetEase(Ease.InOutSine);
    }
}
