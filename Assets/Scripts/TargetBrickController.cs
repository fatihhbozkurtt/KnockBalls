using DG.Tweening;
using UnityEngine;

public class TargetBrickController : MonoBehaviour
{
    [Header("Configuration")]
    public int pointValue;
    bool collidedAlready;


    private void OnCollisionEnter(Collision other)
    {
        if (!GameManager.instance.isLevelActive) return;

        if (other.transform.TryGetComponent(out GroundBrickCatcher _))
        {
            if (collidedAlready) return;

            collidedAlready = true;
            GroundBrickCatcher.instance.IncrementCaughtBricksCount(targetBrick: this);
            Disappear();
        }
    }
    void Disappear()
    {
        transform.DOScale(Vector3.zero, 2).OnComplete(() => Destroy(gameObject));

    }

}


