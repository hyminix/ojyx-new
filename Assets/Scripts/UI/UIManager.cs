// --- UI/UIManager.cs --- (Simplification, suppression de GlobalView)
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using com.hyminix.game.ojyx.Managers;

public class UIManager : MonoBehaviour
{
    [Header("Text Elements")]
    [SerializeField] private TextMeshProUGUI playerTurnText;
    [SerializeField] private TextMeshProUGUI stateText;
    [SerializeField] private TextMeshProUGUI infoText;

    [Header("Fade Settings")]
    [SerializeField] private Image fadeImage;

    // PLUS BESOIN DE GlobalViewPanel, Volume, Bloom, DepthOfField, isGlobalViewOpen

    private void Start()
    {
        // DOTween init
        DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(200, 50);

        // Config fadeImage
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            CanvasGroup canvasGroup = fadeImage.GetComponent<CanvasGroup>();
            if (canvasGroup == null) canvasGroup = fadeImage.gameObject.AddComponent<CanvasGroup>();
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            fadeImage.raycastTarget = false;
        }

        // PLUS DE LOGIQUE DE VUE GLOBALE ICI
    }

    private void Update()
    {
        // PLUS DE GESTION DU SCROLL ICI
    }

    public void SetPlayerTurnText(int playerID)
    {
        if (playerTurnText != null)
        {
            playerTurnText.text = "Tour du Joueur " + playerID;
        }
    }

    public void SetStateText(string stateName)
    {
        if (stateText != null)
        {
            stateText.text = "État : " + stateName;
        }
    }

    public void SetInfoText(string info)
    {
        if (infoText != null)
        {
            infoText.text = info;
        }
    }

    // Fonctions de fade
    public void FadeIn(float duration)
    {
        if (fadeImage != null)
        {
            CanvasGroup canvasGroup = fadeImage.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.DOFade(0f, duration); // Vers transparent
            }
        }
    }

    public void FadeOut(float duration)
    {
        if (fadeImage != null)
        {
            CanvasGroup canvasGroup = fadeImage.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.DOFade(1f, duration); // Vers opaque
            }
        }
    }
}