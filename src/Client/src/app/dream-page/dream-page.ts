import { Component } from '@angular/core';
import {Dream, Dreams} from '../dreams';
import {ActivatedRoute, Router} from '@angular/router';

@Component({
  selector: 'app-dream-page',
  imports: [],
  templateUrl: './dream-page.html',
  styleUrl: './dream-page.scss'
})
export class DreamPage {
  dream: Dream = {} as Dream;
  error: string = '';
  loading: boolean = true;

  constructor(private dreamsService: Dreams,
              private router: ActivatedRoute) {}

  ngOnInit(): void{
    this.loading = true;

    this.router.params.subscribe(params =>{
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
}
