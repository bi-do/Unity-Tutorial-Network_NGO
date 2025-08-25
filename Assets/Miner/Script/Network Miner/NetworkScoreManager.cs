using UnityEngine;
using Unity.Netcode;
using TMPro;
using System;

public class NetworkScoreManager : NetworkBehaviour
{
    public static NetworkScoreManager Instance;

    [SerializeField] private TextMeshProUGUI score_UI;

    private NetworkVariable<int> score = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        score.OnValueChanged += OnScoreChange;

        score_UI.text = score.Value.ToString();
    }

    void Awake()
    {
        Instance = this;
    }

    private void OnScoreChange(int previous_value, int new_value)
    {
        score_UI.text = new_value.ToString();
    }

    public void AddScore()
    {
        if (!IsServer)
            return;

        score.Value++;
    }
}