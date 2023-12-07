// Created 17.10.2023
// Models how an entry for a single day looks like

public class EntryRowModel
{
    public DateTime Date { get; set; }
    public string? Location { get; set; }
    public string? Note { get; set; }
}