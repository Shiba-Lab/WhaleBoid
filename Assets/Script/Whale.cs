using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whale : Boid
{
    List<Whale> whaleNeighbors = new List<Whale>();
    private void Update()
    {
        UpdateNeighbors();
        UpdateWalls();
        UpdateAlignment();
        UpdateSeparation();
        UpdateCohesion();

        UpdateMove();
    }

    private protected override void UpdateNeighbors()
    {
        whaleNeighbors.Clear();

        if (!simulation) return;

        float prodThresh = Mathf.Cos(param.boidsFov * Mathf.Deg2Rad);
        float distThresh = param.boidsDistance;

        foreach (Whale other in simulation.Boids)
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
                    whaleNeighbors.Add(other);
                }
            }
        }
    }
    private protected override void UpdateSeparation()
    {
        if (whaleNeighbors.Count == 0) return;

        Vector3 force = Vector3.zero;
        foreach (Whale neighbor in whaleNeighbors)
        {
            force += (pos - neighbor.pos).normalized;
        }
        force /= whaleNeighbors.Count;

        accel += force * param.separationPower;
    }

    private protected override void UpdateAlignment()
    {
        if (whaleNeighbors.Count == 0) return;

        Vector3 averageVelocity = Vector3.zero;
        foreach (Whale neighbor in whaleNeighbors)
        {
            averageVelocity += neighbor.velocity;
        }
        averageVelocity /= whaleNeighbors.Count;

        accel += (averageVelocity - velocity) * param.alignmentPower;
    }

    private protected override void UpdateCohesion()
    {
        if (whaleNeighbors.Count == 0) return;

        Vector3 averagePos = Vector3.zero;
        foreach (Whale neighbor in whaleNeighbors)
        {
            averagePos += neighbor.pos;
        }
        averagePos /= whaleNeighbors.Count;

        accel += (averagePos - pos) * param.cohesionPower;
    }
}