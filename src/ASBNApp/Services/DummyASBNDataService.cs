// Created 30.11.2023
// Dummy version of the IASBNDataService interface to get the ui up and running

class DummyASBNDataService : IASBNDataService
{

    public string ReadData() => "";

    public void WriteData(string text)
    {
        Console.WriteLine(text);
    }

    public async Task<IEnumerable<EntryRowModel>> GetWeek(int? year, int? week)
    {

        // TODO: What if we don't have data to return? Make the return value for GetWeek ?

        if (year == 2023 && week == 49) {
            var result =  new List<EntryRowModel>(){
                new EntryRowModel(){
                    Note = "Hannes hat den Stift geklaut", 
                    Date = new DateTime(2023, 12, 04), 
                    Location = "MMS" },
                new EntryRowModel(){
                    Note = "Hannes hat den Stift geklaut 2", 
                    Date = new DateTime(2023, 12, 05), 
                    Location = "BSZ ET DD" },
                new EntryRowModel(){
                    Note = "Hannes hat den Stift geklaut 3", 
                    Date = new DateTime(2023, 12, 06), 
                    Location = "HUB Dresden" },
                new EntryRowModel(){
                    Note = "Hannes hat den Stift geklaut 4", 
                    Date = new DateTime(2023, 12, 07), 
                    Location = "BSZ ET DD" },
                new EntryRowModel(){
                    Note = "Hannes hat den Stift geklaut 5", 
                    Date = new DateTime(2023, 12, 08), 
                    Location = "BSZ ET DD" },
            };
            return await Task.FromResult(result.AsEnumerable());
        } else if (year == 2023 && week == 50) {
            var result =  new List<EntryRowModel>(){
                new EntryRowModel(){
                    Note = "Dieter hat den Stift geklaut", 
                    Date = new DateTime(2023, 12, 11), 
                    Location = "MMS" },
                new EntryRowModel(){
                    Note = "Dieter hat den Stift geklaut 2", 
                    Date = new DateTime(2023, 12, 12), 
                    Location = "BSZ ET DD" },
                new EntryRowModel(){
                    Note = "Dieter hat den Stift geklaut 3", 
                    Date = new DateTime(2023, 12, 13), 
                    Location = "HUB Dresden" },
                new EntryRowModel(){
                    Note = "Dieter hat den Stift geklaut 4", 
                    Date = new DateTime(2023, 12, 14), 
                    Location = "BSZ ET DD" },
                new EntryRowModel(){
                    Note = "Dieter hat den Stift geklaut 5", 
                    Date = new DateTime(2023, 12, 15), 
                    Location = "BSZ ET DD" },
            };
            return await Task.FromResult(result.AsEnumerable());
        } else {
            var result = new List<EntryRowModel>() {};
            return await Task.FromResult(result.AsEnumerable());
        }
        
        
    }

    public async Task SaveWeek(IEnumerable<EntryRowModel> entries)
    {
        throw new NotImplementedException();
    }


    public async Task<EntryRowModel> GetDay(DateTime? date)
    {
        throw new NotImplementedException();
    }

    public async Task SaveDay(EntryRowModel entry)
    {
        throw new NotImplementedException();
    }
}