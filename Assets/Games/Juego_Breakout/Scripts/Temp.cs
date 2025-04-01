using UnityEngine;
using TMPro;
public class Temp : MonoBehaviour
{
    public TextMeshProUGUI texto;
    

    public void refreshText(float time)
    {
        texto.text = time.ToString("f0");
    }

}

