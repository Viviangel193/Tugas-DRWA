<!DOCTYPE html>
<html>
<head>
    <title>Books List</title>
</head>
<body>
    <h1>Books List</h1>
    
    <!-- Tombol untuk membuat data baru -->
    <a href="{{ url('/create') }}"><button>Create New Book</button></a>
    
    <!-- Form untuk menambahkan data baru -->
    <form action="{{ url('/books') }}" method="post">
        @csrf
        <table>
            <tr>
                <td>Book Name:</td>
                <td><input type="text" name="bookName"></td>
            </tr>
            <tr>
                <td>Price:</td>
                <td><input type="text" name="price"></td>
            </tr>
            <tr>
                <td>Category:</td>
                <td><input type="text" name="category"></td>
            </tr>
            <tr>
                <td>Author:</td>
                <td><input type="text" name="author"></td>
            </tr>
            <tr>
                <td>Editor:</td>
                <td><input type="text" name="editor"></td>
            </tr>
            <tr>
                <td colspan="2"><button type="submit">Add Book</button></td>
            </tr>
        </table>
    </form>
    
    <table border="1">
        <thead>
            <tr>
                <th>ID</th>
                <th>Book Name</th>
                <th>Price</th>
                <th>Category</th>
                <th>Author</th>
                <th>Editor</th>
                <th>Action</th> <!-- Kolom untuk tombol CRUD -->
            </tr>
        </thead>
        <tbody>
            @if(!empty($books) && is_array($books))
                @foreach($books as $book)
                    <tr>
                        <td>{{ $book['id'] }}</td>
                        <td>{{ $book['bookName'] }}</td>
                        <td>{{ $book['price'] }}</td>
                        <td>{{ $book['category'] }}</td>
                        <td>{{ $book['author'] }}</td>
                        <td>{{ $book['editor'] ?? 'N/A' }}</td>
                        <td>
                            <!-- Tombol untuk mengedit -->
                            <a href="{{ url('/edit/'.$book['id']) }}">Edit</a>
                            
                            <!-- Tombol untuk menghapus -->
                            <form action="{{ url('/delete/'.$book['id']) }}" method="post">
                                @csrf
                                @method('delete')
                                <button type="submit">Delete</button>
                            </form>
                        </td>
                    </tr>
                @endforeach
            @else
                <tr>
                    <td colspan="7">No books found.</td>
                </tr>
            @endif
        </tbody>
    </table>
</body>
</html>
