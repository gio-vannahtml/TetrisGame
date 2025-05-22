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
        SceneManager.LoadScene("Level - Easy");
    }

    public void LoadMediumLevel()
    {
        SceneManager.LoadScene("Level - Medium");
    }

    public void LoadDifficultLevel()
    {
        SceneManager.LoadScene("Level - Difficult");
    }

    public void LoadBossLevel()
    {
        SceneManager.LoadScene("Level - Boss level");
    }

    public void LoadTutorialLevel()
    {
        SceneManager.LoadScene("Level - Tutorial");
    }

}