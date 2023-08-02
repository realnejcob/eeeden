//https://github.com/DavidF-Dev/Unity-DeveloperConsole

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DavidFDev.DevConsole;
using UnityEngine.SceneManagement;

public class ConsoleManager : MonoBehaviour {
    private void Awake() {
        DevConsole.EnableConsole();
        DevConsole.SetToggleKey(UnityEngine.InputSystem.Key.Escape);

        DevConsole.AddCommand(Command.Create(
            name: "reload_active_scene",
            aliases: "r",
            helpText: "Reloads active scene",
            callback: () => {
                DevConsole.CloseConsole();
                SceneManager.LoadScene(0);
            }));
    }
}
