import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environment/environment';
import { Dream } from '../dreams/dreams';

export interface Order {
  orderId: string;
  createdAt: Date;
  orderDreams: OrderDream[];
}

export interface OrderDream {
  dreamId: string;
  title: string;
}

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private urlSuffix = 'Order';
  constructor(private http: HttpClient) { }

  addDreamToOrder(dream: Dream) {
    var data = JSON.parse(localStorage.getItem('Order-Dreams') || '[]');
    if (!data.some((item: any) => item.id === dream.id)) {
      data.push(dream);
      localStorage.setItem('Order-Dreams', JSON.stringify(data));
    }
  }

  getUserOrders() {
    return this.http.get<Order[]>(`${environment.apiUrl}${this.urlSuffix}`);
  }

  createOrder(dreamIds: string[]) {
    return this.http.post<Order>(`${environment.apiUrl}${this.urlSuffix}`, {
      dreamIds: dreamIds
    });
  }
}
