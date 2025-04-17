using UnityEngine;
using TMPro;
public class Tiempo : MonoBehaviour
{
    public TextMeshProUGUI texto;
    
    public void RefreshText(float time)
    {
        texto.text = time.ToString("f0");
    }

}

