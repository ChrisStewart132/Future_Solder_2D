using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Future_War_2D.Interfaces;

public class Gun : MonoBehaviour
{
    public Transform target; // Target to aim towards
    public GameObject projectile_prefab;
    public List<Collider2D> colliders_ignored;
    AudioSource sound;
    public bool player_controlled = false;
    public bool mouse_controlled = false;

    public int rpm = 60;
    private float cooldown = 0;

    void Awake()
    {
        sound = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    void FixedUpdate()
    {

        if (cooldown > 0)
            cooldown -= Time.fixedDeltaTime;


        bool can_shoot = cooldown <= 0;
        if(can_shoot && player_controlled && mouse_controlled && Input.GetMouseButton(0))
        {
            Vector3 mouse_pos = World.get_mouse_position();
            Vector3 dir = mouse_pos - transform.position;
            shoot(dir.normalized);
            cooldown = 60f / rpm;
        }
    }

    public void shoot(Vector3 dir)
    {
        // Rotate the gun to aim at the direction
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 270;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // create concrete IProjectile (projectile, rocekt, missile)
        GameObject go = GameObject.Instantiate(projectile_prefab);
        go.transform.position = transform.position;

        // get IProjectile interface and init
        IProjectile projectile = go.GetComponent<IProjectile>();
        projectile.set_colliders_ignored(colliders_ignored);
        projectile.shoot(dir.normalized);

        // if Missile...
        if(go.GetComponent<Missile>() != null)
        {
            go.GetComponent<Missile>().target = target;
        }

        sound.Play();
    }
}
