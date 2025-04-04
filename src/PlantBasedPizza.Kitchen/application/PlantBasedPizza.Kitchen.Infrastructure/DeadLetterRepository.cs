using MongoDB.Driver;

namespace PlantBasedPizza.Kitchen.Infrastructure;

public interface IDeadLetterRepository
{
    Task StoreAsync(DeadLetterMessage message);

    Task<IEnumerable<DeadLetterMessage>> GetDeadLetterItems();
}

public class DeadLetterRepository : IDeadLetterRepository
{
    private readonly IMongoCollection<DeadLetterMessage> _deadLetters;

    public DeadLetterRepository(MongoClient client)
    {
        var database = client.GetDatabase("SimplePizzaWinkel");
        _deadLetters = database.GetCollection<DeadLetterMessage>("kitchen_deadletters");
    }

    public async Task StoreAsync(DeadLetterMessage message)
    {
        await _deadLetters.InsertOneAsync(message);
    }

    public async Task<IEnumerable<DeadLetterMessage>> GetDeadLetterItems()
    {
        var items = await _deadLetters.FindAsync(x => true);
        
        return await items.ToListAsync();
    }
}