// --- CardPlaceholderView.cs --- (Nouveau script pour gérer l'affichage d'une carte dans la vue globale)

using UnityEngine;
using UnityEngine.UI; // Important: utilise UnityEngine.UI pour Image, PAS TMPro.
using TMPro;
using com.hyminix.game.ojyx.Models;

public class CardPlaceholderView : MonoBehaviour
{
    [SerializeField] private Image cardImage; // L'image de fond de la carte.
    [SerializeField] private TextMeshProUGUI valueText; // Le texte affichant la valeur.

    // Couleurs pour les différentes valeurs (à adapter).
    [SerializeField] private Color negativeColor = Color.magenta; //Pour -1 et -2
    [SerializeField] private Color zeroColor = Color.blue;
    [SerializeField] private Color lowValueColor = Color.green;    // 1-4
    [SerializeField] private Color midValueColor = Color.yellow;   // 5-8
    [SerializeField] private Color highValueColor = Color.red;    // 9-12
    [SerializeField] private Color hiddenColor = Color.gray;    //Pour les cartes face cachée

    public void SetCard(Card card)
    {
        if (card.IsFaceUp)
        {
            valueText.text = card.Data.value.ToString();
            //On defini la couleur en fonction de ça valeur
            switch (card.Data.value)
            {
                case int n when n < 0: // -1 et -2
                    cardImage.color = negativeColor;
                    break;
                case 0:
                    cardImage.color = zeroColor;
                    break;
                case int n when n >= 1 && n <= 4:
                    cardImage.color = lowValueColor;
                    break;
                case int n when n >= 5 && n <= 8:
                    cardImage.color = midValueColor;
                    break;
                case int n when n >= 9 && n <= 12:
                    cardImage.color = highValueColor;
                    break;
                default:
                    cardImage.color = Color.black; // Erreur ?
                    Debug.LogError("Valeur de carte invalide : " + card.Data.value);
                    break;
            }

        }
        else
        {
            //Si la carte et caché ont affiche la couleur de carte caché et ont vide le texte
            cardImage.color = hiddenColor;
            valueText.text = "";
        }
    }

    // Méthode pour définir la carte comme "vide" (aucun affichage).
    public void SetEmpty()
    {
        cardImage.color = Color.clear; // Complètement transparent.
        valueText.text = "";
    }
}