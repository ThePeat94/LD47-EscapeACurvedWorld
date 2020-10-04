using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 Dir { get; set; }
    
    private void Start()
    {
        Destroy(this.gameObject, 5f);
    }

    private void Update()
    {
        if(this.Dir != Vector3.zero)
            this.transform.Translate(this.Dir * 5f * Time.deltaTime);
    }
}