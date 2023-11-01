/* using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class JugadorBolita : MonoBehaviour
{

    private Rigidbody salto;

    float velocidad = 10f;


    float movVertical;
    float movHorizontal;
    float movEjeZ;

    public float saltoForce = 0.5f;
    private bool enElSuelo = true;

    [SerializeField]
    GameObject PantallaDerrota;


    float tiempoderrota = 2.0f;

    bool personajemuerto = false;

    


    // Start is called before the first frame update
    void Start()
    {
        salto = GetComponent<Rigidbody>();
        enElSuelo = true;
        

        PantallaDerrota.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //MOVER PERSONAJE variables

        movVertical = Input.GetAxis("Vertical") * Time.deltaTime * velocidad;
        movHorizontal = Input.GetAxis("Horizontal") * Time.deltaTime * velocidad;

        transform.Translate(movHorizontal, movEjeZ, movVertical);


        if (Input.GetKeyDown("space") && enElSuelo == true)
        {

            Saltar();
        }

        //TIEMPO muerto

        if (personajemuerto)
        {
            tiempoderrota -= Time.deltaTime;

            if (tiempoderrota <= 0.0f)
            {
                // Restablece las restricciones de movimiento

                personajemuerto = false;

                // Reiniciar la escena actual
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);

                // Para reiniciar otros elementos del juego si es necesario
                GameController.ReiniciarCapsulas();

                PantallaDerrota.SetActive(false);

                // Reiniciar el tiempo para que siga funcionando
                tiempoderrota = 2.0f;
            }



        }


    }

    void Saltar()
    {

        salto.AddForce(Vector2.up * saltoForce, ForceMode.Impulse);
        enElSuelo = false;

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("suelo"))
        {
            enElSuelo = true;
        }

        if (collision.gameObject.CompareTag("enemigo"))
        {
            salto.velocity = Vector2.zero;
            PantallaDerrota.SetActive(true);
            personajemuerto = true;

        }
    }

    
} */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JugadorBolita : MonoBehaviour
{
    private Rigidbody salto;
    public Transform camara; // Referencia a la cámara en tercera persona.

    public float velocidad = 10f;
    public float saltoForce = 0.5f;

    private bool enElSuelo = true;

    [SerializeField]
    GameObject PantallaDerrota;

    float tiempoderrota = 2.0f;
    bool personajemuerto = false;


    private AudioSource caida;


    private void Awake()
    {
        caida = gameObject.GetComponent<AudioSource>();

    }


    void Start()
    {
        salto = GetComponent<Rigidbody>();
        enElSuelo = true;
        PantallaDerrota.SetActive(false);
    }

    void Update()
    {
        // MOVER PERSONAJE variables
        float movVertical = Input.GetAxis("Vertical");
        float movHorizontal = Input.GetAxis("Horizontal");

        // Calcula el movimiento en relación con la cámara.
        Vector3 movimiento = camara.transform.forward * movVertical + camara.transform.right * movHorizontal;
        movimiento.y = 0; // Mantén el movimiento en el plano horizontal.

        // Normaliza el vector para mantener la velocidad constante al mover diagonalmente.
        if (movimiento.magnitude > 1)
        {
            movimiento.Normalize();
        }

        transform.Translate(movimiento * velocidad * Time.deltaTime);

        if (Input.GetKeyDown("space") && enElSuelo == true)
        {
            Saltar();
        }

        // TIEMPO muerto
        if (personajemuerto)
        {
            tiempoderrota -= Time.deltaTime;

            if (tiempoderrota <= 0.0f)
            {
                // Restablece las restricciones de movimiento.
                personajemuerto = false;

                // Reiniciar la escena actual
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);

                // Para reiniciar otros elementos del juego si es necesario
                GameController.ReiniciarCapsulas();

                PantallaDerrota.SetActive(false);

                // Reiniciar el tiempo para que siga funcionando
                tiempoderrota = 2.0f;
            }
        }
    }

    void Saltar()
    {
        salto.AddForce(Vector2.up * saltoForce, ForceMode.Impulse);
        enElSuelo = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("suelo"))
        {
            enElSuelo = true;
        }

        if (collision.gameObject.CompareTag("Plataforma"))
        {
            enElSuelo = true;
        }

        if (collision.gameObject.CompareTag("enemigo"))
        {

            // Código para desactivar la musica de la camara

            GameObject camaraPrincipal = GameObject.FindGameObjectWithTag("MainCamera");

            if (camaraPrincipal != null)
            {
                // Acceder al componente para eliminar o desactivar
                AudioSource componente = camaraPrincipal.GetComponent<AudioSource>();

                if (componente != null)
                {
                    // Desactivar el componente
                    componente.enabled = false; // Esto desactivará el componente
                }
            }

            caida.enabled = true;
            caida.Play();


            salto.velocity = Vector2.zero;
            PantallaDerrota.SetActive(true);
            personajemuerto = true;
        }
    }
}

