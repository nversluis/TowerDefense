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

        getAccountFromServer();
	}

    static public void sendScoreToServer(string naam, string password)
    {
        WWW www = new WWW("http://drproject.twi.tudelft.nl:8087/setAccount?username=" + naam + "&password=" + password);

        WaitForRequest(www);
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
            // Naam
            info.Add(split[i]);
            // Difficulty
            info.Add(split[i + 1]);

            accounts.Add(info);
        }
    }

    public List<List<string>> getAccounts()
    {
        return accounts;
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

}
