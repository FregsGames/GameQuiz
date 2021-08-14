
using System.Collections.Generic;

[System.Serializable]
public class Platform
{
    public int id;
    public string name;
    public List<Game> games = new List<Game>();
}
