using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GameManagerFichas : MonoBehaviour {
    Ficha[] fichas;//array de tipo ficha
    int similaridadActual = -1;//variable que guarda valor de similaridad para recolocar las fichas
    Ficha espacio;
    int randomProbability;
    RaycastHit hit;
    Ficha actualFicha;
    bool finished = true;
    int contador = 0;
    int vueltas = 7;
    bool manualMode = false; 

    // Use this for initialization
   





    void Start () {

      fichas = new Ficha[16]; 
        for (int i = 0; i<16; i++)
        {
            fichas[i] = GameObject.Find((i + 1).ToString()).GetComponent<Ficha>();//guardamos el componente fichas en el array de fichas
        }
        espacio = fichas[15];//guardamos la posicion del espacio en la variable espacio
        espacio.IsSpace = true; //le decimos a esa ficha que es un espacio mediante su propiedad



        manualMode = true; //el mono manual esta activo
        /*
        Reordenar();
        print(ComprobarSimilaridad());



        StartCoroutine(RutinaOrdenar());
        */
	}
	

    //funcion que se ejecuta cuando pulsamos el boton ordenar
    public void ButtonOrdenar()
    {
        manualMode = false;//ponemos el mono manual a false

        StartCoroutine(RutinaOrdenar());//llamamos a la courutine rutina ordenar
    }

    

    int ComprobarSimilaridad()
    {
        int i;
        //empieza el bucle y va incrementando segun vaya viendo fichas bloqueadas y devuelve el valor hasta donde llegue de i el maximo valor que dara sera 16 ya q es el tamaño del array y sera ahi cuando todas las fichas esten bloqueadas porque ya estan en su posicion
        for ( i = 0; i < fichas.Length && fichas[i].Block; i++) ;

        return i; 


    }


    //funcion que se ejecuta cuando pulsamos el boton desordenar, reordena el transform de las fichas
    public void Reordenar()
    {
        int random;

        List<Vector3> aux = new List<Vector3>(); 

        //recorro array de fichas
        for (int i = 0; i<fichas.Length; i++)
        {
            do {
                random = Random.Range(0, fichas.Length);//en random guardo un entero que es el resultado del random entre cero y el numero de fichas que existen

            } while (PerteneceAlArray(fichas[random].GetOriginalPosition(), aux));//comprobamos si coinciden la posicion aleatoria con la posicion i
           
                fichas[i].transform.position = fichas[random].GetOriginalPosition();//si no coincide agregamos la posicion random a la posicion i de la vuelta
                aux.Add(fichas[random].GetOriginalPosition());//guardamos original de la lista para poder comparar en la funcion de pertenece al array
            

        }
    }

    //funcion que compara posicion que le pasamos con la lista que creamos y comprueba si coinciden
    private bool PerteneceAlArray(Vector3 vector, List<Vector3> arr)
    {
        bool resultado = false;

        for (int i = 0; i < arr.Count && !resultado; i++)
        {
            resultado = arr[i] == vector;


        }


        return resultado;


    }


    private void FijarYaColocado()
    {
        bool perdido = true; 
        for (int i = 0; i < fichas.Length && perdido; i++)
        {
            perdido = fichas[i].GetOriginalPosition() == fichas[i].transform.position;//si la posicion original de la ficha es igual a la posicion del array fichas
            fichas[i].Block = perdido; //bloqueamos la ficha ya que estara en su sitio
        }


    }

    private void SwapFicha(Ficha ficha1, Ficha ficha2)
    {
        Vector3 aux = ficha1.transform.position;//variable que guarda el transform de la ficha uno

       
        ficha1.transform.position = ficha2.transform.position;//la ficha uno se iguala al tranform de la ficha 2
        ficha2.transform.position = aux;//y la dos adquiere el transform de la ficha uno
        

    }



    IEnumerator RutinaOrdenar()
    {


        
        Ficha randomFicha;//variable que guarda una ficha
      
        do
        {
           
          
                for (int i = 0; i < fichas.Length; i++)
                {
                //si la ficha no esta bloqueada y comprobar similaridad es distinto de 16
                if (!fichas[i].Block && ComprobarSimilaridad()!= 16)
                {
                   
                     
                            randomFicha = fichas[Random.Range(0, fichas.Length)];//hacemos aleatorio en el array de fichas y guardamos la que salga en la variable ficha

                            //si la ficha esta desbloqueada
                            if (!randomFicha.Block)
                            {
                                
                                SwapFicha(randomFicha, fichas[i]);//cambio la ficha aleatoria por la ficha de la posicion actual
                                FijarYaColocado();//compruebo si este cambio de fichas coincide con el transform original

                            }
                          

                     }

                       
                          
                  }

                    
                
            

            


            yield return new WaitForSeconds(0.000005f);
        } while (ComprobarSimilaridad() != 16);//mientras la variable de similaridad sea distinta de 16 ejecutara el bucle

        print("Estamos en el final " + ComprobarSimilaridad());
        manualMode = true;//vuelvo a poner el modo manual
        desbloquearFichas();//desbloqueo las fichas ya que estan de nuevo colocadas para poder reordenarlas si es preciso
        



    }


    //recorre el array fichas desbloqueandolas para que puedan ser reordenadas
    private void desbloquearFichas()
    {
        for (int i = 0; i < fichas.Length; i++)
        {
            
            fichas[i].Block = false;
        }
    }


	// Update is called once per frame
	void Update () {

        //si el modo manual esta activo
        if (manualMode)
        {
            RaycastHit hit;
            RaycastHit[] hits = new  RaycastHit[4];

            //si tengo el raton pulsado y se lanza un raycast ha la posicion del raton y con ese raycast compruebo que estoy pulsando un objeto con collider y que pertenece al array de fichas entonces ejecuto el codigo
            if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit) && hit.collider != null && hit.collider.gameObject.GetComponent<Ficha>() != null)
            {
                Physics.Raycast(hit.transform.position, hit.transform.right, out hits[0]);//lanzo cuatro raycast izq derech arriba y abajo y lo que obtengo lo guardo en cada posicion del array
                Physics.Raycast(hit.transform.position, -hit.transform.right, out hits[1]);

                Physics.Raycast(hit.transform.position, hit.transform.up, out hits[2]);

                Physics.Raycast(hit.transform.position, -hit.transform.up, out hits[3]);

                //recorro el array de hits
                for (int i = 0; i < hits.Length; i++)
                {
                    //si lo que choca con el rayo es un collider pertenece al array de fichas y la propiedad de espacios le devuelve que es el espacio entonces se mete y ejecuta el codigo
                    if (hits[i].collider != null && hits[i].collider.gameObject.GetComponent<Ficha>() != null && hits[i].collider.gameObject.GetComponent<Ficha>().IsSpace)
                    {
                        SwapFicha(hit.collider.gameObject.GetComponent<Ficha>(), hits[i].collider.gameObject.GetComponent<Ficha>());
                        //llamamos ala funcion swap que nos cambia el transform de la ficha que hemos pinchado con el transform del espacio
                    }

                }

            }

        }
        
    }
        
    
}
