using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Moralis
using MoralisWeb3ApiSdk;
using Moralis.WebGL;

//WalletConnect
using WalletConnectSharp.Core.Models;
using WalletConnectSharp.Unity;

public class AppManager : MonoBehaviour
{
    #region PUBLIC_FIELDS

    public MoralisController moralisController;
    public WalletConnect walletConnect;

    [Header("UI Elements")]
    public GameObject editorPanel;
    public GameObject mobilePanel;
    public GameObject webPanel;
    public GameObject loggedInPanel;
    public TextMeshProUGUI infoLabel;

    #endregion

    #region PRIVATE_FIELDS

    private GameObject _currentActivePanel;

    #endregion

    #region UNITY_LIFECYCLE

    private void Awake()
    {
        infoLabel.text = String.Empty;
        
        if (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            editorPanel.SetActive(true);
            _currentActivePanel = editorPanel;
        }
        else
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                walletConnect.gameObject.SetActive(false);
                webPanel.SetActive(true);
                _currentActivePanel = mobilePanel;
            }
            
            if (Application.platform == RuntimePlatform.Android)
            {
                mobilePanel.SetActive(true);
                _currentActivePanel = mobilePanel;
            }
        }
    }

    private async void Start()
    {
        if (moralisController != null)
        {
            await moralisController.Initialize();
        }
        else
        {
            Debug.LogError("MoralisController not found.");
        }
    }

    #endregion

    #region MORALIS AND WALLETCONNECT

    public async void WalletConnectHandler(WCSessionData data)
    {
        Debug.Log("Wallet connection received");
      
        string address = data.accounts[0].ToLower();
        string appId = MoralisInterface.GetClient().ApplicationId;
        long serverTime = 0;

        
        Dictionary<string, object> serverTimeResponse = await MoralisInterface.GetClient().Cloud.RunAsync<Dictionary<string, object>>("getServerTime", new Dictionary<string, object>());

        if (serverTimeResponse == null || !serverTimeResponse.ContainsKey("dateTime") ||
            !long.TryParse(serverTimeResponse["dateTime"].ToString(), out serverTime))
        {
            Debug.Log("Failed to retrieve server time from Moralis Server!");
        }

        if (!MoralisInterface.IsLoggedIn())
        {
            Debug.Log($"Sending sign request for {address} ...");

            string signMessage = $"Moralis Authentication\n\nId: {appId}:{serverTime}";
            string response = await walletConnect.Session.EthPersonalSign(address, signMessage);
        
            Debug.Log(response);

            Debug.Log($"Signature {response} for {address} was returned.");

            // moralis auth data yarat
            Dictionary<string, object> authData = new Dictionary<string, object> { { "id", address }, { "signature", response }, { "data", signMessage } }; 

            Debug.Log("Logging in user.");

            // Giriþ dene
            var user = await MoralisInterface.LogInAsync(authData);

            if (user != null)
            {
                Debug.Log($"User {user.username} logged in successfully. ");
                infoLabel.text = $"Kullanýcý {user.username} giris yaptý!";
            }
            else
            {
                Debug.Log("User login failed.");
                infoLabel.text = "Giris basarisiz";
            }
        }

        UserLoggedInHandler();
    }
    
    public void HandleWalletConnected()
    {
        infoLabel.text = "Giris basarili. Lutfen Metamask uzerinden imzalayin";
    }

    public void HandleWalletDisconnected()
    {
        _currentActivePanel.SetActive(true);
        loggedInPanel.SetActive(false);
        
        infoLabel.text = "Cuzdan baglantisi koptu";
    }

    public void WalletConnectSessionEstablished(WalletConnectUnitySession session)
    {
        InitializeWeb3();
    }
    
    private void InitializeWeb3()
    {
        MoralisInterface.SetupWeb3();
    }
    
    #if UNITY_WEBGL
    
    private async UniTask LoginWithWeb3()
    {
        string userAddr = "";
        if (!Web3GL.IsConnected())
        {
            userAddr = await MoralisInterface.SetupWeb3();
        }
        else
        {
            userAddr = Web3GL.Account();
        }

        if (string.IsNullOrWhiteSpace(userAddr))
        {
            Debug.LogError("Could not login or fetch account from web3.");
        }
        else 
        {
            string address = Web3GL.Account().ToLower();
            string appId = MoralisInterface.GetClient().ApplicationId;
            long serverTime = 0;

            Dictionary<string, object> serverTimeResponse = await MoralisInterface.GetClient().Cloud.RunAsync<Dictionary<string, object>>("getServerTime", new Dictionary<string, object>());

            if (serverTimeResponse == null || !serverTimeResponse.ContainsKey("dateTime") ||
                !long.TryParse(serverTimeResponse["dateTime"].ToString(), out serverTime))
            {
                Debug.Log("Failed to retrieve server time from Moralis Server!");
            }

            string signMessage = $"Moralis Authentication\n\nId: {appId}:{serverTime}";

            string signature = await Web3GL.Sign(signMessage);

            Debug.Log($"Signature {signature} for {address} was returned.");

            Dictionary<string, object> authData = new Dictionary<string, object> { { "id", address }, { "signature", signature }, { "data", signMessage } };

            Debug.Log("Logging in user.");

            var user = await MoralisInterface.LogInAsync(authData);

            if (user != null)
            {
                Debug.Log($"User {user.username} logged in successfully. ");
            }
            else
            {
                Debug.Log("User login failed.");
            }
            
            UserLoggedInHandler();
        }
    }
    
    #endif
    
    #endregion

    #region PRIVATE_METHODS

    private async void UserLoggedInHandler()
    {
        var user = await MoralisInterface.GetUserAsync();

        if (user != null)
        {
            _currentActivePanel.SetActive(false);
            loggedInPanel.SetActive(true);
            infoLabel.text = $"Kullanýcý {user.username} giris yaptý";
        }
    }
    
    private async void LogOut()
    {
        await MoralisInterface.LogOutAsync();
        
    
        if (walletConnect.isActiveAndEnabled)
        {
            await walletConnect.Session.Disconnect();
            walletConnect.CLearSession();   
        }
    }

    #endregion
    
    #region EDITOR_METHODS

    #if UNITY_WEBGL
    public async void OnLoginClicked()
    {
        await LoginWithWeb3();
    }
    
    #endif
    
    public void OnJoinRoomClicked()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnCloseClicked()
    {
        LogOut();

        if (Application.platform == RuntimePlatform.WebGLPlayer || Application.isEditor) return;
        
        Application.Quit();
    }

    #endregion
}