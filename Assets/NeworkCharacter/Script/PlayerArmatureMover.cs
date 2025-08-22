using StarterAssets;
using TMPro;
using Unity.Cinemachine;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerArmatureMover : NetworkBehaviour
{
    [SerializeField] private CharacterController cc;
    [SerializeField] private PlayerInput player_input;
    [SerializeField] private StarterAssetsInputs starter_asset;
    [SerializeField] private ThirdPersonController controller;
    [SerializeField] private Transform player_root;

    [SerializeField] private GameObject bomb_prefab;

    void Awake()
    {
        cc.enabled = false;
        player_input.enabled = false;
        starter_asset.enabled = false;
        controller.enabled = false;


    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsOwner)
        {
            cc.enabled = true;
            player_input.enabled = true;
            starter_asset.enabled = true;
            controller.enabled = true;

            var cinemachine = GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineCamera>();
            cinemachine.Target.TrackingTarget = player_root;
        }
    }

    void Update()
    {
        if (!IsOwner)
            return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            AddScroeServerRPC();
        }

        if (Input.GetMouseButtonDown(0))
        {
              ThrowBombServerRpc();
        }
    }

    [ServerRpc]
    private void ThrowBombServerRpc()
    {
        Instantiate(this.bomb_prefab, this.transform.position + Vector3.forward, Quaternion.identity);
    }

    [ServerRpc]
    private void AddScroeServerRPC()
    {
        ScoreManager.Instance.AddScore();
    }



}
