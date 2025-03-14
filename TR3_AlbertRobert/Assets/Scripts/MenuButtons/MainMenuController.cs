using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    private UIDocument uiDocument;

    private void OnEnable()
    {
        // Obtenim la referència al UIDocument
        uiDocument = GetComponent<UIDocument>();

        // Obtenim els botons per les seves IDs
        Button comencarJocButton = uiDocument.rootVisualElement.Q<Button>("ComencarButton");
        Button skinsButton = uiDocument.rootVisualElement.Q<Button>("SkinsButton");
        Button tancarJocButton = uiDocument.rootVisualElement.Q<Button>("TancarJocButton");

        // Assignem les funcions als events de clic dels botons
        comencarJocButton.clicked += OnComencarJocClicked;
        skinsButton.clicked += OnSkinsButtonClicked;
        tancarJocButton.clicked += OnTancarJocClicked;
    }

    private void OnComencarJocClicked()
    {
        // Canviar a l'escena de la partida del joc
        SceneManager.LoadScene("Game"); // Substitueix "PartidaJoc" pel nom de la teva escena
    }

    private void OnSkinsButtonClicked()
    {
        // Canviar a l'escena de selecció de skins
        // SceneManager.LoadScene("SeleccioSkins"); // Substitueix "SeleccioSkins" pel nom de la teva escena
        Debug.Log("Skins");
    }

    private void OnTancarJocClicked()
    {
        // Tancar el joc
        Application.Quit();

        // Si estàs en el mode d'edició, mostra un missatge perquè es vegi que funciona
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}