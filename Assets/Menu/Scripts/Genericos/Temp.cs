using UnityEngine;
using TMPro;
public class Temp : MonoBehaviour
{
    public TextMeshProUGUI texto;
    
    public void RefreshText(float time)
    {
        texto.text = time.ToString("f0");
    }

}

