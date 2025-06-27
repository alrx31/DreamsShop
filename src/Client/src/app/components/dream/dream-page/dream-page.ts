import { Component } from '@angular/core';
import {Dream, Dreams} from '../../../services/dreams';
import {ActivatedRoute, Router} from '@angular/router';
import {BackButton} from '../../aditional/back-button/back-button';
import {Loader} from '../../aditional/loader/loader';

@Component({
  selector: 'app-dream-page',
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
}
