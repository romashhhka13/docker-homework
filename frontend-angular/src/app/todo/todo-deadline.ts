import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TodoDeadlineService  {
  // Минимальная дата - текущий момент
  getMinDate(): string {
    return new Date().toISOString().slice(0, 16);
  }

  // Проверяет, просрочен ли дедлайн
  isOverdue(deadline: Date | null): boolean {
    if (!deadline) return false;
    return new Date(deadline) < new Date();
  }

  // Проверяет валидность дедлайна (не раньше текущего времени)
  isDeadlineValid(deadline: string | null): boolean {
    if (!deadline) return true; // если дедлайн не установлен - валидно
    
    const deadlineDate = new Date(deadline);
    const now = new Date();
    return deadlineDate >= now;
  }

  // Форматирует дедлайн для отображения
  formatDeadline(deadline: Date | null): string {
    if (!deadline) return '';
    return new Date(deadline).toLocaleString('ru-RU', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }
}