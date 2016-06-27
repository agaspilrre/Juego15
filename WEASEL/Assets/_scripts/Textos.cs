using UnityEngine;
using UnityEngine.UI;//para poder utilizar text
using System.Collections;

public class Textos : MonoBehaviour {

    [Header("Distancia entre texto")]
    public float textDistance = 30;//distancia entre textos

    int minChar = 32;//basado en codigo ascii
    int maxChar = 91;

    public string textoABuscar = "ME LLAMO LUIS";//frase que tiene q buscar el random se puede cambiar desde la interfaz grafica al ser una variable publica

    public Text[] textos=new Text[20];//array de textos
    RectTransform auxT;//variable auxiliar del recttransform para establecer posicion y tamaños de los textos
    

    


    public Text textoReferencia;//variable que guarda el texto que hay que buscar
    public Text fMasCercana;//variable texto que guarda la frase que mas puntuacion saca de las 20 frases aleatorias
    int contadorFuncion;//variable para controlar cuantas veces se llama a la funcion

    
    char[] frase;//array de letras
    int puntos;//variable que registra puntuacion de cada frase
    int[] puntuacionesCadaFrase=new int[20];//array que guarda la puntuacion de cada frase

    public bool otraVuelta=false;//booleano que controla las llamadas a la funcion
    

    // Use this for initialization
    void Start () {

        textoReferencia.text = textoABuscar;//igualamos la frase de referencia al texto que hay que buscar
        fMasCercana.text = "vacio";//al comenzar el programa contiene la palabra vacio

        for(int i = 0; i < textos.Length; i++)
        {
            textos[i].text = "hola";
            auxT = textos[i].GetComponent<RectTransform>();//obtengo transform del texto padre
            auxT.anchoredPosition = this.GetComponent<RectTransform>().anchoredPosition + (i + 1) * Vector2.down * textDistance;//establecemos la distancia entre los textos podemos aumentarla con la variable publica texdistance
            auxT.anchorMax = this.GetComponent<RectTransform>().anchorMax;
            auxT.anchorMin = this.GetComponent<RectTransform>().anchorMin;//establezco los anchos minimos y maximos del rectangulo
            auxT.pivot = this.GetComponent<RectTransform>().pivot;//obtengo el pivote padre

            auxT.sizeDelta = this.GetComponent<RectTransform>().sizeDelta;//hace que el ancho del rectangulo sea igual al ancho del cuadro padre
        }

        frasesRandom();//llamo a la funcion frases random

    }
	
	// Update is called once per frame
	void Update () {

        //comprueba la variable otra vuelta en el momento que esta se pone a true ejecuta el codigo del interior
        if (otraVuelta==true)
        {
            
            otraVuelta = false;//pongo booleano a falso para que no llame constantemente a la funcion
            frasesRandom();//llamo a la funcion frasesrandom 

        }
           
	
	}

   

    public void frasesRandom()
    {
        //recorremos el array de textos
        for(int i = 0; i < textos.Length; i++)
        {
            textos[i].text = letrasRandom();//en cada posicion le asignamos el texto que devuelve letrasrandom otra funcion descrita mas abajo
            puntuacionesCadaFrase[i] = puntos;//guardamos los puntos de la frase que recibimos de letras random
        }

       comprobarFraseMasCercana();//cuando tenemos las 20 frases llamamos a esta funcion para que compruebe todas las frases y elija la que tiene mas puntuacion

        
    }


    public string letrasRandom()
    {
        puntos = 0;//cada vez q pasa a este metodo los puntos se igualan a cero
        contadorFuncion++;//cuenta las veces que llama a la funcion
        frase = new char[textoABuscar.Length];//creamos array de chars con el tamaño de textoabuscar
        //esta condicion es porque al estar llamando desde el update se metia tan rapido que sobrescribia los datos sin poder compararlos y hacia bucle infinito
        if (contadorFuncion > 100)
        {
            frase = fMasCercana.text.ToCharArray();//convierto la frase mas cercana en un array de char y lo almaceno en el array frase
        }
        
        char[] aBuscar = textoABuscar.ToCharArray();//convierto el texto a buscar en array de char para poder compararlo con frase
        

        for (int i = 0; i < frase.Length; i++)
        {
            
           
            //si la posicion es iguala la posicion de la frase referencia sumo punto
            if (frase[i] == aBuscar[i])
            {
                puntos++;
            }

            else
            {
                frase[i] = (char)Random.Range(minChar, maxChar);//si no hago un random en la posicion de la letra y despues comparo si el random generado es igual a la posicion de referencia sumando asi puntos
                if (frase[i] == aBuscar[i])
                {
                    puntos++;
                }
            }
            
        }



        


        return CharArrayToString(frase); ;//devuelvo el array de char convertido en string
    }

    //funcion que convierte un array de char en string
    string CharArrayToString(char[] arr)
    {
        string aux = "";



        for (int i = 0; i < arr.Length; i++)
        {
            aux += arr[i];
        }


        return aux;
    }


    public void comprobarFraseMasCercana()
    {

        int aux = 0;
        int posicion = 0;//guardar la posicion
        

        //guardo auxiliar
        //recorro el array que alamcenaba las puntuaciones de cada frase
        for(int i=0; i < puntuacionesCadaFrase.Length; i++)
        {
            if (aux < puntuacionesCadaFrase[i])
            {
                aux = puntuacionesCadaFrase[i];//si aux q inicialmente vale cero encuentra una posicion con numero mas alto al suyo se guarda esa posicion y los puntos que contienen
                posicion = i;
            }
        }

        fMasCercana.text = textos[posicion].text;//la frase mas cercana es igual a la frase contenida en la posicion indicada con la puntuacion mas alta del bucle anterior
        

        


        //si aux tiene una puntuacion mayor o igual que el tamaño de textos a buscar significa que la frase ya se ha construido
        if (aux >= textoABuscar.Length)
        {
            fMasCercana.color = new Color(0, 1, 0, 1);//pongo el texto encontrado en color verde
            contadorFuncion = 0;//pongo contador de funcion a 0
        }

        else
        {
            //igualo todos los textos del array a la frase mas cercana para que haga el random a raiz de esa frase

            
            for (int i = 0; i < textos.Length; i++)
            {
                textos[i].text = fMasCercana.text;//igualo el array de textos adquiriendo la frase mas cercana que el programa ha comprobado
            }
            

            otraVuelta = true;//pongo esta variable a true para que desde el update me vuelve a llamar a la funcion generar frase ya que todavia no hemos encontrado la frase
          
        }

       

    }

    



}
