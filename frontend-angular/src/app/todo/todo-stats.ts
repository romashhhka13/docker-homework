import { Injectable } from '@angular/core';
import { Todo } from './todoModel';
import { TodoDeadlineService } from './todo-deadline';

@Injectable({
  providedIn: 'root'
})
export class TodoStatsService {
  constructor(private todoDeadlineService: TodoDeadlineService) {}

  getTodoStats(todos: Todo[]): any {
    const stats = {
      completed: 0,
      active: 0,
      overdue: 0,
      total: todos.length
    };
    todos.forEach(todo => {
      if (todo.isCompleted) {
        stats.completed++;
      } else if (this.todoDeadlineService.isOverdue(todo.deadline)) {
        stats.overdue++;
      } else {
        stats.active++;
      }
    });
    return stats;
  }

  getChartData(todos: Todo[]): any {
    const stats = this.getTodoStats(todos);
    const total = stats.total || 1;
    
    return {
      labels: [
        `Выполнено - ${stats.completed} (${Math.round((stats.completed / total) * 100)}%)`,
        `Активные - ${stats.active} (${Math.round((stats.active / total) * 100)}%)`,
        `Просроченные - ${stats.overdue} (${Math.round((stats.overdue / total) * 100)}%)`
      ],
      datasets: [
        {
          data: [stats.completed, stats.active, stats.overdue],
          backgroundColor: [
            '#4CAF50',
            '#2196F3', 
            '#F44336'
          ],
          hoverBackgroundColor: [
            '#66BB6A',
            '#42A5F5',
            '#EF5350'
          ],
          borderColor: '#fff',
          borderWidth: 3,
          hoverBorderWidth: 4
        }
      ]
    };
  }

  // Обновленный метод для данных гистограммы
  getDeadlineChartData(todos: Todo[]): any {
    const overdueCount = this.getOverdueCount(todos);
    const next7Days = this.getNext7Days();
    const deadlinesCount = this.countDeadlinesByDay(todos, next7Days);
    
    // Добавляем просроченные в начало
    const allLabels = ['Просроченные', ...next7Days.map(date => this.formatDateLabel(date))];
    const allData = [overdueCount, ...deadlinesCount];
    
    return {
      labels: allLabels,
      datasets: [
        {
          label: 'Дедлайны',
          data: allData,
          backgroundColor: this.getBackgroundColors(allData.length),
          borderColor: 'rgba(102, 126, 234, 1)',
          borderWidth: 2,
          borderRadius: 8,
          hoverBackgroundColor: this.getHoverColors(allData.length),
          hoverBorderColor: 'rgba(102, 126, 234, 1)',
          barPercentage: 0.7,
          categoryPercentage: 0.8
        }
      ]
    };
  }

  // Получаем количество просроченных задач
  private getOverdueCount(todos: Todo[]): number {
    return todos.filter(todo => 
      !todo.isCompleted && this.todoDeadlineService.isOverdue(todo.deadline)
    ).length;
  }

  // Получаем даты на ближайшие 7 дней
  private getNext7Days(): Date[] {
    const dates: Date[] = [];
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    
    for (let i = 0; i < 7; i++) {
      const date = new Date(today);
      date.setDate(today.getDate() + i);
      dates.push(date);
    }
    
    return dates;
  }

  // Считаем дедлайны по дням (исключая просроченные)
  private countDeadlinesByDay(todos: Todo[], dates: Date[]): number[] {
    return dates.map(date => {
      return todos.filter(todo => {
        if (!todo.deadline || todo.isCompleted || this.todoDeadlineService.isOverdue(todo.deadline)) {
          return false;
        }
        
        const deadlineDate = new Date(todo.deadline);
        deadlineDate.setHours(0, 0, 0, 0);
        
        return deadlineDate.getTime() === date.getTime();
      }).length;
    });
  }

  // Форматируем дату для подписи
  private formatDateLabel(date: Date): string {
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    
    const tomorrow = new Date(today);
    tomorrow.setDate(today.getDate() + 1);
    
    if (date.getTime() === today.getTime()) {
      return 'Сегодня';
    } else if (date.getTime() === tomorrow.getTime()) {
      return 'Завтра';
    } else {
      const dayNames = ['Вс', 'Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб'];
      const monthNames = ['янв', 'фев', 'мар', 'апр', 'май', 'июн', 'июл', 'авг', 'сен', 'окт', 'ноя', 'дек'];
      
      const dayName = dayNames[date.getDay()];
      const day = date.getDate();
      const month = monthNames[date.getMonth()];
      
      return `${dayName}, ${day} ${month}`;
    }
  }

