using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class RebindInputMenuManager : MonoBehaviour
{
    InputActionAsset actions;
    private string intpufilePath;

    private void Awake()
    {
        actions = GameManager.Instance.input.actions;
        // Set the path to save the .txt file in the persistent data path
        intpufilePath = Path.Combine(Application.persistentDataPath, "rebinds.txt");

        LoadInput();
    }

    public void LoadInput()
    {
        if (File.Exists(intpufilePath))
        {
            // Read the JSON data from the file
            var rebinds = File.ReadAllText(intpufilePath);
            if (!string.IsNullOrEmpty(rebinds))
                actions.LoadBindingOverridesFromJson(rebinds);
        }
    }

    public void SaveInput()
    {
        // Save the JSON data to the file
        var rebinds = actions.SaveBindingOverridesAsJson();
        File.WriteAllText(intpufilePath, rebinds);
    }

    public void ResetAllInput()
    {
        foreach (var action in actions)
        {
            action.RemoveAllBindingOverrides();
        }
    }
}
