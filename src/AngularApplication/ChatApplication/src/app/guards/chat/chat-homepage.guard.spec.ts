import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { chatHomepageGuard } from './chat-homepage.guard';

describe('chatHomepageGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => chatHomepageGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
