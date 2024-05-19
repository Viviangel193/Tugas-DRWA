<!DOCTYPE html>
<html>
<head>
    <title>Books List</title>
</head>
<body>
    <h1>Books List</h1>
    
    <!-- Tombol untuk membuat data baru -->
    <a href="{{ url('/create') }}"><button>Create New Book</button></a>
    
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
            <tbody>
                @if (!empty($books) && is_array($books))
                    @foreach ($books as $book)
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
            
        </tbody>
    </table>
</body>
</html>
