import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environment/environment';

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

  getUserOrders() {
    return this.http.get<Order[]>(`${environment.apiUrl}${this.urlSuffix}`);
  }
}
