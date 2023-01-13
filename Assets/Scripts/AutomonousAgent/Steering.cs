using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;

public static class Steering
{
    public static Vector3 Seek(Agent agent, GameObject target)
    {
        Vector3 force = CalculateSteering(agent, (target.transform.position - agent.transform.position));
        return force;
    }

    public static Vector3 Flee(Agent agent, GameObject target)
    {
        Vector3 force = CalculateSteering(agent, (agent.transform.position - target.transform.position));
        return force;
    }
    public static Vector3 CalculateSteering(Agent agent, Vector3 direction)
    {
        Vector3 ndirection = direction.normalized;
        Vector3 desired = ndirection * agent.movement.maxSpeed;
        Vector3 steer = desired - agent.movement.velocity;
        Vector3 force = Vector3.ClampMagnitude(steer, agent.movement.maxForce);

        return force;
    }


    public static Vector3 Wander(AutomonousAgent agent)
    {
        // randomly adjust angle +/- displacement 
        agent.wanderAngle = agent.wanderAngle + Random.Range(-agent.data.wanderDisplacement, agent.data.wanderDisplacement);
        // create rotation quaternion around y-axis (up) 
        Quaternion rotation = Quaternion.AngleAxis(agent.wanderAngle, Vector3.up);
        // calculate point on circle radius 
        Vector3 point = rotation * (Vector3.forward * agent.data.wanderRadius);
        // set point in front of agent at distance length 
        Vector3 forward = agent.transform.forward * agent.data.wanderDistance;

        Debug.DrawRay(agent.transform.position, forward + point, Color.magenta);

        Vector3 force = CalculateSteering(agent, forward + point);

        return force;


    }
    public static Vector3 Cohesion(Agent agent, GameObject[] neighbors)
    {
        // Get center position of neighbors
        Vector3 center = Vector3.zero;
        foreach (GameObject neighbor in neighbors)
        {
            // Add position of neighbor to center
            center += neighbor.transform.position;
        }
        // Calculate the center by dividing center by the number of neighbors
        center /= neighbors.Length;
        // Steer towards center
         Vector3 force = CalculateSteering(agent, center - agent.transform.position);
        return force;
    }

    public static Vector3 Separation(Agent agent, GameObject[] neighbors, float radius)
    {
        Vector3 separation = Vector3.zero;
        // Accumalate separation vector of neighbors
        foreach (GameObject neighbor in neighbors)
        {
            // Create separation direction (neighbor position <- agent position)
            Vector3 direction =  neighbor.transform.position - agent.transform.position;
            if (direction.magnitude < radius)
            {
                separation += direction / direction.sqrMagnitude;
            }
        }
        // Steer toward separation
        Vector3 force = CalculateSteering(agent, separation);
        return force;

    }

    public static Vector3 Alignment(Agent agent, GameObject[] neighbors)
    {
        Vector3 averageVelocity = Vector3.zero;
        // Accumulate velocit of neighbors of the game object and then movement velocity
        foreach (GameObject neighbor in neighbors)
        {
            // Need to get the Agent component of the game object and then movement velocity
            averageVelocity += neighbor.GetComponent<Agent>().movement.velocity;
        }
        // Calculate the average by dividing the average velocity by the number of neighbors
        averageVelocity /= neighbors.Length;

        // Steer towards the avergae velocity of the neighbors
        Vector3 force = CalculateSteering(agent, averageVelocity);

        return force;
    }

}
