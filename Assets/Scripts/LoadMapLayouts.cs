using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public class LoadMapLayouts : MonoBehaviour
{

    public static List<Vector3> posConnected = new List<Vector3>();

    public static List<GameObject> allPos = new List<GameObject>();

    public static ArrayList positions = new ArrayList();

    public static GameObject startPlane;
    public static GameObject endPlane;

    static LoadMapLayouts instance;

    public GameObject loadMapsPanel;
    public GameObject editorPlane;
    public GameObject loadingScreen;
    public GameObject canvas;

    public Camera cam;
    public Camera backGroundCamera;

    public Button nextBut;
    public Button prevBut;
    public Button loadButton;
    public Button predefinedButton;

    public AudioClip shhhh;
    public AudioClip click;
    public AudioClip startGame;
    public AudioClip tab;

    string customMapsPath;
    string predefinedMapsPath;
    string currentFileSelected;
    string currentPath;

    int currentPage;
    int maxPages;
    int amountOfEnds;
    int amountOfStarts;
    int width;
    int length;
    int connected;


    float planewidth;

    Vector3 startPos;
    Vector3 endPos;

    Color CnoPlane;
    Color Cstart;
    Color Cend;
    Color CConnected;
    Color CNotConnected;

    List<Button> loadButtons = new List<Button>();
    List<Button> currentButtonsSelected = new List<Button>();

    List<GameObject> floors = new List<GameObject>();

    List<WayPoint> Nodes = new List<WayPoint>();

    List<string> currentFilesSelected = new List<string>();

    AudioSource cameraAudioSource;

    Button currentButSelected;

    ResourceManager resourceManager;

    bool ready;

    public GameObject spacebar;

    float volume;

    AudioClip editor;

    void Start()
    {
        volume = (float)PlayerPrefs.GetInt("SFX") / 100f;

        float musicVolume = (float)PlayerPrefs.GetInt("BGM") / 100f;


        cameraAudioSource = GameObject.Find("BackGroundCamera").GetComponent<AudioSource>();
        resourceManager = GameObject.Find("ResourceManager").GetComponent<ResourceManager>();

        planewidth = resourceManager.planewidth;

        CnoPlane = resourceManager.noPlane;
        Cstart = resourceManager.start;
        Cend = resourceManager.end;
        CConnected = resourceManager.connected;
        CNotConnected = resourceManager.notConnected;

        editor = resourceManager.editorMusic;

        currentPage = 1;

        customMapsPath = Application.dataPath + "/CustomMaps/";
        predefinedMapsPath = Application.dataPath + "/PredefinedMaps/";

        if (!File.Exists(customMapsPath))
        {
            Directory.CreateDirectory(customMapsPath);
        }

        if (!File.Exists(predefinedMapsPath))
        {
            Directory.CreateDirectory(predefinedMapsPath);
        }

        instance = this;

        currentPath = predefinedMapsPath;
        currentPage = 1;

        ColorBlock cb = predefinedButton.colors;
        cb.normalColor = new Color(.5f, .5f, .5f, 1);
        predefinedButton.colors = cb;

        ShowSavedMaps();
        GameObject.Find("backingMusic").GetComponent<AudioSource>().clip = editor;
        GameObject.Find("backingMusic").GetComponent<AudioSource>().volume = musicVolume;
        GameObject.Find("backingMusic").GetComponent<AudioSource>().Play();
    }

    public void ButtonClick()
    {
        cameraAudioSource.PlayOneShot(click, volume);
    }

    public void TabClick()
    {
        cameraAudioSource.PlayOneShot(tab, volume);
    }

    public void PredefinedButton1(Button predefinedbutton)
    {
        currentPath = predefinedMapsPath;
        currentPage = 1;

        ColorBlock cb = predefinedbutton.colors;
        cb.normalColor = new Color(.5f, .5f, .5f, 1);
        predefinedbutton.colors = cb;

        ShowSavedMaps();
        cam.gameObject.SetActive(false);

    }

    public void PredefinedButton2(Button custombutton)
    {
        ColorBlock cb2 = custombutton.colors;
        cb2.normalColor = new Color(1, 1, 1, 0);
        custombutton.colors = cb2;
    }


    public void CustomButton1(Button custombutton)
    {
        currentPath = customMapsPath;
        currentPage = 1;

        ColorBlock cb = custombutton.colors;
        cb.normalColor = new Color(.5f, .5f, .5f, 1);
        custombutton.colors = cb;

        ShowSavedMaps();
        cam.gameObject.SetActive(false);


    }

    public void CustomButton2(Button predefinedbutton)
    {

        ColorBlock cb2 = predefinedbutton.colors;
        cb2.normalColor = new Color(1, 1, 1, 0);
        predefinedbutton.colors = cb2;

    }

    public void customButton()
    {
        ShowSavedMaps();
        currentPath = customMapsPath;
        currentPage = 1;
    }

    public void MainMenuButton()
    {
        Application.LoadLevel("Main menu");
    }

    // Navigating through maps one page further
    public void NextButton()
    {
        if (currentPage < maxPages)
        {
            currentPage++;
            cam.gameObject.SetActive(false);
            ShowSavedMaps();
        }
    }

    // Navigating through maps one page back
    public void PreviousButton()
    {
        if (currentPage > 1)
        {
            currentPage--;
            cam.gameObject.SetActive(false);
            ShowSavedMaps();

        }
    }

    // Use this for initialization
    void ShowSavedMaps()
    {
        int k = 0;
        int j = 0;
        cam.pixelRect = new Rect(Screen.width / 2, Screen.height / 2 - 100, 200, 200);

        loadButtons = new List<Button>();
        foreach (Transform child in loadMapsPanel.transform)
        {
            if (child.gameObject.name.Contains("load"))
                Destroy(child.gameObject);
        }

        int rows = (int)Mathf.Floor(loadMapsPanel.GetComponent<RectTransform>().rect.height / 50) - 1;
        int columns = 1;
        int filesPerPage = rows * columns;

        nextBut.gameObject.SetActive(true);
        prevBut.gameObject.SetActive(true);

        nextBut.GetComponentInChildren<Text>().text = "Next " + filesPerPage + " Maps";
        prevBut.GetComponentInChildren<Text>().text = "Previous " + filesPerPage + " Maps";




        //create a list with the names of all layouts.
        BinaryFormatter bf = new BinaryFormatter();
        //FileStream file = File.
        string[] dirFiles = Directory.GetFiles(currentPath, "*.txt");
        int nextpage = filesPerPage * currentPage;
        int prevpage = filesPerPage * (currentPage - 1);
        int numNotConnected = 0;
        for (int i = 0; i < dirFiles.Length; i++)
        {
            dirFiles[i] = dirFiles[i].Replace(currentPath, "");
            dirFiles[i] = dirFiles[i].Replace(".txt", "");
            loadMapFromFile(dirFiles[i]);

            if (connected == 1)
            {

                if (i < nextpage && i >= prevpage)
                {


                    j = (i - k) % filesPerPage;

                    Button but = (Button)Instantiate(loadButton, Vector3.zero, Quaternion.identity);
                    loadButtons.Add(but);
                    but.transform.SetParent(loadMapsPanel.gameObject.transform);
                    but.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0f, 0f, 0f);
                    but.GetComponent<RectTransform>().anchoredPosition = new Vector2(200, 50 * (rows - j));
                    but.GetComponent<RectTransform>().sizeDelta = new Vector2(1000, 200);
                    but.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1) / 4;

                    but.GetComponentInChildren<Text>().text = dirFiles[i];
                    string fileName = dirFiles[i];

                    but.onClick.AddListener(delegate
                    {
                        cameraAudioSource.PlayOneShot(shhhh, volume);
                        selectFileName(but);
                    });
                }
            }
            else
            {
                numNotConnected += 1;
                if (i < nextpage && i >= prevpage)
                {
                    nextpage += 1;
                    k += 1;
                }

                else if (i <= filesPerPage * (currentPage - 1))
                {
                    prevpage += 1;
                    nextpage += 1;
                    k += 1;
                }
            }
            maxPages = (int)Mathf.Ceil(((float)dirFiles.Length - numNotConnected) / (float)filesPerPage);

        }

        if (currentPage == maxPages)
        {
            nextBut.gameObject.SetActive(false);
        }
        if (currentPage == 1)
        {
            prevBut.gameObject.SetActive(false);

        }


    }

    private void selectFileName(Button but)
    {
        cam.gameObject.SetActive(true);

        if (!currentButtonsSelected.Contains(but))
        {

            foreach (Button button in loadButtons)
            {
                ColorBlock cb = button.colors;
                cb.normalColor = new Color(1, 1, 1, 0);
                button.colors = cb;

            }
            currentButSelected = but;
            currentFileSelected = but.GetComponentInChildren<Text>().text;

            currentButtonsSelected = new List<Button>();
            currentFilesSelected = new List<string>();

            currentButtonsSelected.Add(but);
            currentFilesSelected.Add(but.GetComponentInChildren<Text>().text);

            loadMapFromFile(currentFileSelected, true);

            ColorBlock cblock = but.colors;
            cblock.normalColor = new Color(0.5f, 0.5f, 0.5f, 1f);
            but.colors = cblock;

        }
    }

    //load from file and displays a simple minimap version in the loadscreen.
    private void loadMapFromFile(string fileName, bool confirming = false)
    {
        if (fileName != null)
        {
            if (confirming)
            {
                string line;

                List<int> datas = new List<int>();

                StreamReader file = new StreamReader(currentPath + fileName + ".txt");

                file.ReadLine();
                int amountOfStartstemp = int.Parse(file.ReadLine());
                int amountOfEndstemp = int.Parse(file.ReadLine());
                connected = int.Parse(file.ReadLine());
                length = int.Parse(file.ReadLine());
                width = int.Parse(file.ReadLine());
                Reset();

                amountOfStarts = amountOfStartstemp;
                amountOfEnds = amountOfEndstemp;
                int num1;
                while ((line = file.ReadLine()) != null)
                {
                    bool isInt = int.TryParse(line, out num1);
                    if (isInt)
                    {
                        datas.Add(int.Parse(line));
                    }
                }
                file.Close();

                resourceManager.length = length;
                resourceManager.width = width;

                ResourceManager.mostNorth = 0;
                ResourceManager.mostEast = 0;
                ResourceManager.mostSouth = 1000;
                ResourceManager.mostWest = 1000;
                //add positions


                for (int i = 0; i < datas.Count; i += 2)
                {
                    if (!positions.Contains(new Vector2(datas[i], datas[i + 1])))
                    {
                        positions.Add(new Vector2(datas[i], datas[i + 1]));
                        ResourceManager.mostNorth = Mathf.Max(ResourceManager.mostNorth, (int)datas[i + 1]);
                        ResourceManager.mostEast = Mathf.Max(ResourceManager.mostEast, (int)datas[i]);
                        ResourceManager.mostSouth = Mathf.Min(ResourceManager.mostSouth, (int)datas[i + 1]);
                        ResourceManager.mostWest = Mathf.Min(ResourceManager.mostWest, (int)datas[i]);
                    }
                }
                //
                //Generate start point, end point, and all others, set start point to connected and run
                if (amountOfStarts != 0)
                {
                    startPos = new Vector3(datas[0], 0, datas[1]);
                    resourceManager.startPos = new Vector2(startPos.x, startPos.z);
                }
                if (amountOfEnds != 0)
                {
                    endPos = new Vector3(datas[datas.Count - 2], 0, datas[datas.Count - 1]);
                    resourceManager.endPos = new Vector2(endPos.x, endPos.z);
                }

                foreach (GameObject floor1 in floors)
                {
                    if (floor1.transform.position / planewidth == endPos && amountOfEnds != 0)
                    {
                        endPlane = floor1;
                        floor1.renderer.material.color = Cend;
                        posConnected.Add(floor1.transform.position / planewidth);

                    }
                    else if (floor1.transform.position / planewidth == startPos && amountOfStarts != 0)
                    {
                        startPlane = floor1;
                        floor1.renderer.material.color = Cstart;
                        posConnected.Add(floor1.transform.position / planewidth);
                    }


                    else if (positions.Contains(new Vector2(floor1.transform.position.x, floor1.transform.position.z) / planewidth))
                    {
                        floor1.renderer.material.color = CNotConnected;
                    }
                }
                Vector3 panelPos = cam.WorldToScreenPoint(loadMapsPanel.transform.position) / 2;
                //ChangeTypes camera position and size to fit in load screen
                cam.transform.position = new Vector3(length - 1, 1, width - 1) * resourceManager.planewidth / 2;
                cam.orthographicSize = Mathf.Max(length, width + 1) * resourceManager.planewidth / 2;
                float mincaminfo = loadMapsPanel.GetComponent<RectTransform>().rect.height / 1080f;
                cam.rect = new Rect(0.4f, 0.3f, .4f, .4f);
                Recalculate();
                file.Close();

            }
            else
            {
                StreamReader file = new StreamReader(currentPath + fileName + ".txt");

                file.ReadLine();
                file.ReadLine();
                file.ReadLine();
                connected = int.Parse(file.ReadLine());
            }
        }


    }

    //reset the floor. Clearing everything. Then reconstruct it with width and length
    private void Reset()
    {
        foreach (GameObject floor1 in floors)
        {
            Destroy(floor1);
        }

        posConnected = new List<Vector3>();
        allPos = new List<GameObject>();
        floors = new List<GameObject>();
        positions = new ArrayList();
        amountOfEnds = 0;
        amountOfStarts = 0;

        if (width != 0 || length != 0)
        {
            for (int w = 0; w < width; w++)
            { //order important!
                for (int l = 0; l < length; l++)
                {
                    GameObject floor = (GameObject)Instantiate(editorPlane, new Vector3(l * planewidth, 0, w * planewidth), Quaternion.identity);
                    floor.layer = 15;
                    floor.transform.localScale *= planewidth / 10;
                    floor.renderer.material.color = CnoPlane;
                    floor.transform.parent = transform;
                    floors.Add(floor);
                    allPos.Add(floor);
                }
            }
        }
        else
        {
            for (int w = 0; w < 10; w++)
            { //order important!
                for (int l = 0; l < 10; l++)
                {
                    GameObject floor = (GameObject)Instantiate(editorPlane, new Vector3(l * planewidth, 0, w * planewidth), Quaternion.identity);
                    floor.layer = 15;
                    floor.transform.localScale *= planewidth / 10;
                    floor.renderer.material.color = CnoPlane;
                    floor.transform.parent = transform;
                    floors.Add(floor);
                    allPos.Add(floor);
                }
            }
        }

        float tempL = length - 1;
        float tempW = width - 1;
        cam.transform.position = new Vector3(tempL / 2, 1, tempW / 2) * planewidth;
        cam.orthographicSize = Mathf.Max(length, width + 1) * planewidth / 2;
        cam.rect = new Rect(0.25f, 0.2f, 0.5f, 0.6f);
    }

    // Method that recalculates the whole path to the start is connected to
    public static void Recalculate()
    {
        // Creating a new list
        posConnected = new List<Vector3>();

        // make each plane that is green, red
        foreach (GameObject plane in allPos)
        {
            if (plane.renderer.material.color == instance.CConnected && plane != startPlane && plane != endPlane)
                plane.renderer.material.color = instance.CNotConnected;
        }

        // if the startplane exists calculate a new route that is connected to the start
        if (startPlane != null)
        {
            convertAround(startPlane);
        }
    }

    // Method that checks all positions around a connected plane and keeps on going checking planes that are connected to it. It turns the path green if it is connected to the start. It also adds all the green planes position to the posConnected list.
    public static void convertAround(GameObject plane)
    {
        // Getting the position of the plane
        Vector2 planePos = new Vector2(plane.transform.position.x, plane.transform.position.z) / instance.planewidth;

        // Getting the index of the plane
        int index = allPos.IndexOf(plane);

        // Creating temporary gameobject that is the plane to be checked
        GameObject checkPlane;

        // looping 4 times to get all planes in all directions
        for (int i = 0; i < 4; i++)
        {
            // Checking all neighbouring planes
            if (i == 0 && plane.transform.position.z / instance.planewidth > 0 && (index - instance.length) >= 0)
                checkPlane = allPos[index - instance.length];

            else if (i == 1 && (plane.transform.position.z / instance.planewidth) < (instance.width - 1) && (index + instance.length) < allPos.Count)
                checkPlane = allPos[index + instance.length];

            else if (i == 2 && (plane.transform.position.x / instance.planewidth) < (instance.length - 1) && (index + 1) < allPos.Count)
                checkPlane = allPos[index + 1];

            else if (i == 3 && plane.transform.position.x > 0 && (index - 1) >= 0)
                checkPlane = allPos[index - 1];

            else
                checkPlane = null;

            // If the plane exists
            if (checkPlane != null)
            {

                // if the planes has a start end or path color and it has not been added to the posConnected list
                if ((checkPlane.renderer.material.color == instance.CNotConnected || checkPlane.renderer.material.color == instance.Cstart || checkPlane.renderer.material.color == instance.Cend) && !posConnected.Contains(checkPlane.transform.position / instance.planewidth))
                {
                    // if the plane is a path plane
                    if (checkPlane.renderer.material.color == instance.CNotConnected)
                    {
                        // Turn the plane green
                        checkPlane.renderer.material.color = instance.CConnected;
                    }

                    // add its location to the posConnected list
                    posConnected.Add(checkPlane.transform.position / instance.planewidth);

                    // start this same proces for this found connected plane to see whether it has path connections
                    convertAround(checkPlane);
                }
            }
        }
    }


    public void GenerateLevel()
    {
        if (currentFileSelected != null && currentFilesSelected.Count == 1)
        {
            for (int i = 0; i < positions.Count; i++)
            { //get right sizes of the positions array
                positions[i] = (Vector2)positions[i];
                Vector2 tempPos = (Vector2)positions[i];
                ResourceManager.mostNorth = Mathf.Max(ResourceManager.mostNorth, (int)tempPos.y);
                ResourceManager.mostEast = Mathf.Max(ResourceManager.mostEast, (int)tempPos.x);
                ResourceManager.mostSouth = Mathf.Min(ResourceManager.mostSouth, (int)tempPos.y);
                ResourceManager.mostWest = Mathf.Min(ResourceManager.mostWest, (int)tempPos.x);
            }
            startPos = new Vector2(startPos.x, startPos.z);
            endPos = new Vector2(endPos.x, endPos.z);
            resourceManager.startPos = startPos;
            resourceManager.endPos = endPos;
            loadingScreen.SetActive(true);
            cameraAudioSource.PlayOneShot(startGame, 5 * volume);
            Invoke("startSpawn", 1.802f);
            // spawnLevel();
            resourceManager.Nodes = Nodes;
        }
    }
    void startSpawn()
    {
        instance.StartCoroutine(instance.spawnLevel());
    }

    // Method generating the map that has just been built and displaying the loading screen with some text
    IEnumerator spawnLevel()
    {
        loadingScreen.GetComponentInChildren<Text>().text = "Loading: Destroying what you just built";
        yield return new WaitForSeconds(0.1f);
        loadingScreen.GetComponentInChildren<Text>().text = "Loading: Rebuilding the floors you just built";
        yield return new WaitForSeconds(0.1f);
        GenerateFloor();
        loadingScreen.GetComponentInChildren<Text>().text = "Loading: You build the floors, we place the walls!...";
        yield return new WaitForSeconds(0.1f);
        RandomMaze.GenerateWall(positions, planewidth, resourceManager.wallPrefab, resourceManager.torch, resourceManager.height, length - 1, width - 1, GameObject.Find("World"), endPos, startPos);
        loadingScreen.GetComponentInChildren<Text>().text = "Loading: Dwogres wanted a red carpet to walk on, generating...";
        yield return new WaitForSeconds(0.1f);
        Nodes = RandomMaze.SpawnNodes(positions, resourceManager.nodeSize, planewidth, Nodes, length, width, resourceManager.drawNavigationGrid, true, endPos, startPos);
        loadingScreen.GetComponentInChildren<Text>().text = "Loading: Giving birth to Player...";
        yield return new WaitForSeconds(0.1f);
        Destroy(backGroundCamera.GetComponent<AudioListener>());
        RandomMaze.spawnPlayer(resourceManager.player, resourceManager.mainCamera, resourceManager.Goal, resourceManager.enemySpawner, resourceManager.GUI, resourceManager.eventListener, resourceManager.startPos, resourceManager.endPos, resourceManager.Minimapcamera, resourceManager.width, resourceManager.length, resourceManager.planewidth);
        loadingScreen.GetComponentInChildren<Text>().text = "Loading: Lighting torches...";

        resourceManager.Nodes = Nodes;
        disableLevelEditor();
        Time.timeScale = 0;
        ready = true;
        loadingScreen.GetComponentInChildren<Text>().gameObject.SetActive(false);
        spacebar.SetActive(true);


    }
    void GenerateFloor()
    {
        foreach (Vector2 posi in positions)
        {
            GameObject floor = (GameObject)Instantiate(resourceManager.planePrefab, new Vector3(posi.x, 0, posi.y) * planewidth, Quaternion.identity);
            floor.gameObject.transform.localScale = new Vector3(planewidth / 20, 0.1f, planewidth / 20); //Scale the floor
            floor.transform.parent = GameObject.Find("World").transform; //Set the floor to the gameObject.
            floor.name = "Floor"; //name the floor Floor
            GameObject ceil = (GameObject)Instantiate(resourceManager.planePrefab, new Vector3(posi.x, resourceManager.height, posi.y) * planewidth, Quaternion.identity);
            ceil.gameObject.transform.localScale = new Vector3(planewidth / 20, 0.1f, planewidth / 20); //Scale the floor
            ceil.transform.parent = GameObject.Find("World").transform; //Set the floor to the gameObject.
            ceil.transform.Rotate(new Vector3(180, 0, 0));
            ceil.name = "Ceiling"; //name the floor Floor
        }
    }
    //disable the gui, so you can play the game
    private void disableLevelEditor()
    {
        foreach (GameObject floor1 in floors)
        {
            Destroy(floor1);
        }
        cam.gameObject.SetActive(false);
        Destroy(canvas);
        Destroy(GameObject.Find("EditorLight"));

    }
    void Update()
    {
        if (ready && Input.GetKeyDown(KeyCode.Space))
        {

            loadingScreen.SetActive(false);
            Time.timeScale = 1;

            Invoke("deleteScript", 0.01f);
        }
    }

    void deleteScript()
    {
        PlayerController.started = true;
        Destroy(this.GetComponent<LoadMapLayouts>());
    }
}