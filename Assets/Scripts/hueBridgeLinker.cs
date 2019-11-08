using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;
using UnityEngine.UI;
 using System.Text.RegularExpressions;


public class hueBridgeLinker : MonoBehaviour
{
    /// <summary>
    /// Brigde data, used to setup the connection.
    /// </summary>
    public delegate void UIEventhandler();
    public event UIEventhandler onIPChange;

    private void OnEnable()
    {
        onIPChange += retryDeviceConnection;
    }

    private void OnDisable()
    {
        onIPChange -= retryDeviceConnection;
    }

    [Header("Bridge Data")]
    public string bridgeIP; // Only for visualization in unity editor;
    public string bridgeUsername; // Only to visualization in unity editor;
    public bool bridgeLinked; // Only to visualization in unity editor;

    string setupName = "HueGameSetup";

    [System.Serializable]
    public struct bridgeData
    {
        public string internalipaddress;
        public string username;

        public string sceneID;

        public string groupID;

    }

    StoredbridgeData defaultBrigdeData, connectedBrigde;

    public static bridgeData currentBridge;

    public static string centralpath, updateScenePath, updateGroupPath;

    /// <summary>
    /// UI.
    /// </summary>
    [Header("UI")]
    GameObject errorPanel;
    GameObject inputFieldTxt;
    GameObject inputField;


    /// <summary>
    /// For bridge Communication.
    /// </summary>

    [System.Serializable]
    public struct errorResponse
    {
        public string type;
        public string address;
        public string description;

    }

    [System.Serializable]
    public struct successResponse
    {
        public string username;

    }

    [System.Serializable]
    public struct jsonGetResponse
    {
        public errorResponse error;
        public successResponse success;

    }


    const string path = "Assets/bridgeLog.txt";
    bool  retryConn, newIp;

    Color red = Color.red;
    Color green = Color.green;

    /// <summary>
    /// Temp variables
    /// </summary>

    struct StoredbridgeData
    {
        //public BridgeIps [] BridgeIps;
        public string [] username;
        public string [] ips;
    }

    
    void Start()
    {
        defaultBrigdeData = new StoredbridgeData ();
        defaultBrigdeData.ips = new string[1];


        errorPanel = GameObject.Find("ErrorPanel");
        inputField = GameObject.Find("InputField");
        inputFieldTxt = GameObject.Find("inputFieldTxt");
        
        errorPanel.SetActive(false);
    
        brigdeLogSearch();

        red.a = 0.3f;
        green.a = 0.3f;
    }

     void brigdeLogSearch () {

        string text = File.ReadAllText(@path);

        connectedBrigde = new StoredbridgeData ();
   
        connectedBrigde = JsonUtility.FromJson<StoredbridgeData> (text);

        defaultBrigdeData.username = connectedBrigde.username;

        if (string.IsNullOrWhiteSpace (connectedBrigde.ips[0])) {
            Debug.Log("Please enter the IP adress of your Hue brigde.. ");
            retryConn = true;
            errorPanel.SetActive(true);
            
            
        } else {
            StartCoroutine(brigdeConnectionTest (connectedBrigde));
        }
    }

    IEnumerator brigdeConnectionTest(StoredbridgeData _loggedBrigdeData)
    {
        bool ipMatch = false;

        foreach (string ip in _loggedBrigdeData.ips)
        {
            UnityWebRequest deviceRequest = UnityWebRequest.Get(ip +"/api/config");
            deviceRequest.timeout = 1;
            yield return deviceRequest.SendWebRequest();

            if (deviceRequest.isNetworkError || deviceRequest.isHttpError)
            {
                Debug.LogError("Could not connec to bridge through IP: " + ip);
                ipMatch = false;
            } else {
                ipMatch = true;
                currentBridge.internalipaddress = ip;

                string brigdeConfig = deviceRequest.downloadHandler.text;

                if (brigdeConfig.Contains("Philips hue")) {
                    Debug.Log("Bridge found. Valid IP.");
                    if (!string.IsNullOrWhiteSpace (_loggedBrigdeData.username[0])) {
                        StartCoroutine (validadeUsernames(_loggedBrigdeData, currentBridge.internalipaddress));
                    } else {
                         StartCoroutine("DeviceSetup");
                    }
                } else {
                    Debug.Log("Bridge NOT found.");
                    ipMatch = false;
                }

                break;
            }
        }

        if (!ipMatch) {
            Debug.Log("None of the loged IP addresses leads to a brigde. Please enter new IP adress.");
            errorPanel.SetActive(true);
            retryConn = true;
        }
    }

