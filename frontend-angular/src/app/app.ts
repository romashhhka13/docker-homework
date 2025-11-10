import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NgFor } from '@angular/common'; 
import { TodoComponent } from './todo/todo'; 

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, NgFor, TodoComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})

export class App {
  protected readonly title = signal('Angular todo');
}

