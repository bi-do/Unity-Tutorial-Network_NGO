using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ConnectManager : MonoBehaviour
{
    [SerializeField] private Button server_btn;
    [SerializeField] private Button host_btn;
    [SerializeField] private Button client_btn;


    void Awake()
    {
        server_btn.onClick.AddListener(() => NetworkManager.Singleton.StartServer());
        host_btn.onClick.AddListener(() => NetworkManager.Singleton.StartHost());
        client_btn.onClick.AddListener(() => NetworkManager.Singleton.StartClient());
    }
}
