using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class FieldCheck : MonoBehaviour
{
    private InputField _thisFirld;
    
    private void Awake()
    {
        _thisFirld = GetComponent<InputField>();
    }

    public bool CheckIsNullOrEmpty() 
    {
        return string.IsNullOrEmpty(_thisFirld.text);
    }
}
