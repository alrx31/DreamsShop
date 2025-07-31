import { Component } from '@angular/core';
import { UserAfterLoginInfo } from '../../environment/UserAfterLoginInfo';
import { ActivatedRoute, Router } from '@angular/router';
import { Loader } from '../aditional/loader/loader';
import { CommonModule } from '@angular/common';
import { environment } from '../../environment/environment';
import { Order, OrderService } from '../../services/order/order';
import { MatMenuContent } from "@angular/material/menu";

@Component({
  selector: 'app-profile',
  imports: [
    Loader,
    CommonModule,
    MatMenuContent
],
  templateUrl: './profile.html',
  styleUrl: './profile.scss'
})

export class Profile {
  userInfo: UserAfterLoginInfo = {} as UserAfterLoginInfo;
  userOrders: Order[] = [];
  errorUserInfo: string = '';
  errorOrders: string = '';
  
  loading: boolean = true;

  constructor(private activatedRoute: ActivatedRoute,
              private orderService: OrderService,
              private router: Router) { }


  ngOnInit(): void {
    this.loading = true;

    let data = JSON.parse(localStorage.getItem('UserInfo') || '{}') as UserAfterLoginInfo;
    if (data) {
      this.userInfo = data;
      if (!this.userInfo.id || !this.userInfo.email || !this.userInfo.name || this.userInfo.role === undefined) {
        this.errorUserInfo = 'Invalid user information';
        console.log(this.errorUserInfo);
        this.router.navigate(['/login']);
      }
    } else {
      this.errorUserInfo = 'User information not found';
      console.log(this.errorUserInfo);
      this.router.navigate(['/login']);
    }
    this.onLoadOrders();
    this.loading = false;
  }

  onLogout() : void {
    localStorage.removeItem('UserInfo');
    localStorage.removeItem(environment.accessTokenName);
    this.router.navigate(['/login']);
  }

  onLoadOrders() : void {
    this.loading = true;
    this.orderService.getUserOrders().subscribe({
      next: (orders) => {
        this.userOrders = orders;
      },
      error: (err) => {
        this.errorOrders = 'Failed to load orders';
        console.log(err);
      }
    });
  }

  ngOnDestroy(): void {
    this.loading = false;
  }
}
