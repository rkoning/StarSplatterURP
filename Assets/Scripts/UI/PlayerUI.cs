using System.Transactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public Color flashColor;

    [Header("Health")]
    public Image healthBar;
    public float minHealthFill = 0.75f;
    public float maxHealthFill = 1f;
    private bool healthFlashing;

    [Header("Shield")]
    public Image shieldBar;
    public float minShieldFill = 0.75f;
    public float maxShieldFill = 1f;
    private bool shieldFlashing;

    [Header("Money")]
    public TextMeshProUGUI moneyText;
    public GameObject moneyTextItemParent;
    public Vector3 transactionStartPosition;
    public Vector3 transactionEndPosition;
    public float transactionDuration;
    public AnimationCurve transactionSpeed;
    public AnimationCurve transactionOpacity;
    public Color profitColorStart;
    public Color profitColorEnd;
    public Color lossColorStart;
    public Color lossColorEnd;

    public Transform itemTextParent;
    private TextMeshProUGUI[] itemTextItems;

    private List<TransactionAnimation> currentItemTransactions = new List<TransactionAnimation>();
    private int currentTextItem;

    private TextMeshProUGUI[] moneyTextItems;
    private int currentMoneyItem;

    private List<TransactionAnimation> currentTransactions = new List<TransactionAnimation>();

    private Player player;

    private Health health;
    private Shield shield;

    private void Start() 
    {
        // initialize moneyTextItems to all children of this gameObject
        moneyTextItems = moneyTextItemParent.GetComponentsInChildren<TextMeshProUGUI>();
        foreach(var mti in moneyTextItems)
        {
            mti.gameObject.SetActive(false);
        }

        itemTextItems = itemTextParent.GetComponentsInChildren<TextMeshProUGUI>();// 421, 2616, 418
        foreach(var iti in itemTextItems)
        {
            iti.transform.parent.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (player)
        {
            UpdateShield();
            UpdateHealth();
            UpdateMoney();
        }

        float dT = Time.deltaTime;
        List<TransactionAnimation> toRemove = new List<TransactionAnimation>();
        foreach (var itemAnimation in currentItemTransactions) {
            float animTime = itemAnimation.GetAnimationTime();
            itemAnimation.remainingDuration += dT;
            if (itemAnimation.IsDone()) {
                itemAnimation.transactionText.transform.parent.gameObject.SetActive(false);
            }
        }

        foreach (var ta in toRemove)
        {
            currentTransactions.Remove(ta);
        }

        toRemove = new List<TransactionAnimation>();
        foreach (var ta in currentTransactions) { 

            float animTime = ta.GetAnimationTime();
            ta.remainingDuration += dT;
            ta.rectTransform.localPosition = Vector3.Lerp(
                transactionStartPosition,
                transactionEndPosition,
                transactionSpeed.Evaluate(animTime)
            );

            if (ta.amount > 0)
            {
                ta.transactionText.color = Color.Lerp(profitColorStart, profitColorEnd, transactionOpacity.Evaluate(animTime));
            }
            else
            {
                ta.transactionText.color = Color.Lerp(lossColorStart, lossColorEnd, transactionOpacity.Evaluate(animTime));
            }
            if (ta.IsDone())
            {
                ta.transactionText.gameObject.SetActive(false);
                toRemove.Add(ta);
            }
        }

        foreach (var ta in toRemove)
        {
            currentTransactions.Remove(ta);
        }
    }

    public void Transaction(float amount)
    {
        TextMeshProUGUI nextMoneyText = moneyTextItems[currentMoneyItem];
        nextMoneyText.gameObject.SetActive(true);
        nextMoneyText.text = amount.ToString();
        Color color = amount > 0 ? profitColorStart : profitColorEnd;
        var t = new TransactionAnimation(nextMoneyText, nextMoneyText.rectTransform, transactionDuration, color, amount);
        currentMoneyItem++;
        if (currentMoneyItem > moneyTextItems.Length - 1)
        {
            currentMoneyItem = 0;
        }
        currentTransactions.Add(t);
    }

    public void Transaction(Item item)
    {
        TextMeshProUGUI nextItemText = itemTextItems[currentTextItem];
        nextItemText.transform.parent.gameObject.SetActive(true);
        nextItemText.text = item.name;
        Debug.Log(item.name);

        var t = new TransactionAnimation(nextItemText, nextItemText.rectTransform, transactionDuration, ItemUtils.RarityColors[item.rarity]);
        
        t.transactionText.GetComponentInParent<Image>().color = t.color;
        currentTextItem++;
        if (currentTextItem > itemTextItems.Length - 1)
        {
            currentTextItem = 0;
        }
        currentItemTransactions.Add(t);
    }

    private void UpdateMoney() {
        if (player.inventory) {
            moneyText.text = player.inventory.GetMoney().ToString();
        }
    }

    private void UpdateShield()
    {
        if (shield)
        {
            UpdateBar(shieldBar, minShieldFill, maxShieldFill, (shield.currentHealth / shield.maxHealth));
        } else
        {
            Debug.LogWarning("Player shield not assigned to PlayerUI, cannot update shield UI!");
        }
    }

    private void UpdateHealth()
    {
        if (health)
        {
            UpdateBar(healthBar, minHealthFill, maxHealthFill, (health.GetCurrentHealth() / health.maxHealth));
        } else
        {
            Debug.LogWarning("PlayerFighter Health not assgined, cannot update health UI!");
        }
    }

    private void UpdateBar(Image bar, float min, float max, float value)
    {
        bar.fillAmount = Mathf.Lerp(min, max, value);
    }

    public void FlashShield()
    {
        if (!shieldFlashing)
        {
            shieldFlashing = true;
            StartCoroutine(IEnumFlashShield());
        }
    }

    public void FlashHealth()
    {
        if (!healthFlashing)
        {
            healthFlashing = true;
            StartCoroutine(IEnumFlashHealth());
        }
    }

    private IEnumerator IEnumFlashShield()
    {
        Color c = shieldBar.color;
        shieldBar.color = flashColor;
        yield return new WaitForSeconds(0.1f);
        shieldBar.color = c;
        yield return new WaitForSeconds(0.05f);
        shieldBar.color = flashColor;
        yield return new WaitForSeconds(0.1f);
        shieldBar.color = c;
        shieldFlashing = false;
    }

    private IEnumerator IEnumFlashHealth()
    {
        Color c = healthBar.color;
        healthBar.color = flashColor;
        yield return new WaitForSeconds(0.1f);
        healthBar.color = c;
        yield return new WaitForSeconds(0.05f);
        healthBar.color = flashColor;
        yield return new WaitForSeconds(0.1f);
        healthBar.color = c;
        healthFlashing = false;
    }

    public void SetPlayer(Player player)
    {
        this.player = player;

        this.shield = this.player.ship.GetComponent<Shield>();
        shield.onShieldBreak += FlashShield;
        shield.onShieldRecharge += FlashShield;

        this.health = this.player.ship.GetComponent<Health>();
        this.health.OnDamaged += FlashHealth;
    }

    private class TransactionAnimation {
        public float remainingDuration;
        public float totalDuration;
        public TextMeshProUGUI transactionText;
        public RectTransform rectTransform;

        public float amount;

        public Color color;

        public TransactionAnimation(TextMeshProUGUI text, RectTransform rectTransform, float duration, Color color) {
            this.transactionText = text;
            this.rectTransform = rectTransform;
            this.totalDuration = duration;
            this.color = color;
        }

        public TransactionAnimation(TextMeshProUGUI text, RectTransform rectTransform, float duration, Color color, float amount)
        {
            this.amount = amount;
            this.transactionText = text;
            this.rectTransform = rectTransform;
            this.totalDuration = duration;
            this.color = color;
        }

        public float GetAnimationTime()
        {
            return remainingDuration / totalDuration;
        }

        public bool IsDone()
        {
            return remainingDuration >= totalDuration;
        }
    }
}
