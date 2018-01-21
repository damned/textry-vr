using System.Collections.Generic;

public class LogicalLettersLayout
{
    public List<List<Letter>> rows;
    public int slots;

    public LogicalLettersLayout(List<List<Letter>> rows, int slots)
    {
        this.rows = rows;
        this.slots = slots;
    }
}