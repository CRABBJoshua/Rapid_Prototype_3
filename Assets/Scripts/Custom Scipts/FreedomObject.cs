using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FreedomObject : MonoBehaviour
{
    public virtual void OnRespawn() { }

    public virtual void OnPlayerMotorContact(PlayerMotor contact) { }
}
