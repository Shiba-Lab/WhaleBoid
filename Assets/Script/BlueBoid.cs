using System.Collections.Generic;
using UnityEngine;

public class BlueBoid : Boid
{
    public Simulation redSimulation
    {
        get
        {
            GameObject redSimulation_ = GameObject.Find("RedSimulation");
            return redSimulation_.GetComponent<Simulation>();
        }
    }
    List<RedBoid> redNeighbors = new List<RedBoid>();   
    List<BlueBoid> blueNeighbors = new List<BlueBoid>();
    private void Update()
    {
        UpdateNeighbors();
        UpdateRedNeighbors();

        UpdateWalls();
        UpdateAlignment();
        UpdateSeparation();
        UpdateCohesion();
        UpdateRunAway();

        UpdateMove();//UpdateMoveは最後
    }

    private protected override void UpdateNeighbors()
    {
        blueNeighbors.Clear();

        if (!simulation) return;

        float prodThresh = Mathf.Cos(param.boidsFov * Mathf.Deg2Rad);
        float distThresh = param.boidsDistance;

        foreach (BlueBoid other in simulation.Boids)
        {
            if (other == this) continue;

            Vector3 to = other.pos - pos;
            float dist = to.magnitude;

            if (dist < distThresh)
            {
                Vector3 dir = to.normalized;
                Vector3 fwd = velocity.normalized;
                var prod = Vector3.Dot(fwd, dir);
                if (prod > prodThresh)
                {
                    blueNeighbors.Add(other);
                }
            }
        }
    }
    private protected override void UpdateSeparation()
    {
        if (blueNeighbors.Count == 0) return;

        Vector3 force = Vector3.zero;
        foreach (BlueBoid neighbor in blueNeighbors)
        {
            force += (pos - neighbor.pos).normalized;
        }
        force /= blueNeighbors.Count;

        accel += force * param.separationPower;
    }

    private protected override void UpdateAlignment()
    {
        if (blueNeighbors.Count == 0) return;

        Vector3 averageVelocity = Vector3.zero;
        foreach (BlueBoid neighbor in blueNeighbors)
        {
            averageVelocity += neighbor.velocity;
        }
        averageVelocity /= blueNeighbors.Count;

        accel += (averageVelocity - velocity) * param.alignmentPower;
    }

    private protected override void UpdateCohesion()
    {
        if (blueNeighbors.Count == 0) return;

        Vector3 averagePos = Vector3.zero;
        foreach (BlueBoid neighbor in blueNeighbors)
        {
            averagePos += neighbor.pos;
        }
        averagePos /= blueNeighbors.Count;

        accel += (averagePos - pos) * param.cohesionPower;
    }

    private protected void UpdateRedNeighbors()
    {
        redNeighbors.Clear();

        if (!redSimulation) return;

        float prodThresh = Mathf.Cos(param.boidsFov * Mathf.Deg2Rad);
        float distThresh = param.boidsDistance;

        foreach (RedBoid other in redSimulation.Boids)
        {
            if (other == this) continue;

            Vector3 to = other.pos - pos;
            float dist = to.magnitude;

            if (dist < distThresh)
            {
                Vector3 dir = to.normalized;
                Vector3 fwd = velocity.normalized;
                var prod = Vector3.Dot(fwd, dir);
                if (prod > prodThresh)
                {
                    redNeighbors.Add(other);
                }
            }
        }
    }
    private protected void UpdateRunAway()
    {
        if (redNeighbors.Count == 0) return;

        Vector3 force = Vector3.zero;
        Vector3 averagePos = Vector3.zero;
        foreach (RedBoid redNeighbor in redNeighbors)
        {
            averagePos += redNeighbor.pos;
            force += (pos - redNeighbor.pos).normalized;
        }
        averagePos /= redNeighbors.Count;
        force /= redNeighbors.Count;

        accel += (force + (pos - averagePos)) * param.runAwayPower;
    }
    //private protected void UpdateRunAway()
    //{
    //    if (redNeighbors.Count == 0) return;

    //    Vector3 force = Vector3.zero;
    //    foreach (RedBoid redNeighbor in redNeighbors)
    //    {
    //        force += (pos - redNeighbor.pos).normalized;
    //    }
    //    force /= redNeighbors.Count;

    //    accel += force * param.runAwayPower;
    //}
}