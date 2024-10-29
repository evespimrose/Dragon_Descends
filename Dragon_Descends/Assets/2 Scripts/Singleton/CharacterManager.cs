using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : SingletonManager<CharacterManager>
{
    public Player player;

    public Transform Target;

    public List<Enemy> enemies;

    private void Update()
    {
        Get_MouseInput();
    }

    private void Get_MouseInput()
    {
        if (Input.GetMouseButton(0))
        {
            print("Input.mousePosition : " + Input.mousePosition);
            Vector2 mousePos = new Vector2(Input.mousePosition.x - 240, Input.mousePosition.y + 130);
            player.Target.position = mousePos;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            player.Target.position = player.transform.position;
        }
    }
}
