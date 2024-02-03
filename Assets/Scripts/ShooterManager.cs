using UnityEngine;
using System;
public class ShooterManager : MonoSingleton<ShooterManager>
{
    public event Action ShootingPerformedSuccessfullyEvent;

    [Header("References")]
    [SerializeField] GameObject ballPrefab;

    [Header("Cofiguration")]
    [SerializeField] bool doublePorjectile;
    [SerializeField] float force;
    [SerializeField] float targetZ;
    [SerializeField] Transform ejectTr;

    int projectileCount;
    private void Start()
    {
        InputManager.instance.MouseButtonDown += MouseButtonDown;

        projectileCount = 1;
        if (doublePorjectile) projectileCount = 2;
    }

    private void MouseButtonDown()
    {
        if (!GameManager.instance.isLevelActive) return;
        if (!BallCounterManager.instance.HasEnoughBall()) return;

        PerformShooting();
    }

    Vector3 GetWorldPoint()
    {
        Vector3 screenPoint = Input.mousePosition;
        screenPoint.z = targetZ;
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(screenPoint);
        return worldPoint;
    }

    void PerformShooting()
    {
        for (int i = 0; i < projectileCount; i++)
        {
            GameObject ball = Instantiate(ballPrefab, ejectTr.position + Vector3.back, Quaternion.identity);
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            Vector3 worldPoint = GetWorldPoint();

            rb.AddForce(worldPoint * force, ForceMode.Impulse);
        }

        ShootingPerformedSuccessfullyEvent?.Invoke();
    }
}
