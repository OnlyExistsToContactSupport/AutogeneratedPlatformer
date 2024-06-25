using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class PlatformFactory : ScriptableObject
{
    //-50/50x,z 0/100y
    //Platform size = 15x15x1
    //7.5x7.5x0.5 from de center


    //Min and Max values for the ending platform, which is the start of the created path
    private int maxStart;
    private int minStart;

    //O valor MAX para as coordenadas x e z pode ser fora da caixa, mas apenas o suficiente para conseguir ter uma parte dentro do mapa.
    //Isto cria apenas um pouco de dificuldade extra pois existe menos espaço de manobra, não dificultando demasiado!
    private float maxHorizontalPosition;

    //Distância máxima de salto
    private float MaxDistanceHorizontal;
    private float MaxDistanceVertical;

    //Variáveis para as posições das plataformas
    private Vector3 nextPosition;
    private Vector3 InitPosition;
    private Vector3 LastPlatform;

    //Variáveis para as direções possíveis
    private Vector2 mainDir;
    private Vector2[] directions;

    //Variável para guardar as posições das plataformas
    public List<Vector3> PlatformsPositions;
    private List<GameObject> platforms;

    //Variáveis para o controlo do número de plataformas
    private int PlatformIndex;
    private int maxPlatforms;

    private float yDist;
    private float distModifier;
    private float auxDif;

    private float MDSquared;

    private int startPosX;
    private int startPosZ;
    private float startPosY;

    private float xDist;
    private float xSquared;
    private float zDist;

    private float newX;
    private float newZ;
    private List<GameObject> ceilings;

    public List<Vector3> GeneratePlatforms(PlatformTypes platformTypes)
    {
        platforms = new List<GameObject>();

        maxStart = 90;//Valores max e min para
        minStart = 80;//a posição inicial da primeira plataforma

        maxHorizontalPosition = 50f;//Valor máximo para as coordenadas x e z para pelo menos parte da plataforma ficar dentro da àrea de jogo
        MaxDistanceHorizontal = 20f;//Distância máxima horizontal de salto
        MaxDistanceVertical = 7.4f;//Distância máxima vertical de salto

        //Controlos do número de plataformas
        maxPlatforms = 30;
        PlatformIndex = 0;

        //Vetor das direções possíveis
        directions = new Vector2[]
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
        mainDir = new Vector2(0, 0);

        InitPosition = new Vector3(0, 0, 0);
        nextPosition = new Vector3(0, 0, 0);
        LastPlatform = new Vector3(0, 0, 0);

        PlatformsPositions = new List<Vector3>();

        //Controls for the maximum distance between platforms
        yDist = 0;
        distModifier = 0;
        auxDif = 0;

        xDist = 0;
        xSquared = 0;
        zDist = 0;

        newX = 0;
        newZ = 0;

        //Calculating the square of the maximum distance between platforms for the Pythagorean theorem
        MDSquared = MaxDistanceHorizontal * MaxDistanceHorizontal;

        //Getting random parameters for the first platform
        startPosX = Random.Range(-1, 1);
        startPosZ = Random.Range(-1, 1);
        startPosY = Random.Range(minStart, maxStart);

        //Getting a main direction for the path based on the initial position
        if (startPosX == 0 && startPosZ == 0)
        {
            mainDir = directions[Random.Range(0, 7)];
        }
        else
        {
            mainDir = new Vector2(-startPosX, -startPosZ);
        }

        //Creating the first platform on one of the 9 possible positions, after having the height
        InitPosition = new Vector3(42.5f * startPosX, startPosY, 42.5f * startPosZ);
        LastPlatform = InitPosition;

        //SpawnPlatform(InitPosition);
        //GameObject Plataforma = Instantiate(Platform, InitPosition, Quaternion.identity);
        //Plataforma.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 0);

        // TODO - Criar uma lista para guardar as posições das plataformas

        PlatformsPositions.Add(LastPlatform);//[PlatformIndex] = LastPlatform;
        PlatformIndex++;
        maxPlatforms--;


        //Loop para criar o path de plataformas
        for (; maxPlatforms > 0 && LastPlatform.y > MaxDistanceVertical; maxPlatforms--)
        {

            yDist = Random.Range(4, MaxDistanceVertical);
            auxDif = (LastPlatform.y - yDist - MaxDistanceVertical) / (maxPlatforms - 1);

            if (auxDif > MaxDistanceVertical)//Verifica se é possível chegar ao chão
            {
                while (auxDif > MaxDistanceVertical)//Caso não seja, ajusta a distância
                {
                    yDist = Random.Range(yDist, MaxDistanceVertical);
                    auxDif = (LastPlatform.y - yDist - MaxDistanceVertical) / (maxPlatforms - 1);
                }
            }

            //Modificador de distância máxima entre plataformas, consoante a altura da plataforma
            distModifier = yDist / MaxDistanceVertical / 2;
            MaxDistanceHorizontal = MaxDistanceHorizontal * distModifier + 15;
            //15 é o metade de cada plataforma, pois é necessário considerar esta distância para a distância máxima entre plataformas
            //Uma vez que as posições utilizadas são as do centro das plataformas

            //Calcula uma distância possível para a próxima plataforma pelo teorema de Pitágoras x^2 + z^2 = MDSquared
            xDist = Random.Range(15f, MaxDistanceHorizontal);//x Distance
            xSquared = xDist * xDist;
            zDist = 15f;//minimum distance for the z axis
            float aux = Random.Range(0, (float)Math.Sqrt(MDSquared - xSquared));

            if (aux > zDist)//Comparação para escolher a menor distância segundo a minima distância possivel
                zDist = aux;

            newX = LastPlatform.x + xDist * mainDir.x;
            newZ = LastPlatform.z + zDist * mainDir.y;

            //Verifica se a nova posição está dentro da área de jogo e ajusta a posição caso não esteja
            if (Math.Abs(newX) > maxHorizontalPosition || Math.Abs(newZ) > maxHorizontalPosition)
            {
                Vector2 auxDir = mainDir;
                float offset = 0;
                while (Math.Abs(newX) > maxHorizontalPosition || Math.Abs(newZ) > maxHorizontalPosition)
                {
                    mainDir = directions[Random.Range(0, 7)];
                    //Verifica se a direção escolhida é a oposta à direção anterior
                    if (mainDir == -auxDir)
                    {
                        //creates a small offset to the new position to it doesn't go right under the 2nd Last Platform
                        offset += 1.5f;
                    }
                    //subtracting so we dont exceed the max distance
                    newX = LastPlatform.x + (xDist - offset) * mainDir.x;
                    newZ = LastPlatform.z + (zDist - offset) * mainDir.y;
                }
            }

            //Cria a nova posição para a plataforma
            nextPosition = new Vector3(newX, LastPlatform.y - yDist, newZ);

            //Verifica se a nova altura é inferior à altura máxima de salto
            if (nextPosition.y < MaxDistanceVertical)// O que indica que é a última plataforma
            {//Caso seja, ajustamos as posições de X e Z, para ficarem dentro dos controlos usados para a geração da Maze
                //A ultima plataformas terá de ficar dentro dos limites do Mapa
                if (Math.Abs(nextPosition.x) > 45)
                {
                    float DifferenceX = nextPosition.x % 45;
                    float SignedDifX = DifferenceX - DifferenceX;
                    nextPosition.x -= SignedDifX;
                }
                if (Math.Abs(nextPosition.z) > 45)
                {
                    float DifferenceZ = nextPosition.z % 45;
                    float SignedDifZ = DifferenceZ - DifferenceZ;
                    nextPosition.z -= SignedDifZ;
                }
                //E centrada no tile usado para a geração do Maze
                if (nextPosition.x % 10 != 5 || nextPosition.z % 10 != 5)
                {
                    float DifferenceX = nextPosition.x % 10;
                    float DifferenceZ = nextPosition.z % 10;
                    float SignedDifX = 5 - DifferenceX;
                    float SignedDifZ = 5 - DifferenceZ;
                    nextPosition.x += SignedDifX;
                    nextPosition.z += SignedDifZ;
                }
            }

            //Saving the position of the new platform
            LastPlatform = nextPosition;
            PlatformsPositions.Add(LastPlatform);//[PlatformIndex] = LastPlatform;
            PlatformIndex++;

            //Debug.Log("A criar uma plataforma na posição: " + LastPlatform);
            //40% de chance de mudar de direção
            if (Random.Range(1, 100) <= 40)
            {
                mainDir = directions[Random.Range(0, 7)];
            }
        }

        Vector3 SecondPlatform = PlatformsPositions[PlatformIndex - 2];
        Vector3 FirstPlatform = PlatformsPositions[PlatformIndex - 1];


        //Retira os tiles de teto mais próximos entre a primeira e a segunda plataforma para o jogador conseguir continuar o caminho
        ceilings = GameObject.FindGameObjectsWithTag("Ceiling").ToList().OrderBy(x => Vector3.Distance(x.transform.position, new Vector3(
            (SecondPlatform.x + FirstPlatform.x) / 2, (SecondPlatform.y + FirstPlatform.y) / 2, (SecondPlatform.z + FirstPlatform.z) / 2))
        ).ToList();

        //Desativa os 6 tiles mais próximos encontrados anteriormente
        for (int i = 0; i < 6; i++)
        {
            //Debug.Log("Setting a platform to inactive");
            ceilings[i].SetActive(false);
        }

        SpawnPlatforms(platformTypes);

        return PlatformsPositions;
    }
    public void ResetCeilings()
    {
        if(ceilings != null)
        {
            if(ceilings.Count > 0) { 
                for (int i = 0; i < ceilings.Count; i++)
                {
                    ceilings[i].SetActive(true);
                }
            }
        }
    }

    private void SpawnPlatforms(PlatformTypes platformTypes)
    {
        for (int i = 0; i < PlatformsPositions.Count - 1; i++)
        {
            GameObject First2Plats = platformTypes.basePlatform;

            if (i == 0)
            {
                platforms.Add(Instantiate(platformTypes.endPlatform, PlatformsPositions[i], Quaternion.identity));
            }
            else if (i < PlatformsPositions.Count - 2)
            {
                GameObject platform;
                switch (UnityEngine.Random.Range(0, 5))
                {
                    case 3:
                        platform = platformTypes.firePlatform;
                        break;
                    case 4:
                        platform = platformTypes.grassPlatform;
                        break;
                    default:
                        platform = platformTypes.basePlatform;
                        break;
                }

                platforms.Add(Instantiate(platform, PlatformsPositions[i], Quaternion.identity));
            }
            else
            {
                //First2Plats.transform.localScale = new Vector3(10, 1, 10);

                GameObject platInstantiate1 = Instantiate(First2Plats, PlatformsPositions[i], Quaternion.identity);
                GameObject platInstantiate2 = Instantiate(First2Plats, PlatformsPositions[++i], Quaternion.identity);

                platInstantiate1.transform.localScale = new Vector3(10, 1, 10);
                platInstantiate2.transform.localScale = new Vector3(10, 1, 10);

                platforms.Add(platInstantiate1);
                platforms.Add(platInstantiate2);
            }
        }
    }
    public void DestroyPlatforms()
    {
        for (int i = 0; i < platforms.Count - 1; i++)
        {
            Destroy(platforms[i]);
        }
    }


    public int GetPlatformIndex()
    {
        return PlatformIndex;
    }
}
