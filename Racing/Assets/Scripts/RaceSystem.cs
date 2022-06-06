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
        // GameObject car2 = Instantiate(carPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        // GameObject car3 = Instantiate(carPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        // GameObject car4 = Instantiate(carPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        _player1 = car1.GetComponent<Car>();
        _player1.carOffset = new Vector3(1,0,-4);
        _player1.playerType = Car.PlayerType.AI;
        _player1.accelerationKey = "r";
        _player1.brakeKey = "f";
        // _player2 = car2.GetComponent<Car>();
        // _player3 = car3.GetComponent<Car>();
        // _player4 = car4.GetComponent<Car>();

        // _player1.carOffset = new Vector3(1,0,-4);
        // _player2.carOffset = new Vector3(1,0,4);
        // _player3.carOffset = new Vector3(-1,0,1.5f);
        // _player4.carOffset = new Vector3(-1,0,-1.5f);
    }

    private void Update()
    {
        _player1.position = trackSystem.position;
        // _player2.position = trackSystem.position;
        // _player3.position = trackSystem.position;
        // _player4.position = trackSystem.position;

        _player1.previousPosition = trackSystem.previousPosition;
        // _player2.previousPosition = trackSystem.previousPosition;
        // _player3.previousPosition = trackSystem.previousPosition;
        // _player4.previousPosition = trackSystem.previousPosition;

    }
}