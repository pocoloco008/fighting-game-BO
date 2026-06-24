using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class ControllerManager : MonoBehaviour
{
    public static ControllerManager Instance;

    [Header("Player GameObjects (Koppel in de Inspector)")]
    public GameObject player1Object;
    public GameObject player2Object;

    [System.Serializable]
    public class PlayerSlot
    {
        public string name;
        public Gamepad gamepad;
        [HideInInspector] public GameObject playerGameObject;
    }

    public List<PlayerSlot> players = new List<PlayerSlot>();
    private Dictionary<int, int> deviceIdToPlayer = new Dictionary<int, int>(); // Gebruik device.deviceId nu

    private float lastAssignTime = 0f;
    private float assignCooldown = 0.5f; // Wacht een halve seconde tussen het toewijzen van controllers

    void Awake()
    {
        Instance = this;

        // Spelers uitschakelen aan het begin
        if (player1Object != null) player1Object.SetActive(false);
        if (player2Object != null) player2Object.SetActive(false);

        // Zorg dat er altijd 2 slots zijn
        if (players.Count < 2)
        {
            players.Clear();
            players.Add(new PlayerSlot { name = "Player 1", playerGameObject = player1Object });
            players.Add(new PlayerSlot { name = "Player 2", playerGameObject = player2Object });
        }
        else
        {
            players[0].playerGameObject = player1Object;
            players[1].playerGameObject = player2Object;
        }

        InputSystem.onAnyButtonPress.Call(OnAnyInput);
    }

    private void OnAnyInput(InputControl control)
    {
        // Voorkom dubbele trigger bugs van één actie
        if (Time.unscaledTime - lastAssignTime < assignCooldown)
            return;

        var device = control.device;

        if (device is not Gamepad pad)
            return;

        if (control != pad.buttonSouth)
            return;

        // Controleer op unieke Device ID (dit filtert ghost devices van één controller grotendeels uit)
        int deviceId = device.deviceId;
        if (deviceIdToPlayer.ContainsKey(deviceId))
            return;

        // Zoek ook of we deze fysieke controller al hebben toegewezen 
        // (Soms geeft PS4 controller iets door als DualShock en daarna als standaard Gamepad maar ze komen van dezelfde hardware)
        foreach (var player in players)
        {
            if (player.gamepad != null && player.gamepad.deviceId == deviceId)
            {
                return;
            }
        }

        // Zoek een leeg slot
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].gamepad == null)
            {
                players[i].gamepad = pad;
                deviceIdToPlayer[deviceId] = i;
                lastAssignTime = Time.unscaledTime;

                Debug.Log($"[ASSIGN] {players[i].name} succesvol gekoppeld aan {pad.displayName} (ID: {deviceId})!");

                if (players[i].playerGameObject != null)
                {
                    players[i].playerGameObject.SetActive(true);
                }

                return;
            }
        }
    }

    public Gamepad GetGamepad(int playerIndex)
    {
        if (playerIndex < 0 || playerIndex >= players.Count)
            return null;

        return players[playerIndex].gamepad;
    }
}