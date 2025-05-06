using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform blueSoldier;
    private bool spawnControl = false;
    private Vector2 spawnPoint;
    private void Update()
    {
        if (spawnControl) { 
            if (spawnPoint.x > blueSoldier.position.x + 1f ||
                spawnPoint.x < blueSoldier.position.x - 1f &&
                spawnPoint.y > blueSoldier.position.y + 1f ||
                spawnPoint.y < blueSoldier.position.y - 1f)
            {
                RandomPosition();
            }
            else
            {
                spawnPoint.x = Random.Range(-15.5f, 15.5f);
                spawnPoint.y = Random.Range(-8f, 8f);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Bullet")
        {
            spawnPoint.x = Random.Range(-15.5f, 15.5f);
            spawnPoint.y = Random.Range(-8f, 8f);
            spawnControl = true;
        }
    }

    void RandomPosition()
    {
        this.gameObject.transform.position = new Vector2(spawnPoint.x, spawnPoint.y);
        spawnControl = false;
    }
}
