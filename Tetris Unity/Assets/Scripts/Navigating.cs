using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonHandler : MonoBehaviour
{
    public void LoadMenuScene()
    {
        SceneManager.LoadScene("Menu Screen");
    }
    public void LoadShopScene()
    {
        SceneManager.LoadScene("Shop");
    }
    public void OpenItemsTab()
    {
        SceneManager.LoadScene("Shop"); // Exact name of the Items scene
    }

    public void OpenUpgradesTab()
    {
        SceneManager.LoadScene("Shop-Upgrades"); // Exact name of the Upgrades scene
    }

    public void LoadEasyLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level - Easy");
    }

    public void LoadMediumLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level - Medium");
    }

    public void LoadDifficultLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level - Difficult");
    }

    public void LoadBossLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level - Boss level");
    }

}