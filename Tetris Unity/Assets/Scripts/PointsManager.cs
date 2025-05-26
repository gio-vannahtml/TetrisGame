using TMPro;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    [Header("Game Over UI")]
    public TextMeshProUGUI GameOverpointsText;
    public TextMeshProUGUI GameOvercombosText;

    [Header("Win UI")]
    public TextMeshProUGUI WinpointsText;
    public TextMeshProUGUI WincombosText;
    
    [Header("Wallet")]
    public int walletPoints = 0;

    public void ShowGameOverData(int finalScore, int finalCombos)
    {
        GameOverpointsText.text = "+ " + finalScore.ToString();
        GameOvercombosText.text = "+ " + finalCombos.ToString();
    }

    public void ShowWinData(int finalScore, int finalCombos)
    {
        WinpointsText.text = "+ " + finalScore.ToString();
        WincombosText.text = "+ " + finalCombos.ToString();
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


