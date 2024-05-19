namespace BlazorSample;

public class TodoItem
{
    public string? Id { get; set; }
    public string? BookName { get; set; }
    public string? Price { get; set; }
    public string? Category { get; set; }
    public string? Author { get; set; }
    public bool IsDone { get; set; } // Add this property
    public string? Title { get; set; } // Add this property
}
