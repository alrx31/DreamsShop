import { Component } from '@angular/core';
import {Dream, Dreams} from '../../../services/dreams/dreams';
import {ActivatedRoute, Router} from '@angular/router';
import {BackButton} from '../../aditional/back-button/back-button';
import {Loader} from '../../aditional/loader/loader';
import { OrderService } from '../../../services/order/order';

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

  constructor(private dreamsService: Dreams,
              private order: OrderService,
              private activatedRoute: ActivatedRoute,
              private router:Router) {}

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
}
