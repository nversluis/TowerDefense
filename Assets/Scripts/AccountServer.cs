using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AccountServer : MonoBehaviour {

    string urlGetAccount;

    static string accountInfo;
    static List<List<string>> accounts;

    WWW getAccount;

	// Use this for initialization
	void Start ()
    {
        urlGetAccount = "http://drproject.twi.tudelft.nl:8087/getAccount";
        getAccount = new WWW(urlGetAccount);
        StartCoroutine(WaitForRequest(getAccount));

        accounts = new List<List<string>>();
	}
	
	// Update is called once per frame
	void Update () {
        if (ScoreServer.getting)
        {
            getAccountFromServer();
        }    
	}

    public static bool register(string naam, string password)
    {
        bool res = false;
        if (!usernameInGebruik(naam))
        {
            if (ScoreServer.connected)
            {
                WWW www = new WWW("http://drproject.twi.tudelft.nl:8087/setAccount?username=" + naam + "&password=" + password);
                WaitForRequest(www);
                res = true;
            }
        }
        return res;
    }

    public void getAccountFromServer()
    {
        //WWW www = new WWW(urlScores);
        WaitForRequest(getAccount);
    }

    static IEnumerator WaitForRequest(WWW www)
    {
        yield return www;
        if (www.error == null || www.error == "")
        {
            accountInfo = www.text.ToString();
            splitAccount();
        }
    }

    public static void splitAccount()
    {
        List<string> info;

        accountInfo = accountInfo.Replace("Username: ", "");
        accountInfo = accountInfo.Replace(", Password: ", ",");
        accountInfo = accountInfo.Replace("<br/>", ",");

        string[] split = accountInfo.Split(',');

        for (int i = 0; i < split.Length - 1; i = i + 2)
        {
            info = new List<string>();
            // Username
            info.Add(split[i]);
            // Password
            info.Add(split[i + 1]);

            accounts.Add(info);
        }
    }

    public static List<List<string>> getAccounts()
    {
        return accounts;
    }

    public static List<string> getUsernames()
    {
        List<string> namen = new List<string>();

        if (accounts.Count > 0)
        {
            for (int i = 0; i < accounts.Count; i++)
            {
                namen.Add(accounts[i][0]);
            }
        }
        return namen;
    }

    public static List<string> getPasswords()
    {
        List<string> passwords = new List<string>();

        if (accounts.Count > 0)
        {
            for (int i = 0; i < accounts.Count; i++)
            {
                passwords.Add(accounts[i][1]);
            }
        }
        return passwords;
    }

    public void printMatrix(List<List<string>> matrix)
    {
        for (int i = 0; i < matrix.Count; i++)
        {
            for (int j = 0; j < matrix[i].Count; j++)
            {
                Debug.Log("matrix[" + i + "][" + j + "] = " + matrix[i][j]);
            }
        }
    }

    public static bool usernameInGebruik(string naam)
    {
        return getUsernames().Contains(naam);
    }

    public static bool usernamePasswordMatch(string naam, string wachtwoord)
    {
        bool res = false;

        List<string> gebruikersnamen = getUsernames();

        if (gebruikersnamen.Contains(naam) && wachtwoord.Contains(wachtwoord))
        {
            int index = gebruikersnamen.IndexOf(naam);
            if (accounts[index][1] == wachtwoord)
            {
                res = true;
            }
        }
        return res;
    }

    public void printArray(List<string> array)
    {
        for (int i = 0; i < array.Count; i++)
        {
            Debug.Log("array[" + i + "] = " + array[i]);
        }
    }

}
