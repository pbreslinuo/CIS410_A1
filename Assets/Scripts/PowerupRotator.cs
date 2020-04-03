using UnityEngine;

public class PowerupRotator : MonoBehaviour
{
    void Update()
    { // spins faster than the yellow ones
        transform.Rotate(new Vector3(45, 45, 45) * Time.deltaTime);
    }
}