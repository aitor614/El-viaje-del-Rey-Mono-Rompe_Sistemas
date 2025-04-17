using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlPremioBreakout : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(EsperarYCargar());
    }

    IEnumerator EsperarYCargar()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("YouWin");
    }
}
