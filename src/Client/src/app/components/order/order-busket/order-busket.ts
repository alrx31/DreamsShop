import { Component } from '@angular/core';
import { Dream } from '../../../services/dreams/dreams';
import { CommonModule } from '@angular/common';
import { Loader } from '../../aditional/loader/loader';
import { OrderService } from '../../../services/order/order';
import { Router } from '@angular/router';

@Component({
  selector: 'app-order-busket',
  imports: [
    CommonModule,
    Loader
  ],
  templateUrl: './order-busket.html',
  styleUrl: './order-busket.scss'
})

export class OrderBusket {
  dreams: Dream[] = [];
  loading = false;
  error: string = '';

  constructor(private orderService: OrderService,
              private router: Router) {}

  ngOnInit() {
    this.loading = true;
    this.error = '';

    var data = JSON.parse(localStorage.getItem('Order-Dreams') || '[]');

    this.dreams = data.map((item: any) => {
      return {
        id: item.id,
        title: item.title,
        description: item.description
      };
    });

    this.loading = false;
  }

  onOrderSubmit() {
    this.loading = true;
    if (this.dreams.length === 0) {
      alert('Your order is empty');
      return;
    }

    this.orderService.createOrder(this.dreams.map(dream => dream.id)).subscribe({
      next: order => {
        alert('Order created successfully');
        this.clearOrder();
      },
      error: err => {
        this.error = err.message;
        console.error(err);
      }
    })

    this.loading = false;
    this.router.navigate(['/']);
  }

  clearOrder() {
    localStorage.removeItem('Order-Dreams');
    this.dreams = [];
  }
}
