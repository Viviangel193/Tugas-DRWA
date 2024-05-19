<!DOCTYPE html>
<html>
<head>
    <title>Edit Book</title>
</head>
<body>
    <h1>Edit Book</h1>
    <form action="{{ url('/update/'.$book['id']) }}" method="post">
        @csrf
        @method('put')
        <label for="bookName">Book Name:</label><br>
        <input type="text" id="bookName" name="bookName" value="{{ $book['bookName'] }}"><br>
        <label for="price">Price:</label><br>
        <input type="text" id="price" name="price" value="{{ $book['price'] }}"><br>
        <label for="category">Category:</label><br>
        <input type="text" id="category" name="category" value="{{ $book['category'] }}"><br>
        <label for="author">Author:</label><br>
        <input type="text" id="author" name="author" value="{{ $book['author'] }}"><br>
        <label for="editor">Editor:</label><br>
        <input type="text" id="editor" name="editor" value="{{ $book['editor'] }}"><br><br>
        <input type="submit" value="Update">
    </form>
</body>
</html>
