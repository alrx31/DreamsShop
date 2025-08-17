import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateDreamPopUp } from './create-dream-pop-up';

describe('CreateDreamPopUp', () => {
  let component: CreateDreamPopUp;
  let fixture: ComponentFixture<CreateDreamPopUp>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateDreamPopUp]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateDreamPopUp);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
