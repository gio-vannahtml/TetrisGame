using TMPro;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    [Header("Game Over UI")]
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI combosText;

    [Header("Wallet")]
    public int walletPoints = 0;

    public void ShowGameOver(int finalScore, int finalCombos)
    {
        pointsText.text = "+ " + finalScore.ToString();
        combosText.text = "+ " + finalCombos.ToString();
    }

    public void AddToWallet(int points)
    {
        walletPoints += points;
        // You can also update wallet UI here later
    }

    public int GetWalletPoints()
    {
        return walletPoints;
    }
}
