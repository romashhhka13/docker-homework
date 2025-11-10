import { Injectable } from '@angular/core';
import { TodoDeadlineService } from './todo-deadline'; // исправлен импорт
import { Todo, FilterType } from './todoModel';

@Injectable({
  providedIn: 'root'
})
export class TodoFilterService { 
  constructor(private todoDeadlineService: TodoDeadlineService) {} 
  
  // Метод для получения отфильтрованных задач
  getFilteredTodos(todos: Todo[], currentFilter: FilterType): Todo[] {
    switch (currentFilter) {
      case 'completed':
        return todos.filter(todo => todo.isCompleted);
      case 'active':
        return todos.filter(todo => !todo.isCompleted);
      case 'favorite':
        return todos.filter(todo => todo.isFavorite);
      case 'missed':
        return todos.filter(todo => 
          this.todoDeadlineService.isOverdue(todo.deadline) && !todo.isCompleted
        );
      default:
        return todos;
    }
  }

  // Метод для проверки активного фильтра
  isActiveFilter(currentFilter: FilterType, filter: FilterType): boolean {
    return currentFilter === filter;
  }
  // Методы для подсчета статистики
  getCompletedCount(todos: Todo[]): number {
    return todos.filter(todo => todo.isCompleted).length;
  }
  getActiveCount(todos: Todo[]): number {
    return todos.filter(todo => !todo.isCompleted).length;
  }
  getFavoriteCount(todos: Todo[]): number {
    return todos.filter(todo => todo.isFavorite).length;
  }
  getMissedCount(todos: Todo[]): number {
    return todos.filter(todo => 
      this.todoDeadlineService.isOverdue(todo.deadline) && !todo.isCompleted
    ).length;
  }

  // Метод для получения общего количества задач по фильтру
  getFilterCount(todos: Todo[], filter: FilterType): number {
    switch (filter) {
      case 'completed':
        return this.getCompletedCount(todos);
      case 'active':
        return this.getActiveCount(todos);
      case 'favorite':
        return this.getFavoriteCount(todos);
      case 'missed':
        return this.getMissedCount(todos);
      default:
        return todos.length;
    }
  }
}