using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AutomonousAgent : Agent
{
    public Perception flockPerception;
    public AutonomousAgentData data;

    public float wanderAngle { get; set; } = 0;

    // Update is called once per frame 
    void Update()
    {
        var gameObjects = perception.GetGameObjects();
        foreach (var gameObject in gameObjects)
        {
            Debug.DrawLine(transform.position, gameObject.transform.position);
        }

        if (gameObjects.Length > 0)
        {
            movement.ApplyForce(Steering.Seek(this, gameObjects[0]) * data.seekWeight);
            movement.ApplyForce(Steering.Flee(this, gameObjects[0]) * data.fleeWeight);
            //movement.ApplyForce(Steering.Wander(this));
        }

        gameObjects = flockPerception.GetGameObjects();
        if (gameObjects.Length > 0)
        {
            movement.ApplyForce(Steering.Cohesion(this, gameObjects) * data.cohesionWeight);
            movement.ApplyForce(Steering.Separation(this, gameObjects, data.separationRadius) * data.separationWeight);
            movement.ApplyForce(Steering.Alignment(this, gameObjects) * data.alignmentWeight);
        }

        // Checks if current acceleration is lower than 10% of the max force then apply Wander force
        if (movement.acceleration.sqrMagnitude <= movement.maxForce * 0.1f)
        {
            movement.ApplyForce(Steering.Wander(this));
        }

        transform.position = Utilities.Wrap(transform.position, new Vector3(-10, -10, -10), new Vector3(10, 10, 10));
    }
}
