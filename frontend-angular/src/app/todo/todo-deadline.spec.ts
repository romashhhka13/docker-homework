import { TestBed } from '@angular/core/testing';

import { TodoDeadlineService  } from './todo-deadline';

describe('TodoDeadline', () => {
  let service: TodoDeadlineService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TodoDeadlineService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
