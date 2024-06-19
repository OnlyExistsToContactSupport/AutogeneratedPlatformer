using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnPlatforms : MonoBehaviour
{
    //-50/50x,z 0/100y
    //Platform size = 10x10x1
    //5x5x0.5 from de center

    public GameObject Platform;
    // Start is called before the first frame update

    public int maxStart = 80;
    public int minStart = 60;

    //O valor MAX para as coordenadas x e z pode ser fora da caixa, mas apenas o suficiente para conseguir ter uma parte dentro do mapa.
    //Isto cria apenas um pouco de dificuldade extra pois existe menos espaço de manobra, não dificultando demasiado!
    public float maxX = 55f;//42.5f;
    public float maxZ = 55f;//42.5f;
    //System.Random rnd = new System.Random();
    public int value;
    public float x, y, z = 0;
    public int testar;
    public int maxPlatforms = 20;
    public int dificulty = 1;
    public int PlatformID = 0;

    public float MaxDistanceHorizontal = 21.169f;
    public float MaxDistanceVertical = 8.35f;
    //public float MinDistHor = 21.213f; //valor minimo para as plataformas não se sobreporem com y = 0;
    private Vector2[] directions = new Vector2[]
    {
        new (0, 1),  // Up
        new (0, -1), // Down
        new (-1, 0), // Left
        new (1, 0),  // Right
        new (-1, 1), // Up-Left
        new (1, 1),  // Up-Right
        new (-1, -1),// Down-Left
        new (1, -1)  // Down-Right
    };

    private Vector3 LastPlatform;
    private Vector3[] PlatformsPositions;
    int PlatformIndex = 0;

    void Start()
    {

        PlatformsPositions = new Vector3[maxPlatforms];

        float MDSquared = MaxDistanceHorizontal * MaxDistanceHorizontal;
        //Getting the Platform's size
        Renderer renderer = Platform.GetComponent<Renderer>();
        Vector3 size = renderer.bounds.size;
        float width = size.x; // Largura (eixo X)
        float height = size.y; // Altura (eixo Y)
        float depth = size.z; // Profundidade (eixo Z)

        //Getting random parameters for the first platform
        int startPosX = Random.Range(-1, 1);
        int startPosZ = Random.Range(-1, 1);
        float startPosY = Random.Range(minStart, maxStart);
        Vector2 mainDir;

        //Getting a main direction for the path
        if (startPosX == 0 && startPosZ == 0)
        {
            mainDir = directions[Random.Range(0, 7)];
        }
        else
        {
            mainDir = new Vector2(-startPosX, -startPosZ);
        }
        //Debug.Log("mainDir= " + mainDir.x + ", " + mainDir.y);

        //Creating the first platform on one of the 9 possible positions
        Vector3 InitPosition = new Vector3(x = maxX * startPosX, y = startPosY, z = maxZ * startPosZ);
        LastPlatform = InitPosition;

      

        Instantiate(GameObject.FindGameObjectWithTag("PlatformTypes").GetComponent<PlatformTypes>().grassPlatform, InitPosition, Quaternion.identity);
        PlatformsPositions[PlatformIndex] = LastPlatform;
        PlatformIndex++;
        maxPlatforms--;

        float yDist;// = Random.Range(0, MaxDistanceVertical);
        //float dif;// = y / maxPlatforms;
        float distModifier;// = dif / MaxDistanceVertical / 2;
                           //MaxDistanceHorizontal = MaxDistanceHorizontal * distModifier + 15;


        for (; maxPlatforms > 0; maxPlatforms--)
        {
            if (y < MaxDistanceVertical)
                break;


            //Verifica se é possível chegar ao chão
            yDist = Random.Range(4, MaxDistanceVertical);
            float auxDif = (y - yDist - MaxDistanceVertical) / (maxPlatforms - 1);
            //Debug.Log("yDist= " + yDist + " auxDif = " + auxDif);
            if (auxDif > MaxDistanceVertical)
            {//Caso não seja, ajusta a distância
                while (auxDif > MaxDistanceVertical)
                {
                    yDist = Random.Range(yDist, MaxDistanceVertical);
                    auxDif = (y - yDist - MaxDistanceVertical) / (maxPlatforms - 1);
                }
                //Debug.Log("Inside Loop: yDist= " + yDist + " auxDif = " + auxDif);
            }

            //Modificador de distância máxima entre plataformas, consoante a altura da plataforma
            distModifier = yDist / MaxDistanceVertical / 2;
            MaxDistanceHorizontal = MaxDistanceHorizontal * distModifier + 15;
            //Debug.Log("distModifier = " + distModifier + " MaxDistanceHorizontal = " + MaxDistanceHorizontal);

            //Calcula uma distância possível para a próxima plataforma
            float xDist = Random.Range(15f, MaxDistanceHorizontal);//x Distance

            float xSquared = xDist * xDist;
            //Debug.Log("xDist = " + xDist + " xSquared = " + xSquared + " MDSquared = " + MDSquared);
            float zDist = 15f;
            float aux = Random.Range(0, (float)Math.Sqrt(MDSquared - xSquared));
            if (aux > zDist)
                zDist = aux;



            //Debug.Log("We have z=SQRT(" + MDSquared + "-" + xSquared+"<=>z="+zDist);

            /*if(Random.Range(0,1)==0)
                xDist = -xDist;
            if(Random.Range(0,1)==1)
                zDist = -zDist;
            Debug.Log("xDist = " + xDist + " zDist = " + zDist);
            Debug.Log("LastPlatformx&z="+ LastPlatform.x + "," + LastPlatform.z);*/

            //xDist *= mainDir.x;
            //zDist *= mainDir.y;
            //Debug.Log("After mainDir multiplication xDist = " + xDist + " zDist = " + zDist);
            //Debug.Log("xDist = " + xDist + " zDist = " + zDist);
            //Debug.Log("mainDir= " + mainDir.x + ", " + mainDir.y);
            //Debug.Log("NewPosition = " + (x + xDist * mainDir.x) + ", " + (y - yDist) + ", " + (z + zDist * mainDir.y));
            //Debug.Log("Limit Check Starting.... for xMax = " + maxX + " and zMax = " + maxZ);
            float newX = x + xDist * mainDir.x;
            float newZ = z + zDist * mainDir.y;
            if (Math.Abs(newX) > maxX || Math.Abs(newZ) > maxZ)
            {
                Vector2 auxDir = mainDir;
                //Debug.Log("New x = " + newX + " New z = " + newZ);
                while (Math.Abs(newX) > maxX || Math.Abs(newZ) > maxZ)
                {
                    mainDir = directions[Random.Range(0, 7)];
                    if (mainDir == -auxDir)
                    {
                        //adding a small offset to the new position
                        newX = x + xDist * mainDir.x + 1.5f;
                        newZ = z + zDist * mainDir.y + 1.5f;
                    }
                    else
                    {
                        newX = x + xDist * mainDir.x;
                        newZ = z + zDist * mainDir.y;
                    }
                }
            }
            //xDist *= mainDir.x;
            /*if (mainDir.x == 0 && mainDir.y == 0)
            {
                mainDir.y = Random.Range(0, 1) == 0 ? -1 : 1;
                if(Math.Abs(z + zDist * mainDir.y) > maxZ)
                {
                    mainDir.y = -mainDir.y;
                }
                zDist *= mainDir.y;
            }*/




            /*mainDir.x = Random.Range(0, 1)==1 ? -1 : 0;
            xDist *= mainDir.x;
            if (mainDir.x == 0 && mainDir.y == 0)
            {
                mainDir.y = Random.Range(0, 1) == 0 ? -1 : 1;
                if(Math.Abs(z + zDist * mainDir.y) > maxZ)
                {
                    mainDir.y = -mainDir.y;
                }
                zDist *= mainDir.y;
            }    
        }
        else if(Math.Abs(z+zDist*mainDir.y) > maxZ)
        {
            Debug.Log("New z = " + (z + zDist * mainDir.y));
            mainDir.y = Random.Range(0, 1)==1 ? -1 : 0;
            zDist *= mainDir.y;
            //zDist = -zDist;
            if (mainDir.x == 0 && mainDir.y == 0)
            {
                mainDir.x = Random.Range(0, 1) == 0 ? -1 : 1;
                if (Math.Abs(x + xDist * mainDir.x) > maxX)
                {
                    mainDir.x = -mainDir.x;
                }
                xDist *= mainDir.x;
            }
        }
        else
        {
            xDist *= mainDir.x;
            zDist *= mainDir.y;
        }*/

            /*if(Math.Abs(LastPlatform.x+xDist) > maxX)
            {
                x = LastPlatform.x - xDist;
            }
            else
            {
                x = LastPlatform.x + xDist;
            }   
            
            if(Math.Abs(LastPlatform.z+zDist) > maxZ)
            {
                z = LastPlatform.z - zDist;
            }
            else
            {
                z = LastPlatform.z + zDist;
            }*/

            //z = Random.Range((float)Math.Sqrt(Math.Pow(MinDistHor, 2) - Math.Pow();//z Distance





            //x = Random.Range(/*platforms[PlatformID-1].transform.position.x*/LastPlatform.x - MaxDistanceHorizontal, /*platforms[PlatformID-1].transform.position.x*/LastPlatform.x + MaxDistanceHorizontal);
            //float zDistance = (float)Math.Sqrt(Math.Pow(MaxDistanceHorizontal, 2) - Math.Pow(x - /*platforms[PlatformID - 1].transform.position.x*/LastPlatform.x, 2));
            //z = Random.Range(LastPlatform.z - zDistance, LastPlatform.z + zDistance);
            Vector3 nextPosition = new Vector3(x = newX, y = y - yDist, z = newZ);
            //Vector3 nextPosition = new Vector3(x = x+xDist, y = y - yDist,z= z+zDist);
            //Debug.Log("Next position = " + nextPosition.x + ", " + nextPosition.y + ", " + nextPosition.z);
            //Vector3 nextPosition = new Vector3(x = Random.Range(Math.Max(x - 15, min - 50), Math.Min(x + 15, max - 50)), y = y - dif, z = Random.Range(Math.Max(z - 15, min - 50), Math.Min(z + 15, max - 50)));
            Instantiate(Platform, nextPosition, Quaternion.identity);
            //Debug.Log("Next position = " + nextPosition.x + ", " + nextPosition.y + ", " + nextPosition.z);
            LastPlatform = nextPosition;
            PlatformsPositions[PlatformIndex] = LastPlatform;
            PlatformIndex++;
            //maxPlatforms--;
            //Debug.Log("Position of Platform =>  x=" + LastPlatform.x + " y=" + LastPlatform.y + " z=" + LastPlatform.z);

            if (Random.Range(1, 10) <= 2)
            {
                //20% de chance de mudar de direção
                mainDir = directions[Random.Range(0, 7)];
                //Debug.Log("mainDir= " + mainDir.x + ", " + mainDir.y);
            }
        }

        //As restante, são semi aleatórias
        //Uma vez que recebem valores aleatórias a uma distância máxima na anterior.


        width = size.x; // Largura (eixo X)
        height = size.y; // Altura (eixo Y)
        depth = size.z; // Profundidade (eixo Z) 
        //Debug.Log("Size of Platform =>  x=" + width + " y=" + height + " z=" + depth);


        //Creating test platforms
        /*Vector3 TestPosition1 = new Vector3(0, 2, 0);
        Instantiate(Platform, TestPosition1, Quaternion.identity);
        Vector3 TestPosition2 = new Vector3(25f, 5, 0);
        Instantiate(Platform, TestPosition2, Quaternion.identity);*/

    }

    // Update is called once per frame
    void Update()
    {

    }
}
