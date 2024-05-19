<?php

use App\Http\Controllers\BookController;

Route::get('/', [BookController::class, 'index']);
Route::get('/create', [BookController::class, 'create']);
Route::post('/books', [BookController::class, 'store']);
Route::get('/edit/{id}', [BookController::class, 'edit']);
Route::put('/update/{id}', [BookController::class, 'update']);
Route::delete('/delete/{id}', [BookController::class, 'destroy']);

