using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{
    [SerializeField]
    private float _delay = 3f;

    private void Start()
    {
        Destroy(this.gameObject, _delay);   
    }
}
