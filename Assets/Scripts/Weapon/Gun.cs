using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject projectile_prefab;
    public List<Collider2D> colliders_ignored;

    void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    public void shoot(Vector3 dir)
    {
        GameObject go = GameObject.Instantiate(projectile_prefab);
        go.transform.position = transform.position;
        Projectile projectile = go.GetComponent<Projectile>();
        projectile.colliders_ignored = colliders_ignored;
        projectile.shoot(dir.normalized);
    }
}
