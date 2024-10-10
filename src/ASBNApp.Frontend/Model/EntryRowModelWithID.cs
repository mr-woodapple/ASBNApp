namespace ASBNApp.Frontend.Model
{
    /// <summary>
    /// Extension model of <see cref="EntryRowModel"/>, but with an ID 
    /// (that is required in the frontend on a few points), mainly to decide
    /// whether saving an entry should cause a PUSH or PATCH, but also for DELETE requests.
    /// </summary>
    public class EntryRowModelWithID : EntryRowModel
    {
        public int? Id { get; set; }
    }
}
