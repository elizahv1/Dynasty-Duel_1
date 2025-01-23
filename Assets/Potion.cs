using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public enum PotionType { Grow, Shrink }
    public PotionType potionType; // Assign in the prefab inspector

    public float sizeChangeAmount = 0.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                if (potionType == PotionType.Grow)
                {
                    player.ChangeSize(1 + sizeChangeAmount); // Grow effect
                }
                else if (potionType == PotionType.Shrink)
                {
                    player.ChangeSize(1 - sizeChangeAmount); // Shrink effect
                }
            }

            Destroy(gameObject);
        }
    }
}

