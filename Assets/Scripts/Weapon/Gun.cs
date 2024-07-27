using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject projectile_prefab;
    public List<Collider2D> colliders_ignored;
    AudioSource sound;
    void Awake()
    {
        sound = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    public void shoot(Vector3 dir)
    {
        // Rotate the gun to aim at the direction
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));


        GameObject go = GameObject.Instantiate(projectile_prefab);
        go.transform.position = transform.position;
        Projectile projectile = go.GetComponent<Projectile>();
        projectile.colliders_ignored = colliders_ignored;
        projectile.shoot(dir.normalized);
        sound.Play();
    }
}
