using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SligShotArea : MonoBehaviour
{
    [SerializeField] private LayerMask _slingshotArea;
    public bool IsWithInSlingshotArea()
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(InputManager.MousePosition);
        if(Physics2D.OverlapPoint(worldPosition, _slingshotArea))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
