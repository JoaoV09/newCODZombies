using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private HudSelect[] hud;

    [Space(10)]
    [SerializeField] private Image lifeBar;
    [SerializeField] private Image hitImage;
    public float fadeDuration = 1f;
    private Coroutine fadeCoroutine;


    [Space(10)]
    [SerializeField] private Image gunSlot;
    [SerializeField] private TextMeshProUGUI guntext;

    [Space(10)]
    [SerializeField] private TextMeshProUGUI interactText;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance == null)
            Instance = this;

        Verifi();
    }

    public void SwitchHud(string _hudName)
    {
        foreach (var item in hud)
        {
            if (item.hudName == _hudName)
            {
                item.select = true;
                item.hud.SetActive(true);
            }
            else
            {
                item.select = false;
                item.hud.SetActive(false);
            }
        }
    }
    public void Verifi()
    {
        foreach (var item in hud)
        {
            item.hud.SetActive(item.select);
        }
    }
    public void SetHUD(Sprite _sprite, string _text)
    {
        gunSlot.sprite = _sprite;
        guntext.text = _text;
    }
    public void SetInteractText(string _text)
    {
        interactText.gameObject.SetActive(!string.IsNullOrEmpty(_text));
        interactText.text = _text;
    }
    public void SetLifeBar(float _life)
    {
        var newScale = new Vector2(Mathf.Lerp(lifeBar.rectTransform.localScale.x, _life / 100, 10 * Time.deltaTime), 1);
        lifeBar.rectTransform.localScale = newScale;
    }
    void OnEnable()
    {
        PlayerMovementAdvanced.OnPlayerHit += TriggerHitEffect;
    }

    void OnDisable()
    {
        PlayerMovementAdvanced.OnPlayerHit -= TriggerHitEffect;
    }

    void TriggerHitEffect()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeEffect());
    }

    IEnumerator FadeEffect()
    {
        // Define a cor com alpha 1 (visível)
        Color color = hitImage.color;
        color.a = 1f;
        hitImage.color = color;

        // Diminui o alpha suavemente até 0
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            hitImage.color = color;
            yield return null;
        }

        // Garante que alpha fique 0 no final
        color.a = 0f;
        hitImage.color = color;
    }

}