using Unity.Netcode;
using UnityEngine;

public class MineralEvent : NetworkBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && IsOwner)
        {
            AddScoreServerRpc();
        }
    }

    [ServerRpc]
    private void AddScoreServerRpc()
    {
        NetworkScoreManager.Instance.AddScore();
        this.GetComponent<NetworkObject>().Despawn();
    }
}
