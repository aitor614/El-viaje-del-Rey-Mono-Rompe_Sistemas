using TMPro;
using UnityEngine;

public class ControlInstrucciones : MonoBehaviour
{
    [Header("Textos")]
    public TextMeshProUGUI titulo;
    public TextMeshProUGUI contenido;

    int tiempo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ActualizarInstrucciones();
    }

    // Update is called once per frame
    void Update()
    {
        ActualizarInstrucciones();
    }


    void ActualizarInstrucciones()
    {
        if (PlayerPrefs.GetInt("TiempoPartida") != tiempo)
        {
            tiempo = PlayerPrefs.GetInt("TiempoPartida", 0);
        }

        switch (PlayerPrefs.GetString("EscenaActual"))
        {
            case "Juego2DEscapeInfierno":
                titulo.text = "Escape del infierno  " + tiempo + "''";
                contenido.text = "Ayuda a Sun Wukong a escalar una montaña infernal plagada de trampas y enemigos. Salta de plataforma en plataforma, esquiva peligros y muévete con agilidad de un lado a otro para alcanzar la cima antes de que los demonios lo atrapen.";
                break;
            case "Juego2DHuidaCelestial":
                titulo.text = "Huida celestial  " + tiempo + "''";
                contenido.text = "Surca los cielos con el espíritu de un joven dragón. Supera obstáculos flotantes con saltos precisos y alcanza las alturas del mundo celestial con ritmo y determinación.";
                break;
            case "Juego2DGolpeBaston":
                titulo.text = "Golpe de bastón  " + tiempo + "''";
                contenido.text = "Canaliza tu energía espiritual para destruir bloques malditos que sellan el paso al templo. Controla el orbe de poder con reflejos impecables y libera las reliquias escondidas tras cada muro místico.";
                break;
            case "JuegoAREspiritusDesencarnados":
                titulo.text = "Espíritus desencarnados  " + tiempo + "''";
                contenido.text = "Explora el mundo real y encuentra espíritus ocultos a tu alrededor. Usa tu vista interior para localizarlos, apunta con precisión y purifícalos tocando la pantalla. ¡Conviértete en el monje más rápido en capturar entidades del más allá!";
                break;
            case "JuegoVRBatallaCelestial":
                titulo.text = "Batalla celestial  " + tiempo + "''";
                contenido.text = "Defiende el último bastión del mundo celestial. Los demonios avanzan sin descanso y tú eres la última línea de defensa. Apunta con precisión, dispara sin miedo y sobrevive al asedio de los soldados voladores.";
                break;
            default:
                titulo.text = "Sin instrucciones";
                contenido.text = "No se han definido instrucciones.";
                break;
        }
    }
}
