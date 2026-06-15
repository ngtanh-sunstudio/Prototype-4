using UnityEngine;

public enum PowerupType
{
    Knockback,
    Rockets
}

public class PowerupPickup : MonoBehaviour
{
    public PowerupType powerupType;
}
