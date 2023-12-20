// Created 17.10.2023
// Models how an entry for a single day looks like

public class EntryRowModel
{
    public string? Note { get; set; }
    public DateTime Date { get; set; }
    public string? Location { get; set; }
    public float? Hours { get; set; }
}