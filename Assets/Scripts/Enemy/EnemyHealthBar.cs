using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    private Slider healthBar;

    // Postos diretamente na hierarquia por serem diferentem para cada inimigo
    public Transform healthBarPosition;
    public Vector3 offset;

    // Vida do inimigo
    private float maxHealth;
    // Actual vida
    private float currentHealth;

    public float airEnemyMaxHealth = 1000;
    public float waterEnemyMaxHealth = 1000;
    public float groundEnemyMaxHealth = 1000;


    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponent<Slider>();
    }
    

    private void Update()
    {
        if(Camera.current != null)
            transform.rotation = Camera.current.transform.rotation;

        if (healthBarPosition != null)
            transform.position = healthBarPosition.position + offset;

        CheckDeath();

    }

    public void SetMaxHealth(float value)
    {
        maxHealth = value;
        currentHealth = value;
    }

    public void TakeDamage(int dano)
    {
        // Dano é dado como percentagem

        float damage = maxHealth * dano / 100;

        currentHealth -= damage;

        healthBar.value = currentHealth / maxHealth;
    }
    public void CheckDeath()
    {
        if(currentHealth <= 0)
        {
            Debug.Log("Parent: " + transform.parent.parent.name);
            transform.parent.parent.GetComponent<Animator>().SetBool("isDead", true);

            Destroy(transform.parent.parent.gameObject, 5f);
        }
    }
}