  // Генерируем цвета для столбцов
  private getBackgroundColors(count: number): string[] {
    const colors: string[] = [];
    
    // Первый столбец (просроченные) - красный
    colors.push('rgba(244, 67, 54, 0.8)');
    
    // Остальные столбцы - градиент синего
    for (let i = 1; i < count; i++) {
      const opacity = 0.6 + (i / count) * 0.4; // Плавное увеличение opacity
      colors.push(`rgba(33, 150, 243, ${opacity})`);
    }
    
    return colors;
  }

  // Генерируем цвета при наведении
  private getHoverColors(count: number): string[] {
    const colors: string[] = [];
    
    // Первый столбец (просроченные) - ярко-красный
    colors.push('rgba(244, 67, 54, 1)');
    
    // Остальные столбцы - ярко-синий
    for (let i = 1; i < count; i++) {
      colors.push('rgba(33, 150, 243, 1)');
    }
    
    return colors;
  }

  getChartOptions(): any {
    return {
      responsive: true,
      maintainAspectRatio: false,
      cutout: '50%',
      plugins: {
        legend: {
          position: 'bottom',
          labels: {
            padding: 20,
            usePointStyle: true,
            font: {
              size: 12,
              weight: '600'
            },
            color: '#2d3748'
          }
        },
        tooltip: {
          backgroundColor: 'rgba(255, 255, 255, 0.95)',
          titleColor: '#2d3748',
          bodyColor: '#4a5568',
          borderColor: '#e2e8f0',
          borderWidth: 1,
          cornerRadius: 8,
          displayColors: true,
          callbacks: {
            label: function(context: any) {
              const label = context.label || '';
              const value = context.raw || 0;
              const total = context.dataset.data.reduce((a: number, b: number) => a + b, 0);
              const percentage = Math.round((value / total) * 100);
              return `${label.split(' - ')[0]}: ${value} задач (${percentage}%)`;
            }
          }
        }
      }
    };
  }

  // Обновленные опции для гистограммы
  getDeadlineChartOptions(): any {
    return {
      responsive: true,
      maintainAspectRatio: false,
      plugins: {
        legend: {
          display: false
        },
        tooltip: {
          backgroundColor: 'rgba(255, 255, 255, 0.95)',
          titleColor: '#2d3748',
          bodyColor: '#4a5568',
          borderColor: '#e2e8f0',
          borderWidth: 1,
          cornerRadius: 8,
          displayColors: true,
          callbacks: {
            title: (context: any) => {
              return context[0].label;
            },
            label: (context: any) => {
              const label = context.label;
              const value = context.raw;
              
              if (label === 'Просроченные') {
                if (value === 0) return 'Нет просроченных дедлайнов';
                if (value === 1) return '1 просроченный дедлайн';
                if (value < 5) return `${value} просроченных дедлайна`;
                return `${value} просроченных дедлайнов`;
              } else {
                if (value === 0) return 'Нет дедлайнов';
                if (value === 1) return '1 дедлайн';
                if (value < 5) return `${value} дедлайна`;
                return `${value} дедлайнов`;
              }
            },
            labelColor: (context: any) => {
              return {
                borderColor: context.dataset.backgroundColor[context.dataIndex],
                backgroundColor: context.dataset.backgroundColor[context.dataIndex],
                borderWidth: 2
              };
            }
          }
        }
      },
      scales: {
        x: {
          grid: {
            display: false
          },
          ticks: {
            font: {
              size: 11,
              weight: '600'
            },
            color: '#4a5568'
          }
        },
        y: {
          beginAtZero: true,
          ticks: {
            precision: 0,
            font: {
              size: 11
            },
            color: '#718096'
          },
          grid: {
            color: 'rgba(226, 232, 240, 0.8)'
          },
          title: {
            display: true,
            text: 'Количество дедлайнов',
            color: '#718096',
            font: {
              size: 12,
              weight: '600'
            }
          }
        }
      }
    };
  }

  // Отдельный метод для получения центрального текста
  getCenterTextPlugin(todos: Todo[]): any {
    const stats = this.getTodoStats(todos);
    
    return {
      id: 'centerText',
      afterDraw: (chart: any) => {
        const ctx = chart.ctx;
        const width = chart.width;
        const height = chart.height;
        
        ctx.save();
        ctx.textAlign = 'center';
        ctx.textBaseline = 'middle';
        
        // Заголовок
        ctx.font = 'bold 16px "Segoe UI", Tahoma, Geneva, Verdana, sans-serif';
        ctx.fillStyle = '#2d3748';
        ctx.fillText('Всего задач:', width / 2, height / 2 - 20);
        
        // Число
        ctx.font = 'bold 28px "Segoe UI", Tahoma, Geneva, Verdana, sans-serif';
        ctx.fillStyle = '#667eea';
        ctx.fillText(stats.total.toString(), width / 2, height / 2 + 10);
        
        ctx.restore();
      }
    };
  }
}