export interface Todo {
  id: number;
  title: string;
  description: string;
  isCompleted: boolean;
  isFavorite: boolean;
  createDate: Date;
  deadline: Date | null;
}

export interface TodoStats {
  completed: number;
  active: number;
  overdue: number;
  total: number;
}

export interface TodayWidgetState {
  id: number;
  todoIds: number[];
  isCollapsed: boolean;
}

export type FilterType = 'all' | 'completed' | 'active' | 'favorite' | 'missed';