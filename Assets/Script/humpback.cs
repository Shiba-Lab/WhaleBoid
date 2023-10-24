using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class humpback : Boid
{
    public Simulation blueSimulation
    {
        get
        {
            GameObject blueSimulation_ = GameObject.Find("BlueSimulation");
            return blueSimulation_.GetComponent<Simulation>();
        }
    }
    //RedBoid= blueSimulation_;
    List<humpback> redNeighbors = new List<humpback>();
    List<BlueBoid> blueNeighbors = new List<BlueBoid>();
    private void Update()
    {
        UpdateNeighbors();
        UpdateBlueNeighbors();

        UpdateWalls();
        UpdateAlignment();
        UpdateSeparation();
        UpdateCohesion();
        UpdateChase();

        UpdateMove();//UpdateMoveÇÕç≈å„
    }

    private protected override void UpdateNeighbors()
    {
        redNeighbors.Clear();

        if (!simulation) return;

        float prodThresh = Mathf.Cos(param.boidsFov * Mathf.Deg2Rad);
        float distThresh = param.boidsDistance;

        foreach (humpback other in simulation.Boids)
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
    private protected override void UpdateSeparation()
    {
        if (redNeighbors.Count == 0) return;

        Vector3 force = Vector3.zero;
        foreach (humpback neighbor in redNeighbors)
        {
            force += (pos - neighbor.pos).normalized;
        }
        force /= redNeighbors.Count;

        accel += force * param.separationPower;
    }

    private protected override void UpdateAlignment()
    {
        if (redNeighbors.Count == 0) return;

        Vector3 averageVelocity = Vector3.zero;
        foreach (humpback neighbor in redNeighbors)
        {
            averageVelocity += neighbor.velocity;
        }
        averageVelocity /= redNeighbors.Count;

        accel += (averageVelocity - velocity) * param.alignmentPower;
    }

    private protected override void UpdateCohesion()
    {
        if (redNeighbors.Count == 0) return;

        Vector3 averagePos = Vector3.zero;
        foreach (humpback neighbor in redNeighbors)
        {
            averagePos += neighbor.pos;
        }
        averagePos /= redNeighbors.Count;

        accel += (averagePos - pos) * param.cohesionPower;
    }
    private protected void UpdateBlueNeighbors()
    {
        blueNeighbors.Clear();

        if (!blueSimulation) return;

        float prodThresh = Mathf.Cos(param.boidsFov * Mathf.Deg2Rad);
        float distThresh = param.boidsDistance;

        foreach (BlueBoid other in blueSimulation.Boids)
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
    private protected void UpdateChase()
    {
        if (blueNeighbors.Count == 0) return;

        Vector3 force = Vector3.zero;
        foreach (BlueBoid blueNeighbor in blueNeighbors)
        {
            force += (blueNeighbor.pos - pos).normalized;
        }
        force /= blueNeighbors.Count;

        accel += force * param.chasePower;
    }
}
