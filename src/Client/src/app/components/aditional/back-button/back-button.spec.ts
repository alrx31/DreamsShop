import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BackButton } from './back-button';

describe('BackButton', () => {
  let component: BackButton;
  let fixture: ComponentFixture<BackButton>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BackButton]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BackButton);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
