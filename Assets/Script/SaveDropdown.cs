using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Dropdown))]
public class SaveDropdown : MonoBehaviour
{

    public Dropdown dropdown;
    public Text SelectedOption;

    void Start()
    {
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }
    void OnDropdownValueChanged(int value)
    {
        SelectedOption.text = dropdown.options[dropdown.value].text;
    }
  
}
