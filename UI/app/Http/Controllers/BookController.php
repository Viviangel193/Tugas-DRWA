<?php

namespace App\Http\Controllers;

use GuzzleHttp\Client;
use Illuminate\Http\Request;

class BookController extends Controller
{
    protected $client;

    public function __construct()
    {
        // Membuat instance GuzzleHTTP Client
        $this->client = new Client(['base_uri' => 'http://localhost:5053/']);
    }

    public function index()
    {
        // Mengambil data dari .NET API
        $response = $this->client->request('GET', 'books');
        $books = json_decode($response->getBody()->getContents(), true);

        // Mengirim data ke view
        return view('welcome', ['books' => $books]);
    }

    public function create()
    {
        // Menampilkan view untuk membuat buku baru
        return view('create');
    }

    public function store(Request $request)
    {
        // Menyimpan data baru
        $response = $this->client->request('POST', 'books', [
            'json' => $request->all(),
        ]);

        // Redirect ke halaman utama setelah menyimpan
        return redirect('/');
    }

    public function edit($id)
    {
        // Mendapatkan detail buku untuk diedit
        $response = $this->client->request('GET', "books/{$id}");
        $book = json_decode($response->getBody()->getContents(), true);

        // Menampilkan view untuk mengedit buku
        return view('edit', ['book' => $book]);
    }

    public function update(Request $request, $id)
    {
        // Mengupdate data buku
        $response = $this->client->request('PUT', "books/{$id}", [
            'json' => $request->all(),
        ]);

        // Kembali ke halaman utama setelah mengupdate
        return redirect('/');
    }

    public function destroy($id)
    {
        // Menghapus data buku
        $response = $this->client->request('DELETE', "books/{$id}");

        // Kembali ke halaman utama setelah menghapus
        return redirect('/');
    }
}
