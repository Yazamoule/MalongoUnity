using System;
using UnityEngine;

public class MenuOption : MonoBehaviour
{
    SaveLoad sv;
    OptionStruct option;
    private void Awake()
    {
    }

    private void Start()
    {
        sv = GameManager.Instance.saveLoad;
        option = GameManager.Instance.option;
    }

    #region save load
    public void ResetInput()
    {
        sv.ResetAllInput();
    }

    public void SaveInput()
    {
        sv.SaveInput();
    }

    public void LoadInput()
    {
        sv.LoadInput();
    }

    public void ResetOption()
    {
        sv.ResetOption();
    }

    public void SaveOption()
    {
        sv.SaveOption();
    }

    public void LoadOption()
    {
        sv.LoadOption();
    }
    #endregion

    public void OnValueChangedControlType(Int32 _num)
    {
        switch (_num)
        {
            case 0:
            option.useMouseFlight = true;
            break;
            case 1:
            option.useMouseFlight = false;
            break;
        }
    }

}
