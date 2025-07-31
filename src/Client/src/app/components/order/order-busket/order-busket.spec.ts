import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderBusket } from './order-busket';

describe('OrderBusket', () => {
  let component: OrderBusket;
  let fixture: ComponentFixture<OrderBusket>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OrderBusket]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OrderBusket);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
