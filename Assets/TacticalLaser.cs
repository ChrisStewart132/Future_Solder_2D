using UnityEngine;

public class TacticalLaser : MonoBehaviour
{
    public float maxRange = 10f;
    public LayerMask hitLayers;
    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void FixedUpdate()
    {
        ShootLaser();
    }

    void ShootLaser()
    {
        Vector3 startPosition = transform.position;
        Vector3 direction = transform.up; // Assuming the laser shoots to the right of the GameObject

        RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, maxRange, hitLayers);
        Vector3 endPosition;

        if (hit.collider != null)
        {
            endPosition = hit.point;
        }
        else
        {
            endPosition = startPosition + direction * maxRange;
        }

        DrawLaser(startPosition, endPosition);
    }

    void DrawLaser(Vector3 start, Vector3 end)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}
