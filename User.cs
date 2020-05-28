using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class User
{
    public string userName;
    public string password;
    public double Kill;
    public double Death;
    
    public User()
    {
        userName = Register.playerName;
        password = Register.password;
        Kill = 0;
        Death = 0;
    }
}
