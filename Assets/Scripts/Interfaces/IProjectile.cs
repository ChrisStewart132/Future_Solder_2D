using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Future_War_2D.Interfaces
{
    public interface IProjectile
    {
        void shoot(Vector2 dir);
        void set_colliders_ignored(List<Collider2D> colliders_ignored);
    }
}
