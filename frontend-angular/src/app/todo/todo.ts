import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BaseChartDirective } from 'ng2-charts';
import { Todo, FilterType, TodayWidgetState } from './todoModel';
import { TodoFilterService } from './todo-filter';
import { TodoDeadlineService } from './todo-deadline';
import { TodoStatsService } from './todo-stats';
import { TodoDragDropService } from './todo-drag-drop';

@Component({
  selector: 'app-todo',
  standalone: true,
  imports: [CommonModule, FormsModule, BaseChartDirective],
  templateUrl: './todo.html',
  styleUrls: ['./todo.scss']
})
export class TodoComponent {
  todos: Todo[] = [
    {
      id: 1,
      title: 'Изучить Angular',
      description: 'Познакомиться с основами фреймворка',
      isCompleted: true,
      isFavorite: true,
      createDate: new Date('2024-01-15'),
      deadline: new Date('2024-09-15')
    },
    {
      id: 2,
      title: 'Создать Todo List',
      description: 'Реализовать красивое приложение для управления задачами',
      isCompleted: false,
      isFavorite: false,
      createDate: new Date('2024-01-16'),
      deadline: new Date('2025-10-17')
    },
    {
      id: 3,
      title: 'Добавить стили',
      description: 'Сделать интерфейс привлекательным и удобным',
      isCompleted: false,
      isFavorite: true,
      createDate: new Date('2024-01-17'),
      deadline: new Date('2025-10-20')
    }
  ];

  newTodoTitle: string = '';
  newTodoDescription: string = '';
  newTodoDeadline: string | null = null;
  showAddForm: boolean = false;

  currentFilter: FilterType = 'all';

  // Переменные для редактирования
  editingTodo: Todo | null = null;
  editTitle: string = '';
  editDescription: string = '';
  editDeadline: string | null = null;

  showChart = false;
  chartData: any;
  chartOptions: any;
  chartPlugins: any[] = [];
  
  deadlineChartData: any;
  deadlineChartOptions: any;

  todayWidget: TodayWidgetState = {
    id: 1,
    todoIds: [],
    isCollapsed: false
  };

  constructor(
    private todoFilterService: TodoFilterService,
    private todoDeadlineService: TodoDeadlineService,
    private TodoStatsService: TodoStatsService,
    private TodoDragDropService: TodoDragDropService
  ) {}

  // Генератор ID
  private generateId(): number {
    return this.todos.length > 0 ? Math.max(...this.todos.map(t => t.id)) + 1 : 1;
  }

  // Новые методы для работы с диаграммой
  toggleChart(): void {
    this.showChart = !this.showChart;
    if (this.showChart) {
      this.updateChart();
    }
  }

  updateChart(): void {
   this.chartData = this.TodoStatsService.getChartData(this.todos);
    this.chartOptions = this.TodoStatsService.getChartOptions();
    this.chartPlugins = [this.TodoStatsService.getCenterTextPlugin(this.todos)];
    
    // Обновляем данные гистограммы
    this.deadlineChartData = this.TodoStatsService.getDeadlineChartData(this.todos);
    this.deadlineChartOptions = this.TodoStatsService.getDeadlineChartOptions();
}

  // Обновляем диаграмму при изменении задач
  onTodosChange(): void {
    if (this.showChart) {
      this.updateChart();
    }
  }

  isTitleValid(title: string): boolean {
    if (!title) return false;
    return true;
  }

  // Метод добавления нового дела
  addTodo(): void {
    if (this.newTodoTitle.trim()) {
      // Проверка валидности дедлайна
      if (!this.isDeadlineValid) {
        alert('Дедлайн не может быть раньше текущего времени!');
        return;
      }

      const newTodo: Todo = {
        id: this.generateId(),
        title: this.newTodoTitle.trim(),
        description: this.newTodoDescription.trim(),
        isCompleted: false,
        isFavorite: false,
        createDate: new Date(),
        deadline: this.newTodoDeadline ? new Date(this.newTodoDeadline) : null
      };

      this.todos = [newTodo, ...this.todos];
      this.newTodoTitle = '';
      this.newTodoDescription = '';
      this.newTodoDeadline = null;
      this.showAddForm = false;
    }
    this.onTodosChange();
  }

