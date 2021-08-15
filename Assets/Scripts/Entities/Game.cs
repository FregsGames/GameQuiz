using System;

[Serializable]
public class Game
{
    public int id;
    public int category;
    public long first_release_date;
    public int[] involved_companies = new int[] { };
    public string name;
    public int[] platforms;
    public double rating;

    public DateTime realDate;
}
