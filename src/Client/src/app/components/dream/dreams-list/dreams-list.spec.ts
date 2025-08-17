import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DreamsList } from './dreams-list';

describe('DreamsList', () => {
  let component: DreamsList;
  let fixture: ComponentFixture<DreamsList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DreamsList]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DreamsList);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
