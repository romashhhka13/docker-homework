import { TestBed } from '@angular/core/testing';

import { TodoDragDropService } from './todo-drag-drop';

describe('TodoDragDrop', () => {
  let service: TodoDragDropService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TodoDragDropService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
