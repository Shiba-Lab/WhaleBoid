using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class Simulation : MonoBehaviour
{
    [SerializeField]
    int boidCount = 100;
    [SerializeField]
    GameObject boidPrefab;
    [SerializeField]
    Param param;
    [SerializeField]
    WallParam wallParam;

    List<Boid> Boids_ = new List<Boid>();
    public ReadOnlyCollection<Boid> Boids
    {
        get { return Boids_.AsReadOnly(); }
    }

    void AddBoid()
    {
        GameObject newRedBoid = Instantiate(boidPrefab, Random.insideUnitSphere, Random.rotation);
        newRedBoid.transform.SetParent(transform);
        Boid boid = newRedBoid.GetComponent<Boid>();
        boid.simulation = this;
        boid.param = param;
        boid.wallParam = wallParam;
        Boids_.Add(boid);
    }

    void RemoveBoid()
    {
        if (Boids_.Count == 0) return;
        int lastIndex = Boids_.Count - 1;
        Boid boid = Boids_[lastIndex];
        Destroy(boid.gameObject);
        Boids_.RemoveAt(lastIndex);
    }

    void Update()
    {
        while (Boids_.Count < boidCount)
        {
            AddBoid();
        }
        while (Boids_.Count > boidCount)
        {
            RemoveBoid();
        }

    }
    void OnDrawGizmos()
    {
        if (!param) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, new Vector3(wallParam.wallX,wallParam.wallY,wallParam.wallZ));
    }
}
