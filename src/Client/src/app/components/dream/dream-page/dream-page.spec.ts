import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DreamPage } from './dream-page';

describe('DreamPage', () => {
  let component: DreamPage;
  let fixture: ComponentFixture<DreamPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DreamPage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DreamPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
