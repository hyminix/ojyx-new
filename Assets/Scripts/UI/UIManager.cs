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

    [Header("Global View")]
    [SerializeField] private GameObject globalViewPanel;
    [SerializeField] private Volume postProcessingVolume;

    private Bloom bloom; // si tu veux intensifier le bloom
    private DepthOfField depthOfField; // si tu veux un flou DOF

    private bool isGlobalViewOpen = false;

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

        // Désactiver la vue globale par défaut
        if (globalViewPanel != null)
        {
            globalViewPanel.SetActive(false);
        }

        // Récupère Bloom ou DepthOfField
        if (postProcessingVolume != null)
        {
            if (postProcessingVolume.profile.TryGet<Bloom>(out bloom))
            {
                bloom.intensity.value = 0f;
            }
            if (postProcessingVolume.profile.TryGet<DepthOfField>(out depthOfField))
            {
                depthOfField.active = false;
            }
        }
    }

    private void Update()
    {
        // Gestion du scroll molette
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (!isGlobalViewOpen && scroll < 0f)
        {
            ShowGlobalViewWithTransition();
        }
        else if (isGlobalViewOpen && scroll > 0f)
        {
            HideGlobalViewWithTransition();
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

    // Active ou désactive un flou stylisé (Bloom / DOF)
    private void BlurBackground(bool enable)
    {
        // Si tu veux booster le Bloom
        if (bloom != null)
        {
            bloom.intensity.value = enable ? 10f : 0f;
        }
        // Ou activer le DepthOfField
        if (depthOfField != null)
        {
            depthOfField.active = enable;
        }
    }

    public void ShowGlobalViewWithTransition()
    {
        isGlobalViewOpen = true;

        // FadeOut, puis on active la vue globale, puis FadeIn
        FadeOut(0.3f);
        DOVirtual.DelayedCall(0.3f, () =>
        {
            ShowGlobalView();
            FadeIn(0.3f);
        });
    }

    public void HideGlobalViewWithTransition()
    {
        isGlobalViewOpen = false;

        FadeOut(0.3f);
        DOVirtual.DelayedCall(0.3f, () =>
        {
            HideGlobalView();
            FadeIn(0.3f);
        });
    }

    public void ShowGlobalView()
    {
        if (globalViewPanel != null)
        {
            globalViewPanel.SetActive(true);
            BlurBackground(true);

            // Petit fade-in du panel
            CanvasGroup cg = globalViewPanel.GetComponent<CanvasGroup>();
            if (!cg) cg = globalViewPanel.AddComponent<CanvasGroup>();
            cg.alpha = 0f;
            cg.DOFade(1f, 0.5f).SetEase(Ease.OutQuad);

            globalViewPanel.GetComponent<GlobalUIManager>().UpdateGlobalView();
        }
    }

    public void HideGlobalView()
    {
        if (globalViewPanel != null)
        {
            // On fade out le panel
            CanvasGroup cg = globalViewPanel.GetComponent<CanvasGroup>();
            if (!cg) cg = globalViewPanel.AddComponent<CanvasGroup>();
            cg.DOFade(0f, 0.5f).SetEase(Ease.OutQuad)
              .OnComplete(() =>
              {
                  globalViewPanel.SetActive(false);
              });

            BlurBackground(false);
        }
    }
}
