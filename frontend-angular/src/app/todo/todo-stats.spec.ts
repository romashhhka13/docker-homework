import { TestBed } from '@angular/core/testing';

import { TodoStatsService } from './todo-stats';

describe('TodoStats', () => {
  let service: TodoStatsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TodoStatsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
