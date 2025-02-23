// --- UIManager.cs ---
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class UIManager : MonoBehaviour
{
    [Header("Text Elements")]
    [SerializeField] private TextMeshProUGUI playerTurnText;
    [SerializeField] private TextMeshProUGUI stateText;
    [SerializeField] private TextMeshProUGUI infoText;

    [Header("Fade Settings")]
    [SerializeField] private Image fadeImage;

    [Header("Global View")]
    [SerializeField] private GameObject globalViewPanel;
    [SerializeField] private Volume postProcessingVolume;
    private Bloom bloom;

    private void Start()
    {
        //DOTween
        DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(200, 50);

        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            // Ajoute un CanvasGroup si absent, et le configure
            CanvasGroup canvasGroup = fadeImage.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = fadeImage.gameObject.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = 0f; // Complètement transparent au départ
            canvasGroup.blocksRaycasts = false; //Permet au fade image de ne pas bloquer les clics
            //On desactive le raycast target de l'image
            fadeImage.raycastTarget = false;
        }

        if (globalViewPanel != null)
        {
            globalViewPanel.SetActive(false);
        }

        if (postProcessingVolume != null)
        {
            if (postProcessingVolume.profile.TryGet<Bloom>(out bloom))
            {
            }
            else
            {
                Debug.LogError("Bloom effect not found in the Volume Profile!");
            }
        }
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

    // *** Utilisation de CanvasGroup.DOFade ***
    public void FadeIn(float duration)
    {
        if (fadeImage != null)
        {
            //On récupére le canvas group et on fait l'appel
            CanvasGroup canvasGroup = fadeImage.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.DOFade(0f, duration); //Fondu vers transparent
            }

        }
    }

    public void FadeOut(float duration)
    {
        if (fadeImage != null)
        {
            //On récupére le canvas group et on fait l'appel
            CanvasGroup canvasGroup = fadeImage.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.DOFade(1f, duration); //Fondu vers opaque
            }
        }
    }
    private void BlurBackground(bool enable)
    {
        if (bloom != null)
        {
            bloom.intensity.value = enable ? 1f : 0f;  // Contrôle l'intensité du Bloom
        }
    }
    public void SetColor(Color color)
    {
        if (fadeImage != null)
        {
            fadeImage.color = color;
        }
    }

    public void ShowGlobalView()
    {
        if (globalViewPanel != null)
        {
            globalViewPanel.SetActive(true);
            BlurBackground(true);
            globalViewPanel.GetComponent<GlobalUIManager>().UpdateGlobalView();
        }
    }

    public void HideGlobalView()
    {
        if (globalViewPanel != null)
        {
            globalViewPanel.SetActive(false);
            BlurBackground(false);
        }
    }
}