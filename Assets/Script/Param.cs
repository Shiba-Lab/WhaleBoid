using UnityEngine;

[CreateAssetMenu(menuName = "Boid/Param")]
public class Param : ScriptableObject
{
    public float initSpeed = 2f;
    public float minSpeed = 2f;
    public float maxSpeed = 5f;
    public float boidsDistance = 1f;
    public float boidsFov = 90f;
    public float separationPower = 5f;
    public float alignmentPower = 2f;
    public float cohesionPower = 3f;
    public float chasePower = 3f;
    public float runAwayPower = 4f;
}
