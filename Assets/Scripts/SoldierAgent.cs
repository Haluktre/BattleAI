using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class SoldierAgent : Agent
{
    public GameObject enemy;
    public float moveSpeed = 5f;
    public float rotationSpeed = 200f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    private Rigidbody2D rb;
    private Vector2 spawnPoint;
    private Enemy enemyRef;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyRef = enemy.GetComponent<Enemy>();
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(enemy.transform.position.x);
        sensor.AddObservation(enemy.transform.position.y);
        sensor.AddObservation(this.gameObject.transform.position.x);
        sensor.AddObservation(this.gameObject.transform.position.y);
        sensor.AddObservation(this.gameObject.transform.rotation.z);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var moveAction = actions.DiscreteActions[0];
        var rotateAction = actions.DiscreteActions[1];
        var fireAction = actions.DiscreteActions[2];

        // Hareket
        Vector2 moveDirection = Vector2.zero;
        switch (moveAction)
        {
            case 0: moveDirection = Vector2.up; break;      // Yukarý
            case 1: moveDirection = Vector2.down; break;    // Aþaðý
            case 2: moveDirection = Vector2.right; break;   // Sað
            case 3: moveDirection = Vector2.left; break;    // Sol
            case 4: moveDirection = Vector2.zero; break;    // Dur
        }

        rb.MovePosition(rb.position + moveDirection.normalized * moveSpeed * Time.fixedDeltaTime);

        // Dönme
        if (rotateAction == 0)
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);  // Sola
        else if (rotateAction == 1)
            transform.Rotate(Vector3.back * rotationSpeed * Time.deltaTime);     // Saða
        
        // Ateþ
        if (fireAction == 0) { 
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            AddReward(-0.5f);
        }
        if (enemyRef.shotControl)
        {
            AddReward(10f);
            enemyRef.shotControl = false;
            EndEpisode();
        }
        AddReward(-0.1f);
    }

    public override void OnEpisodeBegin()
    {
        spawnPoint.x = Random.Range(-15.5f, 15.5f);
        spawnPoint.y = Random.Range(-8f, 8f);
        enemy.transform.position = new Vector2(spawnPoint.x, spawnPoint.y);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActions = actionsOut.DiscreteActions;

        // Hareket
        if (Input.GetKey(KeyCode.W)) discreteActions[0] = 0;
        else if (Input.GetKey(KeyCode.S)) discreteActions[0] = 1;
        else if (Input.GetKey(KeyCode.D)) discreteActions[0] = 2;
        else if (Input.GetKey(KeyCode.A)) discreteActions[0] = 3;
        else discreteActions[0] = 4;

        // Dönme
        if (Input.GetKey(KeyCode.Q)) discreteActions[1] = 0;
        else if (Input.GetKey(KeyCode.E)) discreteActions[1] = 1;
        else discreteActions[1] = 2;

        // Ateþ
        if (Input.GetKey(KeyCode.Space)) discreteActions[2] = 0;
        else discreteActions[2] = 1;
    }
}
