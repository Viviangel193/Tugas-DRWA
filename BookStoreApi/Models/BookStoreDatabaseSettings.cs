public class BookStoreDatabaseSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string BooksCollectionName { get; set; }
    public string AuthorsCollectionName { get; set; }

    public BookStoreDatabaseSettings()
    {
        ConnectionString = string.Empty;
        DatabaseName = string.Empty;
        BooksCollectionName = "books"; // Default collection name for books
        AuthorsCollectionName = "authors"; // Default collection name for authors
    }

    public BookStoreDatabaseSettings(string connectionString, string databaseName, string booksCollectionName, string authorsCollectionName)
    {
        ConnectionString = connectionString;
        DatabaseName = databaseName;
        BooksCollectionName = booksCollectionName;
        AuthorsCollectionName = authorsCollectionName;
    }
}
