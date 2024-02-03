using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleBallController : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] float disappearDuraiton;

    [Header("Debug")]
    [SerializeField] TrailRenderer trailRenderer;
    bool hitBlocks;
    private IEnumerator Start()
    {
        trailRenderer = GetComponentInChildren<TrailRenderer>();
        yield return new WaitForSeconds(disappearDuraiton);
        Disappear();
    }

    void Disappear()
    {
        transform.DOScale(Vector3.zero, disappearDuraiton).OnComplete(() => Destroy(gameObject));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hitBlocks) return; // since OnCollisionEnter is only used for hitting blocks condition, there is nothing wrong to use this early exit here 
        if (collision.transform.TryGetComponent(out TargetBrickController targetBlocks))
        {
            hitBlocks = true;
            if (trailRenderer == enabled) trailRenderer.enabled = false;
        }
    }
}
