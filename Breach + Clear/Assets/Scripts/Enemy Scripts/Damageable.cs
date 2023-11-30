using UnityEngine;

public class Damageable : MonoBehaviour
{
    public float health = 50f;
    public RagdollDeath dieScript;

    public void TakesDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        dieScript.Death();
    }
}
