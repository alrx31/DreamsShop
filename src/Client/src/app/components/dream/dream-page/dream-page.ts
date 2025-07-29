import { Component } from '@angular/core';
import {Dream, Dreams} from '../../../services/dreams/dreams';
import {ActivatedRoute, Router} from '@angular/router';
import {BackButton} from '../../aditional/back-button/back-button';
import {Loader} from '../../aditional/loader/loader';
import { OrderService } from '../../../services/order/order';
import { UserRoles } from '../../../environment/UserRoles';
import { CreateDreamPopUp } from '../create-dream-pop-up/create-dream-pop-up';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-dreams-page',
  imports: [
    BackButton,
    Loader
  ],
  templateUrl: './dream-page.html',
  styleUrl: './dream-page.scss'
})
export class DreamPage {
  dream: Dream = {} as Dream;
  error: string = '';
  loading: boolean = true;
  role: UserRoles | null = null;

  constructor(private dreamsService: Dreams,
              private order: OrderService,
              private activatedRoute: ActivatedRoute,
              private router:Router,
              private dialog: MatDialog) {

    const userInfo = localStorage.getItem('UserInfo');
    if(userInfo){
      this.role = +JSON.parse(userInfo).role as UserRoles;
    }
  }

  ngOnInit(): void{
    this.loading = true;

    this.activatedRoute.params.subscribe(params =>{
      const dreamId = params['dreamId'];
      this.dreamsService.getDream(dreamId).subscribe({
        next: dream => {
          this.dream = dream;
          this.loading = false;
        },
        error: err => {
          this.error = err;
          console.log(err);
        }
      })
    })
  }

  onDeleteButtonSubmit(){
    this.dreamsService.deleteDream(this.dream.id).subscribe({
      next: dream => {
        this.router.navigate(['/']);
      },
      error: err => {
        alert(err.message);
        console.log(err);
      }
    })
  }

  onAddToOrderButtonSubmit() {
    this.order.addDreamToOrder(this.dream);
    alert('Dream added to order');
  }

  onUpdateDreamClick(){
    const dialogRef = this.dialog.open(CreateDreamPopUp, {
      data: {
        dream: this.dream,
        isUpdate: true
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.ngOnInit();
      }
    });
  }

  protected readonly UserRoles = UserRoles;
}
