using UnityEngine;

public class ManaSystem : MonoBehaviour
{
    public static ManaSystem Instance;

    public int maxMana = 10;
    public float manaRegenRate = 1f;
    private float currentMana;

    public event System.Action OnManaChanged; // 마나 변경 시 이벤트

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        currentMana = 0;
        OnManaChanged?.Invoke();
    }

    void Update()
    {
        if (currentMana < maxMana)
        {
            currentMana += manaRegenRate * Time.deltaTime;
            if (currentMana >= maxMana)
                currentMana = maxMana;
            OnManaChanged?.Invoke();
        }
    }

    public bool ConsumeMana(int amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;
            OnManaChanged?.Invoke();
            return true;
        }
        return false;
    }

    public void ResetMana()
    {
        currentMana = 0;
        OnManaChanged?.Invoke();
    }

    public float GetCurrentMana() => currentMana;
    public float GetMaxMana() => maxMana;
}
