using UnityEngine;
[CreateAssetMenu(menuName ="Boid/WallParam")]

public class WallParam : ScriptableObject
{
    public float wallDistance = 1f;
    public float wallPower = 3f;

    public float wallX = 10f;
    public float wallY = 2f;
    public float wallZ = 10f;
}