  // Отмена добавления
  cancelAdd(): void {
    this.newTodoTitle = '';
    this.newTodoDescription = '';
    this.newTodoDeadline = null;
    this.showAddForm = false;
  }

  // Переключение формы добавления
  toggleAddForm(): void {
    this.showAddForm = !this.showAddForm;
  }

  // === МЕТОДЫ РЕДАКТИРОВАНИЯ ===

  // Начало редактирования задачи
  startEdit(todo: Todo): void {
    this.editingTodo = { ...todo }; // создаем копию объекта туду
    this.editTitle = todo.title;
    this.editDescription = todo.description;
    this.editDeadline = todo.deadline ? this.formatDateForInput(todo.deadline) : null;
  }

  // Сохранение изменений
  saveEdit(): void {
    if (this.editingTodo && this.editTitle.trim()) {
      // Проверка валидности дедлайна при редактировании
      if (this.editDeadline && !this.todoDeadlineService.isDeadlineValid(this.editDeadline)) {
        alert('Дедлайн не может быть раньше текущего времени!');
        return;
      }
      //проходим по списку задач и обновляем ту, у которой id совпадает с редактируемой
      this.todos = this.todos.map(todo => 
        todo.id === this.editingTodo!.id 
          ? {
              ...todo,
              title: this.editTitle.trim(),
              description: this.editDescription.trim(),
              deadline: this.editDeadline ? new Date(this.editDeadline) : null
            }
          : todo
      );
      this.cancelEdit();
    }
    this.onTodosChange();
  }

  // Отмена редактирования
  cancelEdit(): void {
    this.editingTodo = null;
    this.editTitle = '';
    this.editDescription = '';
    this.editDeadline = null;
  }
  // Проверка, редактируется ли задача
  isEditing(todoId: number): boolean {
    return this.editingTodo?.id === todoId;
  }

  // Форматирование даты для input[type="datetime-local"]
  private formatDateForInput(date: Date): string {
    return new Date(date).toISOString().slice(0, 16);
  }

  // === ОСНОВНЫЕ МЕТОДЫ ===

  // выполненное 
  toggleComplete(id: number): void {
    this.todos = this.todos.map(todo => 
      todo.id === id ? { ...todo, isCompleted: !todo.isCompleted } : todo
    );
    this.onTodosChange();
  }

  //избранное
  toggleFavorite(id: number): void {
    this.todos = this.todos.map(todo => 
      todo.id === id ? { ...todo, isFavorite: !todo.isFavorite } : todo
    );
    this.onTodosChange();
  }

  //удаление
  deleteTodo(id: number): void {
    // Если удаляем редактируемую задачу, отменяем редактирование
    if (this.editingTodo?.id === id) {
      this.cancelEdit();
    }
    this.todos = this.todos.filter(t => t.id !== id);
    this.onTodosChange();
  }

  // Методы для получения статистики через сервис
  getCompletedCount(): number {
    return this.todoFilterService.getCompletedCount(this.todos);
  }

  getActiveCount(): number {
    return this.todoFilterService.getActiveCount(this.todos);
  }

  getFavoriteCount(): number {
    return this.todoFilterService.getFavoriteCount(this.todos);
  }

  getMissedCount(): number {
    return this.todoFilterService.getMissedCount(this.todos);
  }

  // Метод для изменения фильтра
  setFilter(filter: FilterType): void {
    this.currentFilter = filter;
  }

  // Метод для получения отфильтрованных задач через сервис
  get filteredTodos(): Todo[] {
    return this.todoFilterService.getFilteredTodos(this.todos, this.currentFilter);
  }

  // Метод для проверки активного фильтра через сервис
  isActiveFilter(filter: FilterType): boolean {
    return this.todoFilterService.isActiveFilter(this.currentFilter, filter);
  }

