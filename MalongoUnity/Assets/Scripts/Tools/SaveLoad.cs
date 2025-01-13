using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;

public class SaveLoad
{
    GameManager gm;

    InputActionAsset actions;
    private string intpufilePath;

    private string optionFilePath;


    public SaveLoad() 
    {
        gm = GameManager.Instance;

        actions = GameManager.Instance.input.actions;
        // Set the path to save the .txt file in the persistent data path
        intpufilePath = Path.Combine(Application.persistentDataPath, "rebinds.txt");

        LoadInput();
        InitOption();

    }

    #region Option
    public void InitOption()
    {
        optionFilePath = Path.Combine(Application.persistentDataPath, "option.txt");
        gm.option = new OptionStruct();
        LoadOption();
    }

    public void LoadOption()
    {
        if (File.Exists(optionFilePath))
        {
            gm.option = LoadStructFromIni<OptionStruct>(optionFilePath);
        }
        else
        {
            ResetOption();
        }
    }

    public void ResetOption()
    {
        gm.option.useMouseFlight = true;
        gm.option.playerName = "CANETTE";

        SaveOption();
    }

    public void SaveOption()
    {
        SaveStructToIni(gm.option, optionFilePath);
    }
    #endregion

    #region Input
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
    #endregion

    #region INI
    public static void SaveStructToIni<T>(T data, string filePath) where T : struct
    {
        StringBuilder iniContent = new StringBuilder();

        foreach (var field in typeof(T).GetFields())
        {
            string key = field.Name;
            string value = field.GetValue(data)?.ToString();
            iniContent.AppendLine($"{key}={value}");
        }

        File.WriteAllText(filePath, iniContent.ToString());
    }

    public static T LoadStructFromIni<T>(string filePath) where T : struct
    {
        if (!File.Exists(filePath)) return new T();

        T data = new T();
        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line) || !line.Contains("=")) continue;

            var keyValue = line.Split('=');
            string key = keyValue[0].Trim();
            string value = keyValue[1].Trim();

            var field = typeof(T).GetField(key);
            if (field == null) continue;

            object convertedValue = System.Convert.ChangeType(value, field.FieldType);
            field.SetValueDirect(__makeref(data), convertedValue);
        }

        return data;
    }
    #endregion
}
