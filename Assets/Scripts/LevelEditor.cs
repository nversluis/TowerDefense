using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class LevelEditor : MonoBehaviour
{
	private float planewidth;
	//Size of the planes
	private float height;
	private float nodeSize;
	//Distance between nodepoints

	private GameObject editorPlane;
	private GameObject planePrefab;
	//Floor prefab
	private GameObject wallPrefab;
	//Wall prefab
	private GameObject enemySpawner;
	private GameObject Minimapcamera;
	private GameObject Gate;
	private GameObject torch;
	public Camera cam;
    public GameObject buildingBlocksPanel;
    public GameObject[] buttons;
    static LevelEditor instance;
    int connected;

	public static ArrayList positions = new ArrayList ();
	//Positions of the floors
	private List<Vector3> NodesPos = new List<Vector3> ();
	//Positions of the waypoints/nodes
	private List<WayPoint> Nodes = new List<WayPoint> ();
	//List with all Nodes
	private int length;
	private int width;
	private List<GameObject> floors = new List<GameObject> ();
	private Vector2 startPos;
	private Vector2 endPos;
	private bool playing;
	private int currentPage;
	private int maxPages;
	private Button currentButSelected;
    private List<Button> currentButtonsSelected;
	private string currentFileSelected;
    private List<string> currentFilesSelected;
	//colors
	private Color CnoPlane;
	private Color Cstart;
    private Color Cend;
	private Color CConnected;
	private Color CNotConnected;
	private Color CHighlighted;
	public Button prevBut;
	public Button nextBut;
	private bool drawNavGrid;
    string tempfilename;
    Camera miniCamera;
    public Camera backGroundCamera;
    private List<Button> loadButtons;

	public GameObject newMapScreen;
	public InputField lengthInput;
	public InputField widthInput;
    public Button newMapButton;
	public Button SubmitButton;
	public Button cancelSizeBut;
	public Button newSizeBut;
    public GameObject largeMapText;
    public static bool mapSaved;

    public GameObject areYouSurePanel;
	public Button generateLevelButton;
	public InputField fileNameInput;
	public Button saveLayout;
	public Button loadLayout;
	public Canvas canvas;
	public Text ErrorText;
	public GameObject loadMapsPanel;
	public Button loadButton;
	public Button cancelButton;
	public Button submitLoadButton;
	public Button deleteButton;
    public GameObject saveMapPanel;
    public GameObject mainMenuButton;
    AudioSource cameraAudioSource;
    bool mainMenu;

	public GameObject LoadingScreen;
    public GameObject loadPanel;

	private GameObject player;
	private GameObject camera;
	private bool drawNavigationGrid;
	private string AppPath;

    GameObject startCol;
    GameObject endCol;
    GameObject conCol;


	public static int type;
	public static int amountOfEnds;
	public static int amountOfStarts;
	public static Vector3 endPos3;
	public static Vector3 startPos3;
	public static List<Vector3> posConnected;
	public static List<GameObject> allPos;
	public static GameObject startPlane;
	public static GameObject endPlane;
	public static bool editing;

    public AudioClip click;
    public AudioClip startGame;
    public AudioClip shhhh;

    float volume;


	// Use this for initialization
	private GameObject ResourceManagerObj;
	private ResourceManager resourceManager;

    bool ready;

    public GameObject spacebar;

    AudioClip editor;

	void Start ()
	{
        volume = (float)PlayerPrefs.GetInt("SFX") / 100f;
        playing = false;
		ResourceManagerObj = GameObject.Find ("ResourceManager");
		resourceManager = ResourceManagerObj.GetComponent<ResourceManager> ();
		torch = resourceManager.torch;
		planewidth = resourceManager.planewidth;
		height = resourceManager.height;
		nodeSize = resourceManager.nodeSize;
		editorPlane = resourceManager.editorPlane;
		planePrefab = resourceManager.planePrefab;
		wallPrefab = resourceManager.wallPrefab;
		enemySpawner = resourceManager.enemySpawner;
		Minimapcamera = resourceManager.Minimapcamera;
		Gate = resourceManager.Gate;
		torch = resourceManager.torch;
		drawNavGrid = resourceManager.drawNavigationGrid;
		posConnected = new List<Vector3> ();
		allPos = new List<GameObject> ();
		currentPage = 1;
		CnoPlane = resourceManager.noPlane;
		Cstart = resourceManager.start;
        Cend = resourceManager.end;
		CConnected = resourceManager.connected;
		CNotConnected = resourceManager.notConnected;
		CHighlighted = resourceManager.highlighted;
        AppPath = Application.dataPath + "/CustomMaps/";
		type = 0;
		drawNavigationGrid = resourceManager.drawNavigationGrid;
		player = resourceManager.player;
		camera = resourceManager.mainCamera;
        instance = this;
        miniCamera = GameObject.Find("MiniCam").GetComponent<Camera>();
        amountOfEnds = 0;
        amountOfStarts = 0;
        mainMenu = true;
        editing = false;
        mapSaved = true;
        currentButtonsSelected = new List<Button>();
        currentFilesSelected = new List<string>();
        cameraAudioSource = backGroundCamera.GetComponent<AudioSource>();

        float musicVolume = (float)PlayerPrefs.GetInt("BGM") / 100f;
        editor = resourceManager.editorMusic;
        GameObject.Find("backingMusic").GetComponent<AudioSource>().clip = editor;
        GameObject.Find("backingMusic").GetComponent<AudioSource>().volume = musicVolume;
        GameObject.Find("backingMusic").GetComponent<AudioSource>().Play();

		if (!File.Exists (AppPath)) {
			Directory.CreateDirectory (AppPath);
		}
	}


    public void ButtonClick(){
        cameraAudioSource.PlayOneShot(click,volume);
    }
    // Method for the new map button
    public void NewMapButton()
    {
        // Turning on new map panel and setting the minicamera depth deeper so it is behind the menu
        miniCamera.depth = -10;
        editing = false;

        if (!mainMenu && !mapSaved)
        {
            areYouSurePanel.SetActive(true);
            editing = false;

            foreach (GameObject button in buttons)
            {
                button.SetActive(false);
            }
            newMapButton.gameObject.SetActive(false);
            mainMenuButton.SetActive(false);
            buildingBlocksPanel.SetActive(false);

            GameObject.Find("YesButton").GetComponent<Button>().onClick.AddListener(delegate
            {
                GameObject.Find("YesButton").GetComponent<Button>().onClick.RemoveAllListeners();
                GameObject.Find("NoButton").GetComponent<Button>().onClick.RemoveAllListeners();
                newMapScreen.SetActive(true);
                areYouSurePanel.SetActive(false);
                editing = false;

                
            });

            GameObject.Find("NoButton").GetComponent<Button>().onClick.AddListener(delegate
            {
                GameObject.Find("YesButton").GetComponent<Button>().onClick.RemoveAllListeners();
                GameObject.Find("NoButton").GetComponent<Button>().onClick.RemoveAllListeners();
                areYouSurePanel.SetActive(false);
                foreach (GameObject button in buttons)
                {
                    button.SetActive(true);
                }
                newMapButton.gameObject.SetActive(true);
                mainMenuButton.SetActive(true);
                buildingBlocksPanel.SetActive(true);
                miniCamera.depth = 1;
                editing = true;


            });

        }
        else
        {
            newMapScreen.SetActive(true);
            editing = false;
        }

    }

    // Method for the cancel Map button
    public void CancelSizeButton()
    {

        if (mainMenu)
        {
            // Deactivating new map panel and setting the minicamera depth higher so it is in front of the menu
            newMapScreen.SetActive(false);
            miniCamera.depth = 1;
            editing = false;
        }
        else
        {
            // Deactivating new map panel and setting the minicamera depth higher so it is in front of the menu
            newMapScreen.SetActive(false);
            miniCamera.depth = 1;
            editing = true;

            buildingBlocksPanel.SetActive(true);

            mainMenuButton.SetActive(true);
            newMapButton.gameObject.SetActive(true);
            // Setting all necessary buttons active
            foreach (GameObject button in buttons)
            {
                button.SetActive(true);
            }

            mainMenu = false;
            editing = true;
        }
    }

    // Method for the sub new map button
    public void SubmitNewMapButton()
    {
        // Set minicamera depth to 1 so the editorplane is in front of the gui
        miniCamera.depth = 1;

        // Submitting the size that was entered
        submitSize();

        // Deactivating the new map panel and activating the buildingblockpanel
        newMapScreen.SetActive(false);
        buildingBlocksPanel.SetActive(true);

        // Determining the preview colors for the building block panel
        startCol = GameObject.Find("StartCol");
        startCol.transform.GetChild(1).GetComponent<Image>().color = Cstart;
        endCol = GameObject.Find("EndCol");
        endCol.transform.GetChild(1).GetComponent<Image>().color = Cend;
        conCol = GameObject.Find("ConCol");
        conCol.transform.GetChild(1).GetComponent<Image>().color = CConnected;

        mainMenuButton.SetActive(true);
        newMapButton.gameObject.SetActive(true);
        // Setting all necessary buttons active
        foreach (GameObject button in buttons)
        {
            button.SetActive(true);
        }

        mainMenu = false;
        editing = true;
    }

    // Method for the Start location button
    public void StartLocationButton()
    {
        // set the type to 0 and setting color to the clicked button blueish
        type = 0;
        startCol.transform.GetChild(0).GetComponent<Text>().color = new Color(100f / 255f, 141f / 255f, 255f / 255f);
        endCol.transform.GetChild(0).GetComponent<Text>().color = Color.white;
        conCol.transform.GetChild(0).GetComponent<Text>().color = Color.white;
    }

    // Method for the path button
    public void PathButton()
    {
        // set the type to 2 and setting color to the clicked button blueish
        type = 2;
        startCol.transform.GetChild(0).GetComponent<Text>().color = Color.white;
        endCol.transform.GetChild(0).GetComponent<Text>().color = Color.white;
        conCol.transform.GetChild(0).GetComponent<Text>().color = new Color(100f / 255f, 141f / 255f, 255f / 255f);
    }

    // Method for the end location button
    public void EndLocationButton()
    {
        // set the type to 1 and setting color to the clicked button blueish
        type = 1;
        startCol.transform.GetChild(0).GetComponent<Text>().color = Color.white;
        endCol.transform.GetChild(0).GetComponent<Text>().color = new Color(100f / 255f, 141f / 255f, 255f / 255f);
        conCol.transform.GetChild(0).GetComponent<Text>().color = Color.white;
    }

    // Method for generating button
    public void GenerateLevelButton()
    {
        // generating level
        GenerateLevel();
    }

    // Method save layout button
    public void SaveLayoutButton()
    {
        saveMapPanel.SetActive(true);
        miniCamera.depth = -10;
        editing = false;

        foreach (GameObject button in buttons)
        {
            button.SetActive(false);
        }
        newMapButton.gameObject.SetActive(false);
        mainMenuButton.SetActive(false);
        buildingBlocksPanel.SetActive(false);
    }

    // Method for cancel save button
    public void CancelSavebutton()
    {
        saveMapPanel.SetActive(false);
        miniCamera.depth = 1;
        editing = true;

        foreach (GameObject button in buttons)
        {
            button.SetActive(true);
        }
        newMapButton.gameObject.SetActive(true);
        mainMenuButton.SetActive(true);
        buildingBlocksPanel.SetActive(true);
    
    }

    // Method for submit save button
    public void SubmitSaveButton()
    {
        SavePositionsToFile(fileNameInput.text);
        saveMapPanel.SetActive(false);
        editing = true;
        miniCamera.depth = 1;

        setErrorTekst("Saved map with name: " + fileNameInput.text, false);

        foreach (GameObject button in buttons)
        {
            button.SetActive(true);
        }
        newMapButton.gameObject.SetActive(true);
        mainMenuButton.SetActive(true);
        buildingBlocksPanel.SetActive(true);
        mapSaved = true;
    }

    // Method loads layout when load layout button is clicked
    public void LoadLayoutButton()
    {


        if (!mainMenu && !mapSaved)
        {
            // initial filename
            tempfilename = "temp";

            // keep looping until a file name that has not been used is found
            while (true)
            {
                // if filename is free
                if (!File.Exists(AppPath + tempfilename + ".txt"))
                {
                    break;
                }
                // else keep on adding random numbers behind temp
                else
                    tempfilename = tempfilename + Random.Range(0, 9);
            }


            // save the position to this temporary file
            SavePositionsToFile(tempfilename, true);

            miniCamera.depth = -10;

            areYouSurePanel.SetActive(true);
            editing = false;
            foreach (GameObject button in buttons)
            {
                button.SetActive(false);
            }
            newMapButton.gameObject.SetActive(false);
            mainMenuButton.SetActive(false);
            buildingBlocksPanel.SetActive(false);

            GameObject.Find("YesButton").GetComponent<Button>().onClick.AddListener(delegate
            {
                GameObject.Find("YesButton").GetComponent<Button>().onClick.RemoveAllListeners();
                GameObject.Find("NoButton").GetComponent<Button>().onClick.RemoveAllListeners();
                miniCamera.depth = 1;
                areYouSurePanel.SetActive(false);
                miniCamera.gameObject.SetActive(false);

                Reset();
                // show the saved maps
                ShowSavedMaps();

                editing = false;

                buildingBlocksPanel.SetActive(false);

                foreach (GameObject button in buttons)
                {
                    button.SetActive(false);
                }

                newMapButton.gameObject.SetActive(false);
                mainMenuButton.SetActive(false);

            });

            GameObject.Find("NoButton").GetComponent<Button>().onClick.AddListener(delegate
            {
                GameObject.Find("YesButton").GetComponent<Button>().onClick.RemoveAllListeners();
                GameObject.Find("NoButton").GetComponent<Button>().onClick.RemoveAllListeners();

                areYouSurePanel.SetActive(false);
                foreach (GameObject button in buttons)
                {
                    button.SetActive(true);
                }
                newMapButton.gameObject.SetActive(true);
                mainMenuButton.SetActive(true);
                buildingBlocksPanel.SetActive(true);
                miniCamera.depth = 1;
                editing = true;
                deleteFile(tempfilename,true);
            });
            

        }
        else if(!mainMenu)
        {
            // initial filename
            tempfilename = "temp";

            // keep looping until a file name that has not been used is found
            while (true)
            {
                // if filename is free
                if (!File.Exists(AppPath + tempfilename + ".txt"))
                {
                    break;
                }
                // else keep on adding random numbers behind temp
                else
                    tempfilename = tempfilename + Random.Range(0, 9);
            }
            SavePositionsToFile(tempfilename);

            currentFileSelected = tempfilename;
            miniCamera.gameObject.SetActive(false);

            Reset();
            // show the saved maps
            ShowSavedMaps();

            editing = false;

            buildingBlocksPanel.SetActive(false);

            foreach (GameObject button in buttons)
            {
                button.SetActive(false);
            }

            newMapButton.gameObject.SetActive(false);
            mainMenuButton.SetActive(false);
        }
        else
        {
            miniCamera.gameObject.SetActive(false);

            Reset();
            // show the saved maps
            ShowSavedMaps();

            editing = false;

            buildingBlocksPanel.SetActive(false);

            foreach (GameObject button in buttons)
            {
                button.SetActive(false);
            }

            newMapButton.gameObject.SetActive(false);
            mainMenuButton.SetActive(false);
        }

        
    }

    // Method for the accepting the current selected map to load
    public void SubmitLoadButton()
    {



        if (currentFilesSelected.Count == 1)
        {
            miniCamera.gameObject.SetActive(true);
            buildingBlocksPanel.SetActive(true);
            currentPage = 1;
            

            if (!mainMenu)
            {
                // Deleting the temporary file that was created for keeping the old map that was just created when the load map button was pressed
                deleteFile(tempfilename);

            }

            if (mainMenu)
            {
                // Determining the preview colors for the building block panel
                startCol = GameObject.Find("StartCol");
                startCol.transform.GetChild(1).GetComponent<Image>().color = Cstart;
                endCol = GameObject.Find("EndCol");
                endCol.transform.GetChild(1).GetComponent<Image>().color = Cend;
                conCol = GameObject.Find("ConCol");
                conCol.transform.GetChild(1).GetComponent<Image>().color = CConnected;

                mainMenu = false;

            }
            generateEditorMap(currentFileSelected);
            foreach (GameObject button in buttons)
            {
                button.SetActive(true);
            }
            editing = true;

            setErrorTekst("Loaded new map!", false);

            newMapButton.gameObject.SetActive(true);
            mainMenuButton.SetActive(true);
            miniCamera.depth = 1;
            mapSaved = true;
        }
        else if (currentFilesSelected.Count>1)
        {
            setErrorTekst("Select only one map to load!");
        }
        else
        {
            setErrorTekst("No map selected!");
        }


    }

    // Method for deleting current selected map
    public void DeleteMapButton()
    {
        if (currentFileSelected != null)
        {
            deleteFile(currentFileSelected,true);

            ShowSavedMaps();
        }
        else if (currentFileSelected == null & currentFilesSelected.Count>0)
        {
            foreach (Button button in currentButtonsSelected)
            {
                deleteFile(button.GetComponentInChildren<Text>().text, true);

            }
            currentButtonsSelected = new List<Button>();
            currentFilesSelected = new List<string>();
            ShowSavedMaps();
        }
        else
        {
            setErrorTekst("No map selected!");
        }
    }

    // Method that cancels loading a map and going back to current map (if one was created)
    public void CancelButton()
    {
        miniCamera.gameObject.SetActive(true);
        currentPage = 1;

        if (!mainMenu)
        {
            Reset();
            loadMapFromFile(tempfilename);
            generateEditorMap(tempfilename, true);
            deleteFile(tempfilename, true);

            editing = true;

            buildingBlocksPanel.SetActive(true);

            foreach (GameObject button in buttons)
            {
                button.SetActive(true);
            }
        }
        if (mainMenu)
        {
            cancelLoadScreen();
            buttons[2].gameObject.SetActive(true);
        }

        newMapButton.gameObject.SetActive(true);
        mainMenuButton.SetActive(true);
        miniCamera.depth = 1;


    }

    // Navigating through maps one page further
    public void NextButton()
    {
        if (currentPage < maxPages)
        {
            currentPage++;
            ShowSavedMaps();
            cam.gameObject.SetActive(false);

        }
    }

    public void MainMenuButton()
    {
        if (!mainMenu && !mapSaved)
        {
            areYouSurePanel.SetActive(true);
            editing = false;
            miniCamera.depth = -10;

            foreach (GameObject button in buttons)
            {
                button.SetActive(false);
            }
            newMapButton.gameObject.SetActive(false);
            mainMenuButton.SetActive(false);
            buildingBlocksPanel.SetActive(false);

            GameObject.Find("YesButton").GetComponent<Button>().onClick.AddListener(delegate
            {
                Application.LoadLevel(0);

            });

            GameObject.Find("NoButton").GetComponent<Button>().onClick.AddListener(delegate
            {
                areYouSurePanel.SetActive(false);
                foreach (GameObject button in buttons)
                {
                    button.SetActive(true);
                }
                newMapButton.gameObject.SetActive(true);
                mainMenuButton.SetActive(true);
                buildingBlocksPanel.SetActive(true);
                miniCamera.depth = 1;
                editing = true;
            });

        }
        else
        {
            Application.LoadLevel("Main menu");

        }
    }

    // Navigating through maps one page back
    public void PreviousButton()
    {
        if (currentPage > 1)
        {
            currentPage--;
            ShowSavedMaps();
            cam.gameObject.SetActive(false);

        }
    }

    // Method that checks wheter the button text in the new map panel should be confirm or not valid
    void NewMapScreenButtonCheck()
    {
        // If new map screen is activated and not null
        if (newMapScreen != null && newMapScreen.activeSelf)
        {
            // creating new temporary variables
            int lengthTemp;
            int widthTemp;

            // trying to parse them as int
            bool resultLength = int.TryParse(lengthInput.text, out lengthTemp);
            bool resultWidth = int.TryParse(widthInput.text, out widthTemp);

            if (resultWidth && resultLength && ((int.Parse(lengthInput.text) * int.Parse(widthInput.text)) > 625 || (int.Parse(lengthInput.text) * int.Parse(widthInput.text)) < 49))
            {
                largeMapText.SetActive(true);
            }
            else{
                largeMapText.SetActive(false);
            }

            // if succeeded and the number is more than 0
            if (resultWidth && resultLength && int.Parse(lengthInput.text) > 0 && int.Parse(widthInput.text) > 0)
            {
                // Text in the button is confirm and the button is enabled
                SubmitButton.GetComponentInChildren<Text>().text = "Confirm";
                SubmitButton.enabled = true;
            }
            else
            {
                // else the button is not valid and disabled
                SubmitButton.GetComponentInChildren<Text>().text = "Not Valid";
                SubmitButton.enabled = false;
            }
        }
    }

    // Method that is called in the SubmitNewButton Method and submits the sizes to the resourcemanager
    private void submitSize ()
	{
        // Parsing strings to integers
 		length = int.Parse (lengthInput.text);
        width = int.Parse(widthInput.text);

        // Sending to resourcemanager script
		resourceManager.length = length;
		resourceManager.width = width;

        // Resetting plane with the right size
        Reset();

    }

    // Add a position to the positions list 
    public static void addPos(Vector2 pos)
    {
        // If position is not added yet, add it
        if (!positions.Contains(pos))
        {
            positions.Add(pos);
        }

    }

    // Remove a position from the positions list
    public static void removePos(Vector2 pos)
    {
        positions.Remove(pos);

    }

    // Method that checks and returns a bool if a plane is connected to a plane that has connection to the start and if so checks all planes that are connected to this route
    public static bool checkConnected(GameObject plane)
    {
        // Getting the normalized location
        Vector3 posNormalized = plane.transform.position / instance.planewidth;

        // Checking whether one of the neighbour planes has a connection to the start
        bool res = (posConnected.Contains(posNormalized + new Vector3(1, 0, 0)) || posConnected.Contains(posNormalized - new Vector3(1, 0, 0)) || posConnected.Contains(posNormalized + new Vector3(0, 0, 1)) || posConnected.Contains(posNormalized - new Vector3(0, 0, 1)));

        // if so, check all the planes that are connected to this path with the convertAround method
        if (res)
        {
            convertAround(plane);
        }

        // return if it has connection to the start
        return res;
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
        if (endPlane!=null && posConnected.Contains(endPlane.transform.position / instance.planewidth))
        {
            instance.connected = 1;
        }
        else
        {
            instance.connected = 0;
        }
    }
    

    void Update()
    {
        NewMapScreenButtonCheck();

        if (ready && Input.GetKeyDown(KeyCode.Space))
        {

            LoadingScreen.SetActive(false);
            Time.timeScale = 1;

            Invoke("deleteScript", 0.01f);
        }
    }

    void deleteScript()
    {
        PlayerController.started = true;
        Destroy(this.GetComponent<LevelEditor>());
    }

    // Method generating the map that has just been built and displaying the loading screen with some text
	IEnumerator spawnLevel ()
	{
		LoadingScreen.GetComponentInChildren<Text> ().text = "Loading: Destroying what you just built";
		yield return new WaitForSeconds (0.1f);
		LoadingScreen.GetComponentInChildren<Text> ().text = "Loading: Rebuilding the floors you just built";
		yield return new WaitForSeconds (0.1f);
		GenerateFloor ();
		LoadingScreen.GetComponentInChildren<Text> ().text = "Loading: You build the floors, we place the walls!...";
		yield return new WaitForSeconds (0.1f);
		RandomMaze.GenerateWall(positions, planewidth, wallPrefab, torch, height, length-1, width-1, GameObject.Find("World"), endPos, startPos);
		LoadingScreen.GetComponentInChildren<Text> ().text = "Loading: Dwogres wanted a red carpet to walk on, generating...";
		yield return new WaitForSeconds (0.1f);
		Nodes = RandomMaze.SpawnNodes (positions, nodeSize, planewidth, Nodes, length, width, drawNavigationGrid, true,endPos,startPos);
		LoadingScreen.GetComponentInChildren<Text> ().text = "Loading: Giving birth to Player...";
		yield return new WaitForSeconds (0.1f);
        Destroy(backGroundCamera.GetComponent<AudioListener>());
        RandomMaze.spawnPlayer(player, camera, resourceManager.Goal, enemySpawner, resourceManager.GUI, resourceManager.eventListener, startPos, endPos, Minimapcamera, width, length, planewidth);
		LoadingScreen.GetComponentInChildren<Text> ().text = "Loading: Lighting torches...";
        //yield return new WaitForSeconds (0.1f);
        //RandomMaze.createSingleObjects (planewidth, enemySpawner, endPos, startPos);

		resourceManager.Nodes = Nodes;

        disableLevelEditor();

        Time.timeScale = 0;
        ready = true;
        spacebar.SetActive(true);
        LoadingScreen.GetComponentInChildren<Text>().gameObject.SetActive(false);

	}


	private void GenerateLevel ()
	{
		if (endPlane != null) {

			if (posConnected.Contains (endPlane.transform.position / planewidth)) {
				playing = true;
				for (int i = 0; i < positions.Count; i++) { //get right sizes of the positions array
					positions [i] = (Vector2)positions [i] / planewidth;
					Vector2 tempPos = (Vector2)positions [i];
					ResourceManager.mostNorth = Mathf.Max (ResourceManager.mostNorth, (int)tempPos.y);
					ResourceManager.mostEast = Mathf.Max (ResourceManager.mostEast, (int)tempPos.x);
					ResourceManager.mostSouth = Mathf.Min (ResourceManager.mostSouth, (int)tempPos.y);
					ResourceManager.mostWest = Mathf.Min (ResourceManager.mostWest, (int)tempPos.x);
				}
				startPos = new Vector2 (startPos3.x, startPos3.z);
				endPos = new Vector2 (endPos3.x, endPos3.z);
				resourceManager.startPos = startPos;
				resourceManager.endPos = endPos;
				LoadingScreen.SetActive (true);
                cameraAudioSource.PlayOneShot(startGame,5*volume);
                Invoke("startSpawn", 1.802f);
               // spawnLevel();
                editing = false;
				resourceManager.Nodes = Nodes;
			} else
				setErrorTekst ("Connect end to start!");
		} else
			setErrorTekst ("Place end position!");

	}

    void startSpawn(){
        instance.StartCoroutine (instance.spawnLevel ());
    }




	private void GenerateFloor ()
	{
		foreach (Vector2 posi in positions) {
			GameObject floor = (GameObject)Instantiate (resourceManager.planePrefab, new Vector3 (posi.x, 0, posi.y) * planewidth, Quaternion.identity);
			floor.gameObject.transform.localScale = new Vector3 (planewidth / 20, 0.1f, planewidth / 20); //Scale the floor
			floor.transform.parent = GameObject.Find("World").transform; //Set the floor to the gameObject.
			floor.name = "Floor"; //name the floor Floor
			GameObject ceil = (GameObject)Instantiate (resourceManager.planePrefab, new Vector3 (posi.x, height, posi.y) * planewidth, Quaternion.identity);
			ceil.gameObject.transform.localScale = new Vector3 (planewidth / 20, 0.1f, planewidth / 20); //Scale the floor
            ceil.transform.parent = GameObject.Find("World").transform; //Set the floor to the gameObject.
			ceil.transform.Rotate (new Vector3 (180, 0, 0));
			ceil.name = "Ceiling"; //name the floor Floor
		}
	}

	//disable the gui, so you can play the game
	private void disableLevelEditor ()
	{
		foreach (GameObject floor1 in floors) {
			Destroy (floor1);
		}
		cam.gameObject.SetActive (false);
		Destroy (canvas);
        Destroy(GameObject.Find("EditorLight"));
        
	}

	//reset the floor. Clearing everything. Then reconstruct it with width and length
	private void Reset ()
	{
		foreach (GameObject floor1 in floors) {
			Destroy (floor1);
		}
		posConnected = new List<Vector3> ();
		allPos = new List<GameObject> ();
		floors = new List<GameObject> ();
		positions = new ArrayList ();
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
		cam.transform.position = new Vector3 (tempL / 2, 1, tempW / 2) * planewidth;
		cam.orthographicSize = Mathf.Max (length, width + 1) * planewidth / 2;
		cam.rect = new Rect (0.25f, 0.2f, 0.5f, 0.6f);
	}

	//saves the position to file.
	private void SavePositionsToFile (string filename, bool loading = false)
	{
				string res = "\r\n";

                res += amountOfStarts + "\r\n";
                res += amountOfEnds + "\r\n";

                res += connected + "\r\n";

                res += resourceManager.length + "\r\n" + resourceManager.width + "\r\n";
                if (amountOfStarts != 0)
                {
                    res += startPos3.x.ToString() + "\r\n" + startPos3.z.ToString() + "\r\n";
                }
				foreach (Vector2 pos in positions) {
					if (pos / planewidth != resourceManager.startPos && pos / planewidth != resourceManager.endPos) {
						for (int i = 0; i <= 1; i++) {
							res += (pos [i] / planewidth).ToString () + "\r\n";
						}
					}
				}
                if (amountOfEnds != 0)
                {
                    res += endPos3.x.ToString() + "\r\n" + endPos3.z.ToString() + "\r\n";
                }

				BinaryFormatter bf = new BinaryFormatter ();
				FileStream file = File.Create (AppPath + filename + ".txt");
				bf.Serialize (file, res);
                file.Close();
                Debug.Log("Saved to : " + AppPath);
	}

	//load from file and displays a simple minimap version in the loadscreen.
	private void loadMapFromFile (string fileName)
	{
		if (fileName != null) {

			string line;
			List<int> datas = new List<int> ();
			ArrayList positions = new ArrayList ();
			StreamReader file = new StreamReader (AppPath + fileName + ".txt");
			file.ReadLine ();
            int amountOfStartstemp = int.Parse(file.ReadLine());
            int amountOfEndstemp = int.Parse(file.ReadLine());
            connected = int.Parse(file.ReadLine());
			length = int.Parse (file.ReadLine ());
			width = int.Parse (file.ReadLine ());
            Reset();

            amountOfStarts = amountOfStartstemp;
            amountOfEnds = amountOfEndstemp;
			int num1;
			while ((line = file.ReadLine ()) != null) {
				bool isInt = int.TryParse (line, out num1);
				if (isInt) {
					datas.Add (int.Parse (line));
				}
			}
			file.Close ();

			resourceManager.length = length;
			resourceManager.width = width;

			ResourceManager.mostNorth = 0;
			ResourceManager.mostEast = 0;
			ResourceManager.mostSouth = 1000;
			ResourceManager.mostWest = 1000;
			//add positions
			for (int i = 0; i < datas.Count; i += 2) {
				if (!positions.Contains (new Vector2 (datas [i], datas [i + 1]))) {
					positions.Add (new Vector2 (datas [i], datas [i + 1]));
					ResourceManager.mostNorth = Mathf.Max (ResourceManager.mostNorth, (int)datas [i + 1]);
					ResourceManager.mostEast = Mathf.Max (ResourceManager.mostEast, (int)datas [i]);
					ResourceManager.mostSouth = Mathf.Min (ResourceManager.mostSouth, (int)datas [i + 1]);
					ResourceManager.mostWest = Mathf.Min (ResourceManager.mostWest, (int)datas [i]);
				}
			}
			//
			//Generate start point, end point, and all others, set start point to connected and run
            if (amountOfStarts != 0)
            {
                startPos3 = new Vector3(datas[0], 0, datas[1]);
                resourceManager.startPos = new Vector2(startPos3.x, startPos3.z);
            }
            if (amountOfEnds != 0)
            {
                endPos3 = new Vector3(datas[datas.Count - 2], 0, datas[datas.Count - 1]);
                resourceManager.endPos = new Vector2(endPos3.x, endPos3.z);
            }
			
			foreach (GameObject floor1 in floors) {
				if (floor1.transform.position / planewidth == endPos3 && amountOfEnds !=0) {
					endPlane = floor1;	
					floor1.renderer.material.color = Cend;
					LevelEditor.posConnected.Add (floor1.transform.position / planewidth);

				}
                else if (floor1.transform.position / planewidth == startPos3 && amountOfStarts != 0)
                {
                    LevelEditor.startPlane = floor1;
                    floor1.renderer.material.color = Cstart;
                    LevelEditor.posConnected.Add(floor1.transform.position / planewidth);
                }

                
                else if (positions.Contains (new Vector2 (floor1.transform.position.x, floor1.transform.position.z) / planewidth)) {			
					floor1.renderer.material.color = CNotConnected;
				}
			}
			LevelEditor.positions = positions;
			Vector3 panelPos = cam.WorldToScreenPoint (loadMapsPanel.transform.position) / 2;
			//ChangeTypes camera position and size to fit in load screen
			cam.transform.position = new Vector3 (length - 1, 1, width - 1) * resourceManager.planewidth / 2;
			cam.orthographicSize = Mathf.Max (length, width + 1) * resourceManager.planewidth / 2;
            float mincaminfo = loadMapsPanel.GetComponent<RectTransform>().rect.height / 1080f;
            cam.rect = new Rect(0.4f, 0.3f,.4f, .4f);
            Recalculate();
            file.Close();

		}

	}


	private void generateEditorMap (string filename, bool cancelling = false)
	{
		if (currentFileSelected != null || cancelling) {
			loadMapsPanel.SetActive (false);
			//reconstruct connected
			Recalculate ();

			for (int i = 0; i < positions.Count; i++) { //get right sizes of the positions array so Generate level can work
				positions [i] = (Vector2)positions [i] * planewidth;
			}
			//change camera position and size back
			cam.transform.position = new Vector3 (length - 1, 1, width - 1) * resourceManager.planewidth / 2;
			cam.orthographicSize = Mathf.Max (length, width + 1) * planewidth / 2;
			cam.rect = new Rect (0.25f, 0.2f, 0.5f, 0.6f);
			cam.Render ();
		} else {
			setErrorTekst ("No File Selected");
		}

	}

	// Use this for initialization
	private void ShowSavedMaps ()
	{
        int j = 0;
        bool tempfilefound = false;
		newMapScreen.SetActive (false);
		cam.pixelRect = new Rect (Screen.width / 2, Screen.height / 2 - 100, 200, 200);
		loadMapsPanel.gameObject.SetActive (true);
        loadButtons = new List<Button>();
		foreach (Transform child in loadMapsPanel.transform) {
			if (child.gameObject.name.Contains ("load"))
				Destroy (child.gameObject);
		}
        
        int rows = (int)Mathf.Floor(loadMapsPanel.GetComponent<RectTransform>().rect.height / 50)-1;
		int columns = 1;
		int filesPerPage = rows * columns;

		nextBut.gameObject.SetActive (true);
		prevBut.gameObject.SetActive (true);
		nextBut.GetComponentInChildren<Text> ().text = "Next " + filesPerPage + " Maps";
        prevBut.GetComponentInChildren<Text>().text = "Previous " + filesPerPage + " Maps";


		//create a list with the names of all layouts.
		BinaryFormatter bf = new BinaryFormatter ();
		//FileStream file = File.
		string[] dirFiles = Directory.GetFiles (AppPath, "*.txt"); 
		maxPages = (int)Mathf.Ceil ((float)dirFiles.Length / (float)filesPerPage);
        int nextpage = filesPerPage * currentPage;
        int prevpage = filesPerPage * (currentPage - 1);
		for (int i = 0; i < dirFiles.Length; i++) {
            dirFiles[i] = dirFiles[i].Replace(AppPath, "");
            dirFiles[i] = dirFiles[i].Replace(".txt", "");

            if (dirFiles[i] != tempfilename) {

                if (i < nextpage && i >= prevpage)
                {
                    if (!tempfilefound)
                    {
                        j = i % filesPerPage;
                    }
                    else
                    {
                        j = (i - 1) % filesPerPage;
                    }
                    
                    Button but = (Button)Instantiate(loadButton, Vector3.zero, Quaternion.identity);
                    loadButtons.Add(but);
                    but.transform.SetParent(loadMapsPanel.gameObject.transform);
                    but.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0f, 0f, 0f);
                    but.GetComponent<RectTransform>().anchoredPosition = new Vector2(200, 50* (rows - j));
                    but.GetComponent<RectTransform>().sizeDelta = new Vector2(1000, 200);
                    but.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1)/4;

                    but.GetComponentInChildren<Text>().text = dirFiles[i];
                    string fileName = dirFiles[i];
                    
                    but.onClick.AddListener(delegate
                    {
                        cameraAudioSource.PlayOneShot(shhhh,volume);
                        selectFileName(but);
                    });
                }
			}
            else
            {
                if (i < filesPerPage * currentPage && i >= filesPerPage * (currentPage - 1))
                {
                    tempfilefound = true;

                    nextpage += 1;

                }

                else if (i <= filesPerPage * (currentPage - 1))
                {
                    tempfilefound = true;
                    prevpage += 1;
                    nextpage += 1;
                    Debug.Log(nextpage - prevpage);

                }
            }
		}

        if (currentPage == maxPages)
        {
            nextBut.gameObject.SetActive(false);
        }
        if (currentPage == 1)
        {

            prevBut.gameObject.SetActive(false);
        }
        if (maxPages < 2){
            nextBut.gameObject.SetActive(false);
        }


	}


	private void selectFileName (Button but)
	{
        miniCamera.gameObject.SetActive(true);
        if (!currentButtonsSelected.Contains(but))
        {
            if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
            {

                foreach (Button button in loadButtons)
                {
                    ColorBlock cb = button.colors;
                    cb.normalColor = new Color(1,1,1,0) ;
                    button.colors = cb;

                }

                currentButSelected = but;
                currentFileSelected = but.GetComponentInChildren<Text>().text;

                currentButtonsSelected = new List<Button>();
                currentFilesSelected = new List<string>();

                currentButtonsSelected.Add(but);
                currentFilesSelected.Add(but.GetComponentInChildren<Text>().text);

                loadMapFromFile(currentFileSelected);

                ColorBlock cblock = but.colors;
                cblock.normalColor = new Color(0.5f, 0.5f, 0.5f, 1f);
                but.colors = cblock;

            }

            else
            {


                ColorBlock cblock = but.colors;
                cblock.normalColor = new Color(0.5f, 0.5f, 0.5f, 1f);
                but.colors = cblock;
                currentButtonsSelected.Add(but);
                currentFilesSelected.Add(but.GetComponentInChildren<Text>().text);

                currentButSelected = null;
                currentFileSelected = null;

                loadMapFromFile(currentFilesSelected[currentFilesSelected.Count-1]);

            }

            if (currentButtonsSelected.Count == 0)
            {
                currentButtonsSelected.Add(but);
            }

        }
        else
        {
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {

                ColorBlock cblock = but.colors;
                cblock.normalColor = new Color(1, 1, 1, 0);
                but.colors = cblock;

                currentButtonsSelected.Remove(but);
                currentFilesSelected.Remove(but.GetComponentInChildren<Text>().text);

                currentButSelected = null;
                currentFileSelected = null;
            }
        }

	}

	private void deleteFile (string fileName, bool cancelling = false)
	{
		if (fileName != null) {
			File.Delete (AppPath + fileName + ".txt");
            if (fileName != tempfilename)
            {
                miniCamera.gameObject.SetActive(false);
            }
            if (!cancelling)
			    ShowSavedMaps ();
		} else {
			setErrorTekst ("No File Selected");
		}
	}

	private void cancelLoadScreen ()
	{
		currentButSelected = null;
		currentFileSelected = null;
		loadMapsPanel.SetActive (false);
		Reset ();
		foreach (GameObject plane in allPos) {
			Destroy (plane);
		}
	}

	//methods to display errortext
	public static void setErrorTekst (string tekst, bool error = true)
	{

		instance.CancelInvoke ();
		if (!instance.playing) {
            instance.ErrorText.text = tekst;

            if (error)
                instance.ErrorText.color = Color.red;
            else
                instance.ErrorText.color = Color.green;

			instance.InvokeRepeating ("errorTekstFadeOut", 2f, 0.01f);
		}
	}

	void errorTekstFadeOut ()
	{
		if (!instance.playing) {
            Color color = ErrorText.color;
			color.a -= 0.01f;
            ErrorText.color = color;
			if (color.a <= 0f) {
				color.a = 1;
                ErrorText.color = color;
                ErrorText.text = "";
				CancelInvoke ();
			}
		}
	}
}

