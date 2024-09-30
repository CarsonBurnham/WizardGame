using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Artifact
{
public override void Use()
{
    Debug.Log("I cast Fireball");
}
public float speed = 20.0f;
void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }
public void collide()
{
    Debug.Log("Kaboom");
}
void OnTriggerEnter2D(Collider2D col)
    {
        collide();
        Destroy(gameObject);
    }


}

