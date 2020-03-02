using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

    public int baseHealth = 50;
    public int currentHealth;

    protected bool dead = false;

    public bool Dead {
        get {
            return dead;
        }
    }

    public int Health {
        get {
            return currentHealth;
        }
        set {
            currentHealth = value;
            currentHealth = Mathf.Clamp(currentHealth, 0, baseHealth);
        }
    }
}
