using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceSystem : MonoBehaviour
{
    public GameObject carPrefab;
    public TrackSystem trackSystem;
    private Car _player1;
    private Car _player2;
    private Car _player3;
    private Car _player4;

    private void Start()
    {
        // Instantiate at position (0, 0, 0) and zero rotation.
        GameObject car1 = Instantiate(carPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        GameObject car2 = Instantiate(carPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        GameObject car3 = Instantiate(carPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        GameObject car4 = Instantiate(carPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        _player1 = car1.GetComponent<Car>();
        _player1.carOffset = new Vector3(1,0,-4);
        _player1.playerType = Car.PlayerType.Player;
        _player1.accelerationKey = "r";
        _player1.brakeKey = "f";
        _player1.speed = TrackSystem.speed;

        _player2 = car2.GetComponent<Car>();
        _player2.carOffset = new Vector3(1,0,4);
        _player2.playerType = Car.PlayerType.Player;
        _player2.accelerationKey = "t";
        _player2.brakeKey = "g";
        _player2.speed = TrackSystem.speed;

        _player3 = car3.GetComponent<Car>();
        _player3.carOffset = new Vector3(-1,0,1.5f);
        _player3.playerType = Car.PlayerType.Player;
        _player3.accelerationKey = "y";
        _player3.brakeKey = "h";
        _player3.speed = TrackSystem.speed;
        
        _player4 = car4.GetComponent<Car>();
        _player4.carOffset = new Vector3(-1,0,-1.5f);
        _player4.playerType = Car.PlayerType.Player;
        _player4.accelerationKey = "u";
        _player4.brakeKey = "j";
        _player4.speed = TrackSystem.speed;
    }
}