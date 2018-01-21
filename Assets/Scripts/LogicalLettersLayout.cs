using System.Collections.Generic;

public class LogicalLettersLayout
{
    public List<List<Letter>> rows;
    public int slots;
    public readonly float xFactor;
    public readonly float yFactor;

    public LogicalLettersLayout(List<List<Letter>> rows, int slots, float xFactor, float yFactor)
    {
        this.rows = rows;
        this.slots = slots;
        this.xFactor = xFactor;
        this.yFactor = yFactor;
    }
}