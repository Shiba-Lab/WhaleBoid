using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Whale_fifty : humpback
{
    public Simulation blueSimulation
    {
        get
        {
            GameObject blueSimulation_ = GameObject.Find("BlueSimulation");
            return blueSimulation_.GetComponent<Simulation>();
        }
    }
    /*public Simulation humpbacksimulation
    {
        get
        {
            GameObject humpobackSimulation_ = GameObject.Find("humpback_whale_model23");
            return humpobackSimulation_.GetComponent<Simulation>();
        }
    }
    */

    //RedBoid= blueSimulation_;
    List<humpback> hunpNeighbors = new List<humpback>();
    List<BlueBoid> blueNeighbors = new List<BlueBoid>();

    humpback[] reder = new humpback[0];
    private void Update()
    {
        UpdateNeighbors();
        UpdateBlueNeighbors();

        UpdateWalls();
        UpdateAlignment();
        UpdateSeparation();
        UpdateCohesion();
        UpdateChase();
      //  updatehunp();

        UpdateMove();//UpdateMove�͍Ō�
    }

    void updatehunp()
    {

        if (this != reder[0])
        {
            Vector3 to = reder[0].pos - pos;
            float dist = to.magnitude;

            if (dist < 1000)
            {
                //Vector3 test = Vector3.zero;
                accel = Vector3.zero;
            }
        }

    }
    private protected override void UpdateNeighbors()
    {
        hunpNeighbors.Clear();

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

                    hunpNeighbors.Add(other);
                }
            }
        }
    }
    private protected override void UpdateSeparation()
    {
        if (hunpNeighbors.Count == 0) return;

        Vector3 force = Vector3.zero;
        foreach (humpback neighbor in hunpNeighbors)
        {
            force += (pos - neighbor.pos).normalized;
        }
        force /= hunpNeighbors.Count;

        accel += force * param.separationPower;
    }

    private protected override void UpdateAlignment()
    {
        if (hunpNeighbors.Count == 0) return;

        Vector3 averageVelocity = Vector3.zero;
        foreach (humpback neighbor in hunpNeighbors)
        {
            averageVelocity += neighbor.velocity;
        }
        averageVelocity /= hunpNeighbors.Count;

        accel += (averageVelocity - velocity) * param.alignmentPower;
    }

    private protected override void UpdateCohesion()
    {
        if (hunpNeighbors.Count == 0) return;

        Vector3 averagePos = Vector3.zero;
        foreach (humpback neighbor in hunpNeighbors)
        {
            averagePos += neighbor.pos;
        }
        averagePos /= hunpNeighbors.Count;

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
