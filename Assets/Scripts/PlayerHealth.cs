using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("生命设置")]
    public int maxHealth = 3;
    public float invincibilityDuration = 0.5f;

    [Header("受伤反馈")]
    public DamageFlashUI damageFlashUI;

    private int currentHealth;
    private bool isInvincible;

    void Start()
    {
        currentHealth = maxHealth;

        if (damageFlashUI == null)
            damageFlashUI = FindObjectOfType<DamageFlashUI>();
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible || currentHealth <= 0) return;

        currentHealth -= damage;
        Debug.Log($"玩家受到 {damage} 点伤害，剩余生命: {currentHealth}");

        if (damageFlashUI != null)
            damageFlashUI.Flash();

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityRoutine());
        }
    }

    IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }

    void Die()
    {
        Debug.Log("游戏结束");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