    IEnumerator validadeUsernames(StoredbridgeData _loggedBrigdeData, string _ip)
    {
        bool noAccess = true;

        foreach (string usernames in _loggedBrigdeData.username)
        {
            UnityWebRequest deviceRequest = UnityWebRequest.Get(_ip +"/api/"+ usernames + "/config");
            deviceRequest.timeout = 1;
            yield return deviceRequest.SendWebRequest();

            string input = deviceRequest.downloadHandler.text;
             if (deviceRequest.isNetworkError || deviceRequest.isHttpError)
            {
                Debug.LogError("Connection Error (validadeUsername");
                
            } else {

                if (deviceRequest.downloadHandler.text.Contains("whitelist")) {

                Debug.Log("Valid whitelist user. Booting game up...");
                currentBridge.username = usernames;
                bridgeIP = currentBridge.internalipaddress;
                bridgeUsername = currentBridge.username;
                bridgeLinked = true;
                noAccess = false;

                notarizeBrigdeData(true);

                break;
                } 
            }
        }
        if (noAccess) {
            Debug.Log("No valid whitelist user.");
             StartCoroutine("DeviceSetup");
        }
    }

    public void inputFieldHandler ()
    {
        string inputContent = inputFieldTxt.GetComponent<Text>().text.Trim();
        Debug.Log("yo");
        if (Regex.IsMatch(inputContent,  @"((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)"))
        {
            defaultBrigdeData.ips[0] = inputContent;
            errorPanel.SetActive(false);
            onIPChange();
            Debug.Log("Recalling bridge connection");
        }
        else
        {
            Debug.LogError("Error... Please check if correct IP");
        }
    }

    public void ipChecker () {
        string inputContent = inputFieldTxt.GetComponent<Text>().text.Trim();

        if (Regex.IsMatch(inputContent, @"((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)"))
        {
            inputField.GetComponent<Image>().color = green;
        }
        else
        {
            inputField.GetComponent<Image>().color = red;
        }
    }

    void retryDeviceConnection()
    {
        if (retryConn) {
            StartCoroutine(brigdeConnectionTest(defaultBrigdeData));
            retryConn = false;
            newIp = true;
        } else {
            StartCoroutine("DeviceSetup");
        }
    }

    IEnumerator DeviceSetup ()
    {
        byte[] bodyPost = System.Text.Encoding.UTF8.GetBytes("{\"devicetype\":\"UnityAccessPoint\"}");
        UnityWebRequest deviceRequest = UnityWebRequest.Put(currentBridge.internalipaddress + "/api", bodyPost);
        deviceRequest.method = "POST"; // in order to bypass the put request.....

        Debug.Log("Generating new whitelist username");

        yield return deviceRequest.SendWebRequest();

        if (deviceRequest.isNetworkError)
        {
            Debug.LogError("Post request not send... unable to get new user (" + deviceRequest.error +")");
        }
        else
        {
            string respone = deviceRequest.downloadHandler.text;
            jsonGetResponse getRequest = new jsonGetResponse();
            getRequest = JsonUtility.FromJson<jsonGetResponse>(respone.Replace('[', ' ').Replace(']', ' '));

            if (getRequest.error.description == "link button not pressed")
            {
                Debug.LogError("Please press link button on bridge!");
                errorPanel.SetActive(true);
                errorPanel.transform.GetChild(0).gameObject.SetActive(false);
                errorPanel.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                bridgeLinked = true;
                currentBridge.username = getRequest.success.username;
                bridgeIP = currentBridge.internalipaddress;
                bridgeUsername = currentBridge.username;
                Debug.Log("Connected with brigde. Booting up the game");
                notarizeBrigdeData(false);

            }
        }
    }


    void notarizeBrigdeData(bool _logedBridge)
    {

        StoredbridgeData brigeData2Save = new StoredbridgeData();
        
        if (!_logedBridge && !string.IsNullOrWhiteSpace (connectedBrigde.ips[0])) {
            brigeData2Save.username = new string[connectedBrigde.username.Length + 1];
            for (int i = 0; i < brigeData2Save.username.Length; i++)
            {
                if(i == brigeData2Save.username.Length -1) {
                    brigeData2Save.username[i] = currentBridge.username;
                } else {
                    brigeData2Save.username[i] = connectedBrigde.username[i];
                }
            }
         } else {
             brigeData2Save.username = new string[1];
              brigeData2Save.username[0] = currentBridge.username;
         }

        if (newIp) {

            if (!string.IsNullOrWhiteSpace (connectedBrigde.ips[0])) {

                brigeData2Save.ips = new string[connectedBrigde.username.Length + 1];

                for (int i = 0; i < brigeData2Save.ips.Length; i++)
                {
                    if(i == brigeData2Save.ips.Length -1) {
                        brigeData2Save.ips[i] = currentBridge.internalipaddress;
                    } else {
                        brigeData2Save.ips[i] = connectedBrigde.ips[i];
                    }
                }
            } else {
                brigeData2Save.ips = new string [1];
                brigeData2Save.ips[0] = currentBridge.internalipaddress;
            }           
        } else {
            brigeData2Save.ips = connectedBrigde.ips;
        }

        File.Create(@path).Close();
        StreamWriter writer = new StreamWriter(@path, true);
        writer.Write(JsonUtility.ToJson(brigeData2Save));
        writer.Close();
        errorPanel.SetActive(false);
    }

}