  // Геттеры для работы с дедлайнами через сервис
  get minDate(): string {
    return this.todoDeadlineService.getMinDate();
  }

  get isDeadlineValid(): boolean {
    return this.todoDeadlineService.isDeadlineValid(this.newTodoDeadline);
  }
  // Валидация дедлайна при редактировании
  get isEditDeadlineValid(): boolean {
    return this.todoDeadlineService.isDeadlineValid(this.editDeadline);
  }

  // Метод для очистки дедлайна
  clearDeadline(): void {
    this.newTodoDeadline = null;
  }

  // Метод для очистки дедлайна при редактировании
  clearEditDeadline(): void {
    this.editDeadline = null;
  }

  // Проверяет, просрочен ли дедлайн через сервис
  isOverdue(deadline: Date | null): boolean {
    return this.todoDeadlineService.isOverdue(deadline);
  }

  // Получаем статистику для отображения текстовой информации
  get stats(): any {
    return this.TodoStatsService.getTodoStats(this.todos);
  }

  // Методы для Drag & Drop
  onDragStart(todo: Todo, event: DragEvent): void {
    this.TodoDragDropService.setDraggedTodo(todo);
    event.dataTransfer?.setData('text/plain', todo.id.toString());
    event.dataTransfer!.effectAllowed = 'move'; 
    // Добавляем класс для визуальной обратной связи
    const element = event.target as HTMLElement;
    element.classList.add('dragging');
  }
  onDragEnd(event: DragEvent): void {
    const element = event.target as HTMLElement;
    element.classList.remove('dragging');
    this.TodoDragDropService.clearDraggedTodo();
  }
  onDragOver(event: DragEvent): void {
    event.preventDefault();
    event.dataTransfer!.dropEffect = 'move';
    
    const target = event.currentTarget as HTMLElement;
    target.classList.add('drag-over');
  }
  onDragLeave(event: DragEvent): void {
    const target = event.currentTarget as HTMLElement;
    target.classList.remove('drag-over');
  }
  onDropToToday(event: DragEvent): void {
    event.preventDefault();  
    const target = event.currentTarget as HTMLElement;
    target.classList.remove('drag-over');  
    const draggedTodo = this.TodoDragDropService.getDraggedTodo();
    if (draggedTodo && !this.todayWidget.todoIds.includes(draggedTodo.id)) {
      this.todayWidget.todoIds.push(draggedTodo.id);
    }  
    this.TodoDragDropService.clearDraggedTodo();
  }

  // Удаление задачи из виджета "Сегодня"
  removeFromToday(todoId: number, event: Event): void {
    event.stopPropagation();
    this.todayWidget.todoIds = this.todayWidget.todoIds.filter(id => id !== todoId);
  }
  // Переключение свертывания виджета
  toggleTodayWidget(): void {
    this.todayWidget.isCollapsed = !this.todayWidget.isCollapsed;
  }
  // Получение задач для виджета "Сегодня"
  getTodayTodos(): Todo[] {
    return this.todos.filter(todo => 
      this.todayWidget.todoIds.includes(todo.id)
    );
  }
  // Проверка, находится ли задача в виджете "Сегодня"
  isInTodayWidget(todoId: number): boolean {
    return this.todayWidget.todoIds.includes(todoId);
  }
  // Очистка виджета "Сегодня"
  clearTodayWidget(): void {
    this.todayWidget.todoIds = [];
  }
  // Автоматическое добавление задач с дедлайном на сегодня
  autoAddTodayTasks(): void {
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    
    const todayTodos = this.todos.filter(todo => {
      if (!todo.deadline || todo.isCompleted) return false;
      
      const deadlineDate = new Date(todo.deadline);
      deadlineDate.setHours(0, 0, 0, 0);
      
      return deadlineDate.getTime() === today.getTime() && 
             !this.todayWidget.todoIds.includes(todo.id);
    });   
    todayTodos.forEach(todo => {
      this.todayWidget.todoIds.push(todo.id);
    });
  }
}