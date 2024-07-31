using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Future_War_2D.Interfaces
{
    public interface IMovement
    {
        void Move(Vector2 direction);
        void Stop();
    }
}
