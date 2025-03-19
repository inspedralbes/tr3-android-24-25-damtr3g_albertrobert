using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private string menuSceneName = "Menu";


    private void OnEnable()
    {
        // Obtenim la referència al UIDocument
        uiDocument = GetComponent<UIDocument>();

        // Obtenim els botons per les seves IDs
        Button tryAgainButton = uiDocument.rootVisualElement.Q<Button>("tryAgain");
        Button sortirButton = uiDocument.rootVisualElement.Q<Button>("sortir");

        // Assignem les funcions als events de clic dels botons
        tryAgainButton.clicked += ReloadScene;
        sortirButton.clicked += LoadMainMenu;

        // Assegurar-se que la UI és visible
        uiDocument.rootVisualElement.style.display = DisplayStyle.Flex;
    }

    private void ReloadScene()
    {
        Time.timeScale = 1;
        // Tornar a carregar l'escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void LoadMainMenu()
    {
        Time.timeScale = 1;
        // Carregar l'escena del menú principal
        SceneManager.LoadScene(menuSceneName);
    }
}