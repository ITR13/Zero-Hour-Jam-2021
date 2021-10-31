using System;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
    public Action OnClick { private get; set; }

    private void OnMouseDown()
    {
        OnClick?.Invoke();
    }
}
