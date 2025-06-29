import { TestBed } from '@angular/core/testing';

import { Dreams } from './dreams';

describe('Dreams', () => {
  let service: Dreams;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Dreams);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
