using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventButton : MonoBehaviour
{
    [SerializeField] UnityEvent onMouseDown;
    private void OnMouseDown()
    {
        onMouseDown.Invoke();
    }
}
