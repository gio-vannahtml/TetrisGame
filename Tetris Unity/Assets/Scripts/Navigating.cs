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
}