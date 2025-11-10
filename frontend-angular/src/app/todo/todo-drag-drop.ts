import { Injectable } from '@angular/core';
import { Todo } from './todoModel';

@Injectable({
  providedIn: 'root'
})
export class TodoDragDropService {
private draggedTodo: Todo | null = null;

  setDraggedTodo(todo: Todo): void {
    this.draggedTodo = todo;
  }

  getDraggedTodo(): Todo | null {
    return this.draggedTodo;
  }

  clearDraggedTodo(): void {
    this.draggedTodo = null;
  }
}
